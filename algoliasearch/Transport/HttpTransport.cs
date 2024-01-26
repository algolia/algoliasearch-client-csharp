using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Clients;
using Algolia.Search.Exceptions;
using Algolia.Search.Http;
using Algolia.Search.Serializer;
using Algolia.Search.Utils;

namespace Algolia.Search.Transport;

/// <summary>
/// Transport logic of the library
/// Holding an instance of the requester and the retry strategy
/// </summary>
internal class HttpTransport
{
  private readonly IHttpRequester _httpClient;
  private readonly CustomJsonSerializer _serializer;
  private readonly RetryStrategy _retryStrategy;
  private readonly AlgoliaConfig _algoliaConfig;
  private string _errorMessage;

  private class VoidResult
  {
  }

  /// <summary>
  /// Instantiate the transport class with the given configuration and requester
  /// </summary>
  /// <param name="config">Algolia Config</param>
  /// <param name="httpClient">An implementation of http requester <see cref="IHttpRequester"/> </param>
  public HttpTransport(AlgoliaConfig config, IHttpRequester httpClient)
  {
    _algoliaConfig = config ?? throw new ArgumentNullException(nameof(config));
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    _serializer = new CustomJsonSerializer(JsonConfig.AlgoliaJsonSerializerSettings);
    _retryStrategy = new RetryStrategy(config);
  }

  /// <summary>
  /// Execute the request (more likely request with no body like GET or Delete)
  /// </summary>
  /// <typeparam name="TResult"></typeparam>
  /// <param name="method">The HttpMethod <see cref="HttpMethod"/></param>
  /// <param name="uri">The endpoint URI</param>
  /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
  /// <param name="ct">Optional cancellation token</param>
  public async Task<TResult> ExecuteRequestAsync<TResult>(HttpMethod method, string uri,
    InternalRequestOptions requestOptions = null,
    CancellationToken ct = default)
    where TResult : class =>
    await ExecuteRequestAsync<TResult, object>(method, uri, requestOptions, ct)
      .ConfigureAwait(false);

  /// <summary>
  /// Execute the request, without response
  /// </summary>
  /// <param name="method">The HttpMethod <see cref="HttpMethod"/></param>
  /// <param name="uri">The endpoint URI</param>
  /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
  /// <param name="ct">Optional cancellation token</param>
  public async Task ExecuteRequestAsync(HttpMethod method, string uri,
    InternalRequestOptions requestOptions = null,
    CancellationToken ct = default) =>
    await ExecuteRequestAsync<VoidResult>(method, uri, requestOptions, ct)
      .ConfigureAwait(false);

  /// <summary>
  /// Call api with retry strategy
  /// </summary>
  /// <typeparam name="TResult">Return type</typeparam>
  /// <typeparam name="TData">Data type</typeparam>
  /// <param name="method">The HttpMethod <see cref="HttpMethod"/></param>
  /// <param name="uri">The endpoint URI</param>
  /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
  /// <param name="ct">Optional cancellation token</param>
  private async Task<TResult> ExecuteRequestAsync<TResult, TData>(HttpMethod method, string uri,
    InternalRequestOptions requestOptions = null,
    CancellationToken ct = default)
    where TResult : class
    where TData : class
  {
    if (string.IsNullOrWhiteSpace(uri))
    {
      throw new ArgumentNullException(nameof(uri));
    }

    if (method == null)
    {
      throw new ArgumentNullException(nameof(method));
    }

    var request = new Request
    {
      Method = method,
      Headers = GenerateHeaders(requestOptions?.HeaderParameters),
      Compression = _algoliaConfig.Compression
    };

    var callType =
      (requestOptions?.UseReadTransporter != null && requestOptions.UseReadTransporter.Value) ||
      method == HttpMethod.Get
        ? CallType.Read
        : CallType.Write;

    foreach (var host in _retryStrategy.GetTryableHost(callType))
    {
      request.Body = CreateRequestContent(requestOptions?.Data, request.CanCompress);
      request.Uri = BuildUri(host.Url, uri, requestOptions?.CustomPathParameters, requestOptions?.PathParameters,
        requestOptions?.QueryParameters);
      var requestTimeout =
        TimeSpan.FromTicks((requestOptions?.Timeout ?? GetTimeOut(callType)).Ticks * (host.RetryCount + 1));

      if (string.IsNullOrWhiteSpace(request.Body) && (method == HttpMethod.Post || method == HttpMethod.Put))
      {
        request.Body = "{}";
      }

      var response = await _httpClient
        .SendRequestAsync(request, requestTimeout, _algoliaConfig.ConnectTimeout ?? Defaults.ConnectTimeout, ct)
        .ConfigureAwait(false);

      _errorMessage = response.Error;

      switch (_retryStrategy.Decide(host, response))
      {
        case RetryOutcomeType.Success:
          if (typeof(TResult) == typeof(VoidResult))
          {
            return new VoidResult() as TResult;
          }

          return await _serializer.Deserialize<TResult>(response.Body);
        case RetryOutcomeType.Retry:
          continue;
        case RetryOutcomeType.Failure:
          throw new AlgoliaApiException(response.Error, response.HttpStatusCode);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    throw new AlgoliaUnreachableHostException("RetryStrategy failed to connect to Algolia. Reason: " + _errorMessage);
  }

  /// <summary>
  /// Generate stream for serializing objects
  /// </summary>
  /// <param name="data">Data to send</param>
  /// <param name="compress">Whether the stream should be compressed or not</param>
  /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
  /// <returns></returns>
  private string CreateRequestContent<T>(T data, bool compress)
  {
    return data == null ? null : _serializer.Serialize(data);
  }

  /// <summary>
  /// Generate common headers from the config
  /// </summary>
  /// <param name="optionalHeaders"></param>
  /// <returns></returns>
  private IDictionary<string, string> GenerateHeaders(IDictionary<string, string> optionalHeaders = null)
  {
    return optionalHeaders != null && optionalHeaders.Any()
      ? optionalHeaders.MergeWith(_algoliaConfig.DefaultHeaders)
      : _algoliaConfig.DefaultHeaders;
  }

  /// <summary>
  /// Build uri depending on the method
  /// </summary>
  /// <param name="url"></param>
  /// <param name="baseUri"></param>
  /// <param name="customPathParameters"></param>
  /// <param name="pathParameters"></param>
  /// <param name="optionalQueryParameters"></param>
  /// <returns></returns>
  private static Uri BuildUri(string url, string baseUri, IDictionary<string, string> customPathParameters = null,
    IDictionary<string, string> pathParameters = null,
    IDictionary<string, string> optionalQueryParameters = null)
  {
    var path = $"{baseUri}";
    if (pathParameters != null)
    {
      foreach (var parameter in pathParameters)
      {
        path = path.Replace("{" + parameter.Key + "}", Uri.EscapeDataString(parameter.Value));
      }
    }

    if (customPathParameters != null)
    {
      foreach (var parameter in customPathParameters)
      {
        path = path.Replace("{" + parameter.Key + "}", parameter.Value);
      }
    }

    if (optionalQueryParameters != null)
    {
      var queryParams = optionalQueryParameters.ToQueryString();
      return new UriBuilder { Scheme = "https", Host = url, Path = path, Query = queryParams }.Uri;
    }

    return new UriBuilder { Scheme = "https", Host = url, Path = path }.Uri;
  }

  /// <summary>
  /// Compute the request timeout with the given call type and configuration
  /// </summary>
  /// <param name="callType"></param>
  /// <returns></returns>
  private TimeSpan GetTimeOut(CallType callType)
  {
    return callType switch
    {
      CallType.Read => _algoliaConfig.ReadTimeout ?? Defaults.ReadTimeout,
      CallType.Write => _algoliaConfig.WriteTimeout ?? Defaults.WriteTimeout,
      _ => Defaults.WriteTimeout
    };
  }
}

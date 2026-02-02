using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Clients;
using Algolia.Search.Exceptions;
using Algolia.Search.Http;
using Algolia.Search.Serializer;
using Algolia.Search.Utils;
using Microsoft.Extensions.Logging;

namespace Algolia.Search.Transport;

/// <summary>
/// Transport logic of the library
/// Holding an instance of the requester and the retry strategy
/// </summary>
internal class HttpTransport
{
  private readonly IHttpRequester _httpClient;
  private readonly ISerializer _serializer;
  private readonly RetryStrategy _retryStrategy;
  internal AlgoliaConfig _algoliaConfig;
  private string _errorMessage;
  private readonly ILogger<HttpTransport> _logger;

  private class VoidResult { }

  /// <summary>
  /// Instantiate the transport class with the given configuration and requester
  /// </summary>
  /// <param name="config">Algolia Config</param>
  /// <param name="httpClient">An implementation of http requester <see cref="IHttpRequester"/> </param>
  /// <param name="loggerFactory">Logger factory</param>
  public HttpTransport(
    AlgoliaConfig config,
    IHttpRequester httpClient,
    ILoggerFactory loggerFactory
  )
  {
    _algoliaConfig = config ?? throw new ArgumentNullException(nameof(config));
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    _retryStrategy = new RetryStrategy(config);
    _serializer = new DefaultJsonSerializer(loggerFactory);
    _logger = loggerFactory.CreateLogger<HttpTransport>();
  }

  /// <summary>
  /// Execute the request (more likely request with no body like GET or Delete)
  /// </summary>
  /// <typeparam name="TResult"></typeparam>
  /// <param name="method">The HttpMethod <see cref="HttpMethod"/></param>
  /// <param name="uri">The endpoint URI</param>
  /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
  /// <param name="ct">Optional cancellation token</param>
  public async Task<TResult> ExecuteRequestAsync<TResult>(
    HttpMethod method,
    string uri,
    InternalRequestOptions requestOptions = null,
    CancellationToken ct = default
  )
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
  public async Task ExecuteRequestAsync(
    HttpMethod method,
    string uri,
    InternalRequestOptions requestOptions = null,
    CancellationToken ct = default
  ) => await ExecuteRequestAsync<VoidResult>(method, uri, requestOptions, ct).ConfigureAwait(false);

  /// <summary>
  /// Call api with retry strategy
  /// </summary>
  /// <typeparam name="TResult">Return type</typeparam>
  /// <typeparam name="TData">Data type</typeparam>
  /// <param name="method">The HttpMethod <see cref="HttpMethod"/></param>
  /// <param name="uri">The endpoint URI</param>
  /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
  /// <param name="ct">Optional cancellation token</param>
  private async Task<TResult> ExecuteRequestAsync<TResult, TData>(
    HttpMethod method,
    string uri,
    InternalRequestOptions requestOptions = null,
    CancellationToken ct = default
  )
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
      Compression = _algoliaConfig.Compression,
    };

    var callType =
      (requestOptions?.UseReadTransporter != null && requestOptions.UseReadTransporter.Value)
      || method == HttpMethod.Get
        ? CallType.Read
        : CallType.Write;

    foreach (var host in _retryStrategy.GetTryableHost(callType))
    {
      request.Body = CreateRequestContent(requestOptions?.Data, request.CanCompress, _logger);
      request.Uri = BuildUri(
        host,
        uri,
        requestOptions?.CustomPathParameters,
        requestOptions?.PathParameters,
        requestOptions?.QueryParameters
      );
      var requestTimeout = GetTimeOut(callType, requestOptions);
      var baseConnectTimeout =
        requestOptions?.ConnectTimeout ?? _algoliaConfig.ConnectTimeout ?? Defaults.ConnectTimeout;
      var connectTimeout = TimeSpan.FromTicks(baseConnectTimeout.Ticks * (host.RetryCount + 1));

      if (request.Body == null && (method == HttpMethod.Post || method == HttpMethod.Put))
      {
        request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));
      }

      if (_logger.IsEnabled(LogLevel.Trace))
      {
        _logger.LogTrace("Sending request to {Method} {Uri}", request.Method, request.Uri);
        _logger.LogTrace("Request timeout: {RequestTimeout} (s)", requestTimeout.TotalSeconds);
        _logger.LogTrace("Connect timeout: {ConnectTimeout} (s)", connectTimeout.TotalSeconds);
        foreach (var header in request.Headers)
        {
          _logger.LogTrace("Header: {HeaderName} : {HeaderValue}", header.Key, header.Value);
        }
      }

      var response = await _httpClient
        .SendRequestAsync(request, requestTimeout, connectTimeout, ct)
        .ConfigureAwait(false);

      _errorMessage = response.Error;

      switch (_retryStrategy.Decide(host, response))
      {
        case RetryOutcomeType.Success:
          if (typeof(TResult) == typeof(VoidResult))
          {
            return new VoidResult() as TResult;
          }

          if (_logger.IsEnabled(LogLevel.Trace))
          {
            var reader = new StreamReader(response.Body);
            var json = await reader.ReadToEndAsync().ConfigureAwait(false);
            _logger.LogTrace("Response HTTP {HttpCode}: {Json}", response.HttpStatusCode, json);
            response.Body.Seek(0, SeekOrigin.Begin);
          }

          // Returns the raw response when using `*WithHTTPInfo` methods.
          if (typeof(TResult) == typeof(AlgoliaHttpResponse))
          {
            return response as TResult;
          }

          var deserialized = await _serializer
            .Deserialize<TResult>(response.Body)
            .ConfigureAwait(false);

          if (_logger.IsEnabled(LogLevel.Trace))
          {
            _logger.LogTrace("Object created: {objectCreated}", deserialized);
          }

          return deserialized;
        case RetryOutcomeType.Retry:
          if (_logger.IsEnabled(LogLevel.Debug))
          {
            _logger.LogDebug(
              "Retrying ... Retryable error for response HTTP {HttpCode} : {Error}",
              response.HttpStatusCode,
              response.Error
            );
          }

          continue;
        case RetryOutcomeType.Failure:
          if (_logger.IsEnabled(LogLevel.Error))
          {
            _logger.LogError(
              "Retry strategy with failure outcome. Response HTTP {HttpCode} : {Error}",
              response.HttpStatusCode,
              response.Error
            );
          }

          throw new AlgoliaApiException(response.Error, response.HttpStatusCode);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    if (_logger.IsEnabled(LogLevel.Error))
    {
      _logger.LogError("Retry strategy failed: {ErrorMessage}", _errorMessage);
    }

    throw new AlgoliaUnreachableHostException(
      "RetryStrategy failed to connect to Algolia. If the error persists, please visit our help center https://alg.li/support-unreachable-hosts or reach out to the Algolia Support team: https://alg.li/support Reason: "
        + _errorMessage
    );
  }

  /// <summary>
  /// Generate stream for serializing objects
  /// </summary>
  /// <param name="data">Data to send</param>
  /// <param name="compress">Whether the stream should be compressed or not</param>
  /// <param name="logger">Logger</param>
  /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
  /// <returns></returns>
  private MemoryStream CreateRequestContent<T>(T data, bool compress, ILogger logger)
  {
    var serializedData = _serializer.Serialize(data);

    if (_logger.IsEnabled(LogLevel.Trace))
    {
      logger.LogTrace("Serialized request data: {Json}", serializedData);
    }

    return data == null ? null : Compression.CreateStream(serializedData, compress);
  }

  /// <summary>
  /// Generate common headers from the config
  /// </summary>
  /// <param name="optionalHeaders"></param>
  /// <returns></returns>
  private IDictionary<string, string> GenerateHeaders(
    IDictionary<string, string> optionalHeaders = null
  )
  {
    return optionalHeaders != null && optionalHeaders.Any()
      ? optionalHeaders.MergeWith(_algoliaConfig.BuildHeaders())
      : _algoliaConfig.BuildHeaders();
  }

  /// <summary>
  /// Build uri depending on the method
  /// </summary>
  /// <param name="host"></param>
  /// <param name="baseUri"></param>
  /// <param name="customPathParameters"></param>
  /// <param name="pathParameters"></param>
  /// <param name="optionalQueryParameters"></param>
  /// <returns></returns>
  private static Uri BuildUri(
    StatefulHost host,
    string baseUri,
    IDictionary<string, string> customPathParameters = null,
    IDictionary<string, string> pathParameters = null,
    IDictionary<string, string> optionalQueryParameters = null
  )
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

    var builder = new UriBuilder
    {
      Scheme = host.Scheme.ToString(),
      Host = host.Url,
      Path = path,
    };

    if (optionalQueryParameters != null && optionalQueryParameters.Any())
    {
      builder.Query = optionalQueryParameters.ToQueryString();
    }

    if (host.Port.HasValue)
    {
      builder.Port = host.Port.Value;
    }

    return builder.Uri;
  }

  /// <summary>
  /// Compute the request timeout with the given call type and configuration
  /// </summary>
  /// <param name="callType"></param>
  /// <param name="requestOptions"></param>
  /// <returns></returns>
  private TimeSpan GetTimeOut(CallType callType, InternalRequestOptions requestOptions = null)
  {
    return callType switch
    {
      CallType.Read => requestOptions?.ReadTimeout
        ?? _algoliaConfig.ReadTimeout
        ?? Defaults.ReadTimeout,
      CallType.Write => requestOptions?.WriteTimeout
        ?? _algoliaConfig.WriteTimeout
        ?? Defaults.WriteTimeout,
      _ => Defaults.WriteTimeout,
    };
  }
}

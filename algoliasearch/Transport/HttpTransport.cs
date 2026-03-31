using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    var tryableHosts = _retryStrategy.GetTryableHost(callType).ToList();
    var maxAttempts = tryableHosts.Count;
    var attemptNumber = 0;
    var overallStopwatch = Stopwatch.StartNew();

    foreach (var host in tryableHosts)
    {
      attemptNumber++;
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
        _logger.LogTrace(
          "Sending request: {Method} {Uri}",
          request.Method,
          SanitizeUrl(request.Uri)
        );
        _logger.LogTrace("Request timeout: {RequestTimeout} (s)", requestTimeout.TotalSeconds);
        _logger.LogTrace("Connect timeout: {ConnectTimeout} (s)", connectTimeout.TotalSeconds);
        foreach (var header in FilterHeaders(request.Headers))
        {
          _logger.LogTrace("Header: {HeaderName}: {HeaderValue}", header.Key, header.Value);
        }
      }

      var requestStopwatch = Stopwatch.StartNew();
      var response = await _httpClient
        .SendRequestAsync(request, requestTimeout, connectTimeout, ct)
        .ConfigureAwait(false);
      requestStopwatch.Stop();

      _errorMessage = response.Error;

      switch (_retryStrategy.Decide(host, response))
      {
        case RetryOutcomeType.Success:
          if (_logger.IsEnabled(LogLevel.Information))
          {
            _logger.LogInformation(
              "{Method} {SanitizedUrl} - {StatusCode} ({Duration}ms)",
              request.Method,
              SanitizeUrl(request.Uri),
              response.HttpStatusCode,
              requestStopwatch.ElapsedMilliseconds
            );

            if (attemptNumber > 1)
            {
              overallStopwatch.Stop();
              _logger.LogInformation(
                "Request completed on attempt {Attempt}/{MaxAttempts} (total: {TotalDuration}ms)",
                attemptNumber,
                maxAttempts,
                overallStopwatch.ElapsedMilliseconds
              );
            }
          }

          if (_logger.IsEnabled(LogLevel.Trace))
          {
            if (response.ResponseHeaders != null)
            {
              foreach (var header in response.ResponseHeaders)
              {
                _logger.LogTrace(
                  "Response header: {HeaderName}: {HeaderValue}",
                  header.Key,
                  header.Value
                );
              }
            }

            if (response.Body != null)
            {
              var reader = new StreamReader(response.Body);
              var json = await reader.ReadToEndAsync().ConfigureAwait(false);
              _logger.LogTrace("Response HTTP {HttpCode}: {Json}", response.HttpStatusCode, json);
              response.Body.Seek(0, SeekOrigin.Begin);
            }
          }

          if (typeof(TResult) == typeof(VoidResult))
          {
            return new VoidResult() as TResult;
          }

          // Returns the raw response when using `*WithHTTPInfo` methods.
          if (typeof(TResult) == typeof(AlgoliaHttpResponse))
          {
            return response as TResult;
          }

          var deserialized = await _serializer
            .Deserialize<TResult>(response.Body)
            .ConfigureAwait(false);

          if (_logger.IsEnabled(LogLevel.Debug))
          {
            _logger.LogDebug("Object created: {ObjectCreated}", deserialized);
          }

          return deserialized;
        case RetryOutcomeType.Retry:
          if (_logger.IsEnabled(LogLevel.Information))
          {
            _logger.LogInformation(
              "Retry {RetryCount}/{MaxRetries}: Timeout on {Host} after {ConnectTimeout}ms",
              attemptNumber,
              maxAttempts - 1,
              host.Url,
              (int)connectTimeout.TotalMilliseconds
            );
          }

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
      _logger.LogError(
        "Request failed after {MaxAttempts} attempts: {ErrorMessage}",
        maxAttempts,
        _errorMessage
      );
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
      logger.LogTrace("Request body: {Json}", serializedData);
    }

    return data == null ? null : Compression.CreateStream(serializedData, compress);
  }

  private static readonly string[] _sensitiveHeaders = { "x-algolia-api-key", "authorization" };

  private static IDictionary<string, string> FilterHeaders(IDictionary<string, string> headers)
  {
    if (headers == null)
      return new Dictionary<string, string>();

    var filtered = new Dictionary<string, string>(headers);
    foreach (var key in _sensitiveHeaders)
    {
      if (filtered.ContainsKey(key))
        filtered[key] = "[FILTERED]";
    }
    return filtered;
  }

  private static string SanitizeUrl(Uri uri)
  {
    if (uri == null)
      return string.Empty;

    if (string.IsNullOrEmpty(uri.Query))
      return uri.ToString();

    var parts = uri.Query.TrimStart('?').Split('&');
    var sanitized = parts.Select(p =>
    {
      var kv = p.Split(new[] { '=' }, 2);
      if (
        kv.Length == 2
        && (
          kv[0].Equals("x-algolia-api-key", StringComparison.OrdinalIgnoreCase)
          || kv[0].Equals("apiKey", StringComparison.OrdinalIgnoreCase)
          || kv[0].Equals("api_key", StringComparison.OrdinalIgnoreCase)
        )
      )
      {
        return kv[0] + "=[FILTERED]";
      }
      return p;
    });

    var builder = new UriBuilder(uri) { Query = string.Join("&", sanitized) };
    return builder.Uri.ToString();
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

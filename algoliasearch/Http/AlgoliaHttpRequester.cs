using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Algolia.Search.Http;

/// <summary>
/// Algolia's HTTP requester
/// You can inject your own by implementing IHttpRequester
/// </summary>
internal class AlgoliaHttpRequester : IHttpRequester
{
  /// <summary>
  /// https://docs.microsoft.com/en-gb/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
  /// </summary>
  private readonly HttpClient _httpClient = new(
    new TimeoutHandler
    {
      InnerHandler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip },
    }
  );

  private readonly ILogger<AlgoliaHttpRequester> _logger;

  public AlgoliaHttpRequester(ILoggerFactory loggerFactory)
  {
    var logger = loggerFactory ?? NullLoggerFactory.Instance;
    _logger = logger.CreateLogger<AlgoliaHttpRequester>();
  }

  /// <summary>
  /// Don't use it directly
  /// Send request to the REST API
  /// </summary>
  /// <param name="request">Request</param>
  /// <param name="requestTimeout">Request timeout</param>
  /// <param name="connectTimeout">Connect timeout</param>
  /// <param name="ct">Optional cancellation token</param>
  /// <returns></returns>
  public async Task<AlgoliaHttpResponse> SendRequestAsync(
    Request request,
    TimeSpan requestTimeout,
    TimeSpan connectTimeout,
    CancellationToken ct = default
  )
  {
    if (request.Method == null)
    {
      throw new ArgumentNullException(nameof(request.Method), "No HTTP method found");
    }

    if (request.Uri == null)
    {
      throw new ArgumentNullException(nameof(request), "No URI found");
    }

    var httpRequestMessage = new HttpRequestMessage
    {
      Method = request.Method,
      RequestUri = request.Uri,
      Content = request.Body != null ? new StreamContent(request.Body) : null,
    };

    if (request.Body != null && httpRequestMessage.Content != null)
    {
      httpRequestMessage.Content.Headers.Clear();
      httpRequestMessage.Content.Headers.Fill(request);
    }

    httpRequestMessage.Headers.Fill(request.Headers);

    httpRequestMessage.SetTimeout(connectTimeout);

    try
    {
      using (httpRequestMessage)
      using (
        var response = await _httpClient.SendAsync(httpRequestMessage, ct).ConfigureAwait(false)
      )
      {
        using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
        {
          if (!response.IsSuccessStatusCode)
          {
            return new AlgoliaHttpResponse
            {
              Error = await StreamToStringAsync(stream),
              HttpStatusCode = (int)response.StatusCode,
            };
          }

          var outputStream = new MemoryStream();
          await stream.CopyToAsync(outputStream).ConfigureAwait(false);
          outputStream.Seek(0, SeekOrigin.Begin);

          return new AlgoliaHttpResponse
          {
            Body = outputStream,
            HttpStatusCode = (int)response.StatusCode,
          };
        }
      }
    }
    catch (TimeoutException ex)
    {
      if (_logger.IsEnabled(LogLevel.Warning))
      {
        _logger.LogWarning(ex, "Timeout while sending request");
      }

      return new AlgoliaHttpResponse { IsTimedOut = true, Error = ex.Message };
    }
    catch (HttpRequestException ex)
    {
      // HttpRequestException is thrown when an underlying issue happened such as
      // network connectivity, DNS failure, server certificate validation.
      if (_logger.IsEnabled(LogLevel.Error))
      {
        _logger.LogError(ex, "Error while sending request {Request}", request);
      }

      return new AlgoliaHttpResponse { IsNetworkError = true, Error = ex.Message };
    }
  }

  private static async Task<string> StreamToStringAsync(Stream stream)
  {
    if (stream == null)
      return null;

    using var sr = new StreamReader(stream);
    var content = await sr.ReadToEndAsync().ConfigureAwait(false);

    return content;
  }
}

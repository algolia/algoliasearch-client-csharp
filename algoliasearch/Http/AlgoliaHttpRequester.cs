using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Http
{
  /// <summary>
  /// Algolia's HTTP requester
  /// You can inject your own by the SearchClient or Analytics Client
  /// </summary>
  internal class AlgoliaHttpRequester : IHttpRequester
  {
    /// <summary>
    /// https://docs.microsoft.com/en-gb/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
    /// </summary>
    private readonly HttpClient _httpClient = new HttpClient(
        new TimeoutHandler
        {
          InnerHandler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip }
        });

    /// <summary>
    /// Don't use it directly
    /// Send request to the REST API
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="requestTimeout">Request timeout</param>
    /// <param name="connectTimeout">Connect timeout</param>
    /// <param name="ct">Optional cancellation token</param>
    /// <returns></returns>
    public async Task<AlgoliaHttpResponse> SendRequestAsync(Request request, TimeSpan requestTimeout, TimeSpan connectTimeout,
      CancellationToken ct = default)
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
        Content = request.Body != null ? new StringContent(request.Body) : null
      };

      if (request.Body != null)
      {
        httpRequestMessage.Content.Headers.Clear();
        httpRequestMessage.Content.Headers.Fill(request);
      }

      httpRequestMessage.Headers.Fill(request.Headers);
      httpRequestMessage.SetTimeout(requestTimeout + connectTimeout);

      try
      {
        using (httpRequestMessage)
        using (HttpResponseMessage response =
            await _httpClient.SendAsync(httpRequestMessage, ct).ConfigureAwait(false))
        {
          using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
          {
            if (response.IsSuccessStatusCode)
            {
              MemoryStream outputStream = new MemoryStream();
              await stream.CopyToAsync(outputStream).ConfigureAwait(false);
              outputStream.Seek(0, SeekOrigin.Begin);

              return new AlgoliaHttpResponse
              {
                Body = outputStream,
                HttpStatusCode = (int)response.StatusCode
              };
            }

            return new AlgoliaHttpResponse
            {
              Error = await StreamToStringAsync(stream),
              HttpStatusCode = (int)response.StatusCode
            };
          }
        }
      }
      catch (TimeoutException e)
      {
        return new AlgoliaHttpResponse { IsTimedOut = true, Error = e.ToString() };
      }
      catch (HttpRequestException e)
      {
        // HttpRequestException is thrown when an underlying issue happened such as
        // network connectivity, DNS failure, server certificate validation.
        return new AlgoliaHttpResponse { IsNetworkError = true, Error = e.Message };
      }
    }

    private async Task<string> StreamToStringAsync(Stream stream)
    {
      string content;

      if (stream == null)
        return null;

      using (var sr = new StreamReader(stream))
      {
        content = await sr.ReadToEndAsync().ConfigureAwait(false);
      }

      return content;
    }
  }
}

using System.Collections.Generic;
using System.Net.Http.Headers;
using Algolia.Search.Utils;

namespace Algolia.Search.Http
{
  internal static class HttpRequestHeadersExtensions
  {
    /// <summary>
    /// Extension method to easily fill the HttpRequesterHeaders object
    /// </summary>
    /// <param name="headers"></param>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    internal static HttpRequestHeaders Fill(this HttpRequestHeaders headers, IDictionary<string, string> dictionary)
    {
      foreach (var header in dictionary)
      {
        headers.Add(header.Key, header.Value);
      }

      return headers;
    }

    /// <summary>
    /// Extension method to easily fill HttpContentHeaders with the Request object
    /// </summary>
    /// <param name="headers"></param>
    /// <param name="request"></param>
    internal static HttpContentHeaders Fill(this HttpContentHeaders headers, Request request)
    {
      if (request.Body != null)
      {
        headers.Add(Defaults.ContentType, Defaults.ApplicationJson);

        if (request.CanCompress)
        {
          headers.ContentEncoding.Add(Defaults.GzipEncoding);
        }
      }

      return headers;
    }
  }
}

using System.Collections.Generic;
using System.Net.Http.Headers;
using Algolia.Search.Utils;

namespace Algolia.Search.Http;

internal static class HttpRequestHeadersExtensions
{
  /// <summary>
  /// Extension method to easily fill the HttpRequesterHeaders object
  /// </summary>
  /// <param name="headers"></param>
  /// <param name="dictionary"></param>
  /// <returns></returns>
  internal static void Fill(this HttpRequestHeaders headers, IDictionary<string, string> dictionary)
  {
    foreach (var header in dictionary)
    {
      headers.TryAddWithoutValidation(header.Key, header.Value);
    }
  }

  /// <summary>
  /// Extension method to easily fill HttpContentHeaders with the Request object
  /// </summary>
  /// <param name="headers"></param>
  /// <param name="request"></param>
  internal static void Fill(this HttpContentHeaders headers, Request request)
  {
    if (request.Body == null)
    {
      return;
    }

    headers.Add(Defaults.ContentType, Defaults.ApplicationJson);

    if (request.CanCompress)
    {
      headers.ContentEncoding.Add(Defaults.GzipEncoding);
    }
  }
}

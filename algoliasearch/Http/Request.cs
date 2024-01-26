using System;
using System.Collections.Generic;
using System.Net.Http;
using Algolia.Search.Models.Common;

namespace Algolia.Search.Http
{
  /// <summary>
  /// Request to send to the API
  /// </summary>
  public class Request
  {
    /// <summary>
    /// The HTTP verb GET,POST etc.
    /// </summary>
    public HttpMethod Method { get; set; }

    /// <summary>
    /// Uri of the request
    /// </summary>
    public Uri Uri { get; set; }

    /// <summary>
    /// Headers a dictionary
    /// </summary>
    public IDictionary<string, string> Headers { get; set; }

    /// <summary>
    /// Body of the request
    /// </summary>
    public string Body { get; set; }
    // public Stream Body { get; set; }

    /// <summary>
    /// Compression type of the request <see cref="CompressionType"/>
    /// </summary>
    public CompressionType Compression { get; set; }

    /// <summary>
    /// Tells if the request can be compressed or not
    /// </summary>
    public bool CanCompress
    {
      get
      {
        if (Method == null)
        {
          return false;
        }

        bool isMethodValid = Method.Equals(HttpMethod.Post) || Method.Equals(HttpMethod.Put);
        bool isCompressionEnabled = Compression.Equals(CompressionType.GZIP);

        return isMethodValid && isCompressionEnabled;
      }
    }
  }
}

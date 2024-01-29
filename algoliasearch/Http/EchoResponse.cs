using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Algolia.Search.Http
{
  /// <summary>
  /// Response returned by the echo API
  /// </summary>
  public class EchoResponse
  {
    /// <summary>
    /// Path of the request
    /// </summary>
    public string Path;

    /// <summary>
    /// Host of the request
    /// </summary>
    public string Host;

    /// <summary>
    /// Method of the request
    /// </summary>
    public HttpMethod Method;

    /// <summary>
    /// Body of the request
    /// </summary>
    public string Body;

    /// <summary>
    /// Query parameters of the request
    /// </summary>
    public Dictionary<string, string> QueryParameters;

    /// <summary>
    /// Headers of the request
    /// </summary>
    public Dictionary<string, string> Headers;

    /// <summary>
    /// Timeouts of the request
    /// </summary>
    public TimeSpan ConnectTimeout;

    /// <summary>
    /// Timeouts of the request
    /// </summary>
    public TimeSpan ResponseTimeout;

    /// <summary>
    /// The body stream
    /// </summary>
    public Stream BodyStream;
  }
}

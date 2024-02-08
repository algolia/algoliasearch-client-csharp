using System.IO;

namespace Algolia.Search.Http;

/// <summary>
/// Response from Algolia's API
/// </summary>
public class AlgoliaHttpResponse
{
  /// <summary>
  /// Http response code
  /// </summary>
  public int HttpStatusCode { get; set; }

  /// <summary>
  /// Stream Response body
  /// </summary>
  public Stream Body { get; set; }

  /// <summary>
  /// TimeOut
  /// </summary>
  public bool IsTimedOut { get; set; }

  /// <summary>
  /// Network connectivity, DNS failure, server certificate validation.
  /// </summary>
  public bool IsNetworkError { get; set; }

  /// <summary>
  /// Http Error message
  /// </summary>
  public string Error { get; set; }
}

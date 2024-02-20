using System;
using System.Collections.Generic;

namespace Algolia.Search.Http;

/// <summary>
/// Request option you can add to your queries
/// Use <see cref="RequestOptionBuilder"/> to help you build this object
/// </summary>
public class RequestOptions
{
  /// <summary>
  /// Custom headers will override default headers if they exist
  /// </summary>
  public IDictionary<string, string> Headers { get; set; }

  /// <summary>
  /// Add custom queries parameters
  /// </summary>
  public IDictionary<string, object> QueryParameters { get; set; }

  /// <summary>
  /// Request timeout in seconds
  /// </summary>
  public TimeSpan? Timeout { get; set; }

  /// <summary>
  /// Constructs a new instance of <see cref="RequestOptions"/>
  /// </summary>
  public RequestOptions()
  {
    QueryParameters = new Dictionary<string, object>();
    Headers = new Dictionary<string, string>();
  }
}

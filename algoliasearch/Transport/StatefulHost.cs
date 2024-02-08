using System;

namespace Algolia.Search.Transport;

/// <summary>
/// Algolia's stateful host
/// </summary>
public class StatefulHost
{
  /// <summary>
  /// Url endpoint without the scheme and the port
  /// </summary>
  public string Url { get; set; }

  /// <summary>
  /// Scheme of the URL (Default: Https)
  /// </summary>
  public HttpScheme Scheme { get; set; } = HttpScheme.Https;

  /// <summary>
  /// Port of the URL (Optional)
  /// </summary>
  public int? Port { get; set; }

  /// <summary>
  /// Is the host up or not
  /// </summary>
  public bool Up { get; set; } = true;

  /// <summary>
  /// Retry count
  /// </summary>
  public int RetryCount { get; set; }

  /// <summary>
  /// Last time the host has been used
  /// </summary>
  public DateTime LastUse { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// CallType accepted by the host
  /// </summary>
  public CallType Accept { get; set; }
}

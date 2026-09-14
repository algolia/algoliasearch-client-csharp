using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Algolia.Search.Transport;

/// <summary>
/// Parses the Retry-After header for same-host 429 waits.
/// </summary>
internal static class RetryAfter
{
  private static readonly TimeSpan DefaultWait = TimeSpan.FromSeconds(1);
  private static readonly TimeSpan MaxDelay = TimeSpan.FromMilliseconds(int.MaxValue);
  private static readonly Regex WholeSeconds = new("^[0-9]+$", RegexOptions.Compiled);

  /// <summary>
  /// Honors Retry-After only as a positive whole number of seconds.
  /// Missing, empty, 0, HTTP-date, and junk values wait 1 second. Values beyond what
  /// <see cref="System.Threading.Tasks.Task.Delay(TimeSpan)"/> accepts wait its maximum.
  /// </summary>
  public static TimeSpan Parse(IDictionary<string, string> headers)
  {
    if (headers == null)
    {
      return DefaultWait;
    }

    string raw = null;
    foreach (var header in headers)
    {
      if (header.Key.Equals("Retry-After", StringComparison.OrdinalIgnoreCase))
      {
        raw = header.Value?.Trim();
        break;
      }
    }

    if (string.IsNullOrEmpty(raw) || !WholeSeconds.IsMatch(raw))
    {
      return DefaultWait;
    }

    if (!long.TryParse(raw, out var seconds))
    {
      // digits only reach this point, so the value does not fit in a long
      return MaxDelay;
    }

    if (seconds <= 0)
    {
      return DefaultWait;
    }

    if (seconds > MaxDelay.TotalSeconds)
    {
      return MaxDelay;
    }

    return TimeSpan.FromSeconds(seconds);
  }
}

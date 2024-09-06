using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Algolia.Search.Clients;

namespace Algolia.Search.Http;

/// <summary>
/// Represent the user-agent header
/// </summary>
public class AlgoliaUserAgent
{
  private readonly IDictionary<string, string> _segments = new Dictionary<string, string>();

  // Get the dotnet runtime version
  private static readonly string DotnetVersion = Environment.Version.ToString();

  /// <summary>
  /// Create a new user-agent header
  /// </summary>
  /// <param name="clientName">The client name</param>
  /// <param name="clientVersion">The client version</param>
  public AlgoliaUserAgent(string clientName, string clientVersion)
  {
    AddSegment("Algolia for Csharp", $"({clientVersion})");
    AddSegment(clientName, $"({clientVersion})");
    AddSegment("Dotnet", $"({DotnetVersion})");
  }

  /// <summary>
  /// Add a new segment to the user-agent header
  /// </summary>
  /// <param name="key">The segment key</param>
  /// <param name="value">The segment value. Will be wrapped in parenthesis</param>
  /// <exception cref="ArgumentException"></exception>
  public void AddSegment(string key, string value)
  {
    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
    {
      throw new ArgumentException("Key and value must be set");
    }

    if (_segments.ContainsKey(key))
      throw new ArgumentException("Key must be unique");

    _segments.Add(new KeyValuePair<string, string>(key, value));
  }


  /// <summary>
  /// Create a valid user-agent header
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
    return string.Join("; ", _segments.Select(s => $"{s.Key} {s.Value}"));
  }
}

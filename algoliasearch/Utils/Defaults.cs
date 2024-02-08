using System;

namespace Algolia.Search.Utils;

/// <summary>
/// Holding all default values of the library
/// </summary>
internal static class Defaults
{
  /// <summary>
  /// Read timeout
  /// </summary>
  public static TimeSpan ReadTimeout = TimeSpan.FromSeconds(5);

  /// <summary>
  /// Write timeout
  /// </summary>
  public static TimeSpan WriteTimeout = TimeSpan.FromSeconds(30);

  /// <summary>
  /// Connect timeout
  /// </summary>
  public static TimeSpan ConnectTimeout = TimeSpan.FromSeconds(2);

  public const string AcceptHeader = "Accept";
  public const string AlgoliaApplicationHeader = "X-Algolia-Application-Id";
  public const string AlgoliaApiKeyHeader = "X-Algolia-API-Key";
  public const string UserAgentHeader = "User-Agent";
  public const string Connection = "Connection";
  public const string KeepAlive = "keep-alive";
  public const string ContentType = "Content-Type";
  public const string ApplicationJson = "application/json; charset=utf-8";
  public const string GzipEncoding = "gzip";
}

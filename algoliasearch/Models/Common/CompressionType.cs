namespace Algolia.Search.Models.Common;

/// <summary>
/// Compression type for outgoing HTTP requests
/// </summary>
public enum CompressionType
{
  /// <summary>
  /// No compression
  /// </summary>
  None = 1,

  /// <summary>
  /// GZip Compression. Only supported by Search API.
  /// </summary>
  Gzip = 2
}

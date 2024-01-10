namespace Algolia.Search.Models
{
  /// <summary>
  /// Compression type for outgoing HTTP requests
  /// </summary>
  public enum CompressionType
  {
    /// <summary>
    /// No compression
    /// </summary>
    NONE = 1,

    /// <summary>
    /// GZip Compression. Only supported by Search API.
    /// </summary>
    GZIP = 2
  }
}

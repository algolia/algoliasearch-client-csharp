namespace Algolia.Search.Utils;

/// <summary>
/// Optional configuration for chunked helpers that batch records and poll for task completion.
/// </summary>
public class ChunkedHelperOptions
{
  /// <summary>
  /// Maximum number of retries when polling for task completion. Defaults to <see cref="RetryHelper.DefaultMaxRetries"/>.
  /// </summary>
  public int MaxRetries { get; set; } = RetryHelper.DefaultMaxRetries;
}

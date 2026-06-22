namespace Algolia.Search.Utils;

/// <summary>
/// Optional configuration for chunked helpers that batch records and poll for task completion.
/// </summary>
public class ChunkedHelperOptions
{
  /// <summary>
  /// Default maximum number of retries used by <c>ReplaceAllObjects</c>.
  /// </summary>
  public const int DefaultReplaceAllObjectsMaxRetries = 800;

  /// <summary>
  /// Maximum number of retries when polling for task completion. Defaults to <see cref="RetryHelper.DefaultMaxRetries"/>.
  /// </summary>
  public int MaxRetries { get; set; } = RetryHelper.DefaultMaxRetries;
}

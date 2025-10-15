using System;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Exceptions;

namespace Algolia.Search.Utils;

/// <summary>
/// A helper class to retry operations
/// </summary>
public static class RetryHelper
{
  /// <summary>
  /// The default maximum number of retries
  /// </summary>
  public const int DefaultMaxRetries = 50;

  /// <summary>
  /// Retry the given function until the validation function returns true or the maximum number of retries is reached
  /// </summary>
  /// <typeparam name="T">The type of the function's return value</typeparam>
  /// <param name="func">The function to retry</param>
  /// <param name="validate">The validation function</param>
  /// <param name="maxRetries">The maximum number of retries</param>
  /// <param name="timeout">A function that takes the retry count and returns the timeout in milliseconds before the next retry</param>
  /// <param name="ct">A cancellation token to cancel the operation</param>
  /// <returns>The result of the function if the validation function returns true</returns>
  /// <exception cref="AlgoliaException">Thrown if the maximum number of retries is reached</exception>
  public static async Task<T> RetryUntil<T>(
    Func<Task<T>> func,
    Func<T, bool> validate,
    int maxRetries = DefaultMaxRetries,
    Func<int, int> timeout = null,
    CancellationToken ct = default
  )
  {
    timeout ??= NextDelay;

    var retryCount = 0;
    while (retryCount < maxRetries)
    {
      var resp = await func().ConfigureAwait(false);
      if (validate(resp))
      {
        return resp;
      }

      await Task.Delay(timeout(retryCount), ct).ConfigureAwait(false);
      retryCount++;
    }

    throw new AlgoliaException(
      "The maximum number of retries exceeded. (" + (retryCount + 1) + "/" + maxRetries + ")"
    );
  }

  private static int NextDelay(int retryCount)
  {
    return Math.Min(retryCount * 200, 5000);
  }
}

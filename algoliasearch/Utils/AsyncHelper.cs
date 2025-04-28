using System;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Utils;

/// <summary>
/// Helper class to run async methods synchronously.
/// </summary>
internal static class AsyncHelper
{
  private static readonly TaskFactory TaskFactory = new(
    CancellationToken.None,
    TaskCreationOptions.None,
    TaskContinuationOptions.None,
    TaskScheduler.Default
  );

  internal static TResult RunSync<TResult>(Func<Task<TResult>> func) =>
    TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();

  internal static void RunSync(Func<Task> func) =>
    TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
}

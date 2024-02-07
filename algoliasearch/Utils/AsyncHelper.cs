using System;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Utils
{
  internal static class AsyncHelper
  {
    private static readonly TaskFactory TaskFactory = new
        TaskFactory(CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);

    internal static TResult RunSync<TResult>(Func<Task<TResult>> func)
        => TaskFactory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();

    internal static void RunSync(Func<Task> func)
        => TaskFactory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
  }
}

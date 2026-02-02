using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Algolia.Search.Clients;
using Algolia.Search.Http;

[assembly: InternalsVisibleTo("Algolia.Search.Tests")]
[assembly: InternalsVisibleTo("Algolia.Search.IntegrationTests")]

namespace Algolia.Search.Transport;

/// <summary>
/// Retry strategy logic in case of error/timeout
/// </summary>
internal class RetryStrategy
{
  /// <summary>
  /// Hosts that will be used by the strategy
  /// Could be default hosts or custom hosts
  /// </summary>
  private readonly List<StatefulHost> _hosts;

  /// <summary>
  /// The synchronization lock for each set RetryStrategy/Transport/Client
  /// </summary>
  /// <returns></returns>
  private readonly object _lock = new();

  /// <summary>
  /// Default constructor
  /// </summary>
  /// <param name="config">Client's configuration</param>
  public RetryStrategy(AlgoliaConfig config)
  {
    _hosts = config.CustomHosts ?? config.DefaultHosts;
  }

  /// <summary>
  /// Returns the tryable host regarding the retry strategy
  /// </summary>
  /// <param name="callType">The Call type <see cref="CallType"/></param>
  public IEnumerable<StatefulHost> GetTryableHost(CallType callType)
  {
    lock (_lock)
    {
      ResetExpiredHosts();

      if (_hosts.Any(h => h.Up && h.Accept.HasFlag(callType)))
      {
        return _hosts.Where(h => h.Up && h.Accept.HasFlag(callType));
      }

      foreach (var host in _hosts.Where(h => h.Accept.HasFlag(callType)))
      {
        Reset(host);
      }

      return _hosts;
    }
  }

  /// <summary>
  /// Update host's state
  /// </summary>
  /// <param name="tryableHost">A stateful host</param>
  /// <param name="response">Algolia's API response</param>
  /// <returns></returns>
  public RetryOutcomeType Decide(StatefulHost tryableHost, AlgoliaHttpResponse response)
  {
    lock (_lock)
    {
      if (!response.IsTimedOut && IsSuccess(response))
      {
        Reset(tryableHost);
        return RetryOutcomeType.Success;
      }

      if (!response.IsTimedOut && IsRetryable(response))
      {
        tryableHost.Up = false;
        tryableHost.LastUse = DateTime.UtcNow;
        return RetryOutcomeType.Retry;
      }

      if (response.IsTimedOut)
      {
        tryableHost.Up = true;
        tryableHost.LastUse = DateTime.UtcNow;
        tryableHost.RetryCount++;
        return RetryOutcomeType.Retry;
      }

      return RetryOutcomeType.Failure;
    }
  }

  /// <summary>
  ///  Tells if the response is a success or not
  /// </summary>
  /// <param name="response">Algolia's API response</param>
  /// <returns></returns>
  private static bool IsSuccess(AlgoliaHttpResponse response)
  {
    return (int)Math.Floor((decimal)response.HttpStatusCode / 100) == 2;
  }

  /// <summary>
  ///  Tells if the response is retryable or not
  /// </summary>
  /// <param name="response">Algolia's API response</param>
  /// <returns></returns>
  private static bool IsRetryable(AlgoliaHttpResponse response)
  {
    var isRetryableHttpCode =
      (int)Math.Floor((decimal)response.HttpStatusCode / 100) != 2
      && (int)Math.Floor((decimal)response.HttpStatusCode / 100) != 4;

    return isRetryableHttpCode || response.IsNetworkError;
  }

  /// <summary>
  /// Reset the given host
  /// </summary>
  /// <param name="host"></param>
  private static void Reset(StatefulHost host)
  {
    host.Up = true;
    host.RetryCount = 0;
    host.LastUse = DateTime.UtcNow;
  }

  /// <summary>
  /// Reset down host after 5 minutes
  /// </summary>
  private void ResetExpiredHosts()
  {
    foreach (var host in _hosts)
    {
      if (!host.Up && DateTime.UtcNow.Subtract(host.LastUse).Minutes > 5)
      {
        Reset(host);
      }
    }
  }
}

/// <summary>
/// Retry strategy outcome values
/// </summary>
public enum RetryOutcomeType
{
  /// <summary>
  /// Success
  /// </summary>
  Success,

  /// <summary>
  /// Retry the call
  /// </summary>
  Retry,

  /// <summary>
  /// Call failed error
  /// </summary>
  Failure,
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Http;

/// <summary>
/// Interface that allow users to inject their custom http requester
/// Don't use directly, use AlgoliaClient to make request with the retry strategy
/// </summary>
public interface IHttpRequester
{
  /// <summary>
  /// Sends the HTTP request
  /// </summary>
  /// <param name="request">Request object</param>
  /// <param name="requestTimeout">Request timeout</param>
  /// <param name="connectTimeout">Connect timeout</param>
  /// <param name="ct">Optional cancellation token</param>
  /// <returns></returns>
  Task<AlgoliaHttpResponse> SendRequestAsync(
    Request request,
    TimeSpan requestTimeout,
    TimeSpan connectTimeout,
    CancellationToken ct = default
  );
}

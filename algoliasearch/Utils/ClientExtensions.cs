using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Clients;
using Algolia.Search.Exceptions;
using Algolia.Search.Http;
using Algolia.Search.Models.Search;

namespace Algolia.Search.Utils;

/// <summary>
/// A tool class to help with common tasks
/// </summary>
public static class ClientExtensions
{
  private const int DefaultMaxRetries = 50;

  /// <summary>
  /// Wait for a task to complete with `indexName` and `taskID`.
  /// </summary>
  /// <param name="client">Algolia Search Client instance</param>
  /// <param name="indexName">The `indexName` where the operation was performed.</param>
  /// <param name="taskId">The `taskID` returned in the method response.</param>
  /// <param name="maxRetries">The maximum number of retry. 50 by default. (optional)</param>
  /// <param name="timeout">The function to decide how long to wait between retries. Math.Min(retryCount * 200, 5000) by default. (optional)</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be merged with the transporter requestOptions. (optional)</param>
  /// <param name="ct">Cancellation token (optional)</param>
  public static async Task<GetTaskResponse> WaitForTaskAsync(this SearchClient client, string indexName, long taskId,
    int maxRetries = DefaultMaxRetries, Func<int, int> timeout = null, RequestOptions requestOptions = null,
    CancellationToken ct = default)
  {
    return await RetryUntil(
      async () => await client.GetTaskAsync(indexName, taskId, requestOptions, ct),
      resp => resp.Status == Models.Search.TaskStatus.Published, maxRetries, timeout, ct).ConfigureAwait(false);
  }

  /// <summary>
  /// Wait for a task to complete with `indexName` and `taskID`. (Synchronous version)
  /// </summary>
  /// <param name="client">Algolia Search Client instance</param>
  /// <param name="indexName">The `indexName` where the operation was performed.</param>
  /// <param name="taskId">The `taskID` returned in the method response.</param>
  /// <param name="maxRetries">The maximum number of retry. 50 by default. (optional)</param>
  /// <param name="timeout">The function to decide how long to wait between retries. Math.Min(retryCount * 200, 5000) by default. (optional)</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be merged with the transporter requestOptions. (optional)</param>
  /// <param name="ct">Cancellation token (optional)</param>
  public static GetTaskResponse WaitForTask(this SearchClient client, string indexName, long taskId,
    int maxRetries = DefaultMaxRetries, Func<int, int> timeout = null, RequestOptions requestOptions = null,
    CancellationToken ct = default) =>
    AsyncHelper.RunSync(() =>
      client.WaitForTaskAsync(indexName, taskId, maxRetries, timeout, requestOptions, ct));

  /// <summary>
  /// Helper method that waits for an API key task to be processed.
  /// </summary>
  /// <param name="client">Algolia Search Client instance</param>
  /// <param name="operation">The `operation` that was done on a `key`.</param>
  /// <param name="key">The key that has been added, deleted or updated.</param>
  /// <param name="apiKey">Necessary to know if an `update` operation has been processed, compare fields of the response with it. (optional - mandatory if operation is UPDATE)</param>
  /// <param name="maxRetries">The maximum number of retry. 50 by default. (optional)</param>
  /// <param name="timeout">The function to decide how long to wait between retries. Math.Min(retryCount * 200, 5000) by default. (optional)</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be merged with the transporter requestOptions. (optional)</param>
  /// <param name="ct">Cancellation token (optional)</param>
  public static async Task<GetApiKeyResponse> WaitForApiKeyAsync(this SearchClient client,
    ApiKeyOperation operation, string key,
    ApiKey apiKey = default, int maxRetries = DefaultMaxRetries, Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default)
  {
    if (operation == ApiKeyOperation.Update)
    {
      if (apiKey == null)
      {
        throw new AlgoliaException("`ApiKey` is required when waiting for an `update` operation.");
      }

      return await RetryUntil(() => client.GetApiKeyAsync(key, requestOptions, ct),
        resp =>
        {
          var apiKeyResponse = new ApiKey
          {
            Acl = resp.Acl,
            Description = resp.Description,
            Indexes = resp.Indexes,
            Referers = resp.Referers,
            Validity = resp.Validity,
            QueryParameters = resp.QueryParameters,
            MaxHitsPerQuery = resp.MaxHitsPerQuery,
            MaxQueriesPerIPPerHour = resp.MaxQueriesPerIPPerHour
          };
          return apiKeyResponse.Equals(apiKey);
        }, maxRetries, timeout, ct).ConfigureAwait(false);
    }

    var addedKey = new GetApiKeyResponse();

    // check the status of the getApiKey method
    await RetryUntil(async () =>
      {
        try
        {
          addedKey = await client.GetApiKeyAsync(key, requestOptions, ct).ConfigureAwait(false);
          // magic number to signify we found the key
          return -2;
        }
        catch (AlgoliaApiException e)
        {
          return e.HttpErrorCode;
        }
      }, (status) =>
      {
        return operation switch
        {
          ApiKeyOperation.Add =>
            // stop either when the key is created or when we don't receive 404
            status is -2 or not 404 and not 0,
          ApiKeyOperation.Delete =>
            // stop when the key is not found
            status == 404,
          _ => false
        };
      },
      maxRetries, timeout, ct);
    return addedKey;
  }

  /// <summary>
  /// Helper method that waits for an API key task to be processed. (Synchronous version)
  /// </summary>
  /// <param name="client">Algolia Search Client instance</param>
  /// <param name="operation">The `operation` that was done on a `key`.</param>
  /// <param name="key">The key that has been added, deleted or updated.</param>
  /// <param name="apiKey">Necessary to know if an `update` operation has been processed, compare fields of the response with it. (optional - mandatory if operation is UPDATE)</param>
  /// <param name="maxRetries">The maximum number of retry. 50 by default. (optional)</param>
  /// <param name="timeout">The function to decide how long to wait between retries. Math.Min(retryCount * 200, 5000) by default. (optional)</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be merged with the transporter requestOptions. (optional)</param>
  /// <param name="ct">Cancellation token (optional)</param>
  public static GetApiKeyResponse WaitForApiKey(this SearchClient client,
    ApiKeyOperation operation, string key,
    ApiKey apiKey = default, int maxRetries = DefaultMaxRetries, Func<int, int> timeout = null, RequestOptions requestOptions = null,
    CancellationToken ct = default) =>
    AsyncHelper.RunSync(() => client.WaitForApiKeyAsync(operation, key, apiKey, maxRetries, timeout, requestOptions, ct));


  /// <summary>
  /// Iterate on the `browse` method of the client to allow aggregating objects of an index.
  /// </summary>
  /// <param name="client"></param>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="browseParams">The `browse` parameters.</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be forwarded to the `browse` method and merged with the transporter requestOptions.</param>
  /// <typeparam name="T">The model of the record</typeparam>
  public static async Task<IEnumerable<T>> BrowseObjectsAsync<T>(this SearchClient client, string indexName,
    BrowseParamsObject browseParams,
    RequestOptions requestOptions = null)
  {
    browseParams.HitsPerPage = 1000;
    var all = await CreateIterable<BrowseResponse<T>>(async prevResp =>
    {
      browseParams.Cursor = prevResp?.Cursor;
      return await client.BrowseAsync<T>(indexName, new BrowseParams(browseParams), requestOptions);
    }, resp => resp is { Cursor: null }).ConfigureAwait(false);

    return all.SelectMany(u => u.Hits);
  }


  /// <summary>
  /// Iterate on the `browse` method of the client to allow aggregating objects of an index. (Synchronous version)
  /// </summary>
  /// <param name="client"></param>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="browseParams">The `browse` parameters.</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be forwarded to the `browse` method and merged with the transporter requestOptions.</param>
  /// <typeparam name="T">The model of the record</typeparam>
  public static IEnumerable<T> BrowseObjects<T>(this SearchClient client, string indexName,
    BrowseParamsObject browseParams,
    RequestOptions requestOptions = null) =>
    AsyncHelper.RunSync(() => client.BrowseObjectsAsync<T>(indexName, browseParams, requestOptions));


  /// <summary>
  /// Iterate on the `SearchRules` method of the client to allow aggregating rules of an index.
  /// </summary>
  /// <param name="client"></param>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="searchRulesParams">The `SearchRules` parameters</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be forwarded to the `searchRules` method and merged with the transporter requestOptions.</param>
  public static async Task<IEnumerable<Rule>> BrowseRulesAsync(this SearchClient client, string indexName,
    SearchRulesParams searchRulesParams,
    RequestOptions requestOptions = null)
  {
    const int hitsPerPage = 1000;
    searchRulesParams.HitsPerPage = hitsPerPage;

    var all = await CreateIterable<Tuple<SearchRulesResponse, int>>(async (prevResp) =>
    {
      var page = prevResp?.Item2 ?? 0;
      var searchSynonymsResponse = await client.SearchRulesAsync(indexName, searchRulesParams, requestOptions);
      return new Tuple<SearchRulesResponse, int>(searchSynonymsResponse, page + 1);
    }, resp => resp?.Item1 is { NbHits: < hitsPerPage }).ConfigureAwait(false);

    return all.SelectMany(u => u.Item1.Hits);
  }

  /// <summary>
  /// Iterate on the `SearchRules` method of the client to allow aggregating rules of an index. (Synchronous version)
  /// </summary>
  /// <param name="client"></param>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="searchRulesParams">The `SearchRules` parameters</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be forwarded to the `searchRules` method and merged with the transporter requestOptions.</param>
  public static IEnumerable<Rule> BrowseRules(this SearchClient client, string indexName,
    SearchRulesParams searchRulesParams,
    RequestOptions requestOptions = null) =>
    AsyncHelper.RunSync(() => client.BrowseRulesAsync(indexName, searchRulesParams, requestOptions));


  /// <summary>
  /// Iterate on the `SearchSynonyms` method of the client to allow aggregating rules of an index.
  /// </summary>
  /// <param name="client"></param>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="synonymsParams">The `SearchSynonyms` parameters.</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be forwarded to the `searchSynonyms` method and merged with the transporter requestOptions.</param>
  public static async Task<IEnumerable<SynonymHit>> BrowseSynonymsAsync(this SearchClient client, string indexName,
    SearchSynonymsParams synonymsParams,
    RequestOptions requestOptions = null)
  {
    const int hitsPerPage = 1000;
    synonymsParams.HitsPerPage = hitsPerPage;
    var all = await CreateIterable<Tuple<SearchSynonymsResponse, int>>(async (prevResp) =>
    {
      synonymsParams.Page = prevResp?.Item2 ?? 0;
      var searchSynonymsResponse = await client.SearchSynonymsAsync(indexName, synonymsParams, requestOptions);
      return new Tuple<SearchSynonymsResponse, int>(searchSynonymsResponse, (prevResp?.Item2 ?? 0) + 1);
    }, resp => resp?.Item1 is { NbHits: < hitsPerPage }).ConfigureAwait(false);

    return all.SelectMany(u => u.Item1.Hits);
  }

  /// <summary>
  /// Iterate on the `SearchSynonyms` method of the client to allow aggregating rules of an index. (Synchronous version)
  /// </summary>
  /// <param name="client"></param>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="synonymsParams">The `SearchSynonyms` parameters.</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be forwarded to the `searchSynonyms` method and merged with the transporter requestOptions.</param>
  public static IEnumerable<SynonymHit> BrowseSynonyms(this SearchClient client, string indexName,
    SearchSynonymsParams synonymsParams,
    RequestOptions requestOptions = null) =>
    AsyncHelper.RunSync(() => client.BrowseSynonymsAsync(indexName, synonymsParams, requestOptions));


  /// <summary>
  /// Executes a synchronous search for the provided search requests, with certainty that we will only request Algolia records (hits). Results will be received in the same order as the queries.
  /// </summary>
  /// <param name="client">Search client</param>
  /// <param name="requests">A list of search requests to be executed.</param>
  /// <param name="searchStrategy">The search strategy to be employed during the search. (optional)</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  /// <exception cref="ArgumentException">Thrown when arguments are not correct</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaApiException">Thrown when the API call was rejected by Algolia</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaUnreachableHostException">Thrown when the client failed to call the endpoint</exception>
  /// <returns>Task of List{SearchResponse{T}}</returns>
  public static async Task<List<SearchResponse<T>>> SearchForHitsAsync<T>(this SearchClient client,
    IEnumerable<SearchForHits> requests, SearchStrategy? searchStrategy = null, RequestOptions options = null,
    CancellationToken cancellationToken = default)
  {
    var queries = requests.Select(t => new SearchQuery(t)).ToList();
    var searchMethod = new SearchMethodParams(queries) { Strategy = searchStrategy };
    var searchResponses = await client.SearchAsync<T>(searchMethod, options, cancellationToken);
    return searchResponses.Results.Where(x => x.IsSearchResponse()).Select(x => x.AsSearchResponse()).ToList();
  }

  /// <summary>
  /// Executes a synchronous search for the provided search requests, with certainty that we will only request Algolia records (hits). Results will be received in the same order as the queries. (Synchronous version)
  /// </summary>
  /// <param name="client">Search client</param>
  /// <param name="requests">A list of search requests to be executed.</param>
  /// <param name="searchStrategy">The search strategy to be employed during the search. (optional)</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  /// <exception cref="ArgumentException">Thrown when arguments are not correct</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaApiException">Thrown when the API call was rejected by Algolia</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaUnreachableHostException">Thrown when the client failed to call the endpoint</exception>
  /// <returns>Task of List{SearchResponse{T}}</returns>
  public static List<SearchResponse<T>> SearchForHits<T>(this SearchClient client,
    IEnumerable<SearchForHits> requests, SearchStrategy? searchStrategy = null, RequestOptions options = null,
    CancellationToken cancellationToken = default) =>
    AsyncHelper.RunSync(() =>
      client.SearchForHitsAsync<T>(requests, searchStrategy, options, cancellationToken));


  /// <summary>
  /// Executes a synchronous search for the provided search requests, with certainty that we will only request Algolia facets. Results will be received in the same order as the queries.
  /// </summary>
  /// <param name="client">Search client</param>
  /// <param name="requests">A list of search requests to be executed.</param>
  /// <param name="searchStrategy">The search strategy to be employed during the search. (optional)</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  /// <exception cref="ArgumentException">Thrown when arguments are not correct</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaApiException">Thrown when the API call was rejected by Algolia</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaUnreachableHostException">Thrown when the client failed to call the endpoint</exception>
  /// <returns>Task of List{SearchResponse{T}}</returns>
  public static async Task<List<SearchForFacetValuesResponse>> SearchForFacetsAsync(this SearchClient client,
    IEnumerable<SearchForFacets> requests, SearchStrategy? searchStrategy, RequestOptions options = null,
    CancellationToken cancellationToken = default)
  {
    var queries = requests.Select(t => new SearchQuery(t)).ToList();
    var searchMethod = new SearchMethodParams(queries) { Strategy = searchStrategy };
    var searchResponses = await client.SearchAsync<object>(searchMethod, options, cancellationToken);
    return searchResponses.Results.Where(x => x.IsSearchForFacetValuesResponse())
      .Select(x => x.AsSearchForFacetValuesResponse()).ToList();
  }

  /// <summary>
  /// Executes a synchronous search for the provided search requests, with certainty that we will only request Algolia facets. Results will be received in the same order as the queries.
  /// </summary>
  /// <param name="client">Search client</param>
  /// <param name="requests">A list of search requests to be executed.</param>
  /// <param name="searchStrategy">The search strategy to be employed during the search. (optional)</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  /// <exception cref="ArgumentException">Thrown when arguments are not correct</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaApiException">Thrown when the API call was rejected by Algolia</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaUnreachableHostException">Thrown when the client failed to call the endpoint</exception>
  /// <returns>Task of List{SearchResponse{T}}</returns>
  public static List<SearchForFacetValuesResponse> SearchForFacets(this SearchClient client,
    IEnumerable<SearchForFacets> requests, SearchStrategy? searchStrategy, RequestOptions options = null,
    CancellationToken cancellationToken = default) =>
    AsyncHelper.RunSync(() => client.SearchForFacetsAsync(requests, searchStrategy, options, cancellationToken));

  private static async Task<T> RetryUntil<T>(Func<Task<T>> func, Func<T, bool> validate,
    int maxRetries = DefaultMaxRetries, Func<int, int> timeout = null, CancellationToken ct = default)
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
      "The maximum number of retries exceeded. (" + (retryCount + 1) + "/" + maxRetries + ")");
  }

  private static int NextDelay(int retryCount)
  {
    return Math.Min(retryCount * 200, 5000);
  }

  private static async Task<List<TU>> CreateIterable<TU>(Func<TU, Task<TU>> executeQuery,
    Func<TU, bool> stopCondition)
  {
    var responses = new List<TU>();
    var current = default(TU);
    do
    {
      var response = await executeQuery(current).ConfigureAwait(false);
      current = response;
      responses.Add(response);
    } while (!stopCondition(current));

    return responses;
  }
}

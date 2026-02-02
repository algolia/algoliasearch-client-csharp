using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Exceptions;
using Algolia.Search.Http;
using Algolia.Search.Models.Search;
using Algolia.Search.Serializer;
using Algolia.Search.Utils;
using Action = Algolia.Search.Models.Search.Action;

namespace Algolia.Search.Clients;

public partial interface ISearchClient
{
  /// <summary>
  /// Wait for a task to complete with `indexName` and `taskID`.
  /// </summary>
  /// <param name="indexName">The `indexName` where the operation was performed.</param>
  /// <param name="taskId">The `taskID` returned in the method response.</param>
  /// <param name="maxRetries">The maximum number of retry. 50 by default. (optional)</param>
  /// <param name="timeout">The function to decide how long to wait between retries. Math.Min(retryCount * 200, 5000) by default. (optional)</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be merged with the transporter requestOptions. (optional)</param>
  /// <param name="ct">Cancellation token (optional)</param>
  Task<GetTaskResponse> WaitForTaskAsync(
    string indexName,
    long taskId,
    int maxRetries = SearchClient.DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  );

  /// <inheritdoc cref="WaitForTaskAsync(string, long, int, Func{int, int}, RequestOptions, CancellationToken)"/>
  GetTaskResponse WaitForTask(
    string indexName,
    long taskId,
    int maxRetries = SearchClient.DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  );

  /// <summary>
  /// Wait for an application-level task to complete with `taskID`.
  /// </summary>
  /// <param name="taskId">The `taskID` returned in the method response.</param>
  /// <param name="maxRetries">The maximum number of retry. 50 by default. (optional)</param>
  /// <param name="timeout">The function to decide how long to wait between retries. Math.Min(retryCount * 200, 5000) by default. (optional)</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be merged with the transporter requestOptions. (optional)</param>
  /// <param name="ct">Cancellation token (optional)</param>
  Task<GetTaskResponse> WaitForAppTaskAsync(
    long taskId,
    int maxRetries = SearchClient.DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  );

  /// <inheritdoc cref="WaitForAppTaskAsync(long, int, Func{int, int}, RequestOptions, CancellationToken)"/>
  GetTaskResponse WaitForAppTask(
    long taskId,
    int maxRetries = SearchClient.DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  );

  /// <summary>
  /// Helper method that waits for an API key task to be processed.
  /// </summary>
  /// <param name="key">The key that has been added, deleted or updated.</param>
  /// <param name="operation">The `operation` that was done on a `key`.</param>
  /// <param name="apiKey">Necessary to know if an `update` operation has been processed, compare fields of the response with it. (optional - mandatory if operation is UPDATE)</param>
  /// <param name="maxRetries">The maximum number of retry. 50 by default. (optional)</param>
  /// <param name="timeout">The function to decide how long to wait between retries. Math.Min(retryCount * 200, 5000) by default. (optional)</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be merged with the transporter requestOptions. (optional)</param>
  /// <param name="ct">Cancellation token (optional)</param>
  Task<GetApiKeyResponse> WaitForApiKeyAsync(
    string key,
    ApiKeyOperation operation,
    ApiKey apiKey = default,
    int maxRetries = SearchClient.DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  );

  /// <inheritdoc cref="WaitForApiKeyAsync(string, ApiKeyOperation, ApiKey, int, Func{int, int}, RequestOptions, CancellationToken)"/>
  GetApiKeyResponse WaitForApiKey(
    string key,
    ApiKeyOperation operation,
    ApiKey apiKey = default,
    int maxRetries = SearchClient.DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  );

  /// <summary>
  /// Iterate on the `browse` method of the client to allow aggregating objects of an index.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="browseParams">The `browse` parameters.</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be forwarded to the `browse` method and merged with the transporter requestOptions.</param>
  /// <typeparam name="T">The model of the record</typeparam>
  Task<IEnumerable<T>> BrowseObjectsAsync<T>(
    string indexName,
    BrowseParamsObject browseParams,
    RequestOptions requestOptions = null
  );

  /// <inheritdoc cref="BrowseObjectsAsync{T}(string, BrowseParamsObject, RequestOptions)"/>
  IEnumerable<T> BrowseObjects<T>(
    string indexName,
    BrowseParamsObject browseParams,
    RequestOptions requestOptions = null
  );

  /// <summary>
  /// Iterate on the `SearchRules` method of the client to allow aggregating rules of an index.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="searchRulesParams">The `SearchRules` parameters</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be forwarded to the `searchRules` method and merged with the transporter requestOptions.</param>
  Task<IEnumerable<Rule>> BrowseRulesAsync(
    string indexName,
    SearchRulesParams searchRulesParams,
    RequestOptions requestOptions = null
  );

  /// <inheritdoc cref="BrowseRulesAsync(string, SearchRulesParams, RequestOptions)"/>
  IEnumerable<Rule> BrowseRules(
    string indexName,
    SearchRulesParams searchRulesParams,
    RequestOptions requestOptions = null
  );

  /// <summary>
  /// Iterate on the `SearchSynonyms` method of the client to allow aggregating rules of an index.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="synonymsParams">The `SearchSynonyms` parameters.</param>
  /// <param name="requestOptions">The requestOptions to send along with the query, they will be forwarded to the `searchSynonyms` method and merged with the transporter requestOptions.</param>
  Task<IEnumerable<SynonymHit>> BrowseSynonymsAsync(
    string indexName,
    SearchSynonymsParams synonymsParams,
    RequestOptions requestOptions = null
  );

  /// <inheritdoc cref="BrowseSynonymsAsync(string, SearchSynonymsParams, RequestOptions)"/>
  IEnumerable<SynonymHit> BrowseSynonyms(
    string indexName,
    SearchSynonymsParams synonymsParams,
    RequestOptions requestOptions = null
  );

  /// <summary>
  /// Generate a virtual API Key without any call to the server.
  /// </summary>
  /// <param name="parentApiKey">Parent API Key</param>
  /// <param name="restriction">Restriction to add the key</param>
  /// <returns></returns>
  string GenerateSecuredApiKey(string parentApiKey, SecuredApiKeyRestrictions restriction);

  /// <summary>
  ///  Get the remaining validity of a key generated by `GenerateSecuredApiKey`.
  /// </summary>
  /// <param name="securedApiKey">The secured API Key</param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="AlgoliaException"></exception>
  TimeSpan GetSecuredApiKeyRemainingValidity(string securedApiKey);

  /// <summary>
  /// Executes a synchronous search for the provided search requests, with certainty that we will only request Algolia records (hits). Results will be received in the same order as the queries.
  /// </summary>
  /// <param name="requests">A list of search requests to be executed.</param>
  /// <param name="searchStrategy">The search strategy to be employed during the search. (optional)</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  /// <exception cref="ArgumentException">Thrown when arguments are not correct</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaApiException">Thrown when the API call was rejected by Algolia</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaUnreachableHostException">Thrown when the client failed to call the endpoint</exception>
  /// <returns>Task of List{SearchResponse{T}}</returns>
  Task<List<SearchResponse<T>>> SearchForHitsAsync<T>(
    IEnumerable<SearchForHits> requests,
    SearchStrategy? searchStrategy = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <inheritdoc cref="SearchForHitsAsync{T}(IEnumerable{SearchForHits}, SearchStrategy?, RequestOptions, CancellationToken)"/>
  List<SearchResponse<T>> SearchForHits<T>(
    IEnumerable<SearchForHits> requests,
    SearchStrategy? searchStrategy = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <summary>
  /// Executes a synchronous search for the provided search requests, with certainty that we will only request Algolia facets. Results will be received in the same order as the queries.
  /// </summary>
  /// <param name="requests">A list of search requests to be executed.</param>
  /// <param name="searchStrategy">The search strategy to be employed during the search. (optional)</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  /// <exception cref="ArgumentException">Thrown when arguments are not correct</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaApiException">Thrown when the API call was rejected by Algolia</exception>
  /// <exception cref="Algolia.Search.Exceptions.AlgoliaUnreachableHostException">Thrown when the client failed to call the endpoint</exception>
  /// <returns>Task of List{SearchResponse{T}}</returns>
  Task<List<SearchForFacetValuesResponse>> SearchForFacetsAsync(
    IEnumerable<SearchForFacets> requests,
    SearchStrategy? searchStrategy,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <inheritdoc cref="SearchForFacetsAsync(IEnumerable{SearchForFacets}, SearchStrategy?, RequestOptions, CancellationToken)"/>
  List<SearchForFacetValuesResponse> SearchForFacets(
    IEnumerable<SearchForFacets> requests,
    SearchStrategy? searchStrategy,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <summary>
  ///  Push a new set of objects and remove all previous ones. Settings, synonyms and query rules are untouched.
  /// Replace all objects in an index without any downtime. Internally, this method copies the existing index settings, synonyms and query rules and indexes all passed objects.
  /// Finally, the temporary one replaces the existing index.
  /// See https://api-clients-automation.netlify.app/docs/custom-helpers/#replaceallobjects for implementation details.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="objects">The list of `objects` to store in the given Algolia `indexName`.</param>
  /// <param name="batchSize">The size of the chunk of `objects`. The number of `batch` calls will be equal to `length(objects) / batchSize`. Defaults to 1000.</param>
  /// <param name="scopes"> The `scopes` to keep from the index. Defaults to ['settings', 'rules', 'synonyms'].</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  Task<ReplaceAllObjectsResponse> ReplaceAllObjectsAsync<T>(
    string indexName,
    IEnumerable<T> objects,
    int batchSize = 1000,
    List<ScopeType> scopes = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class;

  /// <inheritdoc cref="ReplaceAllObjectsAsync{T}(string, IEnumerable{T}, int, RequestOptions, CancellationToken)"/>
  ReplaceAllObjectsResponse ReplaceAllObjects<T>(
    string indexName,
    IEnumerable<T> objects,
    int batchSize = 1000,
    List<ScopeType> scopes = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class;

  /// <summary>
  /// Helper: Chunks the given `objects` list in subset of 1000 elements max in order to make it fit in `batch` requests.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="objects">The list of `objects` to store in the given Algolia `indexName`.</param>
  /// <param name="action">The `batch` `action` to perform on the given array of `objects`. Defaults to `addObject`.</param>
  /// <param name="waitForTasks">Whether or not we should wait until every `batch` tasks has been processed, this operation may slow the total execution time of this method but is more reliable. Defaults to `false`.</param>
  /// <param name="batchSize">The size of the chunk of `objects`. The number of `batch` calls will be equal to `length(objects) / batchSize`. Defaults to 1000.</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  /// <typeparam name="T"></typeparam>
  Task<List<BatchResponse>> ChunkedBatchAsync<T>(
    string indexName,
    IEnumerable<T> objects,
    Action action = Action.AddObject,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class;

  /// <inheritdoc cref="ChunkedBatchAsync{T}(string, IEnumerable{T}, Action, bool, int, RequestOptions, CancellationToken)"/>
  List<BatchResponse> ChunkedBatch<T>(
    string indexName,
    IEnumerable<T> objects,
    Action action = Action.AddObject,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class;

  /// <summary>
  /// Helper: Saves the given array of objects in the given index. The `chunkedBatch` helper is used under the hood, which creates a `batch` requests with at most 1000 objects in it.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="objects">The list of `objects` to store in the given Algolia `indexName`.</param>
  /// <param name="waitForTasks">Whether or not we should wait until every `batch` tasks has been processed, this operation may slow the total execution time of this method but is more reliable..</param>
  /// <param name="batchSize">The size of the chunk of `objects`. The number of `batch` calls will be equal to `length(objects) / batchSize`. Defaults to 1000.</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  /// <typeparam name="T"></typeparam>
  Task<List<BatchResponse>> SaveObjectsAsync<T>(
    string indexName,
    IEnumerable<T> objects,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class;

  /// <inheritdoc cref="SaveObjectsAsync{T}(string, IEnumerable{T}, RequestOptions, CancellationToken)"/>
  List<BatchResponse> SaveObjects<T>(
    string indexName,
    IEnumerable<T> objects,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class;

  /// <summary>
  /// Helper: Saves the given array of objects in the given index. The `chunkedBatch` helper is used under the hood, which creates a `batch` requests with at most 1000 objects in it.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="objects">The list of `objects` to store in the given Algolia `indexName`.</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  /// <typeparam name="T"></typeparam>
  Task<List<BatchResponse>> SaveObjectsAsync<T>(
    string indexName,
    IEnumerable<T> objects,
    RequestOptions options,
    CancellationToken cancellationToken = default
  )
    where T : class;

  /// <inheritdoc cref="SaveObjectsAsync{T}(string, IEnumerable{T}, RequestOptions, CancellationToken)"/>
  List<BatchResponse> SaveObjects<T>(
    string indexName,
    IEnumerable<T> objects,
    RequestOptions options,
    CancellationToken cancellationToken = default
  )
    where T : class;

  /// <summary>
  /// Helper: Deletes every records for the given objectIDs. The `chunkedBatch` helper is used under the hood, which creates a `batch` requests with at most 1000 objectIDs in it.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="objectIDs">The list of `objectIDs` to remove from the given Algolia `indexName`.</param>
  /// <param name="waitForTasks">Whether or not we should wait until every `batch` tasks has been processed, this operation may slow the total execution time of this method but is more reliable..</param>
  /// <param name="batchSize">The size of the chunk of `objects`. The number of `batch` calls will be equal to `length(objects) / batchSize`. Defaults to 1000.</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  Task<List<BatchResponse>> DeleteObjectsAsync(
    string indexName,
    IEnumerable<String> objectIDs,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <inheritdoc cref="DeleteObjectsAsync(string, IEnumerable{String}, RequestOptions, CancellationToken)"/>
  List<BatchResponse> DeleteObjects(
    string indexName,
    IEnumerable<String> objectIDs,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <summary>
  /// Helper: Replaces object content of all the given objects according to their respective `objectID` field. The `chunkedBatch` helper is used under the hood, which creates a `batch` requests with at most 1000 objects in it.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="objects">The list of `objects` to update in the given Algolia `indexName`.</param>
  /// <param name="createIfNotExists">To be provided if non-existing objects are passed, otherwise, the call will fail.</param>
  /// <param name="waitForTasks">Whether or not we should wait until every `batch` tasks has been processed, this operation may slow the total execution time of this method but is more reliable..</param>
  /// <param name="batchSize">The size of the chunk of `objects`. The number of `batch` calls will be equal to `length(objects) / batchSize`. Defaults to 1000.</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  Task<List<BatchResponse>> PartialUpdateObjectsAsync<T>(
    string indexName,
    IEnumerable<T> objects,
    bool createIfNotExists,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class;

  /// <inheritdoc cref="PartialUpdateObjectsAsync{T}(string, IEnumerable{T}, bool, RequestOptions, CancellationToken)"/>
  List<BatchResponse> PartialUpdateObjects<T>(
    string indexName,
    IEnumerable<T> objects,
    bool createIfNotExists,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class;

  /// <summary>
  /// Helper: Check if an index exists.
  /// </summary>
  /// <param name="indexName">The index in which to check.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  Task<bool> IndexExistsAsync(string indexName, CancellationToken cancellationToken = default);

  /// <inheritdoc cref="IndexExistsAsync(string, CancellationToken)"/>
  bool IndexExists(string indexName, CancellationToken cancellationToken = default);

  /// <summary>
  /// Helper: Similar to the `SaveObjects` method but requires a Push connector to be created first,
  /// in order to transform records before indexing them to Algolia.
  /// The ingestion region must have been provided at client instantiation.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="objects">The list of `objects` to store in the given Algolia `indexName`.</param>
  /// <param name="waitForTasks">Whether or not we should wait until every `batch` tasks has been processed, this operation may slow the total execution time of this method but is more reliable.</param>
  /// <param name="batchSize">The size of the chunk of `objects`. The number of `batch` calls will be equal to `length(objects) / batchSize`. Defaults to 1000.</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  Task<List<Algolia.Search.Models.Ingestion.WatchResponse>> SaveObjectsWithTransformationAsync(
    string indexName,
    IEnumerable<object> objects,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <inheritdoc cref="SaveObjectsWithTransformationAsync(string, IEnumerable{object}, bool, int, RequestOptions, CancellationToken)"/>
  List<Algolia.Search.Models.Ingestion.WatchResponse> SaveObjectsWithTransformation(
    string indexName,
    IEnumerable<object> objects,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <summary>
  /// Helper: Similar to the `PartialUpdateObjects` method but requires a Push connector to be created first,
  /// in order to transform records before indexing them to Algolia.
  /// The ingestion region must have been provided at client instantiation.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="objects">The list of `objects` to update in the given Algolia `indexName`.</param>
  /// <param name="createIfNotExists">To be provided if non-existing objects are passed, otherwise, the call will fail.</param>
  /// <param name="waitForTasks">Whether or not we should wait until every `batch` tasks has been processed, this operation may slow the total execution time of this method but is more reliable.</param>
  /// <param name="batchSize">The size of the chunk of `objects`. The number of `batch` calls will be equal to `length(objects) / batchSize`. Defaults to 1000.</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  Task<
    List<Algolia.Search.Models.Ingestion.WatchResponse>
  > PartialUpdateObjectsWithTransformationAsync(
    string indexName,
    IEnumerable<object> objects,
    bool createIfNotExists = true,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <inheritdoc cref="PartialUpdateObjectsWithTransformationAsync(string, IEnumerable{object}, bool, bool, int, RequestOptions, CancellationToken)"/>
  List<Algolia.Search.Models.Ingestion.WatchResponse> PartialUpdateObjectsWithTransformation(
    string indexName,
    IEnumerable<object> objects,
    bool createIfNotExists = true,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <summary>
  /// Helper: Similar to the `ReplaceAllObjects` method but requires a Push connector to be created first,
  /// in order to transform records before indexing them to Algolia.
  /// The ingestion region must have been provided at client instantiation.
  /// A temporary index is created during this process in order to backup your data.
  /// </summary>
  /// <param name="indexName">The index in which to perform the request.</param>
  /// <param name="objects">The list of `objects` to store in the given Algolia `indexName`.</param>
  /// <param name="batchSize">The size of the chunk of `objects`. The number of `batch` calls will be equal to `objects.length / batchSize`. Defaults to 1000.</param>
  /// <param name="scopes">The `scopes` to keep from the index. Defaults to ['settings', 'rules', 'synonyms'].</param>
  /// <param name="options">Add extra http header or query parameters to Algolia.</param>
  /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
  Task<ReplaceAllObjectsWithTransformationResponse> ReplaceAllObjectsWithTransformationAsync(
    string indexName,
    IEnumerable<object> objects,
    int batchSize = 1000,
    List<ScopeType> scopes = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );

  /// <inheritdoc cref="ReplaceAllObjectsWithTransformationAsync(string, IEnumerable{object}, int, List{ScopeType}, RequestOptions, CancellationToken)"/>
  ReplaceAllObjectsWithTransformationResponse ReplaceAllObjectsWithTransformation(
    string indexName,
    IEnumerable<object> objects,
    int batchSize = 1000,
    List<ScopeType> scopes = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  );
}

public partial class SearchClient : ISearchClient
{
  /// <summary>
  /// The default maximum number of retries for search operations.
  /// </summary>
  public const int DefaultMaxRetries = RetryHelper.DefaultMaxRetries;

  /// <inheritdoc/>
  public async Task<GetTaskResponse> WaitForTaskAsync(
    string indexName,
    long taskId,
    int maxRetries = DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  ) =>
    await RetryHelper
      .RetryUntil(
        async () => await GetTaskAsync(indexName, taskId, requestOptions, ct),
        resp => resp.Status == Models.Search.TaskStatus.Published,
        maxRetries,
        timeout,
        ct
      )
      .ConfigureAwait(false);

  /// <inheritdoc/>
  public GetTaskResponse WaitForTask(
    string indexName,
    long taskId,
    int maxRetries = DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  ) =>
    AsyncHelper.RunSync(() =>
      WaitForTaskAsync(indexName, taskId, maxRetries, timeout, requestOptions, ct)
    );

  /// <inheritdoc/>
  public async Task<GetTaskResponse> WaitForAppTaskAsync(
    long taskId,
    int maxRetries = DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  ) =>
    await RetryHelper
      .RetryUntil(
        async () => await GetAppTaskAsync(taskId, requestOptions, ct),
        resp => resp.Status == Models.Search.TaskStatus.Published,
        maxRetries,
        timeout,
        ct
      )
      .ConfigureAwait(false);

  /// <inheritdoc/>
  public GetTaskResponse WaitForAppTask(
    long taskId,
    int maxRetries = DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  ) =>
    AsyncHelper.RunSync(() => WaitForAppTaskAsync(taskId, maxRetries, timeout, requestOptions, ct));

  /// <inheritdoc/>
  public async Task<GetApiKeyResponse> WaitForApiKeyAsync(
    string key,
    ApiKeyOperation operation,
    ApiKey apiKey = default,
    int maxRetries = DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  )
  {
    if (operation == ApiKeyOperation.Update)
    {
      if (apiKey == null)
      {
        throw new AlgoliaException("`ApiKey` is required when waiting for an `update` operation.");
      }

      return await RetryHelper
        .RetryUntil(
          () => GetApiKeyAsync(key, requestOptions, ct),
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
              MaxQueriesPerIPPerHour = resp.MaxQueriesPerIPPerHour,
            };
            return apiKeyResponse.Equals(apiKey);
          },
          maxRetries,
          timeout,
          ct
        )
        .ConfigureAwait(false);
    }

    return await RetryHelper.RetryUntil(
      async () =>
      {
        try
        {
          return await GetApiKeyAsync(key, requestOptions, ct).ConfigureAwait(false);
        }
        catch (AlgoliaApiException e)
        {
          if (e.HttpErrorCode is 404)
          {
            return null;
          }

          throw;
        }
      },
      (response) =>
      {
        return operation switch
        {
          ApiKeyOperation.Add =>
          // stop either when the key is created or when we don't receive 404
          response is not null,
          ApiKeyOperation.Delete =>
          // stop when the key is not found
          response is null,
          _ => false,
        };
      },
      maxRetries,
      timeout,
      ct
    );
  }

  /// <inheritdoc/>
  public GetApiKeyResponse WaitForApiKey(
    string key,
    ApiKeyOperation operation,
    ApiKey apiKey = default,
    int maxRetries = DefaultMaxRetries,
    Func<int, int> timeout = null,
    RequestOptions requestOptions = null,
    CancellationToken ct = default
  ) =>
    AsyncHelper.RunSync(() =>
      WaitForApiKeyAsync(key, operation, apiKey, maxRetries, timeout, requestOptions, ct)
    );

  /// <inheritdoc/>
  public async Task<IEnumerable<T>> BrowseObjectsAsync<T>(
    string indexName,
    BrowseParamsObject browseParams,
    RequestOptions requestOptions = null
  )
  {
    browseParams.HitsPerPage = 1000;
    var all = await CreateIterable<BrowseResponse<T>>(
        async prevResp =>
        {
          browseParams.Cursor = prevResp?.Cursor;
          return await BrowseAsync<T>(indexName, new BrowseParams(browseParams), requestOptions);
        },
        resp => resp is { Cursor: null }
      )
      .ConfigureAwait(false);

    return all.SelectMany(u => u.Hits);
  }

  /// <inheritdoc/>
  public IEnumerable<T> BrowseObjects<T>(
    string indexName,
    BrowseParamsObject browseParams,
    RequestOptions requestOptions = null
  ) => AsyncHelper.RunSync(() => BrowseObjectsAsync<T>(indexName, browseParams, requestOptions));

  /// <inheritdoc/>
  public async Task<IEnumerable<Rule>> BrowseRulesAsync(
    string indexName,
    SearchRulesParams searchRulesParams,
    RequestOptions requestOptions = null
  )
  {
    const int hitsPerPage = 1000;
    searchRulesParams.HitsPerPage = hitsPerPage;

    var all = await CreateIterable<Tuple<SearchRulesResponse, int>>(
        async (prevResp) =>
        {
          var page = prevResp?.Item2 ?? 0;
          var searchSynonymsResponse = await SearchRulesAsync(
            indexName,
            searchRulesParams,
            requestOptions
          );
          return new Tuple<SearchRulesResponse, int>(searchSynonymsResponse, page + 1);
        },
        resp => resp?.Item1 is { Hits.Count: < hitsPerPage }
      )
      .ConfigureAwait(false);

    return all.SelectMany(u => u.Item1.Hits);
  }

  /// <inheritdoc/>
  public IEnumerable<Rule> BrowseRules(
    string indexName,
    SearchRulesParams searchRulesParams,
    RequestOptions requestOptions = null
  ) => AsyncHelper.RunSync(() => BrowseRulesAsync(indexName, searchRulesParams, requestOptions));

  /// <inheritdoc/>
  public async Task<IEnumerable<SynonymHit>> BrowseSynonymsAsync(
    string indexName,
    SearchSynonymsParams synonymsParams,
    RequestOptions requestOptions = null
  )
  {
    const int hitsPerPage = 1000;
    var page = synonymsParams.Page ?? 0;
    synonymsParams.HitsPerPage = hitsPerPage;
    var all = await CreateIterable<Tuple<SearchSynonymsResponse, int>>(
        async (prevResp) =>
        {
          var searchSynonymsResponse = await SearchSynonymsAsync(
            indexName,
            synonymsParams,
            requestOptions
          );
          page = page + 1;
          return new Tuple<SearchSynonymsResponse, int>(searchSynonymsResponse, page);
        },
        resp => resp?.Item1 is { Hits.Count: < hitsPerPage }
      )
      .ConfigureAwait(false);

    return all.SelectMany(u => u.Item1.Hits);
  }

  /// <inheritdoc/>
  public IEnumerable<SynonymHit> BrowseSynonyms(
    string indexName,
    SearchSynonymsParams synonymsParams,
    RequestOptions requestOptions = null
  ) => AsyncHelper.RunSync(() => BrowseSynonymsAsync(indexName, synonymsParams, requestOptions));

  /// <inheritdoc/>
  public string GenerateSecuredApiKey(string parentApiKey, SecuredApiKeyRestrictions restriction)
  {
    var queryParams = restriction.ToQueryString();
    var hash = HmacShaHelper.GetHash(parentApiKey, queryParams);
    return HmacShaHelper.Base64Encode($"{hash}{queryParams}");
  }

  /// <inheritdoc/>
  public TimeSpan GetSecuredApiKeyRemainingValidity(string securedApiKey)
  {
    if (string.IsNullOrWhiteSpace(securedApiKey))
    {
      throw new ArgumentNullException(nameof(securedApiKey));
    }

    var decodedKey = Encoding.UTF8.GetString(Convert.FromBase64String(securedApiKey));

    var regex = new Regex(@"validUntil=\d+");
    var matches = regex.Matches(decodedKey);

    if (matches.Count == 0)
    {
      throw new AlgoliaException("validUntil not found in given secured api key.");
    }

    // Select the validUntil parameter and its value
    var validUntilMatch = matches.Cast<Match>().Select(x => x.Value).First();

    // Extracting and converting the timestamp
    var timeStamp = Convert.ToInt64(validUntilMatch.Replace("validUntil=", string.Empty));

    var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    var unixTime = (long)(DateTime.UtcNow - sTime).TotalSeconds;

    return TimeSpan.FromSeconds(timeStamp - unixTime);
  }

  /// <inheritdoc/>
  public async Task<List<SearchResponse<T>>> SearchForHitsAsync<T>(
    IEnumerable<SearchForHits> requests,
    SearchStrategy? searchStrategy = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
  {
    var queries = requests.Select(t => new SearchQuery(t)).ToList();
    var searchMethod = new SearchMethodParams(queries) { Strategy = searchStrategy };
    var searchResponses = await SearchAsync<T>(searchMethod, options, cancellationToken);
    return searchResponses
      .Results.Where(x => x.IsSearchResponse())
      .Select(x => x.AsSearchResponse())
      .ToList();
  }

  /// <inheritdoc/>
  public List<SearchResponse<T>> SearchForHits<T>(
    IEnumerable<SearchForHits> requests,
    SearchStrategy? searchStrategy = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  ) =>
    AsyncHelper.RunSync(() =>
      SearchForHitsAsync<T>(requests, searchStrategy, options, cancellationToken)
    );

  /// <inheritdoc/>
  public async Task<List<SearchForFacetValuesResponse>> SearchForFacetsAsync(
    IEnumerable<SearchForFacets> requests,
    SearchStrategy? searchStrategy,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
  {
    var queries = requests.Select(t => new SearchQuery(t)).ToList();
    var searchMethod = new SearchMethodParams(queries) { Strategy = searchStrategy };
    var searchResponses = await SearchAsync<object>(searchMethod, options, cancellationToken);
    return searchResponses
      .Results.Where(x => x.IsSearchForFacetValuesResponse())
      .Select(x => x.AsSearchForFacetValuesResponse())
      .ToList();
  }

  /// <inheritdoc/>
  public List<SearchForFacetValuesResponse> SearchForFacets(
    IEnumerable<SearchForFacets> requests,
    SearchStrategy? searchStrategy,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  ) =>
    AsyncHelper.RunSync(() =>
      SearchForFacetsAsync(requests, searchStrategy, options, cancellationToken)
    );

  /// <inheritdoc/>
  public async Task<ReplaceAllObjectsResponse> ReplaceAllObjectsAsync<T>(
    string indexName,
    IEnumerable<T> objects,
    int batchSize = 1000,
    List<ScopeType> scopes = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class
  {
    if (objects == null)
    {
      throw new ArgumentNullException(nameof(objects));
    }

    if (scopes == null)
    {
      scopes = new List<ScopeType> { ScopeType.Settings, ScopeType.Rules, ScopeType.Synonyms };
    }

    var rng = RandomNumberGenerator.Create();
    var bytes = new byte[4];
    rng.GetBytes(bytes);

    var randomSuffix = (Math.Abs(BitConverter.ToInt32(bytes, 0)) % 900001) + 100000;
    var tmpIndexName = $"{indexName}_tmp_{randomSuffix}";

    try
    {
      var copyResponse = await OperationIndexAsync(
          indexName,
          new OperationIndexParams(OperationType.Copy, tmpIndexName) { Scope = scopes },
          options,
          cancellationToken
        )
        .ConfigureAwait(false);

      var batchResponse = await ChunkedBatchAsync(
          tmpIndexName,
          objects,
          Action.AddObject,
          true,
          batchSize,
          options,
          cancellationToken
        )
        .ConfigureAwait(false);

      await WaitForTaskAsync(
          tmpIndexName,
          copyResponse.TaskID,
          requestOptions: options,
          ct: cancellationToken
        )
        .ConfigureAwait(false);

      copyResponse = await OperationIndexAsync(
          indexName,
          new OperationIndexParams(OperationType.Copy, tmpIndexName) { Scope = scopes },
          options,
          cancellationToken
        )
        .ConfigureAwait(false);
      await WaitForTaskAsync(
          tmpIndexName,
          copyResponse.TaskID,
          requestOptions: options,
          ct: cancellationToken
        )
        .ConfigureAwait(false);

      var moveResponse = await OperationIndexAsync(
          tmpIndexName,
          new OperationIndexParams(OperationType.Move, indexName),
          options,
          cancellationToken
        )
        .ConfigureAwait(false);

      await WaitForTaskAsync(
          tmpIndexName,
          moveResponse.TaskID,
          requestOptions: options,
          ct: cancellationToken
        )
        .ConfigureAwait(false);

      return new ReplaceAllObjectsResponse
      {
        CopyOperationResponse = copyResponse,
        MoveOperationResponse = moveResponse,
        BatchResponses = batchResponse,
      };
    }
    catch (Exception ex)
    {
      await DeleteIndexAsync(tmpIndexName, cancellationToken: cancellationToken)
        .ConfigureAwait(false);

      throw;
    }
  }

  /// <inheritdoc/>
  public ReplaceAllObjectsResponse ReplaceAllObjects<T>(
    string indexName,
    IEnumerable<T> objects,
    int batchSize = 1000,
    List<ScopeType> scopes = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class =>
    AsyncHelper.RunSync(() =>
      ReplaceAllObjectsAsync(indexName, objects, batchSize, scopes, options, cancellationToken)
    );

  /// <inheritdoc/>
  public async Task<List<BatchResponse>> ChunkedBatchAsync<T>(
    string indexName,
    IEnumerable<T> objects,
    Action action = Action.AddObject,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class
  {
    var batchCount = (int)Math.Ceiling((double)objects.Count() / batchSize);
    var responses = new List<BatchResponse>();

    for (var i = 0; i < batchCount; i++)
    {
      var chunk = objects.Skip(i * batchSize).Take(batchSize);
      var batchResponse = await BatchAsync(
          indexName,
          new BatchWriteParams(chunk.Select(x => new BatchRequest(action, x)).ToList()),
          options,
          cancellationToken
        )
        .ConfigureAwait(false);

      responses.Add(batchResponse);
    }

    if (waitForTasks)
    {
      foreach (var batch in responses)
      {
        await WaitForTaskAsync(
            indexName,
            batch.TaskID,
            requestOptions: options,
            ct: cancellationToken
          )
          .ConfigureAwait(false);
      }
    }

    return responses;
  }

  /// <inheritdoc/>
  public List<BatchResponse> ChunkedBatch<T>(
    string indexName,
    IEnumerable<T> objects,
    Action action = Action.AddObject,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class =>
    AsyncHelper.RunSync(() =>
      ChunkedBatchAsync(
        indexName,
        objects,
        action,
        waitForTasks,
        batchSize,
        options,
        cancellationToken
      )
    );

  /// <inheritdoc/>
  public async Task<List<BatchResponse>> SaveObjectsAsync<T>(
    string indexName,
    IEnumerable<T> objects,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class
  {
    return await ChunkedBatchAsync(
        indexName,
        objects,
        Action.AddObject,
        waitForTasks,
        batchSize,
        options,
        cancellationToken
      )
      .ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public List<BatchResponse> SaveObjects<T>(
    string indexName,
    IEnumerable<T> objects,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class =>
    AsyncHelper.RunSync(() =>
      SaveObjectsAsync(indexName, objects, waitForTasks, batchSize, options, cancellationToken)
    );

  /// <inheritdoc/>
  public async Task<List<BatchResponse>> SaveObjectsAsync<T>(
    string indexName,
    IEnumerable<T> objects,
    RequestOptions options,
    CancellationToken cancellationToken = default
  )
    where T : class
  {
    return await SaveObjectsAsync(indexName, objects, false, 1000, options, cancellationToken)
      .ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public List<BatchResponse> SaveObjects<T>(
    string indexName,
    IEnumerable<T> objects,
    RequestOptions options,
    CancellationToken cancellationToken = default
  )
    where T : class =>
    AsyncHelper.RunSync(() => SaveObjectsAsync(indexName, objects, options, cancellationToken));

  /// <inheritdoc/>
  public async Task<List<BatchResponse>> DeleteObjectsAsync(
    string indexName,
    IEnumerable<String> objectIDs,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
  {
    return await ChunkedBatchAsync(
        indexName,
        objectIDs.Select(id => new { objectID = id }),
        Action.DeleteObject,
        waitForTasks,
        batchSize,
        options,
        cancellationToken
      )
      .ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public List<BatchResponse> DeleteObjects(
    string indexName,
    IEnumerable<String> objectIDs,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  ) =>
    AsyncHelper.RunSync(() =>
      DeleteObjectsAsync(indexName, objectIDs, waitForTasks, batchSize, options, cancellationToken)
    );

  /// <inheritdoc/>
  public async Task<List<BatchResponse>> PartialUpdateObjectsAsync<T>(
    string indexName,
    IEnumerable<T> objects,
    bool createIfNotExists,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class
  {
    return await ChunkedBatchAsync(
        indexName,
        objects,
        createIfNotExists ? Action.PartialUpdateObject : Action.PartialUpdateObjectNoCreate,
        waitForTasks,
        batchSize,
        options,
        cancellationToken
      )
      .ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public List<BatchResponse> PartialUpdateObjects<T>(
    string indexName,
    IEnumerable<T> objects,
    bool createIfNotExists,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
    where T : class =>
    AsyncHelper.RunSync(() =>
      PartialUpdateObjectsAsync(
        indexName,
        objects,
        createIfNotExists,
        waitForTasks,
        batchSize,
        options,
        cancellationToken
      )
    );

  private static async Task<List<TU>> CreateIterable<TU>(
    Func<TU, Task<TU>> executeQuery,
    Func<TU, bool> stopCondition
  )
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

  /// <inheritdoc/>
  public async Task<bool> IndexExistsAsync(
    string indexName,
    CancellationToken cancellationToken = default
  )
  {
    try
    {
      await GetSettingsAsync(indexName, null, null, cancellationToken);
    }
    catch (AlgoliaApiException ex) when (ex.HttpErrorCode == 404)
    {
      return await Task.FromResult(false);
    }
    catch (Exception ex)
    {
      throw;
    }

    return await Task.FromResult(true);
  }

  /// <inheritdoc/>
  public bool IndexExists(string indexName, CancellationToken cancellationToken = default) =>
    AsyncHelper.RunSync(() => IndexExistsAsync(indexName, cancellationToken));

  // ==================== SaveObjectsWithTransformation ====================

  /// <inheritdoc/>
  public async Task<
    List<Algolia.Search.Models.Ingestion.WatchResponse>
  > SaveObjectsWithTransformationAsync(
    string indexName,
    IEnumerable<object> objects,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
  {
    if (_ingestionTransporter == null)
    {
      throw new AlgoliaException(
        "`setTransformationRegion` must have been called before calling this method."
      );
    }

    return await _ingestionTransporter
      .ChunkedPushAsync(
        indexName,
        objects,
        Algolia.Search.Models.Ingestion.Action.AddObject,
        waitForTasks,
        batchSize,
        referenceIndexName: null,
        options,
        cancellationToken
      )
      .ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public List<Algolia.Search.Models.Ingestion.WatchResponse> SaveObjectsWithTransformation(
    string indexName,
    IEnumerable<object> objects,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  ) =>
    AsyncHelper.RunSync(() =>
      SaveObjectsWithTransformationAsync(
        indexName,
        objects,
        waitForTasks,
        batchSize,
        options,
        cancellationToken
      )
    );

  // ==================== PartialUpdateObjectsWithTransformation ====================

  /// <inheritdoc/>
  public async Task<
    List<Algolia.Search.Models.Ingestion.WatchResponse>
  > PartialUpdateObjectsWithTransformationAsync(
    string indexName,
    IEnumerable<object> objects,
    bool createIfNotExists = true,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
  {
    if (_ingestionTransporter == null)
    {
      throw new AlgoliaException(
        "`setTransformationRegion` must have been called before calling this method."
      );
    }

    var action = createIfNotExists
      ? Algolia.Search.Models.Ingestion.Action.PartialUpdateObject
      : Algolia.Search.Models.Ingestion.Action.PartialUpdateObjectNoCreate;

    return await _ingestionTransporter
      .ChunkedPushAsync(
        indexName,
        objects,
        action,
        waitForTasks,
        batchSize,
        referenceIndexName: null,
        options,
        cancellationToken
      )
      .ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public List<Algolia.Search.Models.Ingestion.WatchResponse> PartialUpdateObjectsWithTransformation(
    string indexName,
    IEnumerable<object> objects,
    bool createIfNotExists = true,
    bool waitForTasks = false,
    int batchSize = 1000,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  ) =>
    AsyncHelper.RunSync(() =>
      PartialUpdateObjectsWithTransformationAsync(
        indexName,
        objects,
        createIfNotExists,
        waitForTasks,
        batchSize,
        options,
        cancellationToken
      )
    );

  // ==================== ReplaceAllObjectsWithTransformation ====================

  /// <inheritdoc/>
  public async Task<ReplaceAllObjectsWithTransformationResponse> ReplaceAllObjectsWithTransformationAsync(
    string indexName,
    IEnumerable<object> objects,
    int batchSize = 1000,
    List<ScopeType> scopes = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  )
  {
    if (_ingestionTransporter == null)
    {
      throw new AlgoliaException(
        "`setTransformationRegion` must have been called before calling this method."
      );
    }

    if (scopes == null)
    {
      scopes = new List<ScopeType> { ScopeType.Settings, ScopeType.Rules, ScopeType.Synonyms };
    }

    var rng = RandomNumberGenerator.Create();
    var bytes = new byte[4];
    rng.GetBytes(bytes);

    var randomSuffix = (Math.Abs(BitConverter.ToInt32(bytes, 0)) % 900001) + 100000;
    var tmpIndexName = $"{indexName}_tmp_{randomSuffix}";

    try
    {
      // Step 1: Copy settings/rules/synonyms from source to temp
      var copyOperationResponse = await OperationIndexAsync(
          indexName,
          new OperationIndexParams(OperationType.Copy, tmpIndexName) { Scope = scopes },
          options,
          cancellationToken
        )
        .ConfigureAwait(false);

      // Step 2: Push transformed objects to temp index (referencing original index for transformation)
      var watchResponses = await _ingestionTransporter
        .ChunkedPushAsync(
          tmpIndexName,
          objects,
          Algolia.Search.Models.Ingestion.Action.AddObject,
          waitForTasks: true,
          batchSize,
          referenceIndexName: indexName, // CRITICAL: Apply transformation from original index
          options,
          cancellationToken
        )
        .ConfigureAwait(false);

      // Step 3: Wait for copy operation to complete
      await WaitForTaskAsync(
          tmpIndexName,
          copyOperationResponse.TaskID,
          requestOptions: options,
          ct: cancellationToken
        )
        .ConfigureAwait(false);

      // Step 4: Copy again to ensure latest settings/rules/synonyms
      copyOperationResponse = await OperationIndexAsync(
          indexName,
          new OperationIndexParams(OperationType.Copy, tmpIndexName) { Scope = scopes },
          options,
          cancellationToken
        )
        .ConfigureAwait(false);

      await WaitForTaskAsync(
          tmpIndexName,
          copyOperationResponse.TaskID,
          requestOptions: options,
          ct: cancellationToken
        )
        .ConfigureAwait(false);

      // Step 5: Move temp index to replace original
      var moveOperationResponse = await OperationIndexAsync(
          tmpIndexName,
          new OperationIndexParams(OperationType.Move, indexName),
          options,
          cancellationToken
        )
        .ConfigureAwait(false);

      await WaitForTaskAsync(
          tmpIndexName,
          moveOperationResponse.TaskID,
          requestOptions: options,
          ct: cancellationToken
        )
        .ConfigureAwait(false);

      return new ReplaceAllObjectsWithTransformationResponse(
        copyOperationResponse,
        ToSearchWatchResponses(watchResponses),
        moveOperationResponse
      );
    }
    catch
    {
      // Clean up temp index on error
      try
      {
        await DeleteIndexAsync(tmpIndexName, cancellationToken: cancellationToken)
          .ConfigureAwait(false);
      }
      catch
      {
        // Ignore errors during cleanup
      }
      throw;
    }
  }

  /// <inheritdoc/>
  public ReplaceAllObjectsWithTransformationResponse ReplaceAllObjectsWithTransformation(
    string indexName,
    IEnumerable<object> objects,
    int batchSize = 1000,
    List<ScopeType> scopes = null,
    RequestOptions options = null,
    CancellationToken cancellationToken = default
  ) =>
    AsyncHelper.RunSync(() =>
      ReplaceAllObjectsWithTransformationAsync(
        indexName,
        objects,
        batchSize,
        scopes,
        options,
        cancellationToken
      )
    );

  private static List<Models.Search.WatchResponse> ToSearchWatchResponses(
    List<Models.Ingestion.WatchResponse> ingestionResponses
  )
  {
    var searchResponses = new List<Models.Search.WatchResponse>();

    foreach (var response in ingestionResponses)
    {
      var jsonString = JsonSerializer.Serialize(response, JsonConfig.Options);
      var searchResponse = JsonSerializer.Deserialize<Models.Search.WatchResponse>(
        jsonString,
        JsonConfig.Options
      );
      searchResponses.Add(searchResponse);
    }

    return searchResponses;
  }
}

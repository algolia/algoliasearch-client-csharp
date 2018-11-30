/*
* Copyright (c) 2018 Algolia
* http://www.algolia.com/
* Based on the first version developed by Christopher Maneu under the same license:
*  https://github.com/cmaneu/algoliasearch-client-csharp
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using Algolia.Search.Exceptions;
using Algolia.Search.Http;
using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Requests;
using Algolia.Search.Models.Responses;
using Algolia.Search.Transport;
using Algolia.Search.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    public class SearchClient : ISearchClient
    {
        private readonly IRequesterWrapper _requesterWrapper;
        public AlgoliaConfig Config { get; }

        /// <summary>
        /// Initialize a client with default settings
        /// </summary>
        public SearchClient() : this(new AlgoliaConfig(), new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Create a new search client for the given appID
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="apiKey"></param>
        public SearchClient(string applicationId, string apiKey) : this(
            new AlgoliaConfig { ApiKey = apiKey, AppId = applicationId }, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize a client with custom config
        /// </summary>
        /// <param name="config"></param>
        public SearchClient(AlgoliaConfig config) : this(config, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize the client with custom config and custom Requester
        /// </summary>
        /// <param name="config"></param>
        /// <param name="httpRequester"></param>
        public SearchClient(AlgoliaConfig config, IHttpRequester httpRequester)
        {
            if (httpRequester == null)
            {
                throw new ArgumentNullException(nameof(httpRequester), "An httpRequester is required");
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config), "A config is required");
            }

            if (string.IsNullOrWhiteSpace(config.AppId))
            {
                throw new ArgumentNullException(nameof(config.AppId), "Application ID is required");
            }

            if (string.IsNullOrWhiteSpace(config.ApiKey))
            {
                throw new ArgumentNullException(nameof(config.ApiKey), "An API key is required");
            }

            Config = config;
            _requesterWrapper = new RequesterWrapper(config, httpRequester);
        }

        /// <summary>
        /// Initialize an index for the given client
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public SearchIndex InitIndex(string indexName)
        {
            return string.IsNullOrWhiteSpace(indexName)
                ? throw new ArgumentNullException(nameof(indexName), "The Index name is required")
                : new SearchIndex(_requesterWrapper, Config, indexName);
        }

        /// <summary>
        /// Retrieve one or more objects, potentially from different indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public MultipleGetObjectsResponse<T> MultipleGetObjects<T>(IEnumerable<MultipleGetObject> queries,
            RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => MultipleGetObjectsAsync<T>(queries, requestOptions));

        /// <summary>
        /// Retrieve one or more objects, potentially from different indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<MultipleGetObjectsResponse<T>> MultipleGetObjectsAsync<T>(IEnumerable<MultipleGetObject> queries,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (queries == null)
            {
                throw new ArgumentNullException(nameof(queries));
            }

            var request = new MultipleGetObjectsRequest { Requests = queries };

            return await _requesterWrapper.ExecuteRequestAsync<MultipleGetObjectsResponse<T>, MultipleGetObjectsRequest>(HttpMethod.Post,
                    "/1/indexes/*/objects", CallType.Read, request, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// This method allows to send multiple search queries, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public MultipleQueriesResponse<T> MultipleQueries<T>(MultipleQueriesRequest request,
            RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => MultipleQueriesAsync<T>(request, requestOptions));

        /// <summary>
        /// This method allows to send multiple search queries, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<MultipleQueriesResponse<T>> MultipleQueriesAsync<T>(MultipleQueriesRequest request,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await _requesterWrapper.ExecuteRequestAsync<MultipleQueriesResponse<T>, MultipleQueriesRequest>(HttpMethod.Post,
                    "/1/indexes/*/queries", CallType.Read, request, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Perform multiple write operations, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public MultipleIndexBatchIndexingResponse MultipleBatch<T>(IEnumerable<BatchOperation<T>> operations,
            RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => MultipleBatchAsync(operations, requestOptions));

        /// <inheritdoc />
        /// <summary>
        /// Perform multiple write operations, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<MultipleIndexBatchIndexingResponse> MultipleBatchAsync<T>(
            IEnumerable<BatchOperation<T>> operations, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (operations == null)
            {
                throw new ArgumentNullException(nameof(operations));
            }

            var batch = new BatchRequest<T>(operations);

            MultipleIndexBatchIndexingResponse resp = await _requesterWrapper.ExecuteRequestAsync<MultipleIndexBatchIndexingResponse, BatchRequest<T>>(
                    HttpMethod.Post, "/1/indexes/*/batch", CallType.Write, batch, requestOptions, ct)
                .ConfigureAwait(false);

            resp.WaitDelegate = (i, t) => WaitTask(i, t);
            return resp;
        }

        /// <summary>
        /// Get a list of indexes/indices with their associated metadata.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public ListIndexesResponse ListIndexes(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ListIndexesAsync(requestOptions));

        /// <summary>
        /// Get a list of indexes/indices with their associated metadata.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ListIndexesResponse> ListIndexesAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<ListIndexesResponse>(HttpMethod.Get,
                    "/1/indexes", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Delete an index by name
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public DeleteResponse DeleteIndex(string indexName, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteIndexAsync(indexName, requestOptions));

        /// <summary>
        /// Delete an index by name
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> DeleteIndexAsync(string indexName, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException(indexName);
            }

            return await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                    $"/1/indexes/{indexName}", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public ListApiKeysResponse ListApiKeys(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ListApiKeysAsync(requestOptions));

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ListApiKeysResponse> ListApiKeysAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<ListApiKeysResponse>(HttpMethod.Get,
                    "/1/keys", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public ApiKeysResponse GetApiKey(string apiKey, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetApiKeyAsync(apiKey, requestOptions));

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiKeysResponse> GetApiKeyAsync(string apiKey, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException(apiKey);
            }

            return await _requesterWrapper.ExecuteRequestAsync<ApiKeysResponse>(HttpMethod.Get,
                    $"/1/keys/{apiKey}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Add a new API Key with specific permissions/restrictions.
        /// </summary>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public AddApiKeyResponse AddApiKey(ApiKeyRequest acl, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => AddApiKeyAsync(acl, requestOptions));

        /// <summary>
        /// Add a new API Key with specific permissions/restrictions.
        /// </summary>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<AddApiKeyResponse> AddApiKeyAsync(ApiKeyRequest acl, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (acl == null)
            {
                throw new ArgumentNullException(nameof(acl));
            }

            return await _requesterWrapper.ExecuteRequestAsync<AddApiKeyResponse, ApiKeyRequest>(HttpMethod.Post,
                    "/1/keys", CallType.Write, acl, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Update the permissions of an existing API Key.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public UpdateApiKeyResponse
            UpdateApiKey(string apiKey, ApiKeyRequest acl, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => UpdateApiKeyAsync(apiKey, acl, requestOptions));

        /// <summary>
        /// Update the permissions of an existing API Key.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<UpdateApiKeyResponse> UpdateApiKeyAsync(string apiKey, ApiKeyRequest acl,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException(apiKey);
            }

            if (acl == null)
            {
                throw new ArgumentNullException(nameof(acl));
            }

            return await _requesterWrapper.ExecuteRequestAsync<UpdateApiKeyResponse, ApiKeyRequest>(HttpMethod.Put,
                    $"/1/keys/{apiKey}", CallType.Write, acl, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Delete an existing API Key
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public DeleteResponse DeleteApiKey(string apiKey, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteApiKeyAsync(apiKey, requestOptions));

        /// <summary>
        /// Delete an existing API Key
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> DeleteApiKeyAsync(string apiKey, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException(apiKey);
            }

            return await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                    $"/1/keys/{apiKey}", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// List the clusters available in a multi-clusters setup for a single appID
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public IEnumerable<ClustersResponse> ListClusters(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ListClustersAsync(requestOptions));

        /// <summary>
        /// List the clusters available in a multi-clusters setup for a single appID
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ClustersResponse>> ListClustersAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            ListClustersResponse response = await _requesterWrapper
                .ExecuteRequestAsync<ListClustersResponse>(HttpMethod.Get, "/1/clusters", CallType.Read, requestOptions,
                    ct)
                .ConfigureAwait(false);
            return response?.Clusters;
        }

        /// <summary>
        /// Search for userIDs.·
        // The data returned will usually be a few seconds behind real-time, because userID usage may take up to a few seconds propagate to the different cluster
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SearchResponse<UserIdResponse> SearchUserIDs(SearchUserIdsRequest query, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SearchUserIDsAsync(query, requestOptions));

        /// <summary>
        /// Search for userIDs.·
        // The data returned will usually be a few seconds behind real-time, because userID usage may take up to a few seconds propagate to the different cluster
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SearchResponse<UserIdResponse>> SearchUserIDsAsync(SearchUserIdsRequest query,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _requesterWrapper.ExecuteRequestAsync<SearchResponse<UserIdResponse>, SearchUserIdsRequest>(
                    HttpMethod.Post, "/1/clusters/mapping/search", CallType.Read, query, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// List the userIDs assigned to a multi-clusters appID.
        /// </summary>
        /// <param name="hitsPerPage"></param>
        /// <param name="requestOptions"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ListUserIdsResponse ListUserIds(int page = 0, int hitsPerPage = 1000,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ListUserIdsAsync(page, hitsPerPage, requestOptions));

        /// <summary>
        /// List the userIDs assigned to a multi-clusters appID.
        /// </summary>
        /// <param name="hitsPerPage"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ListUserIdsResponse> ListUserIdsAsync(int page = 0, int hitsPerPage = 1000,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            var queryParams = new Dictionary<string, string>
            {
                { "page", page.ToString() },
                { "hitsPerPage", hitsPerPage.ToString() }
            };

            RequestOptions requestOptionsToSend = RequestOptionsHelper.Create(requestOptions, queryParams);

            return await _requesterWrapper.ExecuteRequestAsync<ListUserIdsResponse>(
                    HttpMethod.Get, "/1/clusters/mapping", CallType.Read, requestOptionsToSend, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Returns the userID data stored in the mapping.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public UserIdResponse GetUserId(string userId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetUserIdAsync(userId, requestOptions));

        /// <summary>
        /// Returns the userID data stored in the mapping.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<UserIdResponse> GetUserIdAsync(string userId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(userId);
            }

            return await _requesterWrapper.ExecuteRequestAsync<UserIdResponse>(HttpMethod.Get,
                    $"/1/clusters/mapping/{WebUtility.UrlEncode(userId)}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get the top 10 userIDs with the highest number of records per cluster.
        /// The data returned will usually be a few seconds behind real-time, because userID usage may take up to a few seconds to propagate to the different clusters.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public TopUserIdResponse GetTopUserId(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetTopUserIdAsync(requestOptions));

        /// <summary>
        /// Get the top 10 userIDs with the highest number of records per cluster.
        /// The data returned will usually be a few seconds behind real-time, because userID usage may take up to a few seconds to propagate to the different clusters.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<TopUserIdResponse> GetTopUserIdAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<TopUserIdResponse>(HttpMethod.Get,
                    "/1/clusters/mapping/top", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Assign or Move a userID to a cluster.
        /// The time it takes to migrate (move) a user is proportional to the amount of data linked to the userID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clusterName"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public AssignUserIdResponse
            AssignUserId(string userId, string clusterName, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => AssignUserIdAsync(userId, clusterName, requestOptions));

        /// <summary>
        /// Assign or Move a userID to a cluster.
        /// The time it takes to migrate (move) a user is proportional to the amount of data linked to the userID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clusterName"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<AssignUserIdResponse> AssignUserIdAsync(string userId, string clusterName,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(userId);
            }

            if (string.IsNullOrWhiteSpace(clusterName))
            {
                throw new ArgumentNullException(clusterName);
            }

            var data = new AssignUserIdRequest { Cluster = clusterName };

            var removeUserId = new Dictionary<string, string>() { { "X-Algolia-USER-ID", userId } };

            if (requestOptions?.Headers != null && requestOptions.Headers.Any())
            {
                requestOptions.Headers =
                    requestOptions.Headers.Concat(removeUserId).ToDictionary(x => x.Key, x => x.Value);
            }
            else if (requestOptions != null && requestOptions.Headers == null)
            {
                requestOptions.Headers = removeUserId;
            }
            else
            {
                requestOptions = new RequestOptions { Headers = removeUserId };
            }

            AssignUserIdResponse response = await _requesterWrapper.ExecuteRequestAsync<AssignUserIdResponse, AssignUserIdRequest>(HttpMethod.Post,
                  "/1/clusters/mapping", CallType.Write, data, requestOptions, ct)
              .ConfigureAwait(false);

            response.UserId = userId;
            response.GetUserDelegate = u => GetUserId(u);
            return response;
        }

        /// <summary>
        /// Remove a userID and its associated data from the multi-clusters.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public RemoveUserIdResponse RemoveUserId(string userId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => RemoveUserIdAsync(userId, requestOptions));

        /// <summary>
        /// Remove a userID and its associated data from the multi-clusters.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<RemoveUserIdResponse> RemoveUserIdAsync(string userId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(userId);
            }

            var removeUserId = new Dictionary<string, string>() { { "X-Algolia-USER-ID", userId } };

            if (requestOptions?.Headers != null && requestOptions.Headers.Any())
            {
                requestOptions.Headers =
                    requestOptions.Headers.Concat(removeUserId).ToDictionary(x => x.Key, x => x.Value);
            }
            else if (requestOptions != null && requestOptions.Headers == null)
            {
                requestOptions.Headers = removeUserId;
            }
            else
            {
                requestOptions = new RequestOptions { Headers = removeUserId };
            }

            try
            {
                RemoveUserIdResponse response = await _requesterWrapper.ExecuteRequestAsync<RemoveUserIdResponse>(HttpMethod.Delete,
                       $"/1/clusters/mapping", CallType.Write, requestOptions, ct)
                        .ConfigureAwait(false);

                response.UserId = userId;
                response.RemoveDelegate = u => RemoveUserId(u);
                return response;
            }
            catch (AlgoliaApiException ex)
            {
                if (!ex.Message.Contains("Another mapping operation is already running for this userID"))
                {
                    throw;
                }

                return new RemoveUserIdResponse { UserId = userId, RemoveDelegate = u => RemoveUserId(u) };
            }
        }

        /// <summary>
        /// Get logs for the given index
        /// </summary>
        /// <returns></returns>
        public LogResponse GetLogs(RequestOptions requestOptions = null, int offset = 0, int length = 10, string indexName = null, string type = "all") =>
            AsyncHelper.RunSync(() => GetLogsAsync(requestOptions, offset: offset, length: length, indexName: indexName, type: type));

        /// <summary>
        /// Get logs for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<LogResponse> GetLogsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken), int offset = 0, int length = 10, string indexName = null, string type = "all")
        {
            var queryParams = new Dictionary<string, string>
                {
                    { "offset", offset.ToString()},
                    { "length", length.ToString() },
                    { "indexName", indexName },
                    { "type", type},
                };

            RequestOptions requestOptionsToSend = RequestOptionsHelper.Create(requestOptions, queryParams);

            return await _requesterWrapper.ExecuteRequestAsync<LogResponse>(HttpMethod.Get, "/1/logs", CallType.Read,
                    requestOptionsToSend, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Make a copy of the settings of an index
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public CopyToResponse CopySettings(string sourceIndex, string destinationIndex, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopySettingsAsync(sourceIndex, destinationIndex, requestOptions));

        /// <summary>
        /// Make a copy of the settings of an index
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CopyToResponse> CopySettingsAsync(string sourceIndex, string destinationIndex,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            var scopes = new List<string> { CopyScope.Settings };
            return await CopyIndexAsync(sourceIndex, destinationIndex, scopes).ConfigureAwait(false);
        }

        /// <summary>
        /// Make a copy of the rules of an index
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public CopyToResponse CopyRules(string sourceIndex, string destinationIndex, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopyRulesAsync(sourceIndex, destinationIndex, requestOptions));

        /// <summary>
        /// Make a copy of the rules of an index
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CopyToResponse> CopyRulesAsync(string sourceIndex, string destinationIndex,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            var scopes = new List<string> { CopyScope.Rules };
            return await CopyIndexAsync(sourceIndex, destinationIndex, scopes).ConfigureAwait(false);
        }

        /// <summary>
        /// Make a copy of the synonyms of an index
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public CopyToResponse CopySynonyms(string sourceIndex, string destinationIndex, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopySynonymsAsync(sourceIndex, destinationIndex, requestOptions));

        /// <summary>
        /// Make a copy of the synonyms of an index
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CopyToResponse> CopySynonymsAsync(string sourceIndex, string destinationIndex,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            var scopes = new List<string> { CopyScope.Synonyms };
            return await CopyIndexAsync(sourceIndex, destinationIndex, scopes).ConfigureAwait(false);
        }

        /// <summary>
        /// Make a copy of an index, including its objects, settings, synonyms, and query rules.
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="scope"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public CopyToResponse CopyIndex(string sourceIndex, string destinationIndex, IEnumerable<string> scope = null,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopyIndexAsync(sourceIndex, destinationIndex, scope, requestOptions));

        /// <summary>
        /// Make a copy of an index, including its objects, settings, synonyms, and query rules.
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="scope"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CopyToResponse> CopyIndexAsync(string sourceIndex, string destinationIndex, IEnumerable<string> scope = null,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(destinationIndex))
            {
                throw new ArgumentNullException(destinationIndex);
            }

            string encondedSourceIndex = WebUtility.UrlEncode(sourceIndex);
            var data = new CopyToRequest { Operation = MoveType.Copy, IndexNameDest = destinationIndex, Scope = scope };

            CopyToResponse response = await _requesterWrapper.ExecuteRequestAsync<CopyToResponse, CopyToRequest>(
                    HttpMethod.Post, $"/1/indexes/{encondedSourceIndex}/operation", CallType.Write, data,
                    requestOptions,
                    ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(sourceIndex, t);
            return response;
        }

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public MoveIndexResponse MoveIndex(string sourceIndex, string destinationIndex, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => MoveIndexAsync(sourceIndex, destinationIndex, requestOptions));

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<MoveIndexResponse> MoveIndexAsync(string sourceIndex, string destinationIndex, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(sourceIndex))
            {
                throw new ArgumentNullException(sourceIndex);
            }

            MoveIndexRequest request = new MoveIndexRequest { Operation = MoveType.Move, Destination = destinationIndex };

            MoveIndexResponse response = await _requesterWrapper
                .ExecuteRequestAsync<MoveIndexResponse, MoveIndexRequest>(HttpMethod.Post,
                    $"/1/indexes/{sourceIndex}/operation", CallType.Write, request, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(destinationIndex, t);
            return response;
        }

        /// <summary>
        /// This function waits for the Algolia's API task to finish
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions"></param>
        public void WaitTask(string indexName, long taskId, int timeToWait = 100, RequestOptions requestOptions = null)
        {
            SearchIndex indexToWait = InitIndex(indexName);
            indexToWait.WaitTask(taskId);
        }
    }
}
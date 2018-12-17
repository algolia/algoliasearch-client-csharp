﻿/*
* Copyright (c) 2018 Algolia
* http://www.algolia.com/
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
using Algolia.Search.Models.Personalization;
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
    /// <summary>
    /// Algolia search client implementation of <see cref="ISearchClient"/>
    /// </summary>
    public class SearchClient : ISearchClient
    {
        private readonly IRequesterWrapper _requesterWrapper;
        private readonly AlgoliaConfig _config;

        /// <summary>
        /// Create a new search client for the given appID
        /// </summary>
        /// <param name="applicationId">Your application</param>
        /// <param name="apiKey">Your API key</param>
        public SearchClient(string applicationId, string apiKey) : this(
            new AlgoliaConfig(applicationId, apiKey), new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize a client with custom config
        /// </summary>
        /// <param name="config">Algolia configuration</param>
        public SearchClient(AlgoliaConfig config) : this(config, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize the client with custom config and custom Requester
        /// </summary>
        /// <param name="config">Algolia Config</param>
        /// <param name="httpRequester">Your Http requester implementation of <see cref="IHttpRequester"/></param>
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

            _config = config;
            _requesterWrapper = new RequesterWrapper(config, httpRequester);
        }

        /// <summary>
        /// Initialize an index for the given client
        /// </summary>
        /// <param name="indexName">Your index name</param>
        public SearchIndex InitIndex(string indexName)
        {
            return string.IsNullOrWhiteSpace(indexName)
                ? throw new ArgumentNullException(nameof(indexName), "The Index name is required")
                : new SearchIndex(_requesterWrapper, _config, indexName);
        }

        /// <inheritdoc />
        public MultipleGetObjectsResponse<T> MultipleGetObjects<T>(IEnumerable<MultipleGetObject> queries,
            RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => MultipleGetObjectsAsync<T>(queries, requestOptions));

        /// <inheritdoc />
        public async Task<MultipleGetObjectsResponse<T>> MultipleGetObjectsAsync<T>(
            IEnumerable<MultipleGetObject> queries,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (queries == null)
            {
                throw new ArgumentNullException(nameof(queries));
            }

            var request = new MultipleGetObjectsRequest { Requests = queries };

            return await _requesterWrapper
                .ExecuteRequestAsync<MultipleGetObjectsResponse<T>, MultipleGetObjectsRequest>(HttpMethod.Post,
                    "/1/indexes/*/objects", CallType.Read, request, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public MultipleQueriesResponse<T> MultipleQueries<T>(MultipleQueriesRequest request,
            RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => MultipleQueriesAsync<T>(request, requestOptions));

        /// <inheritdoc />
        public async Task<MultipleQueriesResponse<T>> MultipleQueriesAsync<T>(MultipleQueriesRequest request,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await _requesterWrapper.ExecuteRequestAsync<MultipleQueriesResponse<T>, MultipleQueriesRequest>(
                    HttpMethod.Post,
                    "/1/indexes/*/queries", CallType.Read, request, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public MultipleIndexBatchIndexingResponse MultipleBatch<T>(IEnumerable<BatchOperation<T>> operations,
            RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => MultipleBatchAsync(operations, requestOptions));

        /// <inheritdoc />
        public async Task<MultipleIndexBatchIndexingResponse> MultipleBatchAsync<T>(
            IEnumerable<BatchOperation<T>> operations, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (operations == null)
            {
                throw new ArgumentNullException(nameof(operations));
            }

            var batch = new BatchRequest<T>(operations);

            MultipleIndexBatchIndexingResponse resp = await _requesterWrapper
                .ExecuteRequestAsync<MultipleIndexBatchIndexingResponse, BatchRequest<T>>(
                    HttpMethod.Post, "/1/indexes/*/batch", CallType.Write, batch, requestOptions, ct)
                .ConfigureAwait(false);

            resp.WaitDelegate = (i, t) => WaitTask(i, t);
            return resp;
        }

        /// <inheritdoc />
        public ListIndexesResponse ListIndexes(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ListIndexesAsync(requestOptions));

        /// <inheritdoc />
        public async Task<ListIndexesResponse> ListIndexesAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<ListIndexesResponse>(HttpMethod.Get,
                    "/1/indexes", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public DeleteResponse DeleteIndex(string indexName, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteIndexAsync(indexName, requestOptions));

        /// <inheritdoc />
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

        /// <inheritdoc />
        public string GenerateSecuredApiKeys(string parentApiKey, SecuredApiKeyRestriction restriction)
        {
            string queryParams = QueryStringHelper.BuildRestrictionQueryString(restriction);
            string hash = HmacShaHelper.GetHash(parentApiKey, queryParams);
            return HmacShaHelper.Base64Encode($"{hash}{queryParams}");
        }

        /// <inheritdoc />
        public ListApiKeysResponse ListApiKeys(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ListApiKeysAsync(requestOptions));

        /// <inheritdoc />
        public async Task<ListApiKeysResponse> ListApiKeysAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<ListApiKeysResponse>(HttpMethod.Get,
                    "/1/keys", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public ApiKey GetApiKey(string apiKey, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetApiKeyAsync(apiKey, requestOptions));

        /// <inheritdoc />
        public async Task<ApiKey> GetApiKeyAsync(string apiKey, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException(apiKey);
            }

            try
            {
                ApiKey response = await _requesterWrapper.ExecuteRequestAsync<ApiKey>(HttpMethod.Get,
                        $"/1/keys/{apiKey}", CallType.Read, requestOptions, ct)
                    .ConfigureAwait(false);

                response.GetApiKeyDelegate = null;
                response.Exist = true;
                response.Key = apiKey;
                return response;
            }
            catch (AlgoliaApiException ex)
            {
                // We need to catch the 404 error to allow the Wait() method on the response
                if (ex.HttpErrorCode != 404)
                {
                    throw;
                }

                return new ApiKey { Key = apiKey, GetApiKeyDelegate = k => GetApiKey(k), Exist = false };
            }
        }

        /// <inheritdoc />
        public AddApiKeyResponse AddApiKey(ApiKey acl, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => AddApiKeyAsync(acl, requestOptions));

        /// <inheritdoc />
        public async Task<AddApiKeyResponse> AddApiKeyAsync(ApiKey acl, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (acl == null)
            {
                throw new ArgumentNullException(nameof(acl));
            }

            return await _requesterWrapper.ExecuteRequestAsync<AddApiKeyResponse, ApiKey>(HttpMethod.Post,
                    "/1/keys", CallType.Write, acl, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public UpdateApiKeyResponse UpdateApiKey(ApiKey request, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => UpdateApiKeyAsync(request, requestOptions));

        /// <inheritdoc />
        public async Task<UpdateApiKeyResponse> UpdateApiKeyAsync(ApiKey request,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Value))
            {
                throw new ArgumentNullException(request.Value);
            }

            string key = request.Value;
            // need to unset the value before sending it, otherwise it will appers on the body and error when sent to the API
            request.Value = null;

            return await _requesterWrapper.ExecuteRequestAsync<UpdateApiKeyResponse, ApiKey>(HttpMethod.Put,
                    $"/1/keys/{key}", CallType.Write, request, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public DeleteApiKeyResponse DeleteApiKey(string apiKey, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteApiKeyAsync(apiKey, requestOptions));

        /// <inheritdoc />
        public async Task<DeleteApiKeyResponse> DeleteApiKeyAsync(string apiKey, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException(apiKey);
            }

            DeleteApiKeyResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteApiKeyResponse>(
                    HttpMethod.Delete,
                    $"/1/keys/{apiKey}", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.GetApiKeyDelegate = k => GetApiKey(k);
            response.Key = apiKey;
            return response;
        }

        /// <inheritdoc />
        public IEnumerable<ClustersResponse> ListClusters(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ListClustersAsync(requestOptions));

        /// <inheritdoc />
        public async Task<IEnumerable<ClustersResponse>> ListClustersAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            ListClustersResponse response = await _requesterWrapper
                .ExecuteRequestAsync<ListClustersResponse>(HttpMethod.Get, "/1/clusters", CallType.Read, requestOptions,
                    ct)
                .ConfigureAwait(false);
            return response?.Clusters;
        }

        /// <inheritdoc />
        public SearchResponse<UserIdResponse> SearchUserIDs(SearchUserIdsRequest query,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SearchUserIDsAsync(query, requestOptions));

        /// <inheritdoc />
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

        /// <inheritdoc />
        public ListUserIdsResponse ListUserIds(int page = 0, int hitsPerPage = 1000,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ListUserIdsAsync(page, hitsPerPage, requestOptions));

        /// <inheritdoc />
        public async Task<ListUserIdsResponse> ListUserIdsAsync(int page = 0, int hitsPerPage = 1000,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            var queryParams = new Dictionary<string, string>
            {
                {"page", page.ToString()},
                {"hitsPerPage", hitsPerPage.ToString()}
            };

            requestOptions = requestOptions.AddQueryParams(queryParams);

            return await _requesterWrapper.ExecuteRequestAsync<ListUserIdsResponse>(
                    HttpMethod.Get, "/1/clusters/mapping", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public UserIdResponse GetUserId(string userId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetUserIdAsync(userId, requestOptions));

        /// <inheritdoc />
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

        /// <inheritdoc />
        public TopUserIdResponse GetTopUserId(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetTopUserIdAsync(requestOptions));

        /// <inheritdoc />
        public async Task<TopUserIdResponse> GetTopUserIdAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<TopUserIdResponse>(HttpMethod.Get,
                    "/1/clusters/mapping/top", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public AssignUserIdResponse
            AssignUserId(string userId, string clusterName, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => AssignUserIdAsync(userId, clusterName, requestOptions));

        /// <inheritdoc />
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

            var userIdHeader = new Dictionary<string, string>() { { "X-Algolia-USER-ID", userId } };
            requestOptions = requestOptions.AddHeaders(userIdHeader);

            AssignUserIdResponse response = await _requesterWrapper
                .ExecuteRequestAsync<AssignUserIdResponse, AssignUserIdRequest>(HttpMethod.Post,
                    "/1/clusters/mapping", CallType.Write, data, requestOptions, ct)
                .ConfigureAwait(false);

            response.UserId = userId;
            response.GetUserDelegate = u => GetUserId(u);
            return response;
        }

        /// <inheritdoc />
        public RemoveUserIdResponse RemoveUserId(string userId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => RemoveUserIdAsync(userId, requestOptions));

        /// <inheritdoc />
        public async Task<RemoveUserIdResponse> RemoveUserIdAsync(string userId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(userId);
            }

            var userIdHeader = new Dictionary<string, string>() { { "X-Algolia-USER-ID", userId } };
            requestOptions = requestOptions.AddHeaders(userIdHeader);

            try
            {
                RemoveUserIdResponse response = await _requesterWrapper.ExecuteRequestAsync<RemoveUserIdResponse>(
                        HttpMethod.Delete,
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

        /// <inheritdoc />
        public LogResponse GetLogs(RequestOptions requestOptions = null, int offset = 0, int length = 10,
            string indexName = null, string type = "all") =>
            AsyncHelper.RunSync(() =>
                GetLogsAsync(requestOptions, offset: offset, length: length, indexName: indexName, type: type));

        /// <inheritdoc />
        public async Task<LogResponse> GetLogsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken), int offset = 0, int length = 10, string indexName = null,
            string type = "all")
        {
            var queryParams = new Dictionary<string, string>
            {
                {"offset", offset.ToString()},
                {"length", length.ToString()},
                {"indexName", indexName},
                {"type", type},
            };

            requestOptions = requestOptions.AddQueryParams(queryParams);

            return await _requesterWrapper.ExecuteRequestAsync<LogResponse>(HttpMethod.Get, "/1/logs", CallType.Read,
                    requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public CopyToResponse CopySettings(string sourceIndex, string destinationIndex,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopySettingsAsync(sourceIndex, destinationIndex, requestOptions));

        /// <inheritdoc />
        public async Task<CopyToResponse> CopySettingsAsync(string sourceIndex, string destinationIndex,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            var scopes = new List<string> { CopyScope.Settings };
            return await CopyIndexAsync(sourceIndex, destinationIndex, scopes).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public CopyToResponse CopyRules(string sourceIndex, string destinationIndex,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopyRulesAsync(sourceIndex, destinationIndex, requestOptions));

        /// <inheritdoc />
        public async Task<CopyToResponse> CopyRulesAsync(string sourceIndex, string destinationIndex,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            var scopes = new List<string> { CopyScope.Rules };
            return await CopyIndexAsync(sourceIndex, destinationIndex, scopes).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public CopyToResponse CopySynonyms(string sourceIndex, string destinationIndex,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopySynonymsAsync(sourceIndex, destinationIndex, requestOptions));

        /// <inheritdoc />
        public async Task<CopyToResponse> CopySynonymsAsync(string sourceIndex, string destinationIndex,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            var scopes = new List<string> { CopyScope.Synonyms };
            return await CopyIndexAsync(sourceIndex, destinationIndex, scopes).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public CopyToResponse CopyIndex(string sourceIndex, string destinationIndex, IEnumerable<string> scope = null,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopyIndexAsync(sourceIndex, destinationIndex, scope, requestOptions));

        /// <inheritdoc />
        public async Task<CopyToResponse> CopyIndexAsync(string sourceIndex, string destinationIndex,
            IEnumerable<string> scope = null,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(sourceIndex))
            {
                throw new ArgumentNullException(nameof(sourceIndex));
            }

            if (string.IsNullOrWhiteSpace(destinationIndex))
            {
                throw new ArgumentNullException(nameof(destinationIndex));
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

        /// <inheritdoc />
        public MoveIndexResponse MoveIndex(string sourceIndex, string destinationIndex,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => MoveIndexAsync(sourceIndex, destinationIndex, requestOptions));

        /// <inheritdoc />
        public async Task<MoveIndexResponse> MoveIndexAsync(string sourceIndex, string destinationIndex,
            RequestOptions requestOptions = null,
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

        /// <inheritdoc />
        public GetStrategyResponse GetPersonalizationStrategy(RequestOptions requestOptions = null) =>
             AsyncHelper.RunSync(() => GetPersonalizationStrategyAsync(requestOptions));

        /// <inheritdoc />
        public async Task<GetStrategyResponse> GetPersonalizationStrategyAsync(RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<GetStrategyResponse>(HttpMethod.Get, "/1/recommendation/personalization/strategy", CallType.Read,
                    requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SetStrategyResponse SetPersonalizationStrategy(SetStrategyRequest request, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SetPersonalizationStrategyAsync(request, requestOptions));

        /// <inheritdoc />
        public async Task<SetStrategyResponse> SetPersonalizationStrategyAsync(SetStrategyRequest request, RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<SetStrategyResponse, SetStrategyRequest>(HttpMethod.Post, "/1/recommendation/personalization/strategy", CallType.Write,
                    request, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void WaitTask(string indexName, long taskId, int timeToWait = 100, RequestOptions requestOptions = null)
        {
            SearchIndex indexToWait = InitIndex(indexName);
            indexToWait.WaitTask(taskId);
        }
    }
}
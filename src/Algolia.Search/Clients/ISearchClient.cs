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

using Algolia.Search.Http;
using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Requests;
using Algolia.Search.Models.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    public interface ISearchClient
    {
        AlgoliaConfig Config { get; }

        /// <summary>
        /// Initialize an index for the given client
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        SearchIndex InitIndex(string indexName);

        /// <summary>
        /// Retrieve one or more objects, potentially from different indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        MultipleGetObjectsResponse<T> MultipleGetObjects<T>(IEnumerable<MultipleGetObject> queries, RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// Retrieve one or more objects, potentially from different indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<MultipleGetObjectsResponse<T>> MultipleGetObjectsAsync<T>(IEnumerable<MultipleGetObject> queries, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// This method allows to send multiple search queries, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        MultipleQueriesResponse<T> MultipleQueries<T>(MultipleQueriesRequest request, RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// This method allows to send multiple search queries, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<MultipleQueriesResponse<T>> MultipleQueriesAsync<T>(MultipleQueriesRequest request, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Perform multiple write operations, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        MultipleIndexBatchIndexingResponse MultipleBatch<T>(IEnumerable<BatchOperation<T>> operations, RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// Perform multiple write operations, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<MultipleIndexBatchIndexingResponse> MultipleBatchAsync<T>(IEnumerable<BatchOperation<T>> operations, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Get a list of indexes/indices with their associated metadata.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        ListIndexesResponse ListIndexes(RequestOptions requestOptions = null);

        /// <summary>
        /// Get a list of indexes/indices with their associated metadata.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ListIndexesResponse> ListIndexesAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Delete an index by name
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        DeleteResponse DeleteIndex(string indexName, RequestOptions requestOptions = null);

        /// <summary>
        /// Delete an index by name
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteIndexAsync(string indexName, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        ListApiKeysResponse ListApiKeys(RequestOptions requestOptions = null);

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ListApiKeysResponse> ListApiKeysAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        ApiKeysResponse GetApiKey(string apiKey, RequestOptions requestOptions = null);

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiKeysResponse> GetApiKeyAsync(string apiKey, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Add a new API Key with specific permissions/restrictions.
        /// </summary>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        AddApiKeyResponse AddApiKey(ApiKeyRequest acl, RequestOptions requestOptions = null);

        /// <summary>
        /// Add a new API Key with specific permissions/restrictions.
        /// </summary>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<AddApiKeyResponse> AddApiKeyAsync(ApiKeyRequest acl, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Update the permissions of an existing API Key.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        UpdateApiKeyResponse UpdateApiKey(string apiKey, ApiKeyRequest acl, RequestOptions requestOptions = null);

        /// <summary>
        /// Update the permissions of an existing API Key.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<UpdateApiKeyResponse> UpdateApiKeyAsync(string apiKey, ApiKeyRequest acl, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Delete an existing API Key
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        DeleteResponse DeleteApiKey(string apiKey, RequestOptions requestOptions = null);

        /// <summary>
        /// Delete an existing API Key
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteApiKeyAsync(string apiKey, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// List the clusters available in a multi-clusters setup for a single appID
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        IEnumerable<ClustersResponse> ListClusters(RequestOptions requestOptions = null);

        /// <summary>
        /// List the clusters available in a multi-clusters setup for a single appID
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<ClustersResponse>> ListClustersAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// List the userIDs assigned to a multi-clusters appID.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="hitsPerPage"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        ListUserIdsResponse ListUserIds(int page = 0, int hitsPerPage = 1000, RequestOptions requestOptions = null);

        /// <summary>
        /// List the userIDs assigned to a multi-clusters appID.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="hitsPerPage"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ListUserIdsResponse> ListUserIdsAsync(int page = 0, int hitsPerPage = 1000, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Returns the userID data stored in the mapping.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        UserIdResponse GetUserId(string userId, RequestOptions requestOptions = null);

        /// <summary>
        /// Returns the userID data stored in the mapping.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<UserIdResponse> GetUserIdAsync(string userId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get the top 10 userIDs with the highest number of records per cluster.
        /// The data returned will usually be a few seconds behind real-time, because userID usage may take up to a few seconds to propagate to the different clusters.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        TopUserIdResponse GetTopUserId(RequestOptions requestOptions = null);

        /// <summary>
        /// Get the top 10 userIDs with the highest number of records per cluster.
        /// The data returned will usually be a few seconds behind real-time, because userID usage may take up to a few seconds to propagate to the different clusters.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<TopUserIdResponse> GetTopUserIdAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Assign or Move a userID to a cluster.
        /// The time it takes to migrate (move) a user is proportional to the amount of data linked to the userID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clusterName"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        AssignUserIdResponse AssignUserId(string userId, string clusterName, RequestOptions requestOptions = null);

        /// <summary>
        /// Assign or Move a userID to a cluster.
        /// The time it takes to migrate (move) a user is proportional to the amount of data linked to the userID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clusterName"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<AssignUserIdResponse> AssignUserIdAsync(string userId, string clusterName, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Remove a userID and its associated data from the multi-clusters.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        RemoveUserIdResponse RemoveUserId(string userId, RequestOptions requestOptions = null);

        /// <summary>
        /// Remove a userID and its associated data from the multi-clusters.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<RemoveUserIdResponse> RemoveUserIdAsync(string userId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get logs for the given index
        /// </summary>
        /// <returns></returns>
        LogResponse GetLogs(RequestOptions requestOptions = null, int offset = 0, int length = 10, string indexName = null, string type = "all");

        /// <summary>
        /// Get logs for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<LogResponse> GetLogsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken), int offset = 0, int length = 10, string indexName = null, string type = "all");
    }
}

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
        /// <summary>
        /// Initialize an index for the given client
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        Index InitIndex(string indexName);

        /// <summary>
        /// Retrieve one or more objects, potentially from different indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        MultipleGetObjectsResponse<T> MultipleGetObjects<T>(MultipleGetObjectsRequest queries, RequestOption requestOptions = null) where T : class;

        /// <summary>
        /// Retrieve one or more objects, potentially from different indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<MultipleGetObjectsResponse<T>> MultipleGetObjectsAsync<T>(MultipleGetObjectsRequest queries, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// This method allows to send multiple search queries, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        MultipleQueriesResponse<T> MultipleQueries<T>(MultipleQueriesRequest queries, RequestOption requestOptions = null) where T : class;

        /// <summary>
        /// This method allows to send multiple search queries, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<MultipleQueriesResponse<T>> MultipleQueriesAsync<T>(MultipleQueriesRequest queries, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Perform multiple write operations, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        MultipleBatchResponse MultipleBatch<T>(IEnumerable<BatchOperation<T>> operations, RequestOption requestOptions = null) where T : class;

        /// <summary>
        /// Perform multiple write operations, potentially targeting multiple indices, in a single API call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<MultipleBatchResponse> MultipleBatchAsync<T>(IEnumerable<BatchOperation<T>> operations, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Get a list of indexes/indices with their associated metadata.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        ListIndexesResponse ListIndexes(RequestOption requestOptions = null);

        /// <summary>
        /// Get a list of indexes/indices with their associated metadata.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ListIndexesResponse> ListIndexesAsync(RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Delete an index by name
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        DeleteResponse DeleteIndex(string indexName, RequestOption requestOptions = null);

        /// <summary>
        /// Delete an index by name
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteIndexAsync(string indexName, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        MoveIndexResponse MoveIndex(string sourceIndex, string destinationIndex, RequestOption requestOptions = null);

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<MoveIndexResponse> MoveIndexAsync(string sourceIndex, string destinationIndex, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        ListApiKeysResponse ListApiKeys(RequestOption requestOptions = null);

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ListApiKeysResponse> ListApiKeysAsync(RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        ApiKeysResponse GetApiKey(string apiKey, RequestOption requestOptions = null);

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiKeysResponse> GetApiKeyAsync(string apiKey, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Add a new API Key with specific permissions/restrictions.
        /// </summary>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        AddApiKeyResponse AddApiKey(ApiKeyRequest acl, RequestOption requestOptions = null);

        /// <summary>
        /// Add a new API Key with specific permissions/restrictions.
        /// </summary>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<AddApiKeyResponse> AddApiKeyAsync(ApiKeyRequest acl, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Update the permissions of an existing API Key.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        UpdateApiKeyResponse UpdateApiKey(string apiKey, ApiKeyRequest acl, RequestOption requestOptions = null);

        /// <summary>
        /// Update the permissions of an existing API Key.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="acl"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<UpdateApiKeyResponse> UpdateApiKeyAsync(string apiKey, ApiKeyRequest acl, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Delete an existing API Key
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        DeleteResponse DeleteApiKey(string apiKey, RequestOption requestOptions = null);

        /// <summary>
        /// Delete an existing API Key
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteApiKeyAsync(string apiKey, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// List the clusters available in a multi-clusters setup for a single appID
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        ListClustersResponse ListClusters(RequestOption requestOptions = null);

        /// <summary>
        /// List the clusters available in a multi-clusters setup for a single appID
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ListClustersResponse> ListClustersAsync(RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// List the userIDs assigned to a multi-clusters appID.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SearchResponse<UserIdResponse> ListUserIds(RequestOption requestOptions = null);

        /// <summary>
        /// List the userIDs assigned to a multi-clusters appID.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SearchResponse<UserIdResponse>> ListUserIdsAsync(RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Returns the userID data stored in the mapping.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        UserIdResponse GetUserId(string userId, RequestOption requestOptions = null);

        /// <summary>
        /// Returns the userID data stored in the mapping.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<UserIdResponse> GetUserIdAsync(string userId, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get the top 10 userIDs with the highest number of records per cluster.
        /// The data returned will usually be a few seconds behind real-time, because userID usage may take up to a few seconds to propagate to the different clusters.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        TopUserIdResponse GetTopUserId(RequestOption requestOptions = null);

        /// <summary>
        /// Get the top 10 userIDs with the highest number of records per cluster.
        /// The data returned will usually be a few seconds behind real-time, because userID usage may take up to a few seconds to propagate to the different clusters.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<TopUserIdResponse> GetTopUserIdAsync(RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Assign or Move a userID to a cluster.
        /// The time it takes to migrate (move) a user is proportional to the amount of data linked to the userID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clusterName"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        AddObjectResponse AssignUserId(string userId, string clusterName, RequestOption requestOptions = null);

        /// <summary>
        /// Assign or Move a userID to a cluster.
        /// The time it takes to migrate (move) a user is proportional to the amount of data linked to the userID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clusterName"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<AddObjectResponse> AssignUserIdAsync(string userId, string clusterName, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Remove a userID and its associated data from the multi-clusters.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        DeleteResponse RemoveUserId(string userId, RequestOption requestOptions = null);

        /// <summary>
        /// Remove a userID and its associated data from the multi-clusters.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DeleteResponse> RemoveUserIdAsync(string userId, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get logs for the given index
        /// </summary>
        /// <returns></returns>
        LogResponse GetLogs(RequestOption requestOptions = null);

        /// <summary>
        /// Get logs for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<LogResponse> GetLogsAsync(RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));
    }
}

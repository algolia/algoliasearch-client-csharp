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
using Algolia.Search.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    public interface ISearchClient
    {
        Index InitIndex(string indexName);

        /// <summary>
        /// Get a list of indexes/indices with their associated metadata.
        /// </summary>
        /// <param name="data"></param>
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
        /// <param name="data"></param>
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
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        ApiKeysResponse GetApiKey(string key, RequestOption requestOptions = null);

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiKeysResponse> GetApiKeyAsync(string key, RequestOption requestOptions = null,
                    CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Delete an existing API Key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        DeleteResponse DeleteApiKey(string key, RequestOption requestOptions = null);

        /// <summary>
        /// Delete an existing API Key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteApiKeyAsync(string key, RequestOption requestOptions = null,
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

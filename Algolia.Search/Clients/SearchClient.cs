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
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Responses;
using Algolia.Search.Transport;
using Algolia.Search.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    public class SearchClient<T> : ISearchClient<T> where T : class
    {
        private readonly IRequesterWrapper _requesterWrapper;

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
        public SearchClient(string applicationId, string apiKey) : this(new AlgoliaConfig { ApiKey = apiKey, AppId = applicationId }, new AlgoliaHttpRequester())
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

            if (string.IsNullOrEmpty(config.AppId))
            {
                throw new ArgumentNullException(nameof(config.AppId), "Application ID is required");
            }

            if (string.IsNullOrEmpty(config.ApiKey))
            {
                throw new ArgumentNullException(nameof(config.ApiKey), "An API key is required");
            }

            _requesterWrapper = new RequesterWrapper(config, httpRequester);
        }

        /// <summary>
        /// Initialize an index for the given client
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public Index<T> InitIndex(string indexName)
        {
            return string.IsNullOrEmpty(indexName)
                ? throw new ArgumentNullException(nameof(indexName), "Index name is required")
                : new Index<T>(_requesterWrapper, indexName);
        }

        /// <summary>
        /// Get a list of indexes/indices with their associated metadata.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public ListIndexesResponse ListIndexes(RequestOption requestOptions = null) =>
                    AsyncHelper.RunSync(() => ListIndexesAsync(requestOptions));

        /// <summary>
        /// Get a list of indexes/indices with their associated metadata.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ListIndexesResponse> ListIndexesAsync(RequestOption requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<ListIndexesResponse>(HttpMethod.Get,
                "/1/indexes", CallType.Read, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete an index by name
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public DeleteResponse DeleteIndex(string indexName, RequestOption requestOptions = null) =>
                    AsyncHelper.RunSync(() => DeleteIndexAsync(indexName, requestOptions));

        /// <summary>
        /// Delete an index by name
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> DeleteIndexAsync(string indexName, RequestOption requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                $"/1/indexes/{indexName}", CallType.Write, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public MoveIndexResponse MoveIndex(string sourceIndex, string destinationIndex, RequestOption requestOptions = null) =>
                    AsyncHelper.RunSync(() => MoveIndexAsync(sourceIndex, destinationIndex, requestOptions));

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<MoveIndexResponse> MoveIndexAsync(string sourceIndex, string destinationIndex, RequestOption requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<MoveIndexResponse>(HttpMethod.Post,
                $"/1/indexes/{sourceIndex}/operation", CallType.Write, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public ListApiKeysResponse ListApiKeys(RequestOption requestOptions = null) =>
                    AsyncHelper.RunSync(() => ListApiKeysAsync(requestOptions));

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ListApiKeysResponse> ListApiKeysAsync(RequestOption requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<ListApiKeysResponse>(HttpMethod.Get,
                "/1/keys", CallType.Read, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public ApiKeysResponse GetApiKey(string key, RequestOption requestOptions = null) =>
                    AsyncHelper.RunSync(() => GetApiKeyAsync(key, requestOptions));

        /// <summary>
        /// Get the full list of API Keys.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiKeysResponse> GetApiKeyAsync(string key, RequestOption requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<ApiKeysResponse>(HttpMethod.Get,
                $"/1/keys/{key}", CallType.Read, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete an existing API Key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public DeleteResponse DeleteApiKey(string key, RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteApiKeyAsync(key, requestOptions));

        /// <summary>
        /// Delete an existing API Key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> DeleteApiKeyAsync(string key, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                $"/1/keys/{key}", CallType.Write, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// List the clusters available in a multi-clusters setup for a single appID
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public ListClustersResponse ListClusters(RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => ListClustersAsync(requestOptions));

        /// <summary>
        /// List the clusters available in a multi-clusters setup for a single appID
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ListClustersResponse> ListClustersAsync(RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<ListClustersResponse>(HttpMethod.Delete,
                "/1/clusters", CallType.Write, requestOptions, ct).ConfigureAwait(false);
        }
    }
}

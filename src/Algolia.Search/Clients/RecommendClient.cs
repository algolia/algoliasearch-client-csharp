/*
* Copyright (c) 2021 Algolia
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Http;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Recommend;
using Algolia.Search.Transport;
using Algolia.Search.Utils;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// 
    /// </summary>
    public class RecommendClient : IRecommendClient
    {
        private readonly HttpTransport _transport;

        /// <summary>
        /// Create a new recommend client for the given appID
        /// </summary>
        /// <param name="applicationId">Your application ID</param>
        /// <param name="apiKey">Your Api KEY</param>
        public RecommendClient(string applicationId, string apiKey) : this(
            new SearchConfig(applicationId, apiKey), new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize a client with custom config
        /// </summary>
        /// <param name="config">Algolia config instance</param>
        public RecommendClient(SearchConfig config) : this(config, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize the client with custom config and custom Requester
        /// </summary>
        /// <param name="config">Algolia config instance</param>
        /// <param name="httpRequester">Your Http requester implementation of <see cref="IHttpRequester"/></param>
        public RecommendClient(SearchConfig config, IHttpRequester httpRequester)
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

            _transport = new HttpTransport(config, httpRequester);
        }

        /// <inheritdoc />
        public RecommendResponse<T> GetRecommendations<T>(IEnumerable<RecommendRequest> requests,
            RequestOptions requestOptions = null)  where T : class =>
            AsyncHelper.RunSync(() => GetRecommendationsAsync<T>(requests, requestOptions));

        /// <inheritdoc />
        public async Task<RecommendResponse<T>> GetRecommendationsAsync<T>(
            IEnumerable<RecommendRequest> requests, RequestOptions requestOptions = null,
            CancellationToken ct = default) where T : class
        {
            if (requests == null)
            {
                throw new ArgumentNullException(nameof(requests));
            }

            var request = new RecommendRequests
            {
                Requests = requests.ToList()
            };

            return await _transport.ExecuteRequestAsync<RecommendResponse<T>, RecommendRequests>(
                    HttpMethod.Post, "/1/indexes/*/recommendations", CallType.Read, request, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public RecommendResponse<T> GetRelatedProducts<T>(IEnumerable<RelatedProductsRequest> requests,
            RequestOptions requestOptions = null)  where T : class =>
            AsyncHelper.RunSync(() => GetRelatedProductsAsync<T>(requests, requestOptions));

        /// <inheritdoc />
        public async Task<RecommendResponse<T>> GetRelatedProductsAsync<T>(
            IEnumerable<RelatedProductsRequest> requests, RequestOptions requestOptions = null,
            CancellationToken ct = default) where T : class
        {
          return await GetRecommendationsAsync<T>(requests, requestOptions, ct);
        }

        /// <inheritdoc />
        public RecommendResponse<T> GetFrequentlyBoughtTogether<T>(IEnumerable<BoughtTogetherRequest> requests,
            RequestOptions requestOptions = null)  where T : class =>
            AsyncHelper.RunSync(() => GetFrequentlyBoughtTogetherAsync<T>(requests, requestOptions));

        /// <inheritdoc />
        public async Task<RecommendResponse<T>> GetFrequentlyBoughtTogetherAsync<T>(
            IEnumerable<BoughtTogetherRequest> requests, RequestOptions requestOptions = null,
            CancellationToken ct = default) where T : class
        {
            return await GetRecommendationsAsync<T>(requests, requestOptions, ct);
        }
    }
}

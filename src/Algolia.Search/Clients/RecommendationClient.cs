/*
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

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Http;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Recommendation;
using Algolia.Search.Transport;
using Algolia.Search.Utils;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Algolia search client implementation of <see cref="IRecommendationClient"/>
    /// </summary>
    public class RecommendationClient : IRecommendationClient
    {
        private readonly HttpTransport _transport;

        /// <summary>
        /// Create a new search client for the given appID
        /// </summary>
        /// <param name="applicationId">Your application ID</param>
        /// <param name="apiKey">Your Api KEY</param>
        /// <param name="region">Region where your personalization data is stored and processed</param>
        public RecommendationClient(string applicationId, string apiKey, string region) : this(
            new RecommendationConfig(applicationId, apiKey, region), new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize a client with custom config
        /// </summary>
        /// <param name="config">Algolia config instance</param>
        public RecommendationClient(RecommendationConfig config) : this(config, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize the client with custom config and custom Requester
        /// </summary>
        /// <param name="config">Algolia config instance</param>
        /// <param name="httpRequester">Your Http requester implementation of <see cref="IHttpRequester"/></param>
        public RecommendationClient(RecommendationConfig config, IHttpRequester httpRequester)
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
        public GetStrategyResponse GetPersonalizationStrategy(RequestOptions requestOptions = null)
        {
            return AsyncHelper.RunSync(() => GetPersonalizationStrategyAsync(requestOptions));
        }

        /// <inheritdoc />
        public async Task<GetStrategyResponse> GetPersonalizationStrategyAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            return await _transport.ExecuteRequestAsync<GetStrategyResponse>(HttpMethod.Get,
                    "/1/strategies/personalization", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SetStrategyResponse SetPersonalizationStrategy(SetStrategyRequest request,
            RequestOptions requestOptions = null)
        {
            return AsyncHelper.RunSync(() => SetPersonalizationStrategyAsync(request, requestOptions));
        }

        /// <inheritdoc />
        public async Task<SetStrategyResponse> SetPersonalizationStrategyAsync(SetStrategyRequest request,
            RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            return await _transport.ExecuteRequestAsync<SetStrategyResponse, SetStrategyRequest>(HttpMethod.Post,
                    "/1/strategies/personalization", CallType.Write, request, requestOptions, ct)
                .ConfigureAwait(false);
        }
    }
}

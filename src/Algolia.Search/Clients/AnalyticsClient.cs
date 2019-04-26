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

using Algolia.Search.Http;
using Algolia.Search.Models.Analytics;
using Algolia.Search.Models.Enums;
using Algolia.Search.Transport;
using Algolia.Search.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Algolia Analytics Client implementation of <see cref="IAnalyticsClient"/>
    /// </summary>
    public class AnalyticsClient : IAnalyticsClient
    {
        private readonly HttpTransport _transport;

        /// <summary>
        /// Create a new search client for the given appID
        /// </summary>
        /// <param name="applicationId">Your application ID</param>
        /// <param name="apiKey">Your Api KEY</param>
        public AnalyticsClient(string applicationId, string apiKey) : this(
            new AnalyticsConfig(applicationId, apiKey), new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize a client with custom config
        /// </summary>
        /// <param name="config">Algolia config instance</param>
        public AnalyticsClient(AnalyticsConfig config) : this(config, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize the client with custom config and custom Requester
        /// </summary>
        /// <param name="config">Algolia config instance</param>
        /// <param name="httpRequester">Your Http requester implementation of <see cref="IHttpRequester"/></param>
        public AnalyticsClient(AnalyticsConfig config, IHttpRequester httpRequester)
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
        public ABTest GetABTest(long abTestId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetABTestAsync(abTestId, requestOptions));

        /// <inheritdoc />
        public async Task<ABTest> GetABTestAsync(long abTestId, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            return await _transport.ExecuteRequestAsync<ABTest>(HttpMethod.Get,
                    $"/2/abtests/{abTestId}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public ABTestsReponse GetABTests(int offset = 0, int limit = 10, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetABTestsAsync(offset, limit, requestOptions));

        /// <inheritdoc />
        public async Task<ABTestsReponse> GetABTestsAsync(int offset = 0, int limit = 10,
            RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            var queryParams = new Dictionary<string, string>
            {
                {"offset", offset.ToString()},
                {"limit", limit.ToString()}
            };

            requestOptions = requestOptions.AddQueryParams(queryParams);

            return await _transport.ExecuteRequestAsync<ABTestsReponse>(HttpMethod.Get,
                    "/2/abtests", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public AddABTestResponse AddABTest(ABTest aBTest, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => AddABTestAsync(aBTest, requestOptions));

        /// <inheritdoc />
        public async Task<AddABTestResponse> AddABTestAsync(ABTest aBTest, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            return await _transport.ExecuteRequestAsync<AddABTestResponse, ABTest>(HttpMethod.Post,
                    "/2/abtests", CallType.Write, aBTest, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public StopABTestResponse StopABTest(long abTestId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => StopABTestAsync(abTestId, requestOptions));

        /// <inheritdoc />
        public async Task<StopABTestResponse> StopABTestAsync(long abTestId, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            return await _transport.ExecuteRequestAsync<StopABTestResponse>(HttpMethod.Post,
                    $"/2/abtests/{abTestId}/stop", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public DeleteABTestResponse DeleteABTest(long abTestId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteABTestAsync(abTestId, requestOptions));

        /// <inheritdoc />
        public async Task<DeleteABTestResponse> DeleteABTestAsync(long abTestId, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            return await _transport.ExecuteRequestAsync<DeleteABTestResponse>(HttpMethod.Delete,
                    $"/2/abtests/{abTestId}", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);
        }
    }
}

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
    /// Algolia Analytics Client
    /// </summary>
    public class AnalyticsClient : IAnalyticsClient
    {
        private readonly IRequesterWrapper _requesterWrapper;

        /// <summary>
        /// Initialize a client with default settings
        /// </summary>
        public AnalyticsClient() : this(new AlgoliaConfig(), new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Create a new search client for the given appID
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="apiKey"></param>
        public AnalyticsClient(string applicationId, string apiKey) : this(
            new AlgoliaConfig {ApiKey = apiKey, AppId = applicationId}, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize a client with custom config
        /// </summary>
        /// <param name="config"></param>
        public AnalyticsClient(AlgoliaConfig config) : this(config, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize the client with custom config and custom Requester
        /// </summary>
        /// <param name="config"></param>
        /// <param name="httpRequester"></param>
        public AnalyticsClient(AlgoliaConfig config, IHttpRequester httpRequester)
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

            _requesterWrapper = new RequesterWrapper(config, httpRequester);
        }

        /// <summary>
        /// Get an A/B test information and results.
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public ABTest GetABTest(long abTestId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetABTestAsync(abTestId, requestOptions));

        /// <summary>
        /// Get an A/B test information and results.
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        public async Task<ABTest> GetABTestAsync(long abTestId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<ABTest>(HttpMethod.Get,
                    $"/2/abtests/{abTestId}", CallType.Analytics, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch all existing AB Tests for App that are available for the current API Key. Returns an array of metadata and metrics.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public ABTestsReponse GetABTests(int offset = 0, int limit = 10, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetABTestsAsync(offset, limit, requestOptions));

        /// <summary>
        /// Fetch all existing AB Tests for App that are available for the current API Key. Returns an array of metadata and metrics.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        public async Task<ABTestsReponse> GetABTestsAsync(int offset = 0, int limit = 10,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            var queryParams = new Dictionary<string, string>
            {
                {"offset", offset.ToString()},
                {"limit", limit.ToString()}
            };

            RequestOptions requestOptionsToSend = RequestOptionsHelper.Create(requestOptions, queryParams);

            return await _requesterWrapper.ExecuteRequestAsync<ABTestsReponse>(HttpMethod.Get,
                    "/2/abtests", CallType.Analytics, requestOptionsToSend, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new AB Test with provided configuration.
        /// </summary>
        /// <param name="aBTest"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public AddABTestResponse AddABTest(ABTest aBTest, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => AddABTestAsync(aBTest, requestOptions));

        /// <summary>
        /// Creates a new AB Test with provided configuration.
        /// </summary>
        /// <param name="aBTest"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        public async Task<AddABTestResponse> AddABTestAsync(ABTest aBTest, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<AddABTestResponse, ABTest>(HttpMethod.Post,
                    "/2/abtests", CallType.Analytics, aBTest, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Marks the A/B Test as stopped. At this point, the test is over and cannot be restarted. 
        /// As a result, your application is back to normal: index A will perform as usual, receiving 100% of all search requests. 
        /// Associated metadata and metrics are still stored
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public StopABTestResponse StopABTest(long abTestId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => StopABTestAsync(abTestId, requestOptions));

        /// <summary>
        /// Marks the A/B Test as stopped. At this point, the test is over and cannot be restarted. 
        /// As a result, your application is back to normal: index A will perform as usual, receiving 100% of all search requests. 
        /// Associated metadata and metrics are still stored
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        public async Task<StopABTestResponse> StopABTestAsync(long abTestId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<StopABTestResponse>(HttpMethod.Post,
                    $"/2/abtests/{abTestId}/stop", CallType.Analytics, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the A/B Test and removes all associated metadata and metrics.
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public DeleteABTestResponse DeleteABTest(long abTestId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteABTestAsync(abTestId, requestOptions));

        /// <summary>
        /// Deletes the A/B Test and removes all associated metadata and metrics.
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        public async Task<DeleteABTestResponse> DeleteABTestAsync(long abTestId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<DeleteABTestResponse>(HttpMethod.Delete,
                    $"/2/abtests/{abTestId}", CallType.Analytics, requestOptions, ct)
                .ConfigureAwait(false);
        }
    }
}
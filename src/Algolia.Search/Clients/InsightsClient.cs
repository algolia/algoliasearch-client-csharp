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
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Insights;
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
    /// Implementation of <see cref="IInsightsClient"/>
    /// </summary>
    public class InsightsClient : IInsightsClient
    {
        private readonly HttpTransport _transport;

        /// <summary>
        /// Initialize a new insights client
        /// </summary>
        /// <param name="applicationId">The application ID</param>
        /// <param name="apiKey">The api Key</param>
        /// <param name="region">Server's region</param>
        /// <returns></returns>
        public InsightsClient(string applicationId, string apiKey, string region = "us") :
            this(new InsightsConfig(applicationId, apiKey, region))
        {
        }

        /// <summary>
        /// Initialize a new insights client with configuration
        /// </summary>
        /// <param name="config"></param>
        public InsightsClient(InsightsConfig config) : this(config, new AlgoliaHttpRequester())
        {
        }

        /// <summary>
        /// Initialize a new insights client with configuration and custom httpRequester
        /// </summary>
        /// <param name="config"></param>
        /// <param name="httpRequester"></param>
        public InsightsClient(InsightsConfig config, IHttpRequester httpRequester)
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
        public UserInsightsClient User(string userToken)
        {
            return new UserInsightsClient(userToken, this);
        }

        /// <inheritdoc />
        public InsightsResponse SendEvent(InsightsEvent insightEvent, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SendEventAsync(insightEvent, requestOptions));

        /// <inheritdoc />
        public InsightsResponse SendEvents(IEnumerable<InsightsEvent> insightEvents,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SendEventsAsync(insightEvents, requestOptions));

        /// <inheritdoc />
        public async Task<InsightsResponse> SendEventAsync(InsightsEvent insightEvent,
            RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            if (insightEvent == null)
            {
                throw new ArgumentNullException(nameof(insightEvent));
            }

            return await SendEventsAsync(new List<InsightsEvent> { insightEvent }, requestOptions, ct);
        }

        /// <inheritdoc />
        public async Task<InsightsResponse> SendEventsAsync(IEnumerable<InsightsEvent> insightEvents,
            RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            if (insightEvents == null)
            {
                throw new ArgumentNullException(nameof(insightEvents));
            }

            var request = new InsightsRequest { Events = insightEvents };

            return await _transport.ExecuteRequestAsync<InsightsResponse, InsightsRequest>(HttpMethod.Post,
                    "/1/events", CallType.Write, request, requestOptions, ct)
                .ConfigureAwait(false);
        }
    }
}

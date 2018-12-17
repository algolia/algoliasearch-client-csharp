using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Algolia.Search
{
    /// <summary>
    /// Insights client
    /// </summary>
    public class InsightsClient
    {
        private readonly AlgoliaClient _client;
        protected internal readonly InsightsConfig _config;
        private readonly string _baseUrl;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationId">The application ID</param>
        /// <param name="apiKey">The api Key</param>
        /// <param name="region">Server's region</param>
        /// <param name="userToken">the user token</param>
        /// <param name="client">Algolia's client</param>
        /// <returns></returns>
        public InsightsClient(string applicationId, string apiKey, string region = "us") :
            this(new InsightsConfig { AppId = applicationId, ApiKey = apiKey, Region = region })
        {

        }

        /// <summary>
        /// Initialize a new insights client with configuration
        /// </summary>
        /// <param name="config"></param>
        /// <param name="client"></param>
        public InsightsClient(InsightsConfig config)
        {
            _config = config;
            _baseUrl = $"insights.{_config.Region}.algolia.io";
            _client = new AlgoliaClient(config.AppId, config.ApiKey, new List<string> { _baseUrl });
        }

        /// <summary>
        /// Set the user token
        /// </summary>
        /// <param name="userToken">The user token</param>
        /// <returns></returns>
        public UserInsightsClient User(string userToken)
        {
            return new UserInsightsClient(userToken, this);
        }

        /// <summary>
        /// This command pushes an event to the Insights API.
        /// </summary>
        /// <param name="insightEvent"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public InsightsResponse SendEvent(InsightsEvent insightEvent, RequestOptions requestOptions = null)
        {
            return SendEventAsync(insightEvent, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// This command pushes an event to the Insights API.
        /// </summary>
        /// <param name="insightEvents"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public InsightsResponse SendEvents(IEnumerable<InsightsEvent> insightEvents, RequestOptions requestOptions = null)
        {
            return SendEventsAsync(insightEvents, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// This command pushes an event to the Insights API.
        /// </summary>
        /// <param name="insightEvent"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<InsightsResponse> SendEventAsync(InsightsEvent insightEvent, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            if (insightEvent == null)
            {
                throw new ArgumentNullException(nameof(insightEvent));
            }

            return await SendEventsAsync(new List<InsightsEvent> { insightEvent }, requestOptions, token);
        }

        /// <summary>
        /// This command pushes an array of events to the Insights API.
        /// </summary>
        /// <param name="insightEvents"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<InsightsResponse> SendEventsAsync(IEnumerable<InsightsEvent> insightEvents, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            if (insightEvents == null)
            {
                throw new ArgumentNullException(nameof(insightEvents));
            }

            var request = new InsightsRequest { Events = insightEvents };
            var json = JObject.FromObject(request);

            JObject response = await _client.ExecuteRequest(AlgoliaClient.callType.Insights, "POST", "/1/events", request, token, requestOptions);

            return response.ToObject<InsightsResponse>();
        }
    }
}
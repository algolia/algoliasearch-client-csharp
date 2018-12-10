using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Models;

namespace Algolia.Search
{
    public class UserInsightsClient
    {
        private readonly string _userToken;
        private readonly InsightsClient _insightsClient;

        public UserInsightsClient(string userToken, InsightsClient insightsClient)
        {
            _userToken = userToken;
            _insightsClient = insightsClient;
        }

        // Click

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="filters"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public InsightsResponse ClickedFilters(string eventName, string indexName, IEnumerable<string> filters, RequestOptions requestOptions = null)
        {
            return ClickedFiltersAsync(eventName, indexName, filters, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="filters"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<InsightsResponse> ClickedFiltersAsync(string eventName, string indexName, IEnumerable<string> filters, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            var insightEvent = new InsightsEvent
            {
                EventType = "click",
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                Filters = filters
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="objectIDs"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public InsightsResponse ClickedObjectIDs(string eventName, string indexName, IEnumerable<string> objectIDs, RequestOptions requestOptions = null)
        {
            return ClickedObjectIDsAsync(eventName, indexName, objectIDs, requestOptions).GetAwaiter().GetResult();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="objectIDs"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<InsightsResponse> ClickedObjectIDsAsync(string eventName, string indexName, IEnumerable<string> objectIDs, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            var insightEvent = new InsightsEvent
            {
                EventType = "click",
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                ObjectIDs = objectIDs
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="objectIDs"></param>
        /// <param name="positions"></param>
        /// <param name="queryID"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public InsightsResponse ClickedObjectIDsAfterSearch(string eventName, string indexName, IEnumerable<string> objectIDs, IEnumerable<uint> positions, string queryID, RequestOptions requestOptions = null)
        {
            return ClickedObjectIDsAfterSearchAsync(eventName, indexName, objectIDs, positions, queryID, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="objectIDs"></param>
        /// <param name="positions"></param>
        /// <param name="queryID"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<InsightsResponse> ClickedObjectIDsAfterSearchAsync(string eventName, string indexName, IEnumerable<string> objectIDs, IEnumerable<uint> positions, string queryID, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            var insightEvent = new InsightsEvent
            {
                EventType = "click",
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                ObjectIDs = objectIDs,
                Positions = positions,
                QueryID = queryID
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, token);
        }

        // Conversion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="objectIDs"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public InsightsResponse ConvertedObjectIDs(string eventName, string indexName, IEnumerable<string> objectIDs, RequestOptions requestOptions = null)
        {
            return ConvertedObjectIDsAsync(eventName, indexName, objectIDs, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="objectIDs"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<InsightsResponse> ConvertedObjectIDsAsync(string eventName, string indexName, IEnumerable<string> objectIDs, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            var insightEvent = new InsightsEvent
            {
                EventType = "conversion",
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                ObjectIDs = objectIDs
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="objectIDs"></param>
        /// <param name="queryID"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public InsightsResponse ConvertedObjectIDsAfterSearch(string eventName, string indexName, IEnumerable<string> objectIDs, string queryID, RequestOptions requestOptions = null)
        {
            return ConvertedObjectIDsAfterSearchAsync(eventName, indexName, objectIDs, queryID, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="objectIDs"></param>
        /// <param name="queryID"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<InsightsResponse> ConvertedObjectIDsAfterSearchAsync(string eventName, string indexName, IEnumerable<string> objectIDs, string queryID, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            var insightEvent = new InsightsEvent
            {
                EventType = "conversion",
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                ObjectIDs = objectIDs,
                QueryID = queryID
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, token);
        }

        // View

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="filters"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public InsightsResponse ViewedFilters(string eventName, string indexName, IEnumerable<string> filters, RequestOptions requestOptions = null)
        {
            return ViewedFiltersAsync(eventName, indexName, filters, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="filters"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<InsightsResponse> ViewedFiltersAsync(string eventName, string indexName, IEnumerable<string> filters, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            var insightEvent = new InsightsEvent
            {
                EventType = "view",
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                Filters = filters
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="objectIDs"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public InsightsResponse ViewedObjectIDs(string eventName, string indexName, IEnumerable<string> objectIDs, RequestOptions requestOptions = null)
        {
            return ViewedObjectIDsAsync(eventName, indexName, objectIDs, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="indexName"></param>
        /// <param name="objectIDs"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<InsightsResponse> ViewedObjectIDsAsync(string eventName, string indexName, IEnumerable<string> objectIDs, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            var insightEvent = new InsightsEvent
            {
                EventType = "view",
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                ObjectIDs = objectIDs
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, token);
        }
    }
}
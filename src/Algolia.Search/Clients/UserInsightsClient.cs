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
using Algolia.Search.Utils;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Describe a user identifier class.
    /// </summary>
    public class UserInsightsClient
    {
        private readonly string _userToken;
        private readonly InsightsClient _insightsClient;

        /// <summary>
        /// Create an instance for a user identifier
        /// </summary>
        /// <param name="userToken">A user identifier, format [a-zA-Z0-9_-]{1,64}.</param>
        /// <param name="insightsClient">The insights client</param>
        public UserInsightsClient(string userToken, InsightsClient insightsClient)
        {
            _userToken = userToken;
            _insightsClient = insightsClient;
        }

        // Click

        /// <summary>
        /// Send events when filters are clicked
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="filters">A filter is defined by the ${attr}${op}${value} string e.g. brand:apple. Limited to 10 filters.</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public InsightsResponse ClickedFilters(string eventName, string indexName, IEnumerable<string> filters,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ClickedFiltersAsync(eventName, indexName, filters, requestOptions));

        /// <summary>
        /// Send events when filters are clicked
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="filters">A filter is defined by the ${attr}${op}${value} string e.g. brand:apple. Limited to 10 filters.</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns></returns>
        public async Task<InsightsResponse> ClickedFiltersAsync(string eventName, string indexName,
            IEnumerable<string> filters, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            var insightEvent = new InsightsEvent
            {
                EventType = EventType.Click,
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                Filters = filters
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Send events when objectIDs are clicked
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="objectIDs">Array of index objectIDs. Limited to 20 objects</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public InsightsResponse ClickedObjectIDs(string eventName, string indexName, IEnumerable<string> objectIDs,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ClickedObjectIDsAsync(eventName, indexName, objectIDs, requestOptions));

        /// <summary>
        /// Send events when objectIDs are clicked
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="objectIDs">Array of index objectIDs. Limited to 20 objects</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns></returns>
        public async Task<InsightsResponse> ClickedObjectIDsAsync(string eventName, string indexName,
            IEnumerable<string> objectIDs, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            var insightEvent = new InsightsEvent
            {
                EventType = EventType.Click,
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                ObjectIDs = objectIDs
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Send events after a search when objectIDs are clicked
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="objectIDs">Array of index objectIDs. Limited to 20 objects</param>
        /// <param name="positions">Position of the click in the list of Algolia search results</param>
        /// <param name="queryID">Algolia queryID, format: [a-z1-9]{32}.</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public InsightsResponse ClickedObjectIDsAfterSearch(string eventName, string indexName,
            IEnumerable<string> objectIDs, IEnumerable<uint> positions, string queryID,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() =>
                ClickedObjectIDsAfterSearchAsync(eventName, indexName, objectIDs, positions, queryID, requestOptions));

        /// <summary>
        /// Send events after a search when objectIDs are clicked
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="objectIDs">Array of index objectIDs. Limited to 20 objects</param>
        /// <param name="positions">Position of the click in the list of Algolia search results</param>
        /// <param name="queryID">Algolia queryID, format: [a-z1-9]{32}.</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns></returns>
        public async Task<InsightsResponse> ClickedObjectIDsAfterSearchAsync(string eventName, string indexName,
            IEnumerable<string> objectIDs, IEnumerable<uint> positions, string queryID,
            RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            var insightEvent = new InsightsEvent
            {
                EventType = EventType.Click,
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                ObjectIDs = objectIDs,
                Positions = positions,
                QueryID = queryID
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, ct).ConfigureAwait(false);
        }

        // Conversion

        /// <summary>
        /// Send events of converted objectIDs
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="objectIDs">Array of index objectIDs. Limited to 20 objects</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public InsightsResponse ConvertedObjectIDs(string eventName, string indexName, IEnumerable<string> objectIDs,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ConvertedObjectIDsAsync(eventName, indexName, objectIDs, requestOptions));

        /// <summary>
        /// Send events of converted objectIDs
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="objectIDs">Array of index objectIDs. Limited to 20 objects</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns></returns>
        public async Task<InsightsResponse> ConvertedObjectIDsAsync(string eventName, string indexName,
            IEnumerable<string> objectIDs, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            var insightEvent = new InsightsEvent
            {
                EventType = EventType.Conversion,
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                ObjectIDs = objectIDs
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Send events of converted objectIDs after a search
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="objectIDs">Array of index objectIDs. Limited to 20 objects</param>
        /// <param name="queryID">Algolia queryID, format: [a-z1-9]{32}.</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public InsightsResponse ConvertedObjectIDsAfterSearch(string eventName, string indexName,
            IEnumerable<string> objectIDs, string queryID, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() =>
                ConvertedObjectIDsAfterSearchAsync(eventName, indexName, objectIDs, queryID, requestOptions));

        /// <summary>
        /// Send events of converted objectIDs after a search
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="objectIDs">Array of index objectIDs. Limited to 20 objects</param>
        /// <param name="queryID">Algolia queryID, format: [a-z1-9]{32}.</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns></returns>
        public async Task<InsightsResponse> ConvertedObjectIDsAfterSearchAsync(string eventName, string indexName,
            IEnumerable<string> objectIDs, string queryID, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            var insightEvent = new InsightsEvent
            {
                EventType = EventType.Conversion,
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                ObjectIDs = objectIDs,
                QueryID = queryID
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Send events of filters conversion
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="filters">A filter is defined by the ${attr}${op}${value} string e.g. brand:apple. Limited to 10 filters.</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns></returns>
        public InsightsResponse ConvertedFilters(string eventName, string indexName, IEnumerable<string> filters,
            RequestOptions requestOptions = null, CancellationToken ct = default) =>
            AsyncHelper.RunSync(() => ConvertedFiltersAsync(eventName, indexName, filters, requestOptions, ct));

        /// <summary>
        /// Send events of filters conversion
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="filters">A filter is defined by the ${attr}${op}${value} string e.g. brand:apple. Limited to 10 filters.</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns></returns>
        public async Task<InsightsResponse> ConvertedFiltersAsync(string eventName, string indexName,
            IEnumerable<string> filters, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            var insightEvent = new InsightsEvent
            {
                EventType = EventType.Conversion,
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                Filters = filters,
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, ct).ConfigureAwait(false);
        }

        // View

        /// <summary>
        /// Send events when filters are viewed
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="filters">A filter is defined by the ${attr}${op}${value} string e.g. brand:apple. Limited to 10 filters.</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public InsightsResponse ViewedFilters(string eventName, string indexName, IEnumerable<string> filters,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ViewedFiltersAsync(eventName, indexName, filters, requestOptions));

        /// <summary>
        /// Send events when filters are viewed
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="filters">A filter is defined by the ${attr}${op}${value} string e.g. brand:apple. Limited to 10 filters.</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns></returns>
        public async Task<InsightsResponse> ViewedFiltersAsync(string eventName, string indexName,
            IEnumerable<string> filters, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            var insightEvent = new InsightsEvent
            {
                EventType = EventType.View,
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                Filters = filters
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Send events when objectIDs are viewed
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="objectIDs">Array of index objectIDs. Limited to 20 objects</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        public InsightsResponse ViewedObjectIDs(string eventName, string indexName, IEnumerable<string> objectIDs,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ViewedObjectIDsAsync(eventName, indexName, objectIDs, requestOptions));

        /// <summary>
        /// Send events when objectIDs are viewed
        /// </summary>
        /// <param name="eventName">User defined string, format: any ascii char except control points with length between 1 and 64.</param>
        /// <param name="indexName">Index name, format, same as the engine.</param>
        /// <param name="objectIDs">Array of index objectIDs. Limited to 20 objects</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns></returns>
        public async Task<InsightsResponse> ViewedObjectIDsAsync(string eventName, string indexName,
            IEnumerable<string> objectIDs, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            var insightEvent = new InsightsEvent
            {
                EventType = EventType.View,
                UserToken = _userToken,
                EventName = eventName,
                Index = indexName,
                ObjectIDs = objectIDs
            };

            return await _insightsClient.SendEventAsync(insightEvent, requestOptions, ct).ConfigureAwait(false);
        }
    }
}

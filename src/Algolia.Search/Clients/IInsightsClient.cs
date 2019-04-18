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
using Algolia.Search.Models.Insights;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Interface for Insights client
    /// </summary>
    public interface IInsightsClient
    {
        /// <summary>
        /// Set the user token
        /// </summary>
        /// <param name="userToken">The user token</param>
        UserInsightsClient User(string userToken);

        /// <summary>
        /// This command pushes an event to the Insights API.
        /// </summary>
        /// <param name="insightEvent">An insight event</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        InsightsResponse SendEvent(InsightsEvent insightEvent, RequestOptions requestOptions = null);

        /// <summary>
        /// This command pushes an event to the Insights API.
        /// </summary>
        /// <param name="insightEvents">A list of insights events</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        InsightsResponse SendEvents(IEnumerable<InsightsEvent> insightEvents, RequestOptions requestOptions = null);

        /// <summary>
        /// This command pushes an event to the Insights API.
        /// </summary>
        /// <param name="insightEvent">An insight event</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns></returns>
        Task<InsightsResponse> SendEventAsync(InsightsEvent insightEvent, RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// This command pushes an array of events to the Insights API.
        /// </summary>
        /// <param name="insightEvents">A list of insights events</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">The cancellation token</param>
        /// <returns></returns>
        Task<InsightsResponse> SendEventsAsync(IEnumerable<InsightsEvent> insightEvents,
            RequestOptions requestOptions = null, CancellationToken ct = default);
    }
}

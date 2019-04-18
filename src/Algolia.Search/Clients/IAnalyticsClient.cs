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
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Analytics client
    /// </summary>
    public interface IAnalyticsClient
    {
        /// <summary>
        /// Get an A/B test information and results.
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        ABTest GetABTest(long abTestId, RequestOptions requestOptions = null);

        /// <summary>
        /// Get an A/B test information and results.
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<ABTest> GetABTestAsync(long abTestId, RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Fetch all existing AB Tests for App that are available for the current API Key. Returns an array of metadata and metrics.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        ABTestsReponse GetABTests(int offset = 0, int limit = 10, RequestOptions requestOptions = null);

        /// <summary>
        /// Fetch all existing AB Tests for App that are available for the current API Key. Returns an array of metadata and metrics.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<ABTestsReponse> GetABTestsAsync(int offset = 0, int limit = 10, RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Creates a new AB Test with provided configuration.
        /// </summary>
        /// <param name="aBTest"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        AddABTestResponse AddABTest(ABTest aBTest, RequestOptions requestOptions = null);

        /// <summary>
        /// Creates a new AB Test with provided configuration.
        /// </summary>
        /// <param name="aBTest"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<AddABTestResponse> AddABTestAsync(ABTest aBTest, RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Marks the A/B Test as stopped. At this point, the test is over and cannot be restarted.
        /// As a result, your application is back to normal: index A will perform as usual, receiving 100% of all search requests.
        /// Associated metadata and metrics are still stored
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        StopABTestResponse StopABTest(long abTestId, RequestOptions requestOptions = null);

        /// <summary>
        /// Marks the A/B Test as stopped. At this point, the test is over and cannot be restarted.
        /// As a result, your application is back to normal: index A will perform as usual, receiving 100% of all search requests.
        /// Associated metadata and metrics are still stored
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<StopABTestResponse> StopABTestAsync(long abTestId, RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Deletes the A/B Test and removes all associated metadata and metrics.
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        DeleteABTestResponse DeleteABTest(long abTestId, RequestOptions requestOptions = null);

        /// <summary>
        /// Deletes the A/B Test and removes all associated metadata and metrics.
        /// </summary>
        /// <param name="abTestId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<DeleteABTestResponse> DeleteABTestAsync(long abTestId, RequestOptions requestOptions = null,
            CancellationToken ct = default);
    }
}

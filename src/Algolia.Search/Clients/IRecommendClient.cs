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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Http;
using Algolia.Search.Models.Recommend;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Recommend Client interface
    /// </summary>
    public interface IRecommendClient
    {
        /// <summary>
        /// Get recommendations for given objects
        /// </summary>
        /// <param name="requests">Object ID and index pairs to retreive recommendations for</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        RecommendResponse<T> GetRecommendations<T>(IEnumerable<RecommendRequest> requests,
            RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// Get recommendations for given objects
        /// </summary>
        /// <param name="requests">Object ID and index pairs to retreive recommendations for</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Task CancellationToken</param>
        Task<RecommendResponse<T>> GetRecommendationsAsync<T>(IEnumerable<RecommendRequest> requests,
            RequestOptions requestOptions = null, CancellationToken ct = default) where T : class;

        /// <summary>
        /// Get recommendations for given objects
        /// </summary>
        /// <param name="requests">Object ID and index pairs to retreive recommendations for</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        RecommendResponse<T> GetRelatedProducts<T>(IEnumerable<RelatedProductsRequest> requests,
            RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// Get recommendations for given objects
        /// </summary>
        /// <param name="requests">Object ID and index pairs to retreive recommendations for</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Task CancellationToken</param>
        Task<RecommendResponse<T>> GetRelatedProductsAsync<T>(IEnumerable<RelatedProductsRequest> requests,
            RequestOptions requestOptions = null, CancellationToken ct = default) where T : class;

        /// <summary>
        /// Get recommendations for given objects
        /// </summary>
        /// <param name="requests">Object ID and index pairs to retreive recommendations for</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        RecommendResponse<T> GetFrequentlyBoughtTogether<T>(IEnumerable<BoughtTogetherRequest> requests,
            RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// Get recommendations for given objects
        /// </summary>
        /// <param name="requests">Object ID and index pairs to retreive recommendations for</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Task CancellationToken</param>
        Task<RecommendResponse<T>> GetFrequentlyBoughtTogetherAsync<T>(IEnumerable<BoughtTogetherRequest> requests,
            RequestOptions requestOptions = null, CancellationToken ct = default) where T : class;
    }
}

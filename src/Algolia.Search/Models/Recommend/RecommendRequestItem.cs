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
using System.Collections.Generic;

namespace Algolia.Search.Models.Recommend
{
    /// <summary>
    /// Single item for <see cref="RecommendRequest"/>
    /// </summary>
    public class RecommendRequestItem

    {
        /// <summary>
        /// Required. Name of the index to target.
        /// </summary>
        public string IndexName { get; set; }

        //todo: convert to enum
        /// <summary>
        /// Required. The recommendation model to use, either "related-products" or "bought-together"
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Required. The objectID to get recommendations for.
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// Optional. The threshold to use when filtering recommendations by their score.
        /// </summary>
        public long Threshold { get; set; } = 0;

        /// <summary>
        /// Optional. The maximum number of recommendations to retrieve.
        /// </summary>
        public long MaxRecommendations { get; set; } = 3;

        /// <summary>
        /// Optional. A key-value mapping of search parameters to filter the recommendations.
        /// </summary>
        public Dictionary<string, string> QueryParameters { get; set; }

        /// <summary>
        /// Optional. A key-value mapping of search parameters to use as fallback when there are no recommendations.
        /// </summary>
        public Dictionary<string, string> FallbackParameters { get; set; }
    }
}

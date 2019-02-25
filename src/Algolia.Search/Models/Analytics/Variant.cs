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

using Algolia.Search.Models.Search;

namespace Algolia.Search.Models.Analytics
{
    /// <summary>
    /// https://www.algolia.com/doc/api-reference/api-methods/add-ab-test/#method-param-variant
    /// </summary>
    public class Variant
    {
        /// <summary>
        /// The index name
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// Percentage of the traffic that should be going to the variant. The sum of the percentage should be equal to 100.
        /// </summary>
        public int? TrafficPercentage { get; set; }

        /// <summary>
        /// Description of the variant. This is useful when seing the results in the dashboard or via the API.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Average click position for the variant.
        /// </summary>
        public int? AverageClickPostion { get; set; }

        /// <summary>
        /// Distinct click count for the variant.
        /// </summary>
        public int? ClickCount { get; set; }

        /// <summary>
        /// Click through rate for the variant.
        /// </summary>
        public float? ClickThroughRate { get; set; }

        /// <summary>
        /// Click through rate for the variant.
        /// </summary>
        public int? ConversionCount { get; set; }

        /// <summary>
        /// Distinct conversion count for the variant.
        /// </summary>
        public float? ConversionRate { get; set; }

        /// <summary>
        /// No result count
        /// </summary>
        public int? NoResultCount { get; set; }

        /// <summary>
        /// Search count
        /// </summary>
        public int? SearchCount { get; set; }

        /// <summary>
        /// user Count
        /// </summary>
        public int? UserCount { get; set; }

        /// <summary>
        /// Search parameters for AA Testing
        /// </summary>
        public Query CustomSearchParameters { get; set; }
    }
}
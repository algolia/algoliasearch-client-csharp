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

namespace Algolia.Search.Models.Search
{
    /// <summary>
    /// https://www.algolia.com/doc/api-reference/api-methods/search-for-facet-values/
    /// </summary>
    public class SearchForFacetResponse
    {
        /// <summary>
        /// List of facet hit
        /// </summary>
        public IEnumerable<FacetHit> FacetHits { get; set; }

        /// <summary>
        /// Whether the count returned for each facetHit is exhaustive.
        /// </summary>
        public bool ExhaustiveFacetsCount { get; set; }

        /// <summary>
        /// Processing time.
        /// </summary>
        public int ProcessingTimeMS { get; set; }
    }

    /// <summary>
    /// https://www.algolia.com/doc/api-reference/api-methods/search-for-facet-values/
    /// </summary>
    public class FacetHit
    {
        /// <summary>
        /// Facet value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Highlighted value.
        /// </summary>
        public string Highlighted { get; set; }

        /// <summary>
        /// Number of times the value is present in the dataset.
        /// </summary>
        public long Count { get; set; }
    }
}
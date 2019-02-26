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

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Algolia.Search.Models.Search
{
    /// <summary>
    /// Search response
    /// https://www.algolia.com/doc/api-reference/api-methods/search/
    /// </summary>
    /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
    public class SearchResponse<T> where T : class
    {
        /// <summary>
        /// The hits returned by the search.
        /// Hits are ordered according to the ranking or sorting of the index being queried.
        /// </summary>
        public List<T> Hits { get; set; }

        /// <summary>
        /// Index of the current page (zero-based).
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        ///  Number of hits returned (used only with offset).
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// The offset of the first hit to returned.
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Number of hits matched by the query.
        /// </summary>
        public int NbHits { get; set; }

        /// <summary>
        /// Number of pages returned.
        /// Calculation is based on the total number of hits (nbHits) divided by the number of hits per page (hitsPerPage), rounded up to the nearest integer.
        /// </summary>
        public int NbPages { get; set; }

        /// <summary>
        /// Maximum number of hits returned per page.
        /// </summary>
        public int HitsPerPage { get; set; }

        /// <summary>
        /// Time the server took to process the request, in milliseconds. This does not include network time.
        /// </summary>
        public int ProcessingTimeMs { get; set; }

        /// <summary>
        /// Whether the nbHits is exhaustive (true) or approximate (false).
        /// An approximation is done when the query takes more than 50ms to be processed (this can happen when using complex filters on millions on records).
        /// </summary>
        public bool? ExhaustiveNbHits { get; set; }

        /// <summary>
        /// An echo of the query text.
        /// </summary>
        public bool? ExhaustiveFacetsCount { get; set; }

        /// <summary>
        /// A mapping of each facet name to the corresponding facet counts.
        /// </summary>
        public Dictionary<string, Dictionary<string, long>> Facets { get; set; }

        /// <summary>
        /// Statistics for numerical facets.
        /// </summary>
        [JsonPropertyAttribute("facets_stats")]
        public Dictionary<string, FacetStats> FacetsStats { get; set; }

        /// <summary>
        /// An echo of the query text.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// A markup text indicating which parts of the original query have been removed in order to retrieve a non-empty result set.
        /// </summary>
        public string QueryAfterRemoval { get; set; }

        /// <summary>
        /// A url-encoded string of all search parameters.
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        ///  QueryID
        /// </summary>
        public string QueryID { get; set; }

        /// <summary>
        /// Used to return warnings about the query.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The computed geo location. Warning: for legacy reasons, this parameter is a string and not an object.
        /// Format: ${lat},${lng}, where the latitude and longitude are expressed as decimal floating point number
        /// </summary>
        public string AroundLatLng { get; set; }

        /// <summary>
        /// The automatically computed radius. For legacy reasons, this parameter is a string and not an integer..
        /// </summary>
        public string AutomaticRadius { get; set; }

        /// <summary>
        /// Actual host name of the server that processed the request.
        /// Our DNS supports automatic failover and load balancing, so this may differ from the host name used in the request.
        /// </summary>
        public string ServerUsed { get; set; }

        /// <summary>
        ///  Index name used for the query.
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        ///  Index name used for the query. In case of AB test, the index targetted isn’t always the index used by the query.
        /// </summary>
        public string IndexUsed { get; set; }

        /// <summary>
        ///  In case of AB test, reports the variant ID used. The variant ID is the position in the array of variants (starting at 1).
        /// </summary>
        public int AbTestVariantID { get; set; }

        /// <summary>
        ///  The query string that will be searched, after normalization.
        /// </summary>
        public string ParsedQuery { get; set; }

        /// <summary>
        /// Custom user data
        /// </summary>
        public object UserData { get; set; }

        /// <summary>
        /// Rules applied to the query
        /// </summary>
        public IEnumerable<Dictionary<string, object>> AppliedRules { get; set; }
    }
}
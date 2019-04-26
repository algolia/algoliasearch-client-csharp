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

using Algolia.Search.Serializer;
using Algolia.Search.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Algolia.Search.Models.Search
{
    /// <summary>
    /// For more information regarding the parameters
    /// https://www.algolia.com/doc/api-reference/search-api-parameters/
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Create a new query with an empty search query
        /// </summary>
        /// <param name="searchQuery"></param>
        public Query(string searchQuery = null)
        {
            SearchQuery = searchQuery;
        }

        /// <summary>
        /// The text to search in the index.
        /// </summary>
        [JsonProperty(PropertyName = "query")]
        public string SearchQuery { get; set; } = string.Empty;

        // filtering

        /// <summary>
        /// Filter hits by facet value.
        /// </summary>
        public IEnumerable<IEnumerable<string>> FacetFilters { get; set; }

        /// <summary>
        /// Create filters for ranking purposes, where records that match the filter are ranked highest.
        /// </summary>
        public IEnumerable<IEnumerable<string>> OptionalFilters { get; set; }

        /// <summary>
        /// Filter on numeric attributes.
        /// </summary>
        public IEnumerable<IEnumerable<string>> NumericFilters { get; set; }

        /// <summary>
        /// Filter hits by tags.
        /// tagFilters is a different way of filtering, which relies on the _tags attribute. It uses a simpler syntax than filters.
        /// You can use it when you want to do simple filtering based on tags.
        /// </summary>
        public IEnumerable<IEnumerable<string>> TagFilters { get; set; }

        /// <summary>
        /// Determines how to calculate the total score for filtering.
        /// </summary>
        public bool? SumOrFiltersScores { get; set; }

        /// <summary>
        /// Filter the query with numeric, facet and/or tag filters.
        /// </summary>
        public string Filters { get; set; }

        // Pagination

        /// <summary>
        /// Specify the page to retrieve.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Set the number of hits per page.
        /// </summary>
        public int? HitsPerPage { get; set; }

        /// <summary>
        /// Specify the offset of the first hit to return.
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        ///  Set the number of hits to retrieve (used only with offset).
        /// </summary>
        public int? Length { get; set; }

        // Highlighting-snippet

        /// <summary>
        /// List of attributes to highlight.
        /// </summary>
        public IEnumerable<string> AttributesToHighlight { get; set; }

        /// <summary>
        /// List of attributes to snippet, with an optional maximum number of words to snippet.
        /// </summary>
        public IEnumerable<string> AttributesToSnippet { get; set; }

        /// <summary>
        /// Gives control over which attributes to retrieve and which not to retrieve.
        /// </summary>
        public IEnumerable<string> AttributesToRetrieve { get; set; }

        /// <summary>
        /// The HTML string to insert before the highlighted parts in all highlight and snippet results.
        /// </summary>
        public string HighlightPreTag { get; set; }

        /// <summary>
        /// The HTML string to insert after the highlighted parts in all highlight and snippet results
        /// </summary>
        public string HighlightPostTag { get; set; }

        /// <summary>
        /// String used as an ellipsis indicator when a snippet is truncated.
        /// </summary>
        public string SnippetEllipsisText { get; set; }

        /// <summary>
        /// Restrict highlighting and snippeting to items that matched the query.
        /// </summary>
        public bool? RestrictHighlightAndSnippetArrays { get; set; }

        // Faceting

        /// <summary>
        /// Facets to retrieve.
        /// </summary>
        public IEnumerable<string> Facets { get; set; }

        /// <summary>
        /// Maximum number of facet values to return for each facet during a regular search.
        /// </summary>
        public long? MaxValuesPerFacet { get; set; }

        /// <summary>
        /// Force faceting to be applied after de-duplication (via the Distinct setting).
        /// </summary>
        public bool? FacetingAfterDistinct { get; set; }

        // typos

        /// <summary>
        /// Minimum number of characters a word in the query string must contain to accept matches with 1 typo
        /// </summary>
        public int? MinWordSizefor1Typo { get; set; }

        /// <summary>
        /// Minimum number of characters a word in the query string must contain to accept matches with 2 typos.
        /// </summary>
        public int? MinWordSizefor2Typos { get; set; }

        /// <summary>
        /// Whether to allow typos on numbers (“numeric tokens”) in the query string.
        /// </summary>
        public bool? AllowTyposOnNumericTokens { get; set; }

        /// <summary>
        /// List of attributes on which you want to disable typo tolerance.
        /// </summary>
        public IEnumerable<string> DisableTypoToleranceOnAttributes { get; set; }

        // languages

        // query strategy

        /// <summary>
        /// Controls if and how query words are interpreted as prefixes.
        /// </summary>
        public string QueryType { get; set; }

        /// <summary>
        /// Selects a strategy to remove words from the query when it doesn’t match any hits.
        /// </summary>
        public string RemoveWordsIfNoResults { get; set; }

        /// <summary>
        /// Enables the advanced query syntax.
        /// </summary>
        public bool? AdvancedSyntax { get; set; }

        /// <summary>
        /// A list of words that should be considered as optional when found in the query.
        /// </summary>
        public IEnumerable<string> OptionalWords { get; set; }

        /// <summary>
        /// List of attributes on which you want to disable the exact ranking criterion.
        /// </summary>
        public IEnumerable<string> DisableExactOnAttributes { get; set; }

        /// <summary>
        /// Controls how the exact ranking criterion is computed when the query contains only one word.
        /// </summary>
        public string ExactOnSingleWordQuery { get; set; }

        /// <summary>
        /// List of alternatives that should be considered an exact match by the exact ranking criterion.
        /// </summary>
        public IEnumerable<string> AlternativesAsExact { get; set; }

        // query rules

        /// <summary>
        /// Whether rules should be globally enabled.
        /// </summary>
        public bool? EnableRules { get; set; }

        /// <summary>
        /// Enables contextual rules.
        /// </summary>
        public IEnumerable<string> RuleContexts { get; set; }

        // advanced

        /// <summary>
        /// Enables de-duplication or grouping of results.
        /// </summary>
        public int? Distinct { get; set; }

        /// <summary>
        /// Whether the current query will be taken into account in the Analytics
        /// </summary>
        public bool? Analytics { get; set; }

        /// <summary>
        /// List of tags to apply to the query in the analytics.
        /// </summary>
        public IEnumerable<string> AnalyticsTags { get; set; }

        /// <summary>
        /// Whether to take into account an index’s synonyms for a particular search.
        /// </summary>
        public bool? Synonyms { get; set; }

        /// <summary>
        /// Whether to highlight and snippet the original word that matches the synonym or the synonym itself.
        /// </summary>
        public bool? ReplaceSynonymsInHighlight { get; set; }

        /// <summary>
        /// Precision of the proximity ranking criterion.
        /// </summary>
        public int? MinProximity { get; set; }

        /// <summary>
        /// Choose which fields the response will contain. Applies to search and browse queries.
        /// </summary>
        public IEnumerable<string> ResponseFields { get; set; }

        /// <summary>
        /// Maximum number of facet hits to return during a search for facet values.
        /// </summary>
        public int? MaxFacetHits { get; set; }

        /// <summary>
        /// Whether to include or exclude a query from the processing-time percentile computation.
        /// </summary>
        public bool? PercentileComputation { get; set; }

        /// <summary>
        /// Enable the Click Analytics feature.
        /// </summary>
        public bool? ClickAnalytics { get; set; }

        /// <summary>
        /// The `personalizationImpact` parameter sets the percentage of the impact that personalization has on ranking records.
        /// The value must be between 0 and 100 (inclusive)
        /// This parameter will not be taken into account if `enablePersonalization` is **false**.
        /// </summary>
        public int? PersonalizationImpact { get; set; }

        /// <summary>
        /// Enable personalization for the query
        /// </summary>
        public bool? EnablePersonalization { get; set; }

        /// <summary>
        /// Restricts a given query to look in only a subset of your searchable attributes.
        /// </summary>
        public List<string> RestrictSearchableAttributes { get; set; }

        /// <summary>
        /// Override default sort order for facet values at query time
        /// </summary>
        public string SortFacetValuesBy { get; set; }

        /// <summary>
        /// Controls whether typo tolerance is enabled and how it is applied.
        /// Could be string or bool
        /// </summary>
        [JsonConverter(typeof(MultiTypeObjectConverter))]
        public object TypoTolerance { get; set; }

        /// <summary>
        /// Search for entries around a central geolocation, enabling a geo search within a circular area.
        /// </summary>
        public string AroundLatLng { get; set; }

        /// <summary>
        /// Search for entries around a given location automatically computed from the requester’s IP address.
        /// </summary>
        public bool? AroundLatLngViaIP { get; set; }

        /// <summary>
        /// Define the maximum radius for a geo search (in meters).
        /// </summary>
        public object AroundRadius { get; set; }

        /// <summary>
        /// Precision of geo search (in meters), to add grouping by geo location to the ranking formula.
        /// </summary>
        public long? AroundPrecision { get; set; }

        /// <summary>
        /// Minimum radius (in meters) used for a geo search when aroundRadius is not set.
        /// </summary>
        public long? MinimumAroundRadius { get; set; }

        /// <summary>
        /// Search inside a rectangular area (in geo coordinates).
        /// </summary>
        public List<List<float>> InsideBoundingBox { get; set; }

        /// <summary>
        /// Search inside a polygon (in geo coordinates).
        /// </summary>
        public List<List<float>> InsidePolygon { get; set; }

        /// <summary>
        /// Treats singular, plurals, and other forms of declensions as matching terms.
        /// </summary>
        [JsonConverter(typeof(MultiTypeObjectConverter))]
        public object IgnorePlurals { get; set; }

        /// <summary>
        /// Removes stop (common) words from the query before executing it.
        /// </summary>
        public List<string> RemoveStopWords { get; set; }

        /// <summary>
        /// When true, each hit in the response contains an additional _rankingInfo object
        /// </summary>
        public bool? GetRankingInfo { get; set; }

        /// <summary>
        /// A user identifier.
        /// Format: alpha numeric string [a-zA-Z0-9_-]
        /// Length: between 1 and 64 characters.
        /// </summary>
        public string UserToken { get; set; }

        /// <summary>
        /// Returns the Query as a query string
        /// Example : "query= distinct=0"
        /// </summary>
        public string ToQueryString()
        {
            return QueryStringHelper.ToQueryString(this);
        }
    }
}

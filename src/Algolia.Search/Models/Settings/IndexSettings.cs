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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Algolia.Search.Models.Settings
{

    /// <summary>
    /// For more informations regarding Index settings
    /// https://www.algolia.com/doc/api-reference/settings-api-parameters/
    /// </summary>
#pragma warning disable IDE0051 // disable warning for unused private members
    public class IndexSettings
    {
        // Attributes

        /// <summary>
        /// The complete list of attributes that will be used for searching.
        /// </summary>
        public List<string> SearchableAttributes { get; set; }

        // Handling legacy index settings
        [JsonProperty("attributesToIndex")]
        private List<string> AttributesToIndex { set { if (value != null) { SearchableAttributes = value; } } }

        /// <summary>
        /// The complete list of attributes that will be used for faceting
        /// </summary>
        public List<string> AttributesForFaceting { get; set; }

        /// <summary>
        /// List of attributes that cannot be retrieved at query time.
        /// </summary>
        public List<string> UnretrievableAttributes { get; set; }

        /// <summary>
        /// Gives control over which attributes to retrieve and which not to retrieve.
        /// </summary>
        public List<string> AttributesToRetrieve { get; set; }

        // ranking

        /// <summary>
        /// Controls the way results are sorted.
        /// </summary>
        public List<string> Ranking { get; set; }

        /// <summary>
        /// Specifies the custom ranking criterion.
        /// </summary>
        public List<string> CustomRanking { get; set; }

        /// <summary>
        /// Creates replicas, exact copies of an index.
        /// </summary>
        public List<string> Replicas { get; set; }

        [JsonProperty("slaves")]
        private List<string> Slaves { set { if (value != null) { Replicas = value; } } }

        // faceting

        /// <summary>
        /// Maximum number of facet values to return for each facet during a regular search.
        /// </summary>
        public long? MaxValuesPerFacet { get; set; }

        /// <summary>
        /// Controls how facet values are sorted.
        /// </summary>
        public string SortFacetValuesBy { get; set; }

        // highlight snipetting

        /// <summary>
        /// List of attributes to highlight.
        /// </summary>
        public List<string> AttributesToHighlight { get; set; }

        /// <summary>
        /// List of attributes to snippet, with an optional maximum number of words to snippet
        /// </summary>
        public List<string> AttributesToSnippet { get; set; }

        /// <summary>
        /// The HTML string to insert before the highlighted parts in all highlight and snippet results.
        /// </summary>
        public string HighlightPreTag { get; set; }

        /// <summary>
        /// The HTML string to insert after the highlighted parts in all highlight and snippet results.
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

        // pagination

        /// <summary>
        /// Set the number of hits per page
        /// </summary>
        public long? HitsPerPage { get; set; }

        /// <summary>
        /// Set the maximum number of hits accessible via pagination.
        /// </summary>
        public long? PaginationLimitedTo { get; set; }

        // Typos

        /// <summary>
        /// Minimum number of characters a word in the query string must contain to accept matches with 1 typo.
        /// </summary>
        public int? MinWordSizefor1Typo { get; set; }

        /// <summary>
        /// Minimum number of characters a word in the query string must contain to accept matches with 2 typos.
        /// </summary>
        public int? MinWordSizefor2Typos { get; set; }

        /// <summary>
        /// Controls whether typo tolerance is enabled and how it is applied.
        /// Could be string or bool
        /// </summary>
        [JsonConverter(typeof(MultiTypeObjectConverter))]
        public object TypoTolerance { get; set; }

        /// <summary>
        /// Whether to allow typos on numbers (“numeric tokens”) in the query string.
        /// </summary>
        public bool? AllowTyposOnNumericTokens { get; set; }

        /// <summary>
        /// List of attributes on which you want to disable typo tolerance.
        /// </summary>
        public List<string> DisableTypoToleranceOnAttributes { get; set; }

        /// <summary>
        /// List of words on which you want to disable typo tolerance.
        /// </summary>
        public List<string> DisableTypoToleranceOnWords { get; set; }

        /// <summary>
        /// Control which separators are indexed.
        /// </summary>
        public string SeparatorsToIndex { get; set; }

        /// <summary>
        /// Treats singular, plurals, and other forms of declensions as matching terms.
        /// Could be string[] or bool
        /// </summary>
        [JsonConverter(typeof(MultiTypeObjectConverter))]
        public object IgnorePlurals { get; set; }

        // languages

        /// <summary>
        /// Sets the languages to be used by language-specific settings and functionalities such as ignorePlurals, removeStopWords, and CJK word-detection.
        /// </summary>
        public List<string> QueryLanguages { get; set; }

        // query rules

        /// <summary>
        /// Whether rules should be globally enabled.
        /// </summary>
        public bool? EnableRules { get; set; }

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
        /// AdvancedSyntaxFeatures can be exactPhrase or excludeWords
        /// </summary>
        public List<string> AdvancedSyntaxFeatures { get; set; }

        /// <summary>
        /// A list of words that should be considered as optional when found in the query.
        /// </summary>
        public List<string> OptionalWords { get; set; }

        /// <summary>
        /// List of attributes on which you want to disable prefix matching.
        /// </summary>
        public List<string> DisablePrefixOnAttributes { get; set; }

        /// <summary>
        /// List of attributes on which you want to disable the exact ranking criterion.
        /// </summary>
        public List<string> DisableExactOnAttributes { get; set; }

        /// <summary>
        /// Controls how the exact ranking criterion is computed when the query contains only one word.
        /// </summary>
        public string ExactOnSingleWordQuery { get; set; }

        /// <summary>
        /// List of alternatives that should be considered an exact match by the exact ranking criterion.
        /// </summary>
        public List<string> AlternativesAsExact { get; set; }

        /// <summary>
        /// Removes stop (common) words from the query before executing it.
        /// Could be string[] or bool
        /// </summary>
        [JsonConverter(typeof(MultiTypeObjectConverter))]
        public object RemoveStopWords { get; set; }

        // performance

        /// <summary>
        /// List of numeric attributes that can be used as numerical filters.
        /// </summary>
        public List<string> NumericAttributesForFiltering { get; set; }

        // Handling legacy index settings
        [JsonProperty("numericAttributesToIndex")]
        private List<string> NumericAttributesToIndex { set { if (value != null) { NumericAttributesForFiltering = value; } } }

        /// <summary>
        /// Enables compression of large integer arrays.
        /// </summary>
        public bool? AllowCompressionOfIntegerArray { get; set; }

        // advanced

        /// <summary>
        /// Name of the de-duplication attribute to be used with the distinct feature.
        /// </summary>
        public string AttributeForDistinct { get; set; }

        /// <summary>
        /// Enables de-duplication or grouping of results.
        /// Could be int or bool
        /// </summary>
        [JsonConverter(typeof(MultiTypeObjectConverter))]
        public object Distinct { get; set; }

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
        public List<string> ResponseFields { get; set; }

        /// <summary>
        /// Maximum number of facet hits to return during a search for facet values.
        /// </summary>
        public int? MaxFacetHits { get; set; }

        /// <summary>
        /// List of attributes on which to do a decomposition of camel case words.
        /// </summary>
        public List<string> CamelCaseAttributes { get; set; }

        /// <summary>
        /// Specify on which attributes in your index Algolia should apply word-splitting (“decompounding”)
        /// </summary>
        public Dictionary<string, List<string>> DecompoundedAttributes { get; set; }

        /// <summary>
        /// Characters that should not be automatically normalized by the search engine.
        /// </summary>
        public string KeepDiacriticsOnCharacters { get; set; }

        /// <summary>
        /// Index settings version only for advanced use cases
        /// </summary>

        /// <summary>
        /// Custom settings for advanced use cases
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> CustomSettings;
    }
}

/*
* Copyright (c) 2018 Algolia
* http://www.algolia.com/
* Based on the first version developed by Christopher Maneu under the same license:
*  https://github.com/cmaneu/algoliasearch-client-csharp
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

namespace Algolia.Search.Models.Settings
{
    /// <summary>
    /// For more informations regarding Index settings
    /// https://www.algolia.com/doc/api-reference/settings-api-parameters/
    /// </summary>
    public class IndexSettings
    {
        // Attributes
        public List<string> SearchableAttributes { get; set; }
        public List<string> AttributesForFaceting { get; set; }
        public List<string> UnretrievableAttributes { get; set; }
        public List<string> AttributesToRetrieve { get; set; }

        // ranking
        public List<string> Ranking { get; set; }
        public List<string> CustomRanking { get; set; }
        public List<string> Replicas { get; set; }

        // faceting
        public long? MaxValuesPerFacet { get; set; }
        public string SortFacetValuesBy { get; set; }

        // highlight snipetting
        public List<string> AttributesToHighlight { get; set; }
        public List<string> AttributesToSnippet { get; set; }
        public string HighlightPreTag { get; set; }
        public string HighlightPostTag { get; set; }
        public string SnippetEllipsisText { get; set; }
        public bool? RestrictHighlightAndSnippetArrays { get; set; }

        // pagination
        public long? HitsPerPage { get; set; }
        public long? PaginationLimitedTo { get; set; }

        // Typos
        public int? MinWordSizefor1Typo { get; set; }
        public int? MinWordSizefor2Typos { get; set; }
        public string TypoTolerance { get; set; }
        public bool? AllowTyposOnNumericTokens { get; set; }
        public List<string> DisableTypoToleranceOnAttributes { get; set; }
        public List<string> DisableTypoToleranceOnWords { get; set; }
        public string SeparatorsToIndex { get; set; }

        /// <summary>
        /// Could be string[] or bool
        /// </summary>
        public object IgnorePlurals { get; set; }

        // languages

        // query rules
        public bool? EnableRules { get; set; }

        // query strategy
        public string QueryType { get; set; }
        public string RemoveWordsIfNoResults { get; set; }
        public bool? AdvancedSyntax { get; set; }
        public List<string> OptionalWords { get; set; }
        public List<string> DisablePrefixOnAttributes { get; set; }
        public List<string> DisableExactOnAttributes { get; set; }
        public string ExactOnSingleWordQuery { get; set; }
        public List<string> AlternativesAsExact { get; set; }
        public bool? RemoveStopWords { get; set; }

        // performance
        public List<string> NumericAttributesForFiltering { get; set; }
        public bool? AllowCompressionOfIntegerArray { get; set; }

        // advanced
        public string AttributeForDistinct { get; set; }

        /// <summary>
        /// Could be int or bool
        /// </summary>
        public object Distinct { get; set; }
        public bool? ReplaceSynonymsInHighlight { get; set; }
        public int? MinProximity { get; set; }
        public List<string> ResponseFields { get; set; }
        public int? MaxFacetHits { get; set; }
        public List<string> CamelCaseAttributes { get; set; }
        public List<string> DecompoundedAttributes { get; set; }
        public string KeepDiacriticsOnCharacters { get; set; }
        
        // custom
        public int? Version { get; set; }
    }
}
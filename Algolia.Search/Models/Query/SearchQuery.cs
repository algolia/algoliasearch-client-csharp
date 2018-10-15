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
using Algolia.Search.Utils.Serializer;
using Newtonsoft.Json;

namespace Algolia.Search.Models.Query
{
    /// <summary>
    /// For more informations regarding the parameters
    /// https://www.algolia.com/doc/api-reference/search-api-parameters/
    /// </summary>
    [JsonConverter(typeof(QuerySerializer))]
    public class SearchQuery
    {
        public string Query { get; set; }

        // filterting
        public IEnumerable<string> FacetFilters { get; set; }
        public IEnumerable<string> OptionalFilters { get; set; }
        public string NumericFilters { get; set; }
        public string TagFilters { get; set; }
        public bool? SumOrFiltersScores { get; set; }

        // Pagination
        public int? Page { get; set; }
        public int? HitsPerPage { get; set; }
        public int? Offset { get; set; }
        public int? Length { get; set; }

        // highlighting-snippeting
        public IEnumerable<string> AttributesToHighlight { get; set; }
        public IEnumerable<string> AttributesToSnippet { get; set; }
        public string HighlightPreTag { get; set; }
        public string HighlightPostTag { get; set; }
        public string SnippetEllipsisText { get; set; }
        public bool? RestrictHighlightAndSnippetArrays { get; set; }

        // faceting
        public IEnumerable<string> Facets { get; set; }
        public long? MaxValuesPerFacet { get; set; }
        public bool? FacetingAfterDistinct { get; set; }

        // typos
        public int? MinWordSizefor1Typo { get; set; }
        public int? MinWordSizefor2Typos { get; set; }
        public bool? AllowTyposOnNumericTokens { get; set; }
        public IEnumerable<string> DisableTypoToleranceOnAttributes { get; set; }

        // languages

        // query strategy
        public string QueryType { get; set; }
        public string RemoveWordsIfNoResults { get; set; }
        public bool? AdvancedSyntax { get; set; }
        public IEnumerable<string> OptionalWords { get; set; }
        public IEnumerable<string> DisableExactOnAttributes { get; set; }
        public string ExactOnSingleWordQuery { get; set; }
        public IEnumerable<string> AlternativesAsExact { get; set; }

        // query rules
        public bool? EnableRules { get; set; }
        public IEnumerable<string> RuleContexts { get; set; }

        // advanced
        public int? Distinct { get; set; }
        public bool? Analytics { get; set; }
        public IEnumerable<string> AnalyticsTags { get; set; }
        public bool? Synonyms { get; set; }
        public bool? ReplaceSynonymsInHighlight { get; set; }
        public int? MinProximity { get; set; }
        public IEnumerable<string> ResponseFields { get; set; }
        public int? MaxFacetHits { get; set; }
        public bool? PercentileComputation { get; set; }
    }
}

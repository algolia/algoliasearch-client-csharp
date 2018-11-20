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
using System.Threading.Tasks;
using Algolia.Search.Clients;
using Algolia.Search.Models.Settings;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd
{
    [TestFixture]
    [Parallelizable]
    public class SettingsTest
    {
        private SearchIndex _index;
        private string _indexName;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName = TestHelper.GetTestIndexName("settings");
            _index = BaseTest.SearchClient.InitIndex(_indexName);
        }

        [Test]
        public async Task TestSettings()
        {
            var addObjectResponse = await _index.AddObjectAysnc(new { ObjectId = "one", Attribute = "value" });
            addObjectResponse.Wait();

            IndexSettings settings = new IndexSettings();

            // Attributes
            settings.SearchableAttributes = new List<string> { "attribute1", "attribute2", "attribute3", "ordered(attribute4)", "unordered(attribute5)" };
            settings.AttributesForFaceting = new List<string> { "attribute1", "filterOnly(attribute2)", "searchable(attribute3)" };
            settings.UnretrievableAttributes = new List<string> { "attribute1", "attribute2" };
            settings.AttributesToRetrieve = new List<string> { "attribute3", "attribute4" };

            // Ranking
            settings.Ranking = new List<string> { "asc(attribute1)", "desc(attribute2)", "attribute", "custom", "exact", "filters", "geo", "proximity", "typo", "words" };
            settings.CustomRanking = new List<string> { "asc(attribute1)", "desc(attribute1)" };
            settings.Replicas = new List<string> { _indexName + "_replica1", _indexName + "_replica2" };

            // Faceting
            settings.MaxValuesPerFacet = 100;
            settings.SortFacetValuesBy = "count";

            // Highligthing/snippeting
            settings.AttributesToHighlight = new List<string> { "attribute1", "attribute2" };
            settings.AttributesToSnippet = new List<string> { "attribute1:10", "attribute2:8" };
            settings.HighlightPreTag = "<strong>";
            settings.HighlightPostTag = "</strong>";
            settings.SnippetEllipsisText = "and so on.";
            settings.RestrictHighlightAndSnippetArrays = true;

            // Pagination
            settings.HitsPerPage = 42;
            settings.PaginationLimitedTo = 43;

            // Typos
            settings.MinWordSizefor1Typo = 2;
            settings.MinWordSizefor2Typos = 6;
            // settings.TypoTolerance = false; TBD
            settings.AllowTyposOnNumericTokens = false;
            settings.IgnorePlurals = true;
            settings.DisableTypoToleranceOnAttributes = new List<string> { "attribute1", "attribute2" };
            settings.DisableTypoToleranceOnWords = new List<string> { "word1", "word2" };
            settings.SeparatorsToIndex = "()[]";

            // Query
            settings.QueryType = "prefixNone";
            settings.RemoveWordsIfNoResults = "allOptional";
            settings.AdvancedSyntax = true;
            settings.OptionalWords = new List<string> { "word1", "word2" };
            settings.RemoveStopWords = true;
            settings.DisablePrefixOnAttributes = new List<string> { "attribute1", "attribute2" };
            settings.DisableExactOnAttributes = new List<string> { "attribute1", "attribute2" };
            settings.ExactOnSingleWordQuery = "word";

            // Query rules
            settings.EnableRules = false;

            // Performance
            settings.NumericAttributesForFiltering = new List<string> { "attribute1", "attribute2" };
            settings.AllowCompressionOfIntegerArray = true;

            // Advanced
            settings.AttributeForDistinct = "attribute1";
            settings.Distinct = 2;
            settings.ReplaceSynonymsInHighlight = false;
            settings.MinProximity = 7;
            settings.ResponseFields = new List<string> { "hits", "hitsPerPage" };
            settings.MaxFacetHits = 100;
            settings.CamelCaseAttributes = new List<string> { "attribute1", "attribute2" };
            // settings.DecompoundedAttributes = new List<string>{}; TBD
            settings.KeepDiacriticsOnCharacters = "øé";

            var saveSettingsResponse = await _index.SetSettingsAsync(settings);
            saveSettingsResponse.Wait();

            var getSettingsResponse = await _index.GetSettingsAsync();
            Assert.True(TestHelper.AreObjectsEqual(settings, getSettingsResponse, "Version", "AlternativesAsExact"));

            getSettingsResponse.TypoTolerance = "min";
            // getSettingsResponse.IgnorePlurals TBD
            // getSettingsResponse.RemoveStopWords = "en,fr"; TBD
            // getSettingsResponse.Distinct = true; TBD
        }
    }
}
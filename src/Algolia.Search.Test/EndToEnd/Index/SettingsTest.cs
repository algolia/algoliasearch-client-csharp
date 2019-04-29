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

using Algolia.Search.Clients;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Settings;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Index
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
            var addObjectResponse = await _index.SaveObjectAsync(new { ObjectID = "one", Attribute = "value" });
            addObjectResponse.Wait();

            IndexSettings settings = new IndexSettings
            {
                // Attributes
                SearchableAttributes = new List<string>
                    {"attribute1", "attribute2", "attribute3", "ordered(attribute4)", "unordered(attribute5)"},
                AttributesForFaceting = new List<string>
                    {"attribute1", "filterOnly(attribute2)", "searchable(attribute3)"},
                UnretrievableAttributes = new List<string> { "attribute1", "attribute2" },
                AttributesToRetrieve = new List<string> { "attribute3", "attribute4" },

                // Ranking
                Ranking = new List<string>
                {
                    "asc(attribute1)", "desc(attribute2)", "attribute", "custom", "exact", "filters", "geo",
                    "proximity", "typo", "words"
                },
                CustomRanking = new List<string> { "asc(attribute1)", "desc(attribute1)" },
                Replicas = new List<string> { _indexName + "_replica1", _indexName + "_replica2" },

                // Faceting
                MaxValuesPerFacet = 100,
                SortFacetValuesBy = "count",

                // Highligthing/snippeting
                AttributesToHighlight = new List<string> { "attribute1", "attribute2" },
                AttributesToSnippet = new List<string> { "attribute1:10", "attribute2:8" },
                HighlightPreTag = "<strong>",
                HighlightPostTag = "</strong>",
                SnippetEllipsisText = "and so on.",
                RestrictHighlightAndSnippetArrays = true,

                // Pagination
                HitsPerPage = 42,
                PaginationLimitedTo = 43,

                // Typos
                MinWordSizefor1Typo = 2,
                MinWordSizefor2Typos = 6,
                TypoTolerance = false,
                AllowTyposOnNumericTokens = false,
                IgnorePlurals = true,
                DisableTypoToleranceOnAttributes = new List<string> { "attribute1", "attribute2" },
                DisableTypoToleranceOnWords = new List<string> { "word1", "word2" },
                SeparatorsToIndex = "()[]",

                // Query
                QueryType = "prefixNone",
                RemoveWordsIfNoResults = "allOptional",
                AdvancedSyntax = true,
                OptionalWords = new List<string> { "word1", "word2" },
                RemoveStopWords = true,
                DisablePrefixOnAttributes = new List<string> { "attribute1", "attribute2" },
                DisableExactOnAttributes = new List<string> { "attribute1", "attribute2" },
                ExactOnSingleWordQuery = "word",
                QueryLanguages = new List<string> { "fr", "en" },
                AdvancedSyntaxFeatures = new List<string> { "exactPhrase" },
                AlternativesAsExact = new List<string> { "ignorePlurals" },

                // Query rules
                EnableRules = false,

                // Performance
                NumericAttributesForFiltering = new List<string> { "attribute1", "attribute2" },
                AllowCompressionOfIntegerArray = true,

                // Advanced
                AttributeForDistinct = "attribute1",
                Distinct = 2,
                ReplaceSynonymsInHighlight = false,
                MinProximity = 7,
                ResponseFields = new List<string> { "hits", "hitsPerPage" },
                MaxFacetHits = 100,
                CamelCaseAttributes = new List<string> { "attribute1", "attribute2" },
                DecompoundedAttributes = new Dictionary<string, List<string>>
                {
                    {"de", new List<string> {"attribute1", "attribute2"}},
                    {"fi", new List<string> {"attribute3"}}
                },
                KeepDiacriticsOnCharacters = "øé"
            };

            var saveSettingsResponse = await _index.SetSettingsAsync(settings);
            saveSettingsResponse.Wait();

            var getSettingsResponse = await _index.GetSettingsAsync();
            var spceficPropertiesCheck = new List<string> { "AlternativesAsExact", "DecompoundedAttributes" };
            Assert.True(TestHelper.AreObjectsEqual(settings, getSettingsResponse, spceficPropertiesCheck.ToArray()));

            // Check specific properties (couldn't be done by the helper)
            Assert.True(getSettingsResponse.DecompoundedAttributes.ContainsKey("de"));
            Assert.True(getSettingsResponse.DecompoundedAttributes["de"].Contains("attribute1"));
            Assert.True(getSettingsResponse.DecompoundedAttributes["de"].Contains("attribute2"));
            Assert.True(getSettingsResponse.DecompoundedAttributes.ContainsKey("fi"));
            Assert.True(getSettingsResponse.DecompoundedAttributes["fi"].Contains("attribute3"));

            // Set new values
            settings.TypoTolerance = "min";
            settings.IgnorePlurals = new List<string> { "en", "fr" };
            settings.RemoveStopWords = new List<string> { "en", "fr" };
            settings.Distinct = true;

            var saveSettingsResponseAfterChanges = await _index.SetSettingsAsync(settings);
            saveSettingsResponseAfterChanges.Wait();

            var getSettingsResponseAfterChanges = await _index.GetSettingsAsync();
            spceficPropertiesCheck.AddRange(new List<string> { "TypoTolerance", "IgnorePlurals", "RemoveStopWords" });
            Assert.True(TestHelper.AreObjectsEqual(settings, getSettingsResponseAfterChanges,
                spceficPropertiesCheck.ToArray()));

            // Check specific properties (couldn't be done by test helper)
            Assert.True((string)getSettingsResponseAfterChanges.TypoTolerance == (string)settings.TypoTolerance);
            Assert.True(getSettingsResponseAfterChanges.IgnorePlurals.GetType() == typeof(List<string>));
            Assert.True(getSettingsResponseAfterChanges.RemoveStopWords.GetType() == typeof(List<string>));

            var ignorePlurals = (List<string>)getSettingsResponseAfterChanges.IgnorePlurals;
            var removeStopWords = (List<string>)getSettingsResponseAfterChanges.RemoveStopWords;

            Assert.True(ignorePlurals.Contains("en") && ignorePlurals.Contains("fr"));
            Assert.True(removeStopWords.Contains("en") && removeStopWords.Contains("fr"));
        }
    }
}

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
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Settings;
using Algolia.Search.Models.Synonyms;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd
{
    [TestFixture]
    [Parallelizable]
    public class IndexOperationsTest
    {
        private SearchIndex _index;
        private SearchIndex _settingsIndex;
        private SearchIndex _rulesIndex;
        private SearchIndex _fullCopyIndex;
        private SearchIndex _synonymsIndex;
        private string _settingsIndexName;
        private string _rulesIndexName;
        private string _synonymsIndexName;
        private string _fullCopyIndexName;
        private string _indexName;
        private IEnumerable<IndexOperationObject> _objectsToSave;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName = TestHelper.GetTestIndexName("index_operations");
            _index = BaseTest.SearchClient.InitIndex(_indexName);

            _settingsIndexName = TestHelper.GetTestIndexName("index_operations_settings");
            _settingsIndex = BaseTest.SearchClient.InitIndex(_settingsIndexName);

            _rulesIndexName = TestHelper.GetTestIndexName("index_operations_rules");
            _rulesIndex = BaseTest.SearchClient.InitIndex(_rulesIndexName);

            _synonymsIndexName = TestHelper.GetTestIndexName("index_operations_synonyms");
            _synonymsIndex = BaseTest.SearchClient.InitIndex(TestHelper.GetTestIndexName("index_operations_synonyms"));

            _fullCopyIndexName = TestHelper.GetTestIndexName("index_operations_copy");
            _fullCopyIndex = BaseTest.SearchClient.InitIndex(_fullCopyIndexName);

            _objectsToSave = new List<IndexOperationObject>
            {
                new IndexOperationObject {ObjectID = "one", Company = "apple"},
                new IndexOperationObject {ObjectID = "two", Company = "apple"}
            };
        }

        [Test]
        public async Task IndexOperationsAsyncTest()
        {
            BatchIndexingResponse addObjectResponse = await _index.AddObjectsAysnc(_objectsToSave);
            addObjectResponse.Wait();

            IndexSettings settings = new IndexSettings {AttributesForFaceting = new List<string> {"company"}};
            var setSettingsResponse = await _index.SetSettingsAsync(settings);
            setSettingsResponse.Wait();

            Synonym synonym = new Synonym
            {
                ObjectID = "google.placeholder",
                Type = SynonymType.Placeholder,
                Placeholder = "<GOOG>",
                Replacements = new List<string> {"Google", "GOOG"}
            };

            var saveSynonymResponse = await _index.SaveSynonymAsync(synonym);
            saveSynonymResponse.Wait();

            Rule ruleToSave = new Rule
            {
                ObjectID = "company_auto_faceting",
                Condition = new Condition {Anchoring = "contains", Pattern = "{facet:company}"},
                Consequence = new Consequence
                {
                    Params = new ConsequenceParams
                    {
                        AutomaticFacetFilters = new List<AutomaticFacetFilter>
                        {
                            new AutomaticFacetFilter {Facet = "company"}
                        }
                    }
                }
            };

            var saveRuleResponse = await _index.SaveRuleAsync(ruleToSave);
            saveRuleResponse.Wait();

            var copySettingsTask = BaseTest.SearchClient.CopySettingsAsync(_indexName, _settingsIndexName);
            var copyRulesTask = BaseTest.SearchClient.CopyRulesAsync(_indexName, _rulesIndexName);
            var copySynonymsTask = BaseTest.SearchClient.CopySynonymsAsync(_indexName, _synonymsIndexName);
            var copyFullTask = BaseTest.SearchClient.CopyIndexAsync(_indexName, _fullCopyIndexName);

            CopyToResponse[] tasks =
                await Task.WhenAll(copySettingsTask, copyRulesTask, copySynonymsTask, copyFullTask);
            tasks.ToList().ForEach(x => x.Wait());

            // Check that “index_operations_settings” only contains the same settings as the original index with getSettings
            IndexSettings copySettings = await _settingsIndex.GetSettingsAsync();
            Assert.True(settings.AttributesForFaceting.ElementAt(0)
                .Equals(copySettings.AttributesForFaceting.ElementAt(0)));

            // Check that “index_operations_rules” only contains the same rules as the original index with getRule
            Rule copyRule = await _rulesIndex.GetRuleAsync("company_auto_faceting");
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave, copyRule));

            // Check that “index_operations_synonyms” only contains the same synonyms as the original index with getSynonym
            Synonym copySynonym = await _synonymsIndex.GetSynonymAsync("google.placeholder");
            Assert.True(TestHelper.AreObjectsEqual(synonym, copySynonym));

            // Check that “index_operations_copy” contains both the same settings, rules and synonyms as the original index with getSettings, getRule and getSynonym
            var copyFullSettings = _fullCopyIndex.GetSettingsAsync();
            var copyFullRule = _fullCopyIndex.GetRuleAsync("company_auto_faceting");
            var copyFullSynonym = _fullCopyIndex.GetSynonymAsync("google.placeholder");

            Task.WaitAll(copyFullSettings, copyFullRule, copyFullSynonym);

            Assert.True(settings.AttributesForFaceting.ElementAt(0)
                .Equals(copyFullSettings.Result.AttributesForFaceting.ElementAt(0)));
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave, copyFullRule.Result));
            Assert.True(TestHelper.AreObjectsEqual(synonym, copyFullSynonym.Result));
        }
    }

    public class IndexOperationObject
    {
        public string ObjectID { get; set; }
        public string Company { get; set; }
    }
}
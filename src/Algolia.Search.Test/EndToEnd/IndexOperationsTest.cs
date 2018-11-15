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

using Algolia.Search.Clients;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Responses;
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
        protected SearchIndex _index;
        private IEnumerable<IndexOperationObject> _objectsToSave;
        private string _indexName;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName = TestHelper.GetTestIndexName("index_operations");
            _index = BaseTest.SearchClient.InitIndex(_indexName);

            _objectsToSave = new List<IndexOperationObject>{
                new IndexOperationObject {ObjectID = "one", Company = "apple"},
                new IndexOperationObject {ObjectID = "two", Company = "apple"}
            };
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            var copySettingsTask = BaseTest.SearchClient.DeleteIndexAsync("index_operations_settings");
            var copyRulesTask = BaseTest.SearchClient.DeleteIndexAsync("index_operations_rules");
            var copySynonymsTask = BaseTest.SearchClient.DeleteIndexAsync("index_operations_synonyms");
            var copyFullTask = BaseTest.SearchClient.DeleteIndexAsync("index_operations_copy");

            Task.WaitAll(copySettingsTask, copyRulesTask, copySynonymsTask, copyFullTask);
        }

        [Test]
        public async Task IndexOperationsAsyncTest()
        {
            BatchResponse addObjectResponse = await _index.AddObjectsAysnc(_objectsToSave);
            addObjectResponse.Wait();

            IndexSettings settings = new IndexSettings { AttributesForFaceting = new List<string> { "company" } };
            var setSettingsResponse = await _index.SetSettingsAsync(settings);
            setSettingsResponse.Wait();

            Synonym synonym = new Synonym
            {
                ObjectID = "google.placeholder",
                Type = SynonymType.Placeholder,
                Placeholder = "<GOOG>",
                Replacements = new List<string> { "Google", "GOOG" }
            };

            var saveSynonymResponse = await _index.SaveSynonymAsync(synonym.ObjectID, synonym);
            saveSynonymResponse.Wait();

            Rule ruleToSave = new Rule
            {
                ObjectID = "company_auto_faceting",
                Condition = new Condition { Anchoring = "contains", Pattern = "{facet:company}" },
                Consequence = new Consequence { Params = new ConsequenceParams { AutomaticFacetFilters = new List<string> { "company" } } }
            };

            var saveRuleResponse = await _index.SaveRuleAsync(ruleToSave);
            saveRuleResponse.Wait();

            var copySettingsTask = _index.CopySettingsToAsync("index_operations_settings");
            var copyRulesTask = _index.CopyRulesToAsync("index_operations_rules");
            var copySynonymsTask = _index.CopySynonymsToAsync("index_operations_synonyms");
            var copyFullTask = _index.CopyToAsync("index_operations_copy");

            CopyToResponse[] tasks = await Task.WhenAll(copySettingsTask, copyRulesTask, copySynonymsTask, copyFullTask);
            tasks.ToList().ForEach(x => x.Wait());

            SearchIndex settingsIndex = BaseTest.SearchClient.InitIndex("index_operations_settings");
            IndexSettings copySettings = await settingsIndex.GetSettingsAsync();

            Assert.True(settings.AttributesForFaceting.ElementAt(0).Equals(copySettings.AttributesForFaceting.ElementAt(0)));

            SearchIndex rulesIndex = BaseTest.SearchClient.InitIndex("index_operations_rules");
            Rule copyRule = await rulesIndex.GetRuleAsync("company_auto_faceting");

            Assert.True(TestHelper.AreObjectsEqual(ruleToSave, copyRule));

            SearchIndex synonymsIndex = BaseTest.SearchClient.InitIndex("index_operations_synonyms");
            Synonym copySynonym = await synonymsIndex.GetSynonymAsync("google.placeholder");

            Assert.True(TestHelper.AreObjectsEqual(synonym, copySynonym));

            SearchIndex fullCopyIndex = BaseTest.SearchClient.InitIndex("index_operations_copy");
            var copyFullSettings = fullCopyIndex.GetSettingsAsync();
            var copyFullRule = fullCopyIndex.GetRuleAsync("company_auto_faceting");
            var copyFullSynonym = fullCopyIndex.GetSynonymAsync("google.placeholder");

            Task.WaitAll(copyFullSettings, copyFullRule, copyFullSynonym);

            Assert.True(settings.AttributesForFaceting.ElementAt(0).Equals(copyFullSettings.Result.AttributesForFaceting.ElementAt(0)));
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
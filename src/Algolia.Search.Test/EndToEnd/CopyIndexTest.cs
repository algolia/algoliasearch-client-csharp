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
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Algolia.Search.Models.Settings;
using Algolia.Search.Models.Synonyms;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Responses;

namespace Algolia.Search.Test.EndToEnd
{
    [TestFixture]
    [Parallelizable]
    public class CopyIndexTest
    {
        private SearchIndex _sourceIndex;
        private SearchIndex _settingsIndex;
        private SearchIndex _rulesIndex;
        private SearchIndex _synonymsIndex;
        private SearchIndex _fullIndex;
        private string _sourceIndexName;
        private string _copyIndexSettingsName;
        private string _copyIndexRulesName;
        private string _copyIndexSynonymsName;
        private string _copyIndexFullName;

        [OneTimeSetUp]
        public void Init()
        {
            _sourceIndexName = TestHelper.GetTestIndexName("copy_index");
            _copyIndexSettingsName = TestHelper.GetTestIndexName("copy_index_settings");
            _copyIndexRulesName = TestHelper.GetTestIndexName("copy_index_rules");
            _copyIndexSynonymsName = TestHelper.GetTestIndexName("copy_index_synonyms");
            _copyIndexFullName = TestHelper.GetTestIndexName("copy_index_full");
            _sourceIndex = BaseTest.SearchClient.InitIndex(_sourceIndexName);
            _settingsIndex = BaseTest.SearchClient.InitIndex(_copyIndexSettingsName);
            _rulesIndex = BaseTest.SearchClient.InitIndex(_copyIndexRulesName);
            _synonymsIndex = BaseTest.SearchClient.InitIndex(_copyIndexSynonymsName);
            _fullIndex = BaseTest.SearchClient.InitIndex(_copyIndexFullName);

        }

        [Test]
        public async Task TestCopyIndex()
        {

            var objectsToAdd = new List<CopyIndexObject>
            {
                new CopyIndexObject
                {
                    ObjectID = "one",
                    Company = "apple"
                },
                new CopyIndexObject
                {
                    ObjectID = "two",
                    Company = "algolia"
                }
            };

            var addObjectSrcIndex = await _sourceIndex.AddObjectsAysnc(objectsToAdd).ConfigureAwait(false);
            addObjectSrcIndex.Wait();

            IndexSettings settings = new IndexSettings
            {
                AttributesForFaceting = new List<string> { "company" }
            };

            var setSettings = await _sourceIndex.SetSettingsAsync(settings).ConfigureAwait(false);
            setSettings.Wait();

            var synonym = new Synonym
            {
                ObjectID = "google_placeholder",
                Type = SynonymType.Placeholder,
                Placeholder = "<GOOG>",
                Replacements = new List<string> { "Google", "GOOG" }
            };

            var saveSynonyms = await _sourceIndex.SaveSynonymAsync(synonym).ConfigureAwait(false);
            saveSynonyms.Wait();

            Rule ruleToSave = new Rule
            {
                ObjectID = "company_automatic_faceting",
                Condition = new Condition { Anchoring = "contains", Pattern = "{facet:company}" },
                Consequence = new Consequence
                {
                    Params = new ConsequenceParams
                    {
                        AutomaticFacetFilters = new List<AutomaticFacetFilter>
                        {
                            new AutomaticFacetFilter {Facet = "company" }
                        }
                    }
                }
            };

            var saveRule = await _sourceIndex.SaveRuleAsync(ruleToSave);
            saveRule.Wait();

            var copySetttingsTask = BaseTest.SearchClient.CopySettingsAsync(_sourceIndexName, _copyIndexSettingsName);
            var copyRulesTask = BaseTest.SearchClient.CopyRulesAsync(_sourceIndexName, _copyIndexRulesName);
            var copySynonymsTask = BaseTest.SearchClient.CopySynonymsAsync(_sourceIndexName, _copyIndexSynonymsName);
            var copyFullTask = BaseTest.SearchClient.CopyIndexAsync(_sourceIndexName, _copyIndexFullName);

            CopyToResponse[] saveResponses = await Task.WhenAll(copySetttingsTask, copyRulesTask, copySynonymsTask, copyFullTask);

            foreach (var resp in saveResponses)
            {
                resp.Wait();
            }

            // Check index with only settings
            IndexSettings originalSettings = await _sourceIndex.GetSettingsAsync();
            IndexSettings copiedSettings = await _settingsIndex.GetSettingsAsync();
            Assert.True(TestHelper.AreObjectsEqual(originalSettings, copiedSettings));

            // Check index with only rules
            var copiedRules = await _rulesIndex.GetRuleAsync(ruleToSave.ObjectID);
            Assert.True(TestHelper.AreObjectsEqual(copiedRules, ruleToSave));

            // Check index with only synonyms
            var copiedSynonym = await _synonymsIndex.GetSynonymAsync(synonym.ObjectID);
            Assert.True(TestHelper.AreObjectsEqual(copiedSynonym, synonym));

            // Check full index
            var fullCopiedSettings = await _fullIndex.GetSettingsAsync();
            var fullCopiedRules = await _fullIndex.GetRuleAsync(ruleToSave.ObjectID);
            var fullCopiedSynonym = await _fullIndex.GetSynonymAsync(synonym.ObjectID);
        }

        public class CopyIndexObject
        {
            public string ObjectID { get; set; }
            public string Company { get; set; }
        }
    }
}
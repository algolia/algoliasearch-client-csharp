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
using Algolia.Search.Exceptions;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Settings;
using Algolia.Search.Models.Synonyms;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Account
{
    [TestFixture]
    [Parallelizable]
    public class AccountCopyIndex
    {
        [Test]
        [Parallelizable]
        public void TestAccountCopyIndexSameApp()
        {
            SearchIndex index1 = BaseTest.SearchClient.InitIndex(TestHelper.GetTestIndexName("copy_index"));
            SearchIndex index2 = BaseTest.SearchClient.InitIndex(TestHelper.GetTestIndexName("copy_index_2"));
            AccountClient account = new AccountClient();

            AlgoliaException ex =
                Assert.ThrowsAsync<AlgoliaException>(() => account.CopyIndexAsync<AccountCopyObject>(index1, index2));
            Assert.That(ex.Message.Contains("Source and Destination indices should not be on the same application."));
        }

        [Test]
        [Parallelizable]
        public async Task TestAccountCopyIndex()
        {
            AccountClient accountClient = new AccountClient();

            string indexOneName = TestHelper.GetTestIndexName("copy_index");
            string indexTwoName = TestHelper.GetTestIndexName("copy_index_2");

            SearchIndex index1 = BaseTest.SearchClient.InitIndex(indexOneName);
            SearchIndex index2 = BaseTest.SearchClient2.InitIndex(indexTwoName);

            var objectToAdd = new AccountCopyObject { ObjectID = "one" };
            var addObject = index1.SaveObjectAsync(objectToAdd);

            Rule ruleToSave = new Rule
            {
                ObjectID = "one",
                Condition = new Condition { Anchoring = "is", Pattern = "pattern" },
                Consequence = new Consequence
                {
                    Params = new ConsequenceParams
                    {
                        Query = new ConsequenceQuery
                        {
                            Edits = new List<Edit>
                            {
                                new Edit {Type = EditType.Remove, Delete = "patter"}
                            }
                        }
                    }
                },
            };

            var saveRule = index1.SaveRuleAsync(ruleToSave);

            var synonym = new Synonym
            {
                ObjectID = "one",
                Type = SynonymType.Synonym,
                Synonyms = new List<string> { "one", "two" }
            };

            var saveSynonym = index1.SaveSynonymAsync(synonym);

            IndexSettings settings = new IndexSettings
            {
                AttributesForFaceting = new List<string> { "company" }
            };

            var saveSettings = index1.SetSettingsAsync(settings);

            Task.WaitAll(saveSettings, saveSynonym, saveRule, addObject);

            // Algolia wait
            addObject.Result.Wait();
            saveRule.Result.Wait();
            saveSettings.Result.Wait();
            saveSynonym.Result.Wait();

            var copyIndex = await accountClient.CopyIndexAsync<AccountCopyObject>(index1, index2);
            copyIndex.Wait();

            var getObjectTask = index2.GetObjectAsync<AccountCopyObject>("one");
            var getRule = index2.GetRuleAsync(ruleToSave.ObjectID);
            var getSynonym = index2.GetSynonymAsync(synonym.ObjectID);
            var getSettings = index2.GetSettingsAsync();
            var getOriginalSettings = index1.GetSettingsAsync();

            Assert.True(TestHelper.AreObjectsEqual(objectToAdd, await getObjectTask));
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave, await getRule));
            Assert.True(TestHelper.AreObjectsEqual(synonym, await getSynonym));
            Assert.True(TestHelper.AreObjectsEqual(await getOriginalSettings, await getSettings));

            await index2.DeleteAsync();
        }

        public class AccountCopyObject
        {
            public string ObjectID { get; set; }
        }
    }
}

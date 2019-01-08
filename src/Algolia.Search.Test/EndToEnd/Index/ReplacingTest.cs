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
using Algolia.Search.Models.Synonyms;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Index
{
    [TestFixture]
    [Parallelizable]
    public class ReplacingTest
    {
        private SearchIndex _index;

        [OneTimeSetUp]
        public void Init()
        {
            _index = BaseTest.SearchClient.InitIndex(TestHelper.GetTestIndexName("replacing"));
        }

        [Test]
        public async Task TestReplacing()
        {
            var addResponse = _index.SaveObjectAsync(new ReplaceAllTestObject { ObjectID = "one" });

            var ruleToSave = new Rule
            {
                ObjectID = "one",
                Condition = new Condition { Anchoring = "is", Pattern = "pattern" },
                Consequence = new Consequence
                {
                    Params = new ConsequenceParams
                    {
                        Query = new ConsequenceQuery
                        {
                            Edits = new List<Edit> { new Edit { Type = EditType.Remove, Delete = "pattern" } }
                        }
                    }
                }
            };

            var saveRuleResponse = _index.SaveRuleAsync(ruleToSave);

            var synonymToSave = new Synonym
            {
                ObjectID = "one",
                Type = SynonymType.Synonym,
                Synonyms = new List<string> { "one", "two" }
            };

            var saveSynonymResponse = _index.SaveSynonymAsync(synonymToSave);

            Task.WaitAll(saveSynonymResponse, addResponse, saveRuleResponse);

            addResponse.Result.Wait();
            saveRuleResponse.Result.Wait();
            saveSynonymResponse.Result.Wait();

            var response = await _index.ReplaceAllObjectsAsync(new List<ReplaceAllTestObject>
            {
                new ReplaceAllTestObject {ObjectID = "two"}
            });

            response.Wait();

            var ruleToSave2 = new Rule
            {
                ObjectID = "two",
                Condition = new Condition { Anchoring = "is", Pattern = "pattern" },
                Consequence = new Consequence
                {
                    Params = new ConsequenceParams
                    {
                        Query = new ConsequenceQuery
                        {
                            Edits = new List<Edit> { new Edit { Type = EditType.Remove, Delete = "pattern" } }
                        }
                    }
                }
            };

            var replaceAllRulesResponse = await _index.ReplaceAllRulesAsync(new List<Rule> { ruleToSave2 });

            var synonymToSave2 = new Synonym
            {
                ObjectID = "two",
                Type = SynonymType.Synonym,
                Synonyms = new List<string> { "one", "two" }
            };

            var replaceAllSynonymsResponse = await _index.ReplaceAllSynonymsAsync(new List<Synonym> { synonymToSave2 });

            replaceAllRulesResponse.Wait();
            replaceAllSynonymsResponse.Wait();

            AlgoliaApiException getObjectEx =
                Assert.ThrowsAsync<AlgoliaApiException>(() => _index.GetObjectAsync<ReplaceAllTestObject>("one"));
            AlgoliaApiException getSynonymEx =
                Assert.ThrowsAsync<AlgoliaApiException>(() => _index.GetSynonymAsync("one"));
            AlgoliaApiException getRuleEx = Assert.ThrowsAsync<AlgoliaApiException>(() => _index.GetRuleAsync("one"));

            Assert.That(getObjectEx.HttpErrorCode == 404);
            Assert.That(getSynonymEx.HttpErrorCode == 404);
            Assert.That(getRuleEx.HttpErrorCode == 404);

            var ruleAfterReplace = _index.GetRuleAsync("two");
            var synonymAfterReplace = _index.GetSynonymAsync("two");
            Task.WaitAll(ruleAfterReplace, synonymAfterReplace);

            Assert.True(TestHelper.AreObjectsEqual(ruleAfterReplace.Result, ruleToSave2));
            Assert.True(TestHelper.AreObjectsEqual(synonymAfterReplace.Result, synonymToSave2));
        }
    }

    public class ReplaceAllTestObject
    {
        public string ObjectID { get; set; }
    }
}
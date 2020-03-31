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
using Algolia.Search.Iterators;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;
using Algolia.Search.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Index
{
    [TestFixture]
    [Parallelizable]
    public class QueryRulesTest
    {
        private SearchIndex _index;

        [OneTimeSetUp]
        public void Init()
        {
            _index = BaseTest.SearchClient.InitIndex(TestHelper.GetTestIndexName("rules"));
        }

        [Test]
        public async Task RulesTest()
        {
            var saveObjects = await _index.SaveObjectsAsync(new List<MobilePhone>
            {
                new MobilePhone { ObjectID = "iphone_7", Brand = "Apple", Model = "7" },
                new MobilePhone { ObjectID = "iphone_8", Brand = "Apple", Model = "8" },
                new MobilePhone { ObjectID = "iphone_x", Brand = "Apple", Model = "X" },
                new MobilePhone { ObjectID = "one_plus_one", Brand = "OnePlus", Model = "One" },
                new MobilePhone { ObjectID = "one_plus_two", Brand = "OnePlus", Model = "Two" }
            });

            // Set attributesForFaceting to [“brand”] using setSettings and collect the taskID
            IndexSettings settings =
                new IndexSettings { AttributesForFaceting = new List<string> { "brand", "model" } };
            var setSettingsResponse = await _index.SetSettingsAsync(settings);

            saveObjects.Wait();
            setSettingsResponse.Wait();

            Rule ruleToSave = new Rule
            {
                ObjectID = "brand_automatic_faceting",
                Enabled = false,
                Condition = new Condition { Anchoring = "is", Pattern = "{facet:brand}" },
                Consequence = new Consequence
                {
                    FilterPromotes = false,
                    Params = new ConsequenceParams
                    {
                        AutomaticFacetFilters = new List<AutomaticFacetFilter>
                        {
                            new AutomaticFacetFilter { Facet = "brand", Disjunctive = true, Score = 42 }
                        }
                    }
                },
                Validity = new List<TimeRange>
                {
                    new TimeRange
                    {
                        From = DateTimeHelper.UnixTimeToDateTime(1532439300), // 07/24/2018 13:35:00 UTC
                        Until = DateTimeHelper.UnixTimeToDateTime(1532525700) // 07/25/2018 13:35:00 UTC
                    },
                    new TimeRange
                    {
                        From = DateTimeHelper.UnixTimeToDateTime(1532612100), // 07/26/2018 13:35:00 UTC
                        Until = DateTimeHelper.UnixTimeToDateTime(1532698500) // 07/27/2018 13:35:00 UTC
                    }
                },
                Description = "Automatic apply the faceting on `brand` if a brand value is found in the query"
            };

            var saveRuleResponse = await _index.SaveRuleAsync(ruleToSave);

            Rule ruleToSave2 = new Rule
            {
                ObjectID = "query_edits",
                Condition =
                    new Condition
                    {
                        Anchoring = "is",
                        Pattern = "mobile phone",
                        Alternatives = Alternatives.Of(true)
                    },
                Consequence = new Consequence
                {
                    Params = new ConsequenceParams
                    {
                        Query = new ConsequenceQuery
                        {
                            Edits = new List<Edit>
                            {
                                new Edit { Type = EditType.Remove, Delete = "mobile" },
                                new Edit
                                {
                                    Type = EditType.Replace, Delete = "phone", Insert = "ihpone"
                                },
                            }
                        }
                    }
                }
            };

            Rule ruleToSave3 = new Rule
            {
                ObjectID = "query_promo",
                Consequence = new Consequence { Params = new ConsequenceParams { Filters = "brand:OnePlus" } }
            };

            Rule ruleToSave4 = new Rule
            {
                ObjectID = "query_promo_only_summer",
                Consequence = new Consequence { Params = new ConsequenceParams { Filters = "model:One" } },
                Condition = new Condition { Context = "summer" }
            };

            var batchRulesResponse =
                await _index.SaveRulesAsync(new List<Rule> { ruleToSave2, ruleToSave3, ruleToSave4 });

            saveRuleResponse.Wait();
            batchRulesResponse.Wait();

            // Retrieve all the rules using getRule and check that they were correctly saved
            var getRuleToSave = _index.GetRuleAsync(ruleToSave.ObjectID);
            var getRuleToSave2 = _index.GetRuleAsync(ruleToSave2.ObjectID);
            var getRuleToSave3 = _index.GetRuleAsync(ruleToSave3.ObjectID);
            var getRuleToSave4 = _index.GetRuleAsync(ruleToSave4.ObjectID);

            Rule[] tasks = await Task.WhenAll(getRuleToSave, getRuleToSave2, getRuleToSave3, getRuleToSave4);
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave, tasks[0]));
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave2, tasks[1]));
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave3, tasks[2]));
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave4, tasks[3]));

            var searchWithContext =
                await _index.SearchAsync<MobilePhone>(new Query { RuleContexts = new List<string> { "summer" } });
            Assert.That(searchWithContext.Hits, Has.Exactly(1).Items);

            SearchResponse<Rule> searchRules = await _index.SearchRuleAsync(new RuleQuery());
            Assert.That(searchRules.Hits, Has.Exactly(4).Items);
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave,
                searchRules.Hits.Find(r => r.ObjectID.Equals(ruleToSave.ObjectID))));
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave2,
                searchRules.Hits.Find(r => r.ObjectID.Equals(ruleToSave2.ObjectID))));

            // Iterate over all the rules using ruleIterator and check that they were correctly saved
            List<Rule> rulesFromIterator = new RulesIterator(_index).ToList();

            Assert.True(TestHelper.AreObjectsEqual(ruleToSave,
                rulesFromIterator.Find(r => r.ObjectID.Equals(ruleToSave.ObjectID))));
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave2,
                rulesFromIterator.Find(r => r.ObjectID.Equals(ruleToSave2.ObjectID))));
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave3,
                rulesFromIterator.Find(r => r.ObjectID.Equals(ruleToSave3.ObjectID))));
            Assert.True(TestHelper.AreObjectsEqual(ruleToSave4,
                rulesFromIterator.Find(r => r.ObjectID.Equals(ruleToSave4.ObjectID))));

            // Delete the first rule using deleteRule and check that it was correctly deleted
            var deleteRule = await _index.DeleteRuleAsync(ruleToSave.ObjectID);
            deleteRule.Wait();

            SearchResponse<Rule> searchRulesAfterDelete = await _index.SearchRuleAsync(new RuleQuery());
            Assert.That(searchRulesAfterDelete.Hits, Has.Exactly(3).Items);
            Assert.IsFalse(searchRulesAfterDelete.Hits.Exists(r => r.ObjectID.Equals(ruleToSave.ObjectID)));

            // Clear all the remaining rules using clearRules and check that all rules have been correctly removed
            var clearRule = await _index.ClearRulesAsync();
            clearRule.Wait();

            SearchResponse<Rule> searchRulesAfterClear = await _index.SearchRuleAsync(new RuleQuery());
            Assert.That(searchRulesAfterClear.Hits, Is.Empty);
        }

        public class MobilePhone
        {
            public string ObjectID { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
        }
    }
}

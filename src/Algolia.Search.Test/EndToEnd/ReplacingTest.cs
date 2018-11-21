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
using Algolia.Search.Http;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Rules;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd
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
            Rule ruleToSave2 = new Rule
            {
                ObjectID = "query_edits",
                Condition = new Condition { Anchoring = "is", Pattern = "mobile phone" },
                Consequence = new Consequence
                {
                    Params = new ConsequenceParams
                    {
                        Query = new ConsequenceQuery
                        {
                            Edits = new List<Edit>
                            {
                                new Edit {Type = EditType.Remove, Delete = "mobile"},
                                new Edit {Type = EditType.Replace, Delete = "phone", Insert = "ihpone"},
                            }
                        }
                    }
                },
            };

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("forwardToReplicas", false);
            queryParams.Add("clearExistingRules", true);
            RequestOptions requestOption = new RequestOptions { QueryParameters = queryParams };

            var batchRulesResponse = await _index.SaveRulesAsync(new List<Rule> { ruleToSave2 }, requestOption);
            batchRulesResponse.Wait();
        }
    }
}
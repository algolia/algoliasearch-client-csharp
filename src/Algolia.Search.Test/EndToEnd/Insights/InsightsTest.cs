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
using Algolia.Search.Models.Search;
using NUnit.Framework;
using System.Collections.Generic;

namespace Algolia.Search.Test.EndToEnd.Insights
{
    [TestFixture]
    [Parallelizable]
    public class InsightsTest
    {
        private SearchIndex _index;
        private string _indexName;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName = TestHelper.GetTestIndexName("insights");
            _index = BaseTest.SearchClient.InitIndex(_indexName);
        }

        [Test]
        public void TestInsights()
        {
            InsightsClient insightsClient = new InsightsClient(TestHelper.ApplicationId1, TestHelper.AdminKey1);
            var insights = insightsClient.User("test");

            // click
            insights.ClickedFilters("clickedFilters", _indexName, new List<string> { "brand:apple" });
            insights.ClickedObjectIDs("clickedObjectEvent", _indexName, new List<string> { "1", "2" });

            // Conversion
            insights.ConvertedObjectIDs("convertedObjectIDs", _indexName, new List<string> { "1", "2" });
            insights.ConvertedFilters("converterdFilters", _indexName, new List<string> { "brand:apple" });

            // View
            insights.ViewedFilters("viewedFilters", _indexName, new List<string> { "brand:apple", "brand:google" });
            insights.ViewedObjectIDs("viewedObjectIDs", _indexName, new List<string> { "1", "2" });

            _index.SaveObject(new AlgoliaStub { ObjectID = "one" }).Wait();

            var query = new Query()
            {
                EnablePersonalization = true,
                ClickAnalytics = true
            };

            var search1 = _index.Search<AlgoliaStub>(query);
            insights.ClickedObjectIDsAfterSearch("clickedObjectIDsAfterSearch", _indexName, new List<string> { "1", "2" },
                new List<uint> { 17, 19 }, search1.QueryID);

            var search2 = _index.Search<AlgoliaStub>(query);
            insights.ConvertedObjectIDsAfterSearch("convertedObjectIDsAfterSearch", _indexName,
                new List<string> { "1", "2" }, search2.QueryID);
        }

        public class AlgoliaStub
        {
            public string ObjectID { get; set; }
        }
    }
}
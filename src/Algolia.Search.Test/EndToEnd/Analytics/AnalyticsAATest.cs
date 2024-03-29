/*
* Copyright (c) 2018-2021 Algolia
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Algolia.Search.Clients;
using Algolia.Search.Models.Analytics;
using Algolia.Search.Models.Search;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd.Analytics
{
    [TestFixture]
    [Parallelizable]
    public class AnalyticsAATest
    {
        private SearchIndex _index;
        private string _indexName;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName = TestHelper.GetTestIndexName("aa_testing_dev");
            _index = BaseTest.SearchClient.InitIndex(_indexName);
        }

        [Test]
        [Parallelizable]
        public async Task TestAATesting()
        {
            var now = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string testName = $"csharp-AA-{now}-{Environment.UserName}";

            var addOne = await _index.SaveObjectAsync(new AlgoliaStub { ObjectID = "one" });

            // Create tomorrow datetime without seconds/ms to avoid test to fail
            DateTime utcNow = DateTime.UtcNow.AddDays(1);
            DateTime tomorrow = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, 0,
                utcNow.Kind);

            addOne.Wait();

            var abTest = new AddABTestRequest
            {
                Name = testName,
                Variants = new List<Variant>
                {
                    new Variant { Index = _indexName, TrafficPercentage = 90, Description = "a description" },
                    new Variant
                    {
                        Index = _indexName,
                        TrafficPercentage = 10,
                        Description = "Variant number 2",
                        CustomSearchParameters = new Query { IgnorePlurals = true }
                    }
                },
                EndAt = tomorrow
            };

            AddABTestResponse addAbTest = await BaseTest.AnalyticsClient.AddABTestAsync(abTest);
            _index.WaitTask(addAbTest.TaskID);

            ABTest abTestToCheck = await BaseTest.AnalyticsClient.GetABTestAsync(addAbTest.ABTestId);
            Assert.AreEqual(abTest.Name, abTestToCheck.Name);
            Assert.IsTrue(TestHelper.AreObjectsEqual(abTest.Variants.ElementAt(0), abTestToCheck.Variants.ElementAt(0), "ClickCount", "ConversionCount"));
            Assert.IsTrue(TestHelper.AreObjectsEqual(abTest.Variants.ElementAt(1), abTestToCheck.Variants.ElementAt(1), "ClickCount", "ConversionCount"));
            Assert.AreEqual(abTest.EndAt, abTestToCheck.EndAt);
            Assert.That(abTestToCheck.Status, Is.EqualTo("active"));

            var deleteAbTest = await BaseTest.AnalyticsClient.DeleteABTestAsync(addAbTest.ABTestId);
            _index.WaitTask(deleteAbTest.TaskID);
        }

        public class AlgoliaStub
        {
            public string ObjectID { get; set; }
        }
    }
}

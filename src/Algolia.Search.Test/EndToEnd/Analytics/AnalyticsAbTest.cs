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
using Algolia.Search.Models.Analytics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Analytics
{
    [TestFixture]
    [Parallelizable]
    public class AnalyticsAbTest
    {
        private SearchIndex _index1;
        private SearchIndex _index2;
        private string _index1Name;
        private string _index2Name;

        [OneTimeSetUp]
        public void Init()
        {
            _index1Name = TestHelper.GetTestIndexName("ab_testing");
            _index2Name = TestHelper.GetTestIndexName("ab_testing_dev");
            _index1 = BaseTest.SearchClient.InitIndex(_index1Name);
            _index2 = BaseTest.SearchClient.InitIndex(_index2Name);
        }

        [Test]
        [Parallelizable]
        public async Task TestAbTest()
        {
            var now = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string testName = $"csharp-{now}-{Environment.UserName}";

            var abTests = await BaseTest.AnalyticsClient.GetABTestsAsync();

            if (abTests.ABTests != null)
            {
                var abTestsToDelete =
                    abTests.ABTests?.Where(x => x.Name.Contains("csharp-") && !x.Name.Contains($"csharp-{now}"));

                foreach (var item in abTestsToDelete)
                {
                    await BaseTest.AnalyticsClient.DeleteABTestAsync(item.AbTestId.Value);
                }
            }

            var addOne = await _index1.SaveObjectAsync(new AlgoliaStub { ObjectID = "one" });
            var addTwo = await _index2.SaveObjectAsync(new AlgoliaStub { ObjectID = "one" });

            // Create tomorrow datetime without seconds/ms to avoid test to fail
            DateTime utcNow = DateTime.UtcNow.AddDays(1);
            DateTime tomorrow = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, 0,
                utcNow.Kind);

            var abTest = new ABTest
            {
                Name = testName,
                Variants = new List<Variant>
                {
                    new Variant
                    {
                        Index = _index1Name, TrafficPercentage = 60, Description = "a description"
                    },
                    new Variant
                    {
                        Index = _index2Name, TrafficPercentage = 40, Description = string.Empty
                    }
                },
                EndAt = tomorrow
            };

            addOne.Wait();
            addTwo.Wait();

            AddABTestResponse addAbTest = await BaseTest.AnalyticsClient.AddABTestAsync(abTest);
            abTest.AbTestId = addAbTest.ABTestId;
            _index1.WaitTask(addAbTest.TaskID);

            ABTest abTestToCheck = await BaseTest.AnalyticsClient.GetABTestAsync(abTest.AbTestId.Value);
            Assert.IsTrue(TestHelper.AreObjectsEqual(abTestToCheck, abTest, "CreatedAt", "Status", "ClickCount", "ConversionCount"));
            Assert.IsFalse(abTestToCheck.Status.Equals("stopped"));

            ABTestsReponse listAbTests = await BaseTest.AnalyticsClient.GetABTestsAsync();
            Assert.IsTrue(listAbTests.ABTests.Any(x => x.AbTestId == abTest.AbTestId));
            Assert.IsTrue(TestHelper.AreObjectsEqual(
                listAbTests.ABTests.FirstOrDefault(x => x.AbTestId == abTest.AbTestId), abTest, "CreatedAt", "Status", "ClickCount", "ConversionCount"));

            await BaseTest.AnalyticsClient.StopABTestAsync(abTest.AbTestId.Value);

            ABTest stoppedAbTest = await BaseTest.AnalyticsClient.GetABTestAsync(abTest.AbTestId.Value);
            Assert.IsTrue(stoppedAbTest.Status.Equals("stopped"));

            DeleteABTestResponse deleteAbTest = await BaseTest.AnalyticsClient.DeleteABTestAsync(abTest.AbTestId.Value);
            _index1.WaitTask(deleteAbTest.TaskID);

            AlgoliaApiException ex =
                Assert.ThrowsAsync<AlgoliaApiException>(() => BaseTest.AnalyticsClient.GetABTestAsync(abTest.AbTestId.Value));
            Assert.That(ex.HttpErrorCode == 404);
        }

        public class AlgoliaStub
        {
            public string ObjectID { get; set; }
        }
    }
}
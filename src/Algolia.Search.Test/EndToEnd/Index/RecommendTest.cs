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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Algolia.Search.Clients;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Recommend;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;
using Algolia.Search.Utils;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd.Index
{
    [TestFixture]
    [Parallelizable]
    public class RecommendTest
    {
        private string _indexName;
        private SearchIndex _index;
        private IEnumerable<Product> _employees;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName = TestHelper.GetTestIndexName("recommend");
            _index = BaseTest.SearchClient.InitIndex(_indexName);
            _employees = InitEmployees();
        }

        //fixme: stubbed
        [Test]
        public async Task RecommendTestAsync()
        {
            BatchIndexingResponse addObjectResponse =
                await _index.SaveObjectsAsync(_employees, autoGenerateObjectId: true);
            addObjectResponse.Wait();

            Assert.IsInstanceOf<BatchIndexingResponse>(addObjectResponse);
            Assert.NotNull(addObjectResponse);

            IndexSettings settings = new IndexSettings
            {
                AttributesForFaceting = new List<string> { "searchable(name)" },
                //todo: enable recommendations on this app/index, configure reco model

            };
            var setSettingsResponse = await _index.SetSettingsAsync(settings);
            setSettingsResponse.Wait();

            Assert.IsInstanceOf<SetSettingsResponse>(setSettingsResponse);
            Assert.NotNull(setSettingsResponse);

            var objectIdToRecommendOn = "3";

            var objectIdToPromote1 = "1";
            var objectIdToPromote2 = "2";
            //todo: feed 1 and 2 to recommendation engine


            var recos = await BaseTest.RecommendClient.GetRecommendationsAsync(new RecommendRequestItem
            {
                IndexName = _indexName,
                ObjectID = objectIdToRecommendOn,
                MaxRecommendations = 3,
                Model = "related-products",
            });

            var recommendedIds = recos.Items.First().Hits.Select(x => x.ObjectID);
            Assert.That(recommendedIds, Contains.Item(objectIdToPromote1));
            Assert.That(recommendedIds, Contains.Item(objectIdToPromote2));

            //todo: perform a request with multiple objects and multiple results 
        }

        private IEnumerable<Product> InitEmployees()
        {
            var objectId = 1;
            return new List<Product>()
            {
                new Product { Manufacturer = "Algolia",         ObjectID = objectId++.ToString(), Name="iPhone 1" },
                new Product { Manufacturer = "Algolia",         ObjectID = objectId++.ToString(), Name="iPhone 2" },
                new Product { Manufacturer = "Amazon",          ObjectID = objectId++.ToString(), Name="iPhone 3" },
                new Product { Manufacturer = "Apple",           ObjectID = objectId++.ToString(), Name="iPhone 4" },
                new Product { Manufacturer = "Apple",           ObjectID = objectId++.ToString(), Name="iPhone 5" },
                new Product { Manufacturer = "Arista Networks", ObjectID = objectId++.ToString(), Name="iPhone 6" },
                new Product { Manufacturer = "Google",          ObjectID = objectId++.ToString(), Name="iPhone 7" },
                new Product { Manufacturer = "Google",          ObjectID = objectId++.ToString(), Name="iPhone 8" },
                new Product { Manufacturer = "Google",          ObjectID = objectId++.ToString(), Name="iPhone 9" },
                new Product { Manufacturer = "SpaceX",          ObjectID = objectId++.ToString(), Name="iPhone X" },
                new Product { Manufacturer = "SpaceX",          ObjectID = objectId++.ToString(), Name="iPhone X2" },
                new Product { Manufacturer = "SpaceX",          ObjectID = objectId++.ToString(), Name="iPhone X3 The Last Stand" },
                new Product { Manufacturer = "Yahoo",           ObjectID = objectId++.ToString(), Name="iPhone -1: The Foretold one" },
            };
        }

        public class Product
        {
            public string ObjectID { get; set; }
            public string Manufacturer { get; set; }
            public string Name { get; set; }
            public string QueryID { get; set; }
        }
    }
}

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
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Enums;
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
    public class IndexingTest
    {
        private SearchIndex _index;
        private string _indexName;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName = TestHelper.GetTestIndexName("indexing");
            _index = BaseTest.SearchClient.InitIndex(_indexName);
        }

        [Test]
        public async Task IndexOperationsAsyncTest()
        {
            var addObject = _index.SaveObjectAsync(new AlgoliaStub { ObjectID = "one" });
            var addObjectWoId = _index.SaveObjectAsync(new AlgoliaStub { }, autoGenerateObjectId: true);

            var addObjects = _index.SaveObjectsAsync(new List<AlgoliaStub>
            {
                new AlgoliaStub { ObjectID = "two" },
                new AlgoliaStub { ObjectID = "three" }
            });

            var addObjectsWoId = _index.SaveObjectsAsync(new List<AlgoliaStub>
            {
                new AlgoliaStub { ObjectID = "four" },
                new AlgoliaStub { ObjectID = "five" }
            }, autoGenerateObjectId: true);

            List<AlgoliaStub> objectsToBatch = new List<AlgoliaStub>();

            List<string> ids = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                var id = (i + 1).ToString();
                objectsToBatch.Add(new AlgoliaStub { ObjectID = id });
                ids.Add(id);
            }

            var batch = _index.SaveObjectsAsync(objectsToBatch);

            var tasks = new[] { addObject, addObjectWoId, addObjects, addObjectsWoId, batch };
            Task.WaitAll(tasks);

            foreach (var response in tasks)
            {
                response.Result.Wait();
            }

            var sixFirstRecords = await _index.GetObjectsAsync<AlgoliaStub>(new List<string> { "one", "two", "three", "four", "five" });
            var allRecords = await _index.GetObjectsAsync<AlgoliaStub>(ids);

            List<AlgoliaStub> objectsBrowsed = new List<AlgoliaStub>();
            foreach (var item in _index.Browse<AlgoliaStub>(new BrowseIndexQuery()))
            {
                objectsBrowsed.Add(item);
            }

            Assert.True(objectsBrowsed.Count() == 1006);

            var objectToUpdate = objectsToBatch.ElementAt(0);
            objectToUpdate.Property = "Updated";

            var updateObject = await _index.PartialUpdateObjectAsync(objectToUpdate);
            updateObject.Wait();

            var getUpdatedObject = await _index.GetObjectAsync<AlgoliaStub>(objectToUpdate.ObjectID);
            Assert.True(getUpdatedObject.Property.Equals(objectToUpdate.Property));
        }
    }

    public class AlgoliaStub
    {
        public string ObjectID { get; set; }
        public string Property { get; set; } = "Default";
    }
}
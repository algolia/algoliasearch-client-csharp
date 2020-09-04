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
using Algolia.Search.Exceptions;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Search;
using Algolia.Search.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd.Index
{
    [TestFixture]
    [Parallelizable]
    public class IndexingTest
    {
        private SearchIndex _index;
        private string _indexName;

        private SearchIndex _indexDeleteBy;
        private string _indexDeleteByName;

        private SearchIndex _indexMove;
        private string _indexMoveName;

        private SearchIndex _indexClear;
        private string _indexClearName;

        private SearchIndex _indexWithJObject;
        private string _indexWithJObjectName;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName = TestHelper.GetTestIndexName("indexing");
            _index = BaseTest.SearchClient.InitIndex(_indexName);

            _indexDeleteByName = TestHelper.GetTestIndexName("delete_by");
            _indexDeleteBy = BaseTest.SearchClient.InitIndex(_indexDeleteByName);

            _indexMoveName = TestHelper.GetTestIndexName("move_test_source");
            _indexMove = BaseTest.SearchClient.InitIndex(_indexMoveName);

            _indexClearName = TestHelper.GetTestIndexName("clear_objects");
            _indexClear = BaseTest.SearchClient.InitIndex(_indexClearName);

            _indexWithJObjectName = TestHelper.GetTestIndexName("indexing_with_JObject");
            _indexWithJObject = BaseTest.SearchClient.InitIndex(_indexWithJObjectName);
        }

        [Test]
        [Parallelizable]
        public async Task IndexOperationsAsyncTest()
        {
            // AddObject with ID
            var objectOne = new AlgoliaStub { ObjectId = "one" };
            var addObject = _index.SaveObjectAsync(objectOne);

            // AddObject without ID
            var objectWoId = new AlgoliaStub();
            var addObjectWoId = _index.SaveObjectAsync(objectWoId, autoGenerateObjectId: true);

            // Save two objects with objectID
            var objectsWithIds = new List<AlgoliaStub>
            {
                new AlgoliaStub { ObjectId = "two" }, new AlgoliaStub { ObjectId = "three" }
            };

            var addObjects = _index.SaveObjectsAsync(objectsWithIds);

            // Save two objects w/o objectIDs
            var objectsWoId = new List<AlgoliaStub>
            {
                new AlgoliaStub { Property = "addObjectsWoId" }, new AlgoliaStub { Property = "addObjectsWoId" }
            };

            var addObjectsWoId = _index.SaveObjectsAsync(objectsWoId, autoGenerateObjectId: true);

            // Batch 1000 objects
            var objectsToBatch = new List<AlgoliaStub>();
            var ids = new List<string>();

            for (int i = 0; i < 1000; i++)
            {
                var id = (i + 1).ToString();
                objectsToBatch.Add(new AlgoliaStub { ObjectId = id, Property = $"Property{id}" });
                ids.Add(id);
            }

            var batch = _index.SaveObjectsAsync(objectsToBatch);

            // Wait for all http call to finish
            var responses = await Task.WhenAll(new[] { addObject, addObjectWoId, addObjects, addObjectsWoId, batch })
                .ConfigureAwait(false);

            // Wait for Algolia's task to finish (indexing)
            responses.Wait();

            // Six first records
            var generatedId = addObjectWoId.Result.Responses[0].ObjectIDs.ToList();
            objectWoId.ObjectId = generatedId.ElementAt(0);

            var generatedIDs = addObjectsWoId.Result.Responses[0].ObjectIDs.ToList();
            objectsWoId[0].ObjectId = generatedIDs.ElementAt(0);
            objectsWoId[1].ObjectId = generatedIDs.ElementAt(1);

            var settedIds = new List<string> { "one", "two", "three" };

            var sixFirstRecordsIds = settedIds.Concat(generatedId).Concat(generatedIDs).ToList();
            var sixFirstRecords = (await _index.GetObjectsAsync<AlgoliaStub>(sixFirstRecordsIds)).ToList();
            Assert.That(sixFirstRecords, Has.Exactly(6).Items);

            var objectsToCompare = new List<AlgoliaStub> { objectOne }.Concat(objectsWithIds)
                .Concat(new List<AlgoliaStub> { objectWoId })
                .Concat(objectsWoId)
                .ToList();

            // Check retrieved objects againt original content
            Parallel.For(0, sixFirstRecords.Count, i =>
            {
                Assert.True(TestHelper.AreObjectsEqual(sixFirstRecords[i], objectsToCompare[i]));
            });

            // 1000 records
            var batchResponse = (await _index.GetObjectsAsync<AlgoliaStub>(ids)).ToList();
            Assert.That(batchResponse, Has.Exactly(1000).Items);

            // Check retrieved objects againt original content
            Parallel.For(0, batchResponse.Count, i =>
            {
                Assert.True(TestHelper.AreObjectsEqual(objectsToBatch[i], batchResponse[i]));
            });

            // Browse all index to assert that we have 1006 objects
            var objectsBrowsed = new List<AlgoliaStub>();

            foreach (var item in _index.Browse<AlgoliaStub>(new BrowseIndexQuery()))
                objectsBrowsed.Add(item);

            Assert.That(objectsBrowsed, Has.Exactly(1006).Items);

            // Update one object
            var objectToPartialUpdate = objectsToBatch.ElementAt(0);
            objectToPartialUpdate.Property = "PartialUpdated";

            var partialUpdateObject = await _index.PartialUpdateObjectAsync(objectToPartialUpdate);
            partialUpdateObject.Wait();

            var getUpdatedObject = await _index.GetObjectAsync<AlgoliaStub>(objectToPartialUpdate.ObjectId);
            Assert.That(getUpdatedObject.Property, Is.EqualTo(objectToPartialUpdate.Property));

            // Update two objects
            var objectToPartialUpdate1 = objectsToBatch.ElementAt(1);
            objectToPartialUpdate1.Property = "PartialUpdated1";
            var objectToPartialUpdate2 = objectsToBatch.ElementAt(2);
            objectToPartialUpdate2.Property = "PartialUpdated2";

            var partialUpdateObjects = await _index.PartialUpdateObjectsAsync(new List<AlgoliaStub>
            {
                objectToPartialUpdate1, objectToPartialUpdate2
            });

            partialUpdateObjects.Wait();

            var getUpdatedObjects = (await _index.GetObjectsAsync<AlgoliaStub>(new List<string>
            {
                objectToPartialUpdate1.ObjectId, objectToPartialUpdate2.ObjectId
            })).ToList();

            Assert.That(getUpdatedObjects.ElementAt(0).Property, Is.EqualTo(objectToPartialUpdate1.Property));
            Assert.That(getUpdatedObjects.ElementAt(1).Property, Is.EqualTo(objectToPartialUpdate2.Property));

            // Delete six first objects
            var deleteObjects = await _index.DeleteObjectsAsync(sixFirstRecordsIds);

            // Assert that the objects were deleted
            var objectsBrowsedAfterDelete = new List<AlgoliaStub>();

            deleteObjects.Wait();
            foreach (var item in _index.Browse<AlgoliaStub>(new BrowseIndexQuery()))
                objectsBrowsedAfterDelete.Add(item);

            Assert.That(objectsBrowsedAfterDelete, Has.Exactly(1000).Items);

            // Delete remaining objects
            var deleteRemainingObjects = await _index.DeleteObjectsAsync(ids);
            deleteRemainingObjects.Wait();

            // Assert that all objects were deleted
            var search = await _index.SearchAsync<AlgoliaStub>(new Query(""));
            Assert.That(search.Hits, Is.Empty);
        }

        [Test]
        [Parallelizable]
        public async Task DeleteByTest()
        {
            List<AlgoliaStub> objectsToBatch = new List<AlgoliaStub>();
            List<string> ids = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                var id = (i + 1).ToString();
                objectsToBatch.Add(new AlgoliaStub { ObjectId = id, Tags = new List<string> { "car" } });
                ids.Add(id);
            }

            var batch = await _indexDeleteBy.SaveObjectsAsync(objectsToBatch);
            batch.Wait();

            var delete = await _indexDeleteBy.DeleteObjectAsync("1");
            delete.Wait();

            var searchAfterDelete = await _indexDeleteBy.SearchAsync<AlgoliaStub>(new Query(""));
            Assert.That(searchAfterDelete.Hits, Has.Exactly(9).Items);

            var resp = await _indexDeleteBy.DeleteByAsync(new Query
            {
                TagFilters = new List<List<string>> { new List<string> { "car" } }
            });
            resp.Wait();

            var search = await _indexDeleteBy.SearchAsync<AlgoliaStub>(new Query(""));
            Assert.That(search.Hits, Is.Empty);
        }

        [Test]
        [Parallelizable]
        public async Task ClearObjects()
        {
            List<AlgoliaStub> objectsToBatch = new List<AlgoliaStub>();
            List<string> ids = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                var id = (i + 1).ToString();
                objectsToBatch.Add(new AlgoliaStub { ObjectId = id, Tags = new List<string> { "car" } });
                ids.Add(id);
            }

            var batch = await _indexClear.SaveObjectsAsync(objectsToBatch);
            batch.Wait();

            var clear = await _indexClear.ClearObjectsAsync();
            clear.Wait();

            var search = await _indexClear.SearchAsync<AlgoliaStub>(new Query(""));
            Assert.That(search.Hits, Is.Empty);
        }

        [Test]
        [Parallelizable]
        public async Task MoveIndexTest()
        {
            var objectOne = new JObject { { "objectID", "one" } };
            var addObject = await _indexMove.SaveObjectAsync(objectOne);

            addObject.Wait();

            var indexDestName = TestHelper.GetTestIndexName("move_test_dest");

            var move = await BaseTest.SearchClient.MoveIndexAsync(_indexMoveName, indexDestName);
            move.Wait();

            var listIndices = await BaseTest.SearchClient.ListIndicesAsync();
            Assert.True(listIndices.Items.Exists(x => x.Name.Equals(indexDestName)));
            Assert.False(listIndices.Items.Exists(x => x.Name.Equals(_indexMoveName)));
        }

        [Test]
        [Parallelizable]
        public async Task IndexOperationsAsyncWithJObjectTest()
        {
            //Add JObject with ID 
            var objectOne = new JObject { { "objectID", "one" }, { "title", "Foo" } };
            var addObject = await _indexWithJObject.SaveObjectAsync(objectOne);

            addObject.Wait();

            //Add JObject without ID with autoGenerateObjectId
            var objectOneWoId = new JObject { { "title", "Bar" } };
            var addObjectWoId = await _indexWithJObject.SaveObjectAsync(objectOneWoId, autoGenerateObjectId: true);

            addObjectWoId.Wait();

            //Add JObject without ID without autoGenerateObjectId
            Assert.ThrowsAsync<AlgoliaApiException>(() => _indexWithJObject.SaveObjectAsync(objectOneWoId, autoGenerateObjectId: false));

            //Update record with JObject
            var objectTwo = new JObject { { "objectID", "one" }, { "title", "Baz" } };
            var updateObject = await _indexWithJObject.PartialUpdateObjectAsync(objectTwo);

            updateObject.Wait();

            //Update record with JObject without ID
            var objectTwoWoId = new JObject { { "title", "Bam" } };

            Assert.ThrowsAsync<AlgoliaException>(() => _indexWithJObject.PartialUpdateObjectAsync(objectTwoWoId));

            // Clear the index
            var clear = await _indexWithJObject.ClearObjectsAsync();
            clear.Wait();

            //Check if index is empty
            var search = await _indexClear.SearchAsync<AlgoliaStub>(new Query(""));
            Assert.That(search.Hits, Is.Empty);
        }
    }

    public class AlgoliaObject
    {
        [JsonProperty(PropertyName = "objectID")]
        public string ObjectId { get; set; }
    }

    public class AlgoliaStub : AlgoliaObject
    {
        public string Property { get; set; } = "Default";
        [JsonProperty(PropertyName = "_tags")] public List<string> Tags { get; set; }
    }
}

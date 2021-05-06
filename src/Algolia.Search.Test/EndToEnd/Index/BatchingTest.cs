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
using System.Threading.Tasks;
using Algolia.Search.Clients;
using Algolia.Search.Iterators;
using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Enums;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd.Index
{
    [TestFixture]
    [Parallelizable]
    public class BatchingTest
    {
        private SearchIndex _index;
        private SearchIndex _index2;

        private const string Index1Name = "index_batching";
        private const string Index2Name = "index_batching_2";

        [OneTimeSetUp]
        public void Init()
        {
            _index = BaseTest.SearchClient.InitIndex(TestHelper.GetTestIndexName(Index1Name));
            _index2 = BaseTest.SearchClient.InitIndex(TestHelper.GetTestIndexName(Index2Name));
        }

        [Test]
        public async Task TestBatching()
        {
            List<ObjectToBatch> batchOne = new List<ObjectToBatch>
            {
                new ObjectToBatch { ObjectID = "one", Key = "value" },
                new ObjectToBatch { ObjectID = "two", Key = "value" },
                new ObjectToBatch { ObjectID = "three", Key = "value" },
                new ObjectToBatch { ObjectID = "four", Key = "value" },
                new ObjectToBatch { ObjectID = "five", Key = "value" }
            };

            var batchOneResponse = await _index.SaveObjectsAsync(batchOne);
            batchOneResponse.Wait();

            List<BatchOperation<ObjectToBatch>> operations = new List<BatchOperation<ObjectToBatch>>
            {
                new BatchOperation<ObjectToBatch>
                {
                    Action = BatchActionType.AddObject,
                    Body = new ObjectToBatch { ObjectID = "zero", Key = "value" }
                },
                new BatchOperation<ObjectToBatch>
                {
                    Action = BatchActionType.UpdateObject, Body = new ObjectToBatch { ObjectID = "one", Key = "v" }
                },
                new BatchOperation<ObjectToBatch>
                {
                    Action = BatchActionType.PartialUpdateObject,
                    Body = new ObjectToBatch { ObjectID = "two", Key = "v" }
                },
                new BatchOperation<ObjectToBatch>
                {
                    Action = BatchActionType.PartialUpdateObject,
                    Body = new ObjectToBatch { ObjectID = "two_bis", Key = "value" }
                },
                new BatchOperation<ObjectToBatch>
                {
                    Action = BatchActionType.PartialUpdateObjectNoCreate,
                    Body = new ObjectToBatch { ObjectID = "three", Key = "v" }
                },
                new BatchOperation<ObjectToBatch>
                {
                    Action = BatchActionType.DeleteObject, Body = new ObjectToBatch { ObjectID = "four" }
                },
            };

            var batchTwoResponse = await _index.BatchAsync(operations);
            batchTwoResponse.Wait();

            List<ObjectToBatch> objectsFromIterator = new List<ObjectToBatch>();
            IndexIterator<ObjectToBatch> iterator = new IndexIterator<ObjectToBatch>(_index, new BrowseIndexQuery());

            foreach (var item in iterator)
            {
                objectsFromIterator.Add(item);
            }

            Assert.That(objectsFromIterator, Has.Exactly(6).Items);
            Assert.True(TestHelper.AreObjectsEqual(objectsFromIterator.Find(r => r.ObjectID.Equals("zero")),
                operations.Find(r => r.Body.ObjectID.Equals("zero")).Body));
            Assert.True(TestHelper.AreObjectsEqual(objectsFromIterator.Find(r => r.ObjectID.Equals("one")),
                operations.Find(r => r.Body.ObjectID.Equals("one")).Body));
            Assert.True(TestHelper.AreObjectsEqual(objectsFromIterator.Find(r => r.ObjectID.Equals("two")),
                operations.Find(r => r.Body.ObjectID.Equals("two")).Body));
            Assert.True(TestHelper.AreObjectsEqual(objectsFromIterator.Find(r => r.ObjectID.Equals("two_bis")),
                operations.Find(r => r.Body.ObjectID.Equals("two_bis")).Body));
            Assert.True(TestHelper.AreObjectsEqual(objectsFromIterator.Find(r => r.ObjectID.Equals("three")),
                operations.Find(r => r.Body.ObjectID.Equals("three")).Body));
            Assert.True(TestHelper.AreObjectsEqual(objectsFromIterator.Find(r => r.ObjectID.Equals("five")),
                batchOne.Find(r => r.ObjectID.Equals("five"))));
            Assert.False(objectsFromIterator.Exists(x => x.ObjectID.Equals("four")));
        }

        public class ObjectToBatch
        {
            public string ObjectID { get; set; }
            public string Key { get; set; }
        }

        public class ObjectToBatch2
        {
            public string ObjectID { get; set; }
            public string Key { get; set; }
        }



        [Test]
        public async Task TestBatching_MultipleTypes()
        {
            var batchOne = new List<ObjectToBatch>
            {
                new ObjectToBatch { ObjectID = "one", Key = "value" },
            };
            var batchTwo = new List<ObjectToBatch2>
            {
                new ObjectToBatch2 { ObjectID = "two", Key = "value" },
            };

            var addOneResponse = await _index.SaveObjectsAsync(batchOne);
            addOneResponse.Wait();

            var addTwoResponse = await _index2.SaveObjectsAsync(batchTwo);
            addTwoResponse.Wait();

            var operations = new List<BatchOperation>
            {
                new BatchOperation
                {
                    Action = BatchActionType.UpdateObject,
                    Body = new ObjectToBatch { ObjectID = "one", Key = "v" },
                    IndexName = Index1Name,
                },
                new BatchOperation
                {
                    Action = BatchActionType.UpdateObject,
                    Body = new ObjectToBatch2 { ObjectID = "two", Key = "v" },
                    IndexName = Index2Name,
                },
            };

            var batchTwoResponse = await _index.BatchAsync(operations);
            batchTwoResponse.Wait();

            List<ObjectToBatch> objectsFromIterator = new List<ObjectToBatch>();
            IndexIterator<ObjectToBatch> iterator = new IndexIterator<ObjectToBatch>(_index, new BrowseIndexQuery());

            List<ObjectToBatch2> objectsFromIterator2 = new List<ObjectToBatch2>();
            IndexIterator<ObjectToBatch2> iterator2 = new IndexIterator<ObjectToBatch2>(_index2, new BrowseIndexQuery());

            foreach (var item in iterator)
            {
                objectsFromIterator.Add(item);
            }
            foreach (var item in iterator2)
            {
                objectsFromIterator2.Add(item);
            }

            Assert.That(objectsFromIterator, Has.Exactly(6).Items);
            Assert.True(TestHelper.AreObjectsEqual(objectsFromIterator.Find(r => r.ObjectID.Equals("one")),
                operations.Find(r => (r.Body as ObjectToBatch)?.ObjectID.Equals("one") ?? false).Body));
            Assert.True(TestHelper.AreObjectsEqual(objectsFromIterator.Find(r => r.ObjectID.Equals("two")),
                operations.Find(r => (r.Body as ObjectToBatch2)?.ObjectID.Equals("two") ?? false).Body));
        }
    }
}

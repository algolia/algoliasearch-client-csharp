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
using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Index
{
    [TestFixture]
    [Parallelizable]
    public class BatchingTest
    {
        private SearchIndex _index;

        [OneTimeSetUp]
        public void Init()
        {
            _index = BaseTest.SearchClient.InitIndex(TestHelper.GetTestIndexName("index_batching"));
        }

        [Test]
        public async Task TestBatching()
        {
            List<ObjectToBatch> batchOne = new List<ObjectToBatch>
            {
                new ObjectToBatch {ObjectID = "one", Key = "value"},
                new ObjectToBatch {ObjectID = "two", Key = "value"},
                new ObjectToBatch {ObjectID = "three", Key = "value"},
                new ObjectToBatch {ObjectID = "four", Key = "value"},
                new ObjectToBatch {ObjectID = "five", Key = "value"}
            };

            var batchOneResponse = await _index.SaveObjectsAsync(batchOne);
            batchOneResponse.Wait();

            List<BatchOperation<ObjectToBatch>> operations = new List<BatchOperation<ObjectToBatch>>
            {
                new BatchOperation<ObjectToBatch>
                    {Action = BatchActionType.AddObject, Body = new ObjectToBatch {ObjectID = "zero", Key = "value"}},
                new BatchOperation<ObjectToBatch>
                    {Action = BatchActionType.UpdateObject, Body = new ObjectToBatch {ObjectID = "one", Key = "v"}},
                new BatchOperation<ObjectToBatch>
                {
                    Action = BatchActionType.PartialUpdateObject, Body = new ObjectToBatch {ObjectID = "two", Key = "v"}
                },
                new BatchOperation<ObjectToBatch>
                {
                    Action = BatchActionType.PartialUpdateObject,
                    Body = new ObjectToBatch {ObjectID = "two_bis", Key = "value"}
                },
                new BatchOperation<ObjectToBatch>
                {
                    Action = BatchActionType.PartialUpdateObjectNoCreate,
                    Body = new ObjectToBatch {ObjectID = "three", Key = "v"}
                },
                new BatchOperation<ObjectToBatch>
                    {Action = BatchActionType.DeleteObject, Body = new ObjectToBatch {ObjectID = "four"}},
            };

            var batchTwoResponse = await _index.BatchAsync(operations);
            batchTwoResponse.Wait();

            List<ObjectToBatch> objectsFromIterator = new List<ObjectToBatch>();
            IndexIterator<ObjectToBatch> iterator = new IndexIterator<ObjectToBatch>(_index, new BrowseIndexQuery());

            foreach (var item in iterator)
            {
                objectsFromIterator.Add(item);
            }

            Assert.True(objectsFromIterator.Count == 6);
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
    }
}
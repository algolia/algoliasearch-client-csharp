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

using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Search;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Client
{
    [TestFixture]
    [Parallelizable]
    public class MultipleOperationsTest
    {
        private string _indexName1;
        private string _indexName2;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName1 = TestHelper.GetTestIndexName("multiple_operations");
            _indexName2 = TestHelper.GetTestIndexName("multiple_operations_dev");
        }

        [Test]
        public async Task TestMultipleOperations()
        {
            var objectsToSave = new List<BatchOperation<MultipleOperationClass>>
            {
                new BatchOperation<MultipleOperationClass>
                {
                    IndexName = _indexName1, Action = BatchActionType.AddObject,
                    Body = new MultipleOperationClass {Firstname = "Jimmie"}
                },
                new BatchOperation<MultipleOperationClass>
                {
                    IndexName = _indexName1, Action = BatchActionType.AddObject,
                    Body = new MultipleOperationClass {Firstname = "Jimmie"}
                },
                new BatchOperation<MultipleOperationClass>
                {
                    IndexName = _indexName2, Action = BatchActionType.AddObject,
                    Body = new MultipleOperationClass {Firstname = "Jimmie"}
                },
                new BatchOperation<MultipleOperationClass>
                {
                    IndexName = _indexName2, Action = BatchActionType.AddObject,
                    Body = new MultipleOperationClass {Firstname = "Jimmie"}
                }
            };

            var saveMultiple = await BaseTest.SearchClient.MultipleBatchAsync(objectsToSave);
            saveMultiple.Wait();

            var objectsToRetrieve = new List<MultipleGetObject>
            {
                new MultipleGetObject {IndexName = _indexName1, ObjectID = saveMultiple.ObjectIDs.ElementAt(0)},
                new MultipleGetObject {IndexName = _indexName1, ObjectID = saveMultiple.ObjectIDs.ElementAt(1)},
                new MultipleGetObject {IndexName = _indexName2, ObjectID = saveMultiple.ObjectIDs.ElementAt(2)},
                new MultipleGetObject {IndexName = _indexName2, ObjectID = saveMultiple.ObjectIDs.ElementAt(3)}
            };

            var multipleGet =
                await BaseTest.SearchClient.MultipleGetObjectsAsync<MultipleOperationClass>(objectsToRetrieve);
            Assert.True(multipleGet.Results.Count() == 4);
            Assert.True(multipleGet.Results.All(x => x.Firstname.Equals("Jimmie")));

            for (int i = 0; i < 4; i++)
            {
                Assert.True(multipleGet.Results.ElementAt(i).ObjectID == saveMultiple.ObjectIDs.ElementAt(i));
            }

            List<MultipleQueries> multipleSearch = new List<MultipleQueries>
            {
                new MultipleQueries {IndexName = _indexName1, Params = new Query {HitsPerPage = 2}},
                new MultipleQueries {IndexName = _indexName2, Params = new Query {HitsPerPage = 2}},
            };

            MultipleQueriesRequest request = new MultipleQueriesRequest
            {
                Strategy = StrategyType.None,
                Requests = multipleSearch
            };

            MultipleQueriesRequest request2 = new MultipleQueriesRequest
            {
                Strategy = StrategyType.StopIfEnoughMatches,
                Requests = multipleSearch
            };

            var multiQueri = await BaseTest.SearchClient.MultipleQueriesAsync<MultipleOperationClass>(request);
            var multiQueri2 = await BaseTest.SearchClient.MultipleQueriesAsync<MultipleOperationClass>(request2);

            Assert.True(multiQueri.Results.Count() == 2);
            Assert.True(multiQueri.Results.ElementAt(0).Hits.Count() == 2);
            Assert.True(multiQueri.Results.ElementAt(1).Hits.Count() == 2);

            Assert.True(multiQueri2.Results.Count() == 2);
            Assert.True(multiQueri2.Results.ElementAt(0).Hits.Count() == 2);
            Assert.True(!multiQueri2.Results.ElementAt(1).Hits.Any());
        }

        public class MultipleOperationClass
        {
            public string ObjectID { get; set; }
            public string Firstname { get; set; }
        }
    }
}
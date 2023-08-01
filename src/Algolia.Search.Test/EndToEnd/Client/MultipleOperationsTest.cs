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
using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Search;
using NUnit.Framework;

namespace Algolia.Search.Test.EndToEnd.Client
{
    [TestFixture]
    [Parallelizable]
    public class MultipleOperationsTest
    {
        private string _indexName1;
        private string _indexName2;
        private string _indexName3;

        [OneTimeSetUp]
        public void Init()
        {
            _indexName1 = TestHelper.GetTestIndexName("multiple_operations");
            _indexName2 = TestHelper.GetTestIndexName("multiple_operations_dev");
            _indexName3 = TestHelper.GetTestIndexName("multiple_operations_facet");

        }

        [Test]
        public async Task TestMultipleOperations()
        {
            var objectsToSave = new List<BatchOperation<MultipleOperationClass>>
            {
                new BatchOperation<MultipleOperationClass>
                {
                    IndexName = _indexName1,
                    Action = BatchActionType.AddObject,
                    Body = new MultipleOperationClass { Firstname = "Jimmie" }
                },
                new BatchOperation<MultipleOperationClass>
                {
                    IndexName = _indexName1,
                    Action = BatchActionType.AddObject,
                    Body = new MultipleOperationClass { Firstname = "Jimmie" }
                },
                new BatchOperation<MultipleOperationClass>
                {
                    IndexName = _indexName2,
                    Action = BatchActionType.AddObject,
                    Body = new MultipleOperationClass { Firstname = "Jimmie" }
                },
                new BatchOperation<MultipleOperationClass>
                {
                    IndexName = _indexName2,
                    Action = BatchActionType.AddObject,
                    Body = new MultipleOperationClass { Firstname = "Jimmie" }
                }
            };

            var saveMultiple = await BaseTest.SearchClient.MultipleBatchAsync(objectsToSave);
            saveMultiple.Wait();

            var objectsToRetrieve = new List<MultipleGetObject>
            {
                new MultipleGetObject { IndexName = _indexName1, ObjectID = saveMultiple.ObjectIDs.ElementAt(0) },
                new MultipleGetObject { IndexName = _indexName1, ObjectID = saveMultiple.ObjectIDs.ElementAt(1) },
                new MultipleGetObject { IndexName = _indexName2, ObjectID = saveMultiple.ObjectIDs.ElementAt(2) },
                new MultipleGetObject { IndexName = _indexName2, ObjectID = saveMultiple.ObjectIDs.ElementAt(3) }
            };

            var multipleGet =
                await BaseTest.SearchClient.MultipleGetObjectsAsync<MultipleOperationClass>(objectsToRetrieve);
            Assert.That(multipleGet.Results, Has.Exactly(4).Items);
            Assert.True(multipleGet.Results.All(x => x.Firstname.Equals("Jimmie")));

            for (int i = 0; i < 4; i++)
            {
                Assert.True(multipleGet.Results.ElementAt(i).ObjectID == saveMultiple.ObjectIDs.ElementAt(i));
            }

            List<MultipleQueries> multipleSearch = new List<MultipleQueries>
            {
                new MultipleQueries { IndexName = _indexName1, Params = new Query { HitsPerPage = 2 } },
                new MultipleQueries { IndexName = _indexName2, Params = new Query { HitsPerPage = 2 } },
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

            Assert.That(multiQueri.Results, Has.Exactly(2).Items);
            Assert.That(multiQueri.Results.ElementAt(0).Hits, Has.Exactly(2).Items);
            Assert.That(multiQueri.Results.ElementAt(1).Hits, Has.Exactly(2).Items);

            Assert.That(multiQueri2.Results, Has.Exactly(2).Items);
            Assert.That(multiQueri2.Results.ElementAt(0).Hits, Has.Exactly(2).Items);
            Assert.That(multiQueri2.Results.ElementAt(1).Hits, Is.Empty);
        }
        [Test]
        public async Task TestMultipleQueriesWithQueryMultiIndicesObject()
        {
            var objectsToSave = new List<BatchOperation<MultipleOperationClass>>
            {
                new BatchOperation<MultipleOperationClass>
                {
                    IndexName = _indexName3,
                    Action = BatchActionType.AddObject,
                    Body = new MultipleOperationClass { Firstname = "Jimmie" }
                },
            };

            var saveMultiple = await BaseTest.SearchClient.MultipleBatchAsync(objectsToSave);
            saveMultiple.Wait();

            var query = new Query()
            {
                Explain = new List<string> { "test1", "test2" },
                AroundPrecision = new List<AroundPrecision>
                {
                    new(){From = 0, Value = 1},
                    new(){From = 100, Value = 10}
                },
                CustomParameters = new Dictionary<string, object>()
                {
                    {"hitsPerPage", 10}
                },
                TagFilters = new List<IEnumerable<string>>()
                {
                    new List<string>{ "one", "two"},
                    new List<string>{ "one-two", "two-two"}
                }
            };

            var request = new MultipleQueriesRequest
            {
                Requests = new List<QueryMultiIndices>{
                    new QueryMultiIndices(_indexName3)
                    {
                        Explain = new List<string>{"test1", "test2"},
                        AroundPrecision = new List<AroundPrecision>
                        {
                            new(){From = 0, Value = 1},
                            new(){From = 100, Value = 10}
                        },
                        CustomParameters = new Dictionary<string, object>()
                        {
                            {"hitsPerPage", 10}
                        },
                        TagFilters = new List<IEnumerable<string>>()
                        {
                            new List<string>{ "one", "two"},
                            new List<string>{ "one-two", "two-two"}
                        }
                    }
                }
            };

            var index = BaseTest.SearchClient.InitIndex(_indexName3);

            var responseSearch = index.Search<MultipleOperationClass>(query);
            var responseMultipleQueries = BaseTest.SearchClient.MultipleQueries<MultipleOperationClass>(request);

            Assert.AreEqual(responseSearch.Params, responseMultipleQueries.Results.First().Params);
        }

        [Test]
        public async Task TestMultipleQueriesFacet()
        {
            var objectsToSave = new List<BatchOperation<MultipleOperationClass>>
            {
                new BatchOperation<MultipleOperationClass>
                {
                    IndexName = _indexName3,
                    Action = BatchActionType.AddObject,
                    Body = new MultipleOperationClass { Firstname = "Jimmie" }
                },
            };

            var saveMultiple = await BaseTest.SearchClient.MultipleBatchAsync(objectsToSave);
            saveMultiple.Wait();

            var query = new Query()
            {
                FacetFilters = new List<List<string>>()
                {
                    new List<string>() { "sizeValue:1 \" x 3 \"" }
                }
            };


            MultipleQueriesRequest request = new MultipleQueriesRequest
            {
                Requests = new List<MultipleQueries>()
                {
                    new MultipleQueries
                    {
                        IndexName = _indexName3,
                        Params = query
                    },
                }
            };

            var index = BaseTest.SearchClient.InitIndex(_indexName3);

            var responseSearch = index.Search<MultipleOperationClass>(query);
            var responseMultipleQueries = BaseTest.SearchClient.MultipleQueries<MultipleOperationClass>(request);

            Assert.AreEqual(responseSearch.Params, responseMultipleQueries.Results.First().Params);
        }

        public class MultipleOperationClass
        {
            public string ObjectID { get; set; }
            public string Firstname { get; set; }
        }
    }
}

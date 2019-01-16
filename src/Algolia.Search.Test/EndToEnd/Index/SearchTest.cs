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
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Index
{
    [TestFixture]
    [Parallelizable]
    public class SearchTest
    {
        private SearchIndex _index;
        private IEnumerable<Employee> _employees;

        [OneTimeSetUp]
        public void Init()
        {
            var indexName = TestHelper.GetTestIndexName("search");
            _index = BaseTest.SearchClient.InitIndex(indexName);
            _employees = InitEmployees();
        }

        [Test]
        public async Task SearchTestAsync()
        {
            BatchIndexingResponse addObjectResponse =
                await _index.SaveObjectsAsync(_employees, autoGenerateObjectId: true);
            addObjectResponse.Wait();

            Assert.IsInstanceOf<BatchIndexingResponse>(addObjectResponse);
            Assert.NotNull(addObjectResponse);

            IndexSettings settings = new IndexSettings
            { AttributesForFaceting = new List<string> { "searchable(company)" } };
            var setSettingsResponse = await _index.SetSettingsAsync(settings);
            setSettingsResponse.Wait();

            Assert.IsInstanceOf<SetSettingsResponse>(setSettingsResponse);
            Assert.NotNull(setSettingsResponse);

            Task<SearchResponse<Employee>> searchAlgoliaTask = _index.SearchAsync<Employee>(new Query
            {
                SearchQuery = "algolia"
            });

            Task<SearchResponse<Employee>> searchElonTask = _index.SearchAsync<Employee>(new Query
            {
                SearchQuery = "elon",
                ClickAnalytics = true
            });

            Task<SearchResponse<Employee>> searchElonTask1 = _index.SearchAsync<Employee>(new Query
            {
                SearchQuery = "elon",
                Facets = new List<string> { "*" },
                FacetFilters = new List<List<string>> { new List<string> { "company:tesla" } }
            });

            Task<SearchResponse<Employee>> searchElonTask2 = _index.SearchAsync<Employee>(new Query
            {
                SearchQuery = "elon",
                Facets = new List<string> { "*" },
                Filters = "(company:tesla OR company:spacex)"
            });

            Task<SearchForFacetResponse> searchFacetTask = _index.SearchForFacetValueAsync(new SearchForFacetRequest
            {
                FacetName = "company",
                FacetQuery = "a"
            });

            Task.WaitAll(searchAlgoliaTask, searchElonTask, searchElonTask1, searchElonTask2, searchFacetTask);

            Assert.IsTrue(searchAlgoliaTask.Result.Hits.Count == 2);
            Assert.IsTrue(searchElonTask.Result.QueryID != null);
            Assert.IsTrue(searchElonTask1.Result.Hits.Count == 1);
            Assert.IsTrue(searchElonTask2.Result.Hits.Count == 2);
            Assert.IsTrue(searchFacetTask.Result.FacetHits.Any(x => x.Value.Equals("Algolia")));
            Assert.IsTrue(searchFacetTask.Result.FacetHits.Any(x => x.Value.Equals("Amazon")));
            Assert.IsTrue(searchFacetTask.Result.FacetHits.Any(x => x.Value.Equals("Apple")));
            Assert.IsTrue(searchFacetTask.Result.FacetHits.Any(x => x.Value.Equals("Arista Networks")));
        }

        private IEnumerable<Employee> InitEmployees()
        {
            return new List<Employee>()
            {
                new Employee {Company = "Algolia", Name = "Julien Lemoine"},
                new Employee {Company = "Algolia", Name = "Nicolas Dessaigne"},
                new Employee {Company = "Amazon", Name = "Jeff Bezos"},
                new Employee {Company = "Apple", Name = "Steve Jobs"},
                new Employee {Company = "Apple", Name = "Steve Wozniak"},
                new Employee {Company = "Arista Networks", Name = "Jayshree Ullal"},
                new Employee {Company = "Google", Name = "Lary Page"},
                new Employee {Company = "Google", Name = "Rob Pike"},
                new Employee {Company = "Google", Name = "Sergueï Brin"},
                new Employee {Company = "Microsoft", Name = "Bill Gates"},
                new Employee {Company = "SpaceX", Name = "Elon Musk"},
                new Employee {Company = "Tesla", Name = "Elon Musk"},
                new Employee {Company = "Yahoo", Name = "Marissa Mayer"}
            };
        }

        public class Employee
        {
            public string Company { get; set; }
            public string Name { get; set; }
            public string QueryID { get; set; }
        }
    }
}
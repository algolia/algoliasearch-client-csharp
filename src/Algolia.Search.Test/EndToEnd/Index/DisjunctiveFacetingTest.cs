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
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algolia.Search.Test.EndToEnd.Index
{
    [TestFixture]
    [Parallelizable]
    public class DisjunctiveFacetingTest
    {
        private SearchIndex _index;
        private readonly string _indexName = TestHelper.GetTestIndexName("disjunctive_faceting");

        [OneTimeSetUp]
        public void Init()
        {
            _index = BaseTest.SearchClient.InitIndex(_indexName);
        }

        [Test]
        [Parallelizable]
        public async Task DisjunctiveFacetingTestAsync()
        {
            var settings = new IndexSettings
            {
                AttributesForFaceting = new List<string> { "city", "stars", "facilities" }
            };

            _ = await _index.SetSettingsAsync(settings);

            var saveObjects = await _index.SaveObjectsAsync(new List<Hotel>
            {
                new Hotel { Name = "Hotel A", Stars = "*", Facilities = new List<string>{ "wifi", "bath", "spa" }, City = "Paris", Price = 100 },
                new Hotel { Name = "Hotel B", Stars = "*", Facilities = new List<string>{ "wifi" }, City = "Paris" , Price = 50 },
                new Hotel { Name = "Hotel C", Stars = "**", Facilities = new List<string>{ "bath" }, City = "San Francisco", Price = 110 },
                new Hotel { Name = "Hotel D", Stars = "****", Facilities = new List<string>{ "spa" }, City = "Paris", Price = 300 },
                new Hotel { Name = "Hotel E", Stars = "****", Facilities = new List<string>{ "spa" }, City = "New York", Price = 400 },
            }, autoGenerateObjectId: true);

            saveObjects.Wait();

            var query = new Query("h") { Facets = new List<string> { "city" } };
            var disjunctiveFacets = new List<string> { "stars", "facilities" };
            var facetRefinements = new Dictionary<string, IEnumerable<string>>
            {
                { "stars", new List<string> { "*" } },
            };

            SearchResponse<Hotel> result;

            result = await _index.SearchDisjunctiveFacetingAsync<Hotel>(query, disjunctiveFacets);
            Assert.That(result.Hits, Has.Exactly(5).Items);
            Assert.That(result.Facets, Has.Exactly(1).Items);
            Assert.That(result.DisjunctiveFacets, Has.Exactly(2).Items);

            result = await _index.SearchDisjunctiveFacetingAsync<Hotel>(query, disjunctiveFacets, facetsRefinements: facetRefinements);
            Assert.That(result.Hits, Has.Exactly(2).Items);
            Assert.That(result.Facets, Has.Exactly(1).Items);
            Assert.That(result.DisjunctiveFacets, Has.Exactly(2).Items);
            Assert.That(result.DisjunctiveFacets["stars"]["*"], Is.EqualTo(2));
            Assert.That(result.DisjunctiveFacets["stars"]["**"], Is.EqualTo(1));
            Assert.That(result.DisjunctiveFacets["stars"]["****"], Is.EqualTo(2));

            facetRefinements.Add("city", new List<string> { "Paris" });
            result = await _index.SearchDisjunctiveFacetingAsync<Hotel>(query, disjunctiveFacets, facetsRefinements: facetRefinements);
            Assert.That(result.Hits, Has.Exactly(2).Items);
            Assert.That(result.Facets, Has.Exactly(1).Items);
            Assert.That(result.DisjunctiveFacets, Has.Exactly(2).Items);
            Assert.That(result.DisjunctiveFacets["stars"]["*"], Is.EqualTo(2));
            Assert.That(result.DisjunctiveFacets["stars"]["****"], Is.EqualTo(1));

            facetRefinements["stars"] = new List<string> { "*", "****" };
            result = await _index.SearchDisjunctiveFacetingAsync<Hotel>(query, disjunctiveFacets, facetsRefinements: facetRefinements);
            Assert.That(result.Hits, Has.Exactly(3).Items);
            Assert.That(result.Facets, Has.Exactly(1).Items);
            Assert.That(result.DisjunctiveFacets, Has.Exactly(2).Items);
            Assert.That(result.DisjunctiveFacets["stars"]["*"], Is.EqualTo(2));
            Assert.That(result.DisjunctiveFacets["stars"]["****"], Is.EqualTo(1));

            query.NumericFilters = new List<List<string>>
            {
                new List<string> { "price>100" },
                new List<string> { "price<500" }
            };

            result = await _index.SearchDisjunctiveFacetingAsync<Hotel>(query, disjunctiveFacets, facetsRefinements: facetRefinements);
            Assert.That(result.Hits, Has.Exactly(1).Items);
            Assert.That(result.Facets, Has.Exactly(1).Items);
            Assert.That(result.DisjunctiveFacets, Has.Exactly(2).Items);
            Assert.That(result.DisjunctiveFacets["stars"]["*"], Is.EqualTo(0));
            Assert.That(result.DisjunctiveFacets["stars"]["****"], Is.EqualTo(1));
        }

        class Hotel
        {
            public string Name { get; set; }
            public string Stars { get; set; }
            public IEnumerable<string> Facilities { get; set; }
            public string City { get; set; }
            public int Price { get; set; }
        }
    }
}

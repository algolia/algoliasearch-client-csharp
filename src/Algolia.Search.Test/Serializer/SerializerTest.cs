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

using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Personalization;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;
using Algolia.Search.Serializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Test.Serializer
{
    [TestFixture]
    [Parallelizable]
    public class SerializerTest
    {
        [Test]
        [Parallelizable]
        public void TestQueryStringEmpty()
        {
            Query query = new Query();
            Assert.AreEqual(query.ToQueryString(), "");
        }

        [Test]
        [Parallelizable]
        public void TestQueryWithHtmlEntities()
        {
            Query query = new Query("&?@:=");
            Assert.AreEqual(query.ToQueryString(), "query=%26%3F%40%3A%3D");
        }

        [Test]
        [Parallelizable]
        public void TestQueryWithUTF8()
        {
            Query query = new Query("é®„");
            Assert.AreEqual(query.ToQueryString(), "query=%C3%A9%C2%AE%E2%80%9E");
        }

        [Test]
        [Parallelizable]
        public void TestQueryWithBooleanParam()
        {
            Query query = new Query("") { AroundLatLngViaIP = true };
            Assert.AreEqual(query.ToQueryString(), "query=&aroundLatLngViaIP=true");
        }

        [Test]
        [Parallelizable]
        public void TestQueryWithDistinct()
        {
            Query query = new Query("") { Distinct = 0 };
            Assert.AreEqual(query.ToQueryString(), "query=&distinct=0");
        }

        [Test]
        [Parallelizable]
        public void TestQueryWithList()
        {
            Query query = new Query("") { Facets = new List<string> { "(attribute)" } };
            Assert.AreEqual(query.ToQueryString(), "query=&facets=%28attribute%29");

            Query query2 = new Query("") { RestrictSearchableAttributes = new List<string> { "attr1" } };
            Assert.AreEqual(query2.ToQueryString(), "query=&restrictSearchableAttributes=attr1");
        }

        [Test]
        [Parallelizable]
        public void TestQueryWithMultipleObjects()
        {
            Query query = new Query("") { IgnorePlurals = "true" };
            Assert.AreEqual(query.ToQueryString(), "query=&ignorePlurals=true");
        }

        [Test]
        [Parallelizable]
        public void TestQueryWithNestedList()
        {
            Query query = new Query("")
            {
                FacetFilters = new List<List<string>>
                {
                    new List<string> { "facet1:true" }, new List<string> { "facet2:true" }
                }
            };
            // Expected: query=&facetFilters=[["facet1:true"],["facet2:true"]]
            Assert.AreEqual(query.ToQueryString(),
                "query=&facetFilters=%5B%5B%22facet1%3Atrue%22%5D%2C%5B%22facet2%3Atrue%22%5D%5D");

            Query query2 = new Query("")
            {
                FacetFilters = new List<List<string>> { new List<string> { "facet1:true", "facet2:true" } }
            };
            // Expected: query=&facetFilters=[["facet1:true","facet2:true"]]
            Assert.AreEqual(query2.ToQueryString(),
                "query=&facetFilters=%5B%5B%22facet1%3Atrue%22%2C%22facet2%3Atrue%22%5D%5D");

            Query query3 = new Query("")
            {
                FacetFilters = new List<List<string>>
                {
                    new List<string> { "facet1:true", "facet2:true" }, new List<string> { "facet3:true" }
                }
            };
            // Expected: query=&facetFilters=[["facet1:true","facet2:true"],["facet3:true"]]
            Assert.AreEqual(query3.ToQueryString(),
                "query=&facetFilters=%5B%5B%22facet1%3Atrue%22%2C%22facet2%3Atrue%22%5D%2C%5B%22facet3%3Atrue%22%5D%5D");

            Query query4 = new Query("")
            {
                FacetFilters = new List<List<string>> { new List<string> { "facet1:true" } }
            };
            // Expected: query=&facetFilters=[["facet1:true"]]
            Assert.AreEqual(query4.ToQueryString(), "query=&facetFilters=%5B%5B%22facet1%3Atrue%22%5D%5D");

            Query query5 = new Query("")
            {
                InsideBoundingBox = new List<List<float>>
                {
                    new List<float> { 47.3165f, 4.9665f, 47.3424f, 5.0201f },
                    new List<float> { 40.9234f, 2.1185f, 38.643f, 1.9916f }
                }
            };
            // Expected: query=&insideBoundingBox=[[47.3165,4.9665,47.3424,5.0201],[40.9234,2.1185,38.643,1.9916]]
            Assert.AreEqual(query5.ToQueryString(),
                "query=&insideBoundingBox=%5B%5B47.3165%2C4.9665%2C47.3424%2C5.0201%5D%2C%5B40.9234%2C2.1185%2C38.643%2C1.9916%5D%5D");

            Query query6 = new Query("")
            {
                InsideBoundingBox =
                    new List<List<float>> { new List<float> { 47.3165f, 4.9665f, 47.3424f, 5.0201f } }
            };
            // Expected: query=&insideBoundingBox=[[47.3165,4.9665,47.3424,5.0201]]
            Assert.AreEqual(query6.ToQueryString(),
                "query=&insideBoundingBox=%5B%5B47.3165%2C4.9665%2C47.3424%2C5.0201%5D%5D");
        }

        [Test]
        [Parallelizable]
        public void TestQueryWithCustomParameters()
        {
            Query query = new Query("algolia")
            {
                CustomParameters = new Dictionary<string, object> { { "newParameter", 10 } }
            };
            string json = JsonConvert.SerializeObject(query, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"query\":\"algolia\",\"newParameter\":10}");
        }

        [Test]
        [Parallelizable]
        public void TestAutomaticFacetFilters()
        {
            string json = "[\"lastname\",\"firstname\"]";

            List<AutomaticFacetFilter> deserialized =
                JsonConvert.DeserializeObject<List<AutomaticFacetFilter>>(json, new AutomaticFacetFiltersConverter());

            Assert.True(deserialized.ElementAt(0).Facet.Equals("lastname"));
            Assert.False(deserialized.ElementAt(0).Disjunctive);
            Assert.IsNull(deserialized.ElementAt(0).Score);

            Assert.True(deserialized.ElementAt(1).Facet.Equals("firstname"));
            Assert.False(deserialized.ElementAt(1).Disjunctive);
            Assert.IsNull(deserialized.ElementAt(1).Score);
        }

        [Test]
        [Parallelizable]
        public void TestLegacyFilterFormats()
        {
            // Testing "one string" legacy filters => should be converted to "ORED" nested filters
            // [["color:green","color:yellow"]]
            string stringFilters = "\"color:green,color:yellow\"";

            var serializedStringFilters =
                JsonConvert.DeserializeObject<List<List<string>>>(stringFilters, new FiltersConverter());

            AssertOredResult(serializedStringFilters);

            // Testing "one array" legacy filters => should be converted to "ORED" nested filters
            // [["color:green","color:yellow"]]
            string arrayFilters = "[\"color:green\",\"color:yellow\"]";

            var serializedArrayFilter =
                JsonConvert.DeserializeObject<List<List<string>>>(arrayFilters, new FiltersConverter());

            AssertOredResult(serializedArrayFilter);

            string nestedArrayFilters = "[[\"color:green\",\"color:yellow\"]]";

            var serializedNestedArrayFilter =
                JsonConvert.DeserializeObject<List<List<string>>>(nestedArrayFilters, new FiltersConverter());

            AssertOredResult(serializedNestedArrayFilter);

            // Testing the latest format of filters i.e nested arrays
            string nestedAndedArrayFilters = "[[\"color:green\",\"color:yellow\"],[\"color:blue\"]]";

            var serializedAdedNestedArrayFilter =
                JsonConvert.DeserializeObject<List<List<string>>>(nestedAndedArrayFilters, new FiltersConverter());

            Assert.That(serializedAdedNestedArrayFilter, Has.Count.EqualTo(2));
            Assert.That(serializedAdedNestedArrayFilter.ElementAt(0), Has.Count.EqualTo(2));
            Assert.That(serializedAdedNestedArrayFilter.ElementAt(0).ElementAt(0), Contains.Substring("color:green"));
            Assert.That(serializedAdedNestedArrayFilter.ElementAt(0).ElementAt(1), Contains.Substring("color:yellow"));
            Assert.That(serializedAdedNestedArrayFilter.ElementAt(1), Has.Count.EqualTo(1));
            Assert.That(serializedAdedNestedArrayFilter.ElementAt(1).ElementAt(0), Contains.Substring("color:blue"));

            // Finally, testing that the custom reader is not breaking current implementation
            Rule ruleWithFilters = new Rule
            {
                Consequence = new Consequence
                {
                    Params = new ConsequenceParams
                    {
                        OptionalFilters = new List<List<string>> { new List<string> { "a:b" } },
                        TagFilters =
                            new List<List<string>>
                            {
                                new List<string> { "a:b", "c:d" }, new List<string> { "d:e" }
                            },
                        FacetFilters =
                            new List<List<string>>
                            {
                                new List<string> { "a:b" }, new List<string> { "c:d" }
                            },
                        NumericFilters = new List<List<string>> { new List<string> { "a=100" } }
                    }
                },
            };

            // Json "sent" to the API
            string json = JsonConvert.SerializeObject(ruleWithFilters, JsonConfig.AlgoliaJsonSerializerSettings);

            // Json "retrieved" from the API
            var newRule = JsonConvert.DeserializeObject<Rule>(json);

            Assert.That(newRule.Consequence.Params.OptionalFilters, Has.Count.EqualTo(1));
            Assert.That(newRule.Consequence.Params.OptionalFilters.ElementAt(0), Has.Count.EqualTo(1));
            Assert.That(newRule.Consequence.Params.OptionalFilters.ElementAt(0).ElementAt(0),
                Contains.Substring("a:b"));

            Assert.That(newRule.Consequence.Params.TagFilters, Has.Count.EqualTo(2));
            Assert.That(newRule.Consequence.Params.TagFilters.ElementAt(0), Has.Count.EqualTo(2));
            Assert.That(newRule.Consequence.Params.TagFilters.ElementAt(0).ElementAt(0), Contains.Substring("a:b"));
            Assert.That(newRule.Consequence.Params.TagFilters.ElementAt(0).ElementAt(1), Contains.Substring("c:d"));
            Assert.That(newRule.Consequence.Params.TagFilters.ElementAt(1), Has.Count.EqualTo(1));
            Assert.That(newRule.Consequence.Params.TagFilters.ElementAt(1).ElementAt(0), Contains.Substring("d:e"));

            Assert.That(newRule.Consequence.Params.FacetFilters, Has.Count.EqualTo(2));
            Assert.That(newRule.Consequence.Params.FacetFilters.ElementAt(0), Has.Count.EqualTo(1));
            Assert.That(newRule.Consequence.Params.FacetFilters.ElementAt(1), Has.Count.EqualTo(1));

            Assert.That(newRule.Consequence.Params.NumericFilters, Has.Count.EqualTo(1));
            Assert.That(newRule.Consequence.Params.NumericFilters.ElementAt(0), Has.Count.EqualTo(1));
            Assert.That(newRule.Consequence.Params.NumericFilters.ElementAt(0).ElementAt(0),
                Contains.Substring("a=100"));

            void AssertOredResult(List<List<string>> result)
            {
                Assert.That(result, Has.Count.EqualTo(1));
                Assert.That(result.ElementAt(0), Has.Count.EqualTo(2));
                Assert.That(result.ElementAt(0).ElementAt(0), Contains.Substring("color:green"));
                Assert.That(result.ElementAt(0).ElementAt(1), Contains.Substring("color:yellow"));
            }
        }

        [Test]
        [Parallelizable]
        public void TestConsequenceQueryAsString()
        {
            var payload =
                "{\n"
                + "  \"objectID\": \"rule-2\",\n"
                + "  \"condition\": {\n"
                + "    \"pattern\": \"toto\",\n"
                + "    \"anchoring\": \"is\"\n"
                + "  },\n"
                + "  \"consequence\": {\n"
                + "    \"params\": {\n"
                + "        \"query\": \"tata\",\n"
                + "        \"facetFilters\": [[\"facet\"]]\n"
                + "    }\n"
                + "  }\n"
                + "}";

            var rule = JsonConvert.DeserializeObject<Rule>(payload, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.That(rule, Is.Not.Null);
            Assert.That(rule.ObjectID, Is.EqualTo("rule-2"));

            Assert.That(rule.Condition.Pattern, Is.EqualTo("toto"));
            Assert.That(rule.Condition.Anchoring, Is.EqualTo("is"));

            Assert.That(rule.Consequence.Params.SearchQuery, Is.EqualTo("tata"));
            Assert.That(rule.Consequence.Params.FacetFilters, Has.Count.EqualTo(1));
        }

        [Test]
        [Parallelizable]
        public void TestConsequenceQueryAsRemove()
        {
            var payload =
                "{\n"
                + "  \"objectID\": \"rule-2\",\n"
                + "  \"condition\": {\n"
                + "    \"pattern\": \"toto\",\n"
                + "    \"anchoring\": \"is\"\n"
                + "  },\n"
                + "  \"consequence\": {\n"
                + "    \"params\": {\n"
                + "        \"query\": {\"remove\":[\"lastname\",\"firstname\"]},\n"
                + "        \"facetFilters\": [[\"facet\"]]\n"
                + "    }\n"
                + "  }\n"
                + "}";

            var rule = JsonConvert.DeserializeObject<Rule>(payload, JsonConfig.AlgoliaJsonSerializerSettings);

            Assert.That(rule, Is.Not.Null);
            Assert.That(rule.ObjectID, Is.EqualTo("rule-2"));

            Assert.That(rule.Condition.Pattern, Is.EqualTo("toto"));
            Assert.That(rule.Condition.Anchoring, Is.EqualTo("is"));

            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(0).Type, Is.EqualTo(EditType.Remove));
            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(0).Delete, Is.EqualTo("lastname"));
            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(0).Insert, Is.Null);

            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(1).Type, Is.EqualTo(EditType.Remove));
            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(1).Delete, Is.EqualTo("firstname"));
            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(1).Insert, Is.Null);

            Assert.That(rule.Consequence.Params.FacetFilters, Has.Count.EqualTo(1));
        }

        [Test]
        [Parallelizable]
        public void TestConsequenceQueryAsEdits()
        {
            var payload =
                "{\n"
                + "  \"objectID\": \"rule-2\",\n"
                + "  \"condition\": {\n"
                + "    \"pattern\": \"toto\",\n"
                + "    \"anchoring\": \"is\"\n"
                + "  },\n"
                + "  \"consequence\": {\n"
                + "    \"params\": {\n"
                + "        \"query\":{\n"
                + "    \"edits\": [\n"
                + "       { \"type\": \"remove\", \"delete\": \"old\" },\n"
                + "       { \"type\": \"replace\", \"delete\": \"new\", \"insert\": \"newer\" }\n"
                + "    ]\n"
                + "},"
                + "        \"facetFilters\": [[\"facet\"]]\n"
                + "    }\n"
                + "  }\n"
                + "}";


            var rule = JsonConvert.DeserializeObject<Rule>(payload, JsonConfig.AlgoliaJsonSerializerSettings);

            Assert.That(rule, Is.Not.Null);
            Assert.That(rule.ObjectID, Is.EqualTo("rule-2"));

            Assert.That(rule.Condition.Pattern, Is.EqualTo("toto"));
            Assert.That(rule.Condition.Anchoring, Is.EqualTo("is"));

            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(0).Type, Is.EqualTo(EditType.Remove));
            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(0).Delete, Is.EqualTo("old"));
            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(0).Insert, Is.Null);

            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(1).Type, Is.EqualTo(EditType.Replace));
            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(1).Delete, Is.EqualTo("new"));
            Assert.That(rule.Consequence.Params.Query.Edits.ElementAt(1).Insert, Is.EqualTo("newer"));

            Assert.That(rule.Consequence.Params.FacetFilters, Has.Count.EqualTo(1));
        }

        [Test]
        [Parallelizable]
        public void TestConsequenceQueryEditsAndRemove()
        {
            var payload =
                "{\n"
                + "  \"objectID\": \"rule-2\",\n"
                + "  \"condition\": {\n"
                + "    \"pattern\": \"toto\",\n"
                + "    \"anchoring\": \"is\"\n"
                + "  },\n"
                + "  \"consequence\": {\n"
                + "    \"params\": {\n"
                + "        \"query\":{\"remove\": [\"term1\", \"term2\"], \"edits\": [{\"type\": \"remove\", \"delete\": \"term3\"}]},"
                + "        \"facetFilters\": [[\"facet\"]]\n"
                + "    }\n"
                + "  }\n"
                + "}";

            var rule = JsonConvert.DeserializeObject<Rule>(payload, JsonConfig.AlgoliaJsonSerializerSettings);

            Assert.That(rule, Is.Not.Null);
            Assert.That(rule.ObjectID, Is.EqualTo("rule-2"));

            Assert.That(rule.Condition.Pattern, Is.EqualTo("toto"));
            Assert.That(rule.Condition.Anchoring, Is.EqualTo("is"));

            ConsequenceQuery deserialized = rule.Consequence.Params.Query;

            Assert.AreEqual(3, deserialized.Edits.Count());

            Assert.True(deserialized.Edits.ElementAt(0).Type.Equals("remove"));
            Assert.True(deserialized.Edits.ElementAt(0).Delete.Equals("term1"));
            Assert.Null(deserialized.Edits.ElementAt(0).Insert);

            Assert.True(deserialized.Edits.ElementAt(1).Type.Equals("remove"));
            Assert.True(deserialized.Edits.ElementAt(1).Delete.Equals("term2"));
            Assert.Null(deserialized.Edits.ElementAt(1).Insert);

            Assert.True(deserialized.Edits.ElementAt(2).Type.Equals("remove"));
            Assert.True(deserialized.Edits.ElementAt(2).Delete.Equals("term3"));
            Assert.Null(deserialized.Edits.ElementAt(2).Insert);

            Assert.That(rule.Consequence.Params.FacetFilters, Has.Count.EqualTo(1));
        }

        [Test]
        [Parallelizable]
        public void TestConsequenceQueryQueryStringOverride()
        {
            Rule rule = new Rule
            {
                ObjectID = "brand_automatic_faceting",
                Enabled = false,
                Condition = new Condition { Anchoring = "is", Pattern = "{facet:brand}" },
                Consequence =
                    new Consequence
                    {
                        FilterPromotes = false,
                        Params = new ConsequenceParams
                        {
                            SearchQuery = "test",
                            Query = new ConsequenceQuery
                            {
                                Edits = new List<Edit>
                                {
                                    new Edit { Type = EditType.Remove, Delete = "mobile" },
                                    new Edit
                                    {
                                        Type = EditType.Replace, Delete = "phone", Insert = "ihpone"
                                    },
                                }
                            },
                            Facets = new List<string> { "facet" },
                            AutomaticFacetFilters = new List<AutomaticFacetFilter>
                            {
                                new AutomaticFacetFilter { Facet = "brand", Disjunctive = true, Score = 42 }
                            }
                        }
                    },
                Description = "Automatic apply the faceting on `brand` if a brand value is found in the query"
            };

            var payload = JsonConvert.SerializeObject(rule, JsonConfig.AlgoliaJsonSerializerSettings);
            var deserializedRule =
                JsonConvert.DeserializeObject<Rule>(payload, JsonConfig.AlgoliaJsonSerializerSettings);

            Assert.That(deserializedRule.Consequence.Params.SearchQuery, Is.Null);

            Assert.That(deserializedRule.Consequence.Params.Query.Edits.ElementAt(0).Type, Is.EqualTo(EditType.Remove));
            Assert.That(deserializedRule.Consequence.Params.Query.Edits.ElementAt(0).Delete, Is.EqualTo("mobile"));
            Assert.That(deserializedRule.Consequence.Params.Query.Edits.ElementAt(0).Insert, Is.Null);

            Assert.That(deserializedRule.Consequence.Params.Query.Edits.ElementAt(1).Type,
                Is.EqualTo(EditType.Replace));
            Assert.That(deserializedRule.Consequence.Params.Query.Edits.ElementAt(1).Delete, Is.EqualTo("phone"));
            Assert.That(deserializedRule.Consequence.Params.Query.Edits.ElementAt(1).Insert, Is.EqualTo("ihpone"));
        }

        [Test]
        [Parallelizable]
        public void RuleSerializationCycle()
        {
            {
                Rule rule = new Rule
                {
                    ObjectID = "brand_automatic_faceting",
                    Enabled = false,
                    Condition = new Condition { Anchoring = "is", Pattern = "{facet:brand}" },
                    Consequence =
                        new Consequence
                        {
                            FilterPromotes = false,
                            Params = new ConsequenceParams
                            {
                                SearchQuery = "test",
                                Facets = new List<string> { "facet" },
                                AutomaticFacetFilters = new List<AutomaticFacetFilter>
                                {
                                    new AutomaticFacetFilter
                                    {
                                        Facet = "brand", Disjunctive = true, Score = 42
                                    }
                                }
                            }
                        },
                    Description = "Automatic apply the faceting on `brand` if a brand value is found in the query"
                };

                var payload = JsonConvert.SerializeObject(rule, JsonConfig.AlgoliaJsonSerializerSettings);
                var deserializedRule =
                    JsonConvert.DeserializeObject<Rule>(payload, JsonConfig.AlgoliaJsonSerializerSettings);
                Assert.True(TestHelper.AreObjectsEqual(rule, deserializedRule));
            }

            {
                Rule rule = new Rule
                {
                    ObjectID = "brand_automatic_faceting",
                    Enabled = false,
                    Condition = new Condition { Anchoring = "is", Pattern = "{facet:brand}" },
                    Consequence =
                        new Consequence
                        {
                            FilterPromotes = false,
                            Params = new ConsequenceParams
                            {
                                Query = new ConsequenceQuery
                                {
                                    Edits = new List<Edit>
                                    {
                                        new Edit { Type = EditType.Remove, Delete = "mobile" },
                                        new Edit
                                        {
                                            Type = EditType.Replace,
                                            Delete = "phone",
                                            Insert = "ihpone"
                                        },
                                    }
                                },
                                Facets = new List<string> { "facet" },
                                AutomaticFacetFilters = new List<AutomaticFacetFilter>
                                {
                                    new AutomaticFacetFilter
                                    {
                                        Facet = "brand", Disjunctive = true, Score = 42
                                    }
                                }
                            }
                        },
                    Description = "Automatic apply the faceting on `brand` if a brand value is found in the query"
                };

                var payload = JsonConvert.SerializeObject(rule, JsonConfig.AlgoliaJsonSerializerSettings);
                var deserializedRule =
                    JsonConvert.DeserializeObject<Rule>(payload, JsonConfig.AlgoliaJsonSerializerSettings);
                Assert.True(TestHelper.AreObjectsEqual(rule, deserializedRule));
            }
        }

        [Test]
        [Parallelizable]
        public void TestLegacySettings()
        {
            string json =
                "{ \"attributesToIndex\":[\"attr1\", \"attr2\"],\"numericAttributesToIndex\": [\"attr1\", \"attr2\"],\"slaves\":[\"index1\", \"index2\"]}";

            IndexSettings settings = JsonConvert.DeserializeObject<IndexSettings>(json);
            Assert.IsNotNull(settings.Replicas);
            Assert.True(settings.Replicas.Contains("index1"));
            Assert.True(settings.Replicas.Contains("index2"));

            Assert.IsNotNull(settings.SearchableAttributes);
            Assert.True(settings.SearchableAttributes.Contains("attr1"));
            Assert.True(settings.SearchableAttributes.Contains("attr2"));

            Assert.IsNotNull(settings.NumericAttributesForFiltering);
            Assert.True(settings.NumericAttributesForFiltering.Contains("attr1"));
            Assert.True(settings.NumericAttributesForFiltering.Contains("attr2"));
        }

        [Test]
        [Parallelizable]
        public void TestSettingsJObjectMigration()
        {
            var json = JObject.Parse(
                "{\"customRanking\":[\"desc(population)\", \"asc(name)\"], \"attributesToIndex\":[\"attr1\", \"attr2\"],\"numericAttributesToIndex\": [\"attr1\", \"attr2\"],\"slaves\":[\"index1\", \"index2\"]}");
            IndexSettings settings = json.ToObject<IndexSettings>();

            Assert.IsNotNull(settings.CustomRanking);
            Assert.True(settings.CustomRanking.Contains("desc(population)"));
            Assert.True(settings.CustomRanking.Contains("asc(name)"));

            Assert.IsNotNull(settings.Replicas);
            Assert.True(settings.Replicas.Contains("index1"));
            Assert.True(settings.Replicas.Contains("index2"));

            Assert.IsNotNull(settings.SearchableAttributes);
            Assert.True(settings.SearchableAttributes.Contains("attr1"));
            Assert.True(settings.SearchableAttributes.Contains("attr2"));

            Assert.IsNotNull(settings.NumericAttributesForFiltering);
            Assert.True(settings.NumericAttributesForFiltering.Contains("attr1"));
            Assert.True(settings.NumericAttributesForFiltering.Contains("attr2"));
        }

        [Test]
        [Parallelizable]
        public void TestIndexSettingsWithCustomParameters()
        {
            IndexSettings settings = new IndexSettings
            {
                EnableRules = true, CustomSettings = new Dictionary<string, object> { { "newParameter", 10 } }
            };

            string json = JsonConvert.SerializeObject(settings, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"enableRules\":true,\"newParameter\":10}");
        }

        [Test]
        [Parallelizable]
        public void TestPersonalization()
        {
            var strategyToSave = new SetStrategyRequest
            {
                EventsScoring = new Dictionary<string, EventScoring>
                {
                    { "Add to cart", new EventScoring { Score = 50, Type = "conversion" } },
                    { "Purchase", new EventScoring { Score = 100, Type = "conversion" } }
                },
                FacetsScoring = new Dictionary<string, FacetScoring>
                {
                    { "brand", new FacetScoring { Score = 100 } },
                    { "categories", new FacetScoring { Score = 10 } }
                }
            };

            // Here we test the payload, as this settings are at app level all tests could overlap
            string json = JsonConvert.SerializeObject(strategyToSave, JsonConfig.AlgoliaJsonSerializerSettings);
            string expectedJson =
                "{\"eventsScoring\":{\"Add to cart\":{\"type\":\"conversion\",\"score\":50},\"Purchase\":{\"type\":\"conversion\",\"score\":100}},\"facetsScoring\":{\"brand\":{\"score\":100},\"categories\":{\"score\":10}}}";
            Assert.True(json.Equals(expectedJson));
        }

        [Test]
        [Parallelizable]
        public void TestListIndicesResponses64BitsIntegers()
        {
            Assert.DoesNotThrow(() =>
            {
                JsonConvert.DeserializeObject<IndicesResponse>(
                    "{\"entries\": 100000000000, \"dataSize\": 100000000000, \"fileSize\": 100000000000}");
            });
        }
    }
}

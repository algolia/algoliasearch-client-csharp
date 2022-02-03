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
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Recommend;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;
using Algolia.Search.Serializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Algolia.Search.Test.Serializer
{
#pragma warning disable 612, 618

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
            Query query = new Query("") { AroundLatLngViaIP = true, EnableReRanking = false };
            Assert.AreEqual(query.ToQueryString(), "query=&aroundLatLngViaIP=true&enableReRanking=false");
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
            Assert.AreEqual(query.ToQueryString(), "query=&facets=(attribute)");

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
            // Testing "one string" legacy filters => should be converted to "ANDED" nested filters
            // [["color:green"],["color:yellow"]]
            string stringFilters = "\"color:green,color:yellow\"";

            var serializedStringFilters =
                JsonConvert.DeserializeObject<List<List<string>>>(stringFilters, new FiltersConverter());

            AssertAndedResult(serializedStringFilters);

            // Testing "one array" legacy filters => should be converted to "ANDED" nested filters
            // [["color:green"],["color:yellow"]]
            string arrayFilters = "[\"color:green\",\"color:yellow\"]";

            var serializedArrayFilter =
                JsonConvert.DeserializeObject<List<List<string>>>(arrayFilters, new FiltersConverter());

            AssertAndedResult(serializedArrayFilter);

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

            void AssertAndedResult(List<List<string>> result)
            {
                Assert.That(result, Has.Count.EqualTo(2));
                Assert.That(result.ElementAt(0), Has.Count.EqualTo(1));
                Assert.That(result.ElementAt(0).ElementAt(0), Contains.Substring("color:green"));
                Assert.That(result.ElementAt(1), Has.Count.EqualTo(1));
                Assert.That(result.ElementAt(1).ElementAt(0), Contains.Substring("color:yellow"));
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

            string jsonWithNumericAttributeToIndexNull =
                "{ \"numericAttributesToIndex\": null}";
            IndexSettings settingsNumericAttributesisNull = JsonConvert.DeserializeObject<IndexSettings>(jsonWithNumericAttributeToIndexNull);
            Assert.AreEqual(null, settingsNumericAttributesisNull.NumericAttributesForFiltering);
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
                EnableRules = true,
                CustomSettings = new Dictionary<string, object> { { "newParameter", 10 } }
            };

            string json = JsonConvert.SerializeObject(settings, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"enableRules\":true,\"newParameter\":10}");
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

        [Test]
        [Parallelizable]
        public void TestPartialUpdateOperation_Increment()
        {
            RecordWithPartialUpdateOperation<int> record = new RecordWithPartialUpdateOperation<int>
            {
                ObjectID = "myID",
                Update = PartialUpdateOperation<int>.Increment(2),
            };

            string json = JsonConvert.SerializeObject(record, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"objectID\":\"myID\",\"update\":{\"_operation\":\"Increment\",\"value\":2}}");
        }

        [Test]
        [Parallelizable]
        public void TestPartialUpdateOperation_IncrementFrom_int()
        {
            RecordWithPartialUpdateOperation<int> record = new RecordWithPartialUpdateOperation<int>
            {
                ObjectID = "myID",
                Update = PartialUpdateOperation<int>.IncrementFrom(2),
            };

            string json = JsonConvert.SerializeObject(record, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"objectID\":\"myID\",\"update\":{\"_operation\":\"IncrementFrom\",\"value\":2}}");
        }

        [Test]
        [Parallelizable]
        public void TestPartialUpdateOperation_IncrementFrom_long()
        {
            RecordWithPartialUpdateOperation<long> record = new RecordWithPartialUpdateOperation<long>
            {
                ObjectID = "myID",
                Update = PartialUpdateOperation<long>.IncrementFrom((long)2),
            };

            string json = JsonConvert.SerializeObject(record, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"objectID\":\"myID\",\"update\":{\"_operation\":\"IncrementFrom\",\"value\":2}}");
        }

        [Test]
        [Parallelizable]
        public void TestPartialUpdateOperation_IncrementSet_int()
        {
            RecordWithPartialUpdateOperation<int> record = new RecordWithPartialUpdateOperation<int>
            {
                ObjectID = "myID",
                Update = PartialUpdateOperation<int>.IncrementSet(2),
            };

            string json = JsonConvert.SerializeObject(record, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"objectID\":\"myID\",\"update\":{\"_operation\":\"IncrementSet\",\"value\":2}}");
        }

        [Test]
        [Parallelizable]
        public void TestPartialUpdateOperation_IncrementSet_long()
        {
            RecordWithPartialUpdateOperation<long> record = new RecordWithPartialUpdateOperation<long>
            {
                ObjectID = "myID",
                Update = PartialUpdateOperation<long>.IncrementSet((long)2),
            };

            string json = JsonConvert.SerializeObject(record, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"objectID\":\"myID\",\"update\":{\"_operation\":\"IncrementSet\",\"value\":2}}");
        }

        [Test]
        [Parallelizable]
        public void TestPartialUpdateOperation_Decrement()
        {
            RecordWithPartialUpdateOperation<int> record = new RecordWithPartialUpdateOperation<int>
            {
                ObjectID = "myID",
                Update = PartialUpdateOperation<int>.Decrement(2),
            };

            string json = JsonConvert.SerializeObject(record, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"objectID\":\"myID\",\"update\":{\"_operation\":\"Decrement\",\"value\":2}}");
        }

        [Test]
        [Parallelizable]
        public void TestPartialUpdateOperation_Add()
        {
            RecordWithPartialUpdateOperation<string> record = new RecordWithPartialUpdateOperation<string>
            {
                ObjectID = "myID",
                Update = PartialUpdateOperation<string>.Add("something"),
            };

            string json = JsonConvert.SerializeObject(record, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"objectID\":\"myID\",\"update\":{\"_operation\":\"Add\",\"value\":\"something\"}}");
        }

        [Test]
        [Parallelizable]
        public void TestPartialUpdateOperation_AddUnique()
        {
            RecordWithPartialUpdateOperation<string> record = new RecordWithPartialUpdateOperation<string>
            {
                ObjectID = "myID",
                Update = PartialUpdateOperation<string>.AddUnique("something"),
            };

            string json = JsonConvert.SerializeObject(record, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"objectID\":\"myID\",\"update\":{\"_operation\":\"AddUnique\",\"value\":\"something\"}}");
        }

        [Test]
        [Parallelizable]
        public void TestPartialUpdateOperation_Remove()
        {
            RecordWithPartialUpdateOperation<string> record = new RecordWithPartialUpdateOperation<string>
            {
                ObjectID = "myID",
                Update = PartialUpdateOperation<string>.Remove("something"),
            };

            string json = JsonConvert.SerializeObject(record, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"objectID\":\"myID\",\"update\":{\"_operation\":\"Remove\",\"value\":\"something\"}}");
        }

        [Test]
        [Parallelizable]
        public void TestRecommendRequest()
        {
            var request = new RecommendRequest
            {
                IndexName = "products",
                ObjectID = "B018APC4LE",
                Model = "bought-together",
                Threshold = 10,
                MaxRecommendations = 10,
                QueryParameters = new Query
                {
                    AttributesToRetrieve = new List<string> { "*" }
                }
            };

            string json = JsonConvert.SerializeObject(request, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"indexName\":\"products\",\"model\":\"bought-together\",\"objectID\":\"B018APC4LE\",\"threshold\":10,\"maxRecommendations\":10,\"queryParameters\":{\"attributesToRetrieve\":[\"*\"]}}");
        }

        [Test]
        [Parallelizable]
        public void TestRecommendRequestDefaultThreshold()
        {
            var request = new RecommendRequest
            {
                IndexName = "products",
                ObjectID = "B018APC4LE",
                Model = "bought-together",
                MaxRecommendations = 10,
                QueryParameters = new Query
                {
                    AttributesToRetrieve = new List<string> { "*" }
                }
            };

            string json = JsonConvert.SerializeObject(request, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"indexName\":\"products\",\"model\":\"bought-together\",\"objectID\":\"B018APC4LE\",\"threshold\":0,\"maxRecommendations\":10,\"queryParameters\":{\"attributesToRetrieve\":[\"*\"]}}");
        }

        [Test]
        [Parallelizable]
        public void TestRelatedProductsRequest()
        {
            var request = new RelatedProductsRequest
            {
                IndexName = "products",
                ObjectID = "B018APC4LE",
                Threshold = 10,
                MaxRecommendations = 10,
                QueryParameters = new Query
                {
                    AttributesToRetrieve = new List<string> { "*" }
                }
            };

            string json = JsonConvert.SerializeObject(request, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"model\":\"related-products\",\"indexName\":\"products\",\"objectID\":\"B018APC4LE\",\"threshold\":10,\"maxRecommendations\":10,\"queryParameters\":{\"attributesToRetrieve\":[\"*\"]}}");
        }

        [Test]
        [Parallelizable]
        public void TestBoughtTogetherRequest()
        {
            var request = new BoughtTogetherRequest
            {
                IndexName = "products",
                ObjectID = "B018APC4LE",
                Threshold = 10,
                MaxRecommendations = 10,
                QueryParameters = new Query
                {
                    AttributesToRetrieve = new List<string> { "*" }
                }
            };

            string json = JsonConvert.SerializeObject(request, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual(json, "{\"model\":\"bought-together\",\"indexName\":\"products\",\"objectID\":\"B018APC4LE\",\"threshold\":10,\"maxRecommendations\":10,\"queryParameters\":{\"attributesToRetrieve\":[\"*\"]}}");
        }

        [Test]
        [Parallelizable]
        public void TestRecommendResponse()
        {
            var payload = @"{
  ""results"": [
    {
      ""hits"": [
        {
          ""_highlightResult"": {
            ""category"": {
              ""matchLevel"": ""none"",
              ""matchedWords"": [],
              ""value"": ""Men - T-Shirts""
            },
            ""image_link"": {
              ""matchLevel"": ""none"",
              ""matchedWords"": [],
              ""value"": ""https://example.org/image/D05927-8161-111-F01.jpg""
            },
            ""name"": {
              ""matchLevel"": ""none"",
              ""matchedWords"": [],
              ""value"": ""Jirgi Half-Zip T-Shirt""
            }
          },
          ""_score"": 32.72,
          ""category"": ""Men - T-Shirts"",
          ""image_link"": ""https://example.org/image/D05927-8161-111-F01.jpg"",
          ""name"": ""Jirgi Half-Zip T-Shirt"",
          ""objectID"": ""D05927-8161-111"",
          ""position"": 105,
          ""url"": ""men/t-shirts/d05927-8161-111""
        }
      ],
      ""hitsPerPage"": 1,
      ""nbHits"": 1,
      ""nbPages"": 1,
      ""page"": 0,
      ""processingTimeMS"": 6,
      ""renderingContent"": {}
    }
  ]
}";

            var response = JsonConvert.DeserializeObject<RecommendResponse<RecommendedProduct>>(payload, JsonConfig.AlgoliaJsonSerializerSettings);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Results.Count, Is.EqualTo(1));
            Assert.That(response.Results.ElementAt(0).HitsPerPage, Is.EqualTo(1));
            Assert.That(response.Results.ElementAt(0).NbHits, Is.EqualTo(1));
            Assert.That(response.Results.ElementAt(0).NbPages, Is.EqualTo(1));
            Assert.That(response.Results.ElementAt(0).Page, Is.EqualTo(0));
            Assert.That(response.Results.ElementAt(0).ProcessingTimeMs, Is.EqualTo(6));

            List<RecommendedProduct> recommendedProducts = response.Results.ElementAt(0).Hits;
            Assert.That(recommendedProducts.Count, Is.EqualTo(1));
            Assert.That(recommendedProducts.ElementAt(0).Score, Is.EqualTo(32.72f));
            Assert.That(recommendedProducts.ElementAt(0).ObjectID, Is.EqualTo("D05927-8161-111"));
            Assert.That(recommendedProducts.ElementAt(0).Name, Is.EqualTo("Jirgi Half-Zip T-Shirt"));
            Assert.That(recommendedProducts.ElementAt(0).Category, Is.EqualTo("Men - T-Shirts"));
            Assert.That(recommendedProducts.ElementAt(0).Position, Is.EqualTo(105));
            Assert.That(recommendedProducts.ElementAt(0).Url, Is.EqualTo("men/t-shirts/d05927-8161-111"));
            Assert.That(recommendedProducts.ElementAt(0).ImageLink, Is.EqualTo("https://example.org/image/D05927-8161-111-F01.jpg"));
        }

        [Test]
        [Parallelizable]
        public void TestFacetOrdering()
        {
            // Test serialization
            var settings = new IndexSettings
            {
                RenderingContent = new RenderingContent
                {
                    FacetOrdering = new FacetOrdering
                    {
                        Facets = new FacetsOrder
                        {
                            Order = new List<string> { "size", "brand" }
                        },
                        Values = new Dictionary<string, FacetValuesOrder>
                        {
                            { "brand", new FacetValuesOrder { Order = new List<string> { "uniqlo" } } },
                            { "size", new FacetValuesOrder
                                {
                                    Order = new List<string> { "S", "M", "L" },
                                    SortRemainingBy = "hidden"
                                }
                            }
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(settings, JsonConfig.AlgoliaJsonSerializerSettings);
            Assert.AreEqual("{\"renderingContent\":{\"facetOrdering\":{\"facets\":{\"order\":[\"size\",\"brand\"]},\"values\":{\"brand\":{\"order\":[\"uniqlo\"]},\"size\":{\"order\":[\"S\",\"M\",\"L\"],\"sortRemainingBy\":\"hidden\"}}}}}", json);

            // Test deserialization
            var payload = @"{
  ""renderingContent"": {
    ""facetOrdering"": {
      ""facets"": {
        ""order"": [""brand"", ""color""]
      },
      ""values"": {
        ""brand"": {
          ""order"": [""uniqlo"", ""sony""],
          ""sortRemainingBy"": ""alpha""
        }
      }
    }
  }
}";
            var response = JsonConvert.DeserializeObject<SearchResponse<object>>(payload, JsonConfig.AlgoliaJsonSerializerSettings);

            Assert.IsNotNull(response.RenderingContent);
            Assert.IsNotNull(response.RenderingContent.FacetOrdering);
            Assert.IsNotNull(response.RenderingContent.FacetOrdering.Facets);
            Assert.IsNotNull(response.RenderingContent.FacetOrdering.Facets.Order);
            Assert.True(response.RenderingContent.FacetOrdering.Facets.Order.Contains("brand"));
            Assert.True(response.RenderingContent.FacetOrdering.Facets.Order.Contains("color"));

            Assert.IsNotNull(response.RenderingContent.FacetOrdering.Values);
            Assert.True(response.RenderingContent.FacetOrdering.Values.ContainsKey("brand"));
            FacetValuesOrder brandValues = response.RenderingContent.FacetOrdering.Values["brand"];
            Assert.True(brandValues.Order.Contains("uniqlo"));
            Assert.True(brandValues.Order.Contains("sony"));
            Assert.AreEqual("alpha", brandValues.SortRemainingBy);
        }
    }

    public class Product
    {
        public string ObjectID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Position { get; set; }
        public string Url { get; set; }
        [JsonProperty(PropertyName = "image_link")]
        public string ImageLink { get; set; }
    }

    public class RecommendedProduct : Product, IRecommendHit
    {
        public float Score { get; set; }
    }

#pragma warning restore 612, 618
}

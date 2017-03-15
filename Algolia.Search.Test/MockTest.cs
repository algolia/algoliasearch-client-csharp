using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http;

namespace Algolia.Search.Test
{
    [TestFixture]
    public class MockTest
    {

        private MockHttpMessageHandler getEmptyHandler()
        {
            var mockHttp = new MockHttpMessageHandler();
            // List indexes
            mockHttp.When(HttpMethod.Get, "*/1/indexes/").Respond("application/json", "{\"items\":[]}");

            //Query an index
            mockHttp.When(HttpMethod.Get, "*/1/indexes/{indexName}").Respond(HttpStatusCode.OK, "application/json", "{\"hits\":[],\"page\":0,\"nbHits\":0,\"nbPages\":0,\"ProcessingTimeMS\":1,\"query\":\"\",\"params\":\"\"}");
            mockHttp.When(HttpMethod.Post, "*/1/indexes/{indexName}/query").Respond(HttpStatusCode.OK, "application/json", "{\"hits\":[],\"page\":0,\"nbHits\":0,\"nbPages\":0,\"ProcessingTimeMS\":1,\"query\":\"\",\"params\":\"\"}");

            // Multiple queries
            mockHttp.When(HttpMethod.Get, "*/1/indexes/*/query").Respond(HttpStatusCode.OK, "application/json", "{\"results\":[]}");

            // Delete index
            mockHttp.When(HttpMethod.Delete, "*/1/indexes/{indexName}").Respond(HttpStatusCode.OK, "application/json", "{\"deletedAt\":\"2013-01-18T15:33:13.556Z\",\"taskID\":1}");

            // Clear index
            mockHttp.When(HttpMethod.Post, "*/1/indexes/{indexName}/clear").Respond(HttpStatusCode.OK, "application/json", "{\"updatedAt\":\"2013-01-18T15:33:13.556Z\",\"taskID\":1}");

            //Add object in an index without objectID
            mockHttp.When(HttpMethod.Post, "*/1/indexes/{indexName}").Respond(HttpStatusCode.Created, "application/json", "{\"createdAt\":\"2013-01-18T15:33:13.556Z\",\"taskID\":1,\"objectID\":\"1\"}");

            //Add object in an index with objectID
            mockHttp.When(HttpMethod.Put, "*/1/indexes/{indexName}/{objectID}").Respond(HttpStatusCode.Created, "application/json", "{\"createdAt\":\"2013-01-18T15:33:13.556Z\",\"taskID\":1,\"objectID\":\"1\"}");

            //Partially update an object in an index
            mockHttp.When(HttpMethod.Post, "*/1/indexes/{indexName}/{objectID}/partial").Respond(HttpStatusCode.OK, "application/json", "{\"updatedAt\":\"2013-01-18T15:33:13.556Z\",\"taskID\":1,\"objectID\":\"1\"}");

            //get an object in an index by objectID
            mockHttp.When(HttpMethod.Get, "*/1/indexes/{indexName}/{objectID}").Respond(HttpStatusCode.OK, "application/json", "{}");

            //get several objects in an index by objectID
            mockHttp.When(HttpMethod.Post, "*/1/indexes/*/objects").Respond(HttpStatusCode.OK, "application/json", "{\"results\":[]}");

            //delete an object in an index by objectID
            mockHttp.When(HttpMethod.Delete, "*/1/indexes/{indexName}/{objectID}").Respond(HttpStatusCode.OK, "application/json", "{\"deletedAt\":\"2013-01-18T15:33:13.556Z\",\"taskID\":1}");

            //batch write operations
            mockHttp.When(HttpMethod.Post, "*/1/indexes/{indexName}/batch").Respond(HttpStatusCode.OK, "application/json", "{\"taskID\":1,\"objectIDs\":[]}");

            //Get index settings
            mockHttp.When(HttpMethod.Get, "*/1/indexes/{indexName}/settings").Respond(HttpStatusCode.OK, "application/json", "{\"minWordSizefor1Typo\": 3,\"minWordSizefor2Typos\": 7,\"hitsPerPage\": 20,\"searchableAttributes\": null,\"attributesToRetrieve\": null,\"attributesToSnippet\": null,\"attributesToHighlight\": null,\"ranking\": [\"typo\",\"geo\",\"proximity\",\"attribute\",\"custom\"],\"customRanking\": null,\"separatorsToIndex\": \"\",\"queryType\": \"prefixAll\"}");

            //Browse index content
            mockHttp.When(HttpMethod.Get, "*/1/indexes/{indexName}/browse").Respond(HttpStatusCode.OK, "application/json", "{\"hits\":[],\"page\":0,\"nbHits\":0,\"nbPages\":0,\"ProcessingTimeMS\":1,\"query\":\"\",\"params\":\"\"}");

            //Change index settings
            mockHttp.When(HttpMethod.Put, "*/1/indexes/{indexName}/settings").Respond(HttpStatusCode.OK, "application/json", "{\"updatedAt\":\"2013-01-18T15:33:13.556Z\",\"taskID\":1}");

            //Copy/Move an Index
            mockHttp.When(HttpMethod.Post, "*/1/indexes/{indexName}/operation").Respond(HttpStatusCode.OK, "application/json", "{\"updatedAt\":\"2013-01-18T15:33:13.556Z\",\"taskID\":1}");

            //Get task status
            mockHttp.When(HttpMethod.Get, "*/1/indexes/{indexName}/task/{taskID}").Respond(HttpStatusCode.OK, "application/json", "{\"status\":\"published\",\"pendingTask\":false}");

            //Add index specific API key
            mockHttp.When(HttpMethod.Post, "*/1/indexes/{indexName}/keys").Respond(HttpStatusCode.Created, "application/json", "{\"key\": \"107da8d0afc2d225ff9a7548caaf599f\",\"createdAt\":\"2013-01-18T15:33:13.556Z\"}");

            //List index specific API Keys
            mockHttp.When(HttpMethod.Get, "*/1/indexes/{indexName}/keys").Respond(HttpStatusCode.OK, "application/json", "{\"keys\":[]}");

            //List index specific API Keys for all indexes
            mockHttp.When(HttpMethod.Get, "*/1/indexes/*/keys").Respond(HttpStatusCode.OK, "application/json", "{\"keys\":[]}");

            //Get the rights of an index specific API key
            mockHttp.When(HttpMethod.Get, "*/1/indexes/{indexName}/keys/{key}").Respond(HttpStatusCode.OK, "application/json", "{\"value\": \"107da8d0afc2d225ff9a7548caaf599f\",\"acl\": [\"search\"],\"validity\": 0}");

            //Delete index specific API key
            mockHttp.When(HttpMethod.Delete, "*/1/indexes/{indexName}/keys/{key}").Respond(HttpStatusCode.OK, "application/json", "{\"deletedAt\":\"2013-01-18T15:33:13.556Z\"}");

            //Add a global API key
            mockHttp.When(HttpMethod.Post, "*/1/keys").Respond(HttpStatusCode.Created, "application/json", "{\"key\": \"107da8d0afc2d225ff9a7548caaf599f\",\"createdAt\":\"2013-01-18T15:33:13.556Z\"}");

            //List global API keys
            mockHttp.When(HttpMethod.Get, "*/1/keys").Respond(HttpStatusCode.OK, "application/json", "{\"keys\":[]}");

            //Get the rights of a global API key
            mockHttp.When(HttpMethod.Get, "*/1/keys/{key}").Respond(HttpStatusCode.OK, "application/json", "{\"value\": \"107da8d0afc2d225ff9a7548caaf599f\",\"acl\": [\"search\"],\"validity\": 0}");

            //Delete a global API key
            mockHttp.When(HttpMethod.Delete, "*/1/keys/{key}").Respond(HttpStatusCode.OK, "application/json", "{\"deletedAt\":\"2013-01-18T15:33:13.556Z\"}");

            //Get last logs
            mockHttp.When(HttpMethod.Get, "*/1/logs").Respond(HttpStatusCode.OK, "application/json", "{\"logs\":[]}");

            mockHttp.Fallback.WithAny().Respond(HttpStatusCode.BadRequest);

            return mockHttp;
        }

        [Test]
        public void TestClientWithMock()
        {
            var client = new AlgoliaClient("test", "test", null, getEmptyHandler());
            Assert.AreEqual(JObject.Parse("{\"items\":[]}").ToString(), client.ListIndexes().ToString());
            Assert.AreEqual(JObject.Parse("{\"results\":[]}").ToString(), client.InitIndex("{indexName}").GetObjects(new string[] { "myID" }).ToString());
        }
    }
}

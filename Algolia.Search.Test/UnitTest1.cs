using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Algolia.Search.Test
{

    [TestClass]
    public class AlgoliaClientTest
    {
        private static string _testApplicationKey = "ut";
        private static string _testApiKey = "82f69c6dd1d27787e33de16f63130d9c";
        private static string[] _hosts = new string[] { "localhost.algolia.com:8080", "" };

        [TestMethod]
        public async Task TestListIndexes()
        {
            try
            {
                AlgoliaClient client = new AlgoliaClient(_testApplicationKey, _testApiKey, _hosts);
                var result = await client.ListIndexes();
                Assert.IsFalse(string.IsNullOrWhiteSpace(result["items"].ToString()));
            }
            catch (ArgumentOutOfRangeException e)
            {
            }
        }

        [TestMethod]
        public async Task TestAddObjects()
        {
            StreamReader re = File.OpenText("1000-cities.json");
            JsonTextReader reader = new JsonTextReader(re);
            JObject jsonObject = JObject.Load(reader);

            AlgoliaClient client = new AlgoliaClient(_testApplicationKey, _testApiKey, _hosts);
            Index index = client.InitIndex("cities");
            JObject res = await index.AddObjects((JArray)jsonObject["objects"]);
            JArray objectIDs = (JArray)res["objectIDs"];
            Assert.AreEqual(objectIDs.Count, 1000);
        }

        [TestMethod]
        public async Task TestSearch()
        {
            AlgoliaClient client = new AlgoliaClient(_testApplicationKey, _testApiKey, _hosts);
            Index index = client.InitIndex("cities");
            // Add one object to be sure the test will not fail because index is empty
            var res = await index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["objectID"].ToString()));
            res = await index.Search(new Query("san fran"));
            Assert.AreEqual("San Francisco", res["hits"][0]["name"]);
        }

        [TestMethod]
        public async Task TestSettings()
        {
            AlgoliaClient client = new AlgoliaClient(_testApplicationKey, _testApiKey, _hosts);
            Index index = client.InitIndex("cities");
            var res = await index.SetSettings(JObject.Parse(@"{""customRanking"":[""desc(population)"", ""asc(name)""]}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
            res = await index.GetSettings();
            System.Diagnostics.Debug.WriteLine(res);
        }

        [TestMethod]
        public async Task TestAddObject()
        {
            AlgoliaClient client = new AlgoliaClient(_testApplicationKey, _testApiKey, _hosts);
            Index index = client.InitIndex("cities");
            var res = await index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["objectID"].ToString()));
            res = await index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
            Assert.AreEqual("myID", res["objectID"]);
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            AlgoliaClient client = new AlgoliaClient(_testApplicationKey, _testApiKey, _hosts);
            Index index = client.InitIndex("cities");
            var res = await index.SaveObject(JObject.Parse(@"{""name"":""Los Angeles"", 
                                                              ""population"":3792621, 
                                                              ""objectID"":""myID""}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
            res = await index.PartialUpdateObject(JObject.Parse(@"{""population"":3792621, 
                                                                   ""objectID"":""myID""}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
        }

        [TestMethod]
        public async Task TaskGetObject()
        {
            AlgoliaClient client = new AlgoliaClient(_testApplicationKey, _testApiKey, _hosts);
            Index index = client.InitIndex("cities");
            // Add one object to be sure the test will not fail because index is empty
            var res = await index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
            await index.WaitTask(res["taskID"].ToString());
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["objectID"].ToString()));
            res = await index.GetObject("myID");
            Assert.AreEqual("San Francisco", res["name"]);
            Assert.AreEqual(805235, res["population"]);
            Assert.AreEqual("myID", res["objectID"]);
            res = await index.GetObject("myID", new String[] {"name", "population"});
            Assert.AreEqual("San Francisco", res["name"]);
            Assert.AreEqual(805235, res["population"]);
            Assert.AreEqual("myID", res["objectID"]);
            res = await index.GetObject("myID", new String[] { "name" });
            Assert.AreEqual(null, res["population"]);
            Assert.AreEqual("San Francisco", res["name"]);
            Assert.AreEqual("myID", res["objectID"]);
        }

        [TestMethod]
        public async Task TaskDeleteObject()
        {
            AlgoliaClient client = new AlgoliaClient(_testApplicationKey, _testApiKey, _hosts);
            Index index = client.InitIndex("cities");
            // Add one object to be sure the test will not fail because index is empty
            var res = await index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
            await index.WaitTask(res["taskID"].ToString());
            res = await index.DeleteObject("myID");
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["deletedAt"].ToString()));
        }

        [TestMethod]
        public async Task TaskBatch()
        {
            AlgoliaClient client = new AlgoliaClient(_testApplicationKey, _testApiKey, _hosts);
            Index index = client.InitIndex("cities");
            
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
            objs.Add(JObject.Parse(@"{""name"":""Los Angeles"", ""population"":3792621}"));
            var res = await index.AddObjects(objs);
            JArray objectIDs = (JArray)res["objectIDs"];
            Assert.AreEqual(objectIDs.Count, 2);
            List<JObject> objs2 = new List<JObject>();
            objs2.Add(JObject.Parse(@"{""name"":""San Francisco"", 
                          ""population"": 805235,
                          ""objectID"":""SFO""}"));
            objs2.Add(JObject.Parse(@"{""name"":""Los Angeles"", 
                          ""population"": 3792621,
                          ""objectID"": ""LA""}"));
            res = await index.SaveObjects(objs2);
            objectIDs = (JArray)res["objectIDs"];
            Assert.AreEqual(objectIDs.Count, 2);
        }

        [TestMethod]
        public async Task TaskACL()
        {
            AlgoliaClient client = new AlgoliaClient(_testApplicationKey, _testApiKey, _hosts);
            Index index = client.InitIndex("cities");
            var res = await client.AddUserKey(new String[] { "search" });
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["key"].ToString()));

            var keys = await index.AddUserKey(new String[] { "search" });
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["key"].ToString()));
        }
    }
}

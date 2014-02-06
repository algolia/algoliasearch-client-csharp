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
        private static string _testApplicationID = "";
        private static string _testApiKey = "";
        private static string[] _hosts = new string[] { "localhost.algolia.com:8080", "" };
        private AlgoliaClient _client;
        private Algolia.Search.Index _index;

        public static string safe_name(string name)
        {
            if (Environment.GetEnvironmentVariable("TRAVIS") == null)
            {
                return name;
            }
            String[] id = Environment.GetEnvironmentVariable("TRAVIS_JOB_NUMBER").Split('.');
            return name + "_travis-" + id[id.Length - 1];
        }

        public async Task clearTest()
        {
            try
            {
                await _index.ClearIndex();
            }
            catch (Exception)
            {
                // Index not found
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _testApiKey = Environment.GetEnvironmentVariable("ALGOLIA_API_KEY");
            _testApplicationID = Environment.GetEnvironmentVariable("ALGOLIA_APPLICATION_ID");
            _client = new AlgoliaClient(_testApplicationID, _testApiKey);
            _index = _client.InitIndex(safe_name("àlgol?à-csharp"));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _client = null;
        }

        [TestMethod]
        public async Task TestAddObject()
        {
            await clearTest();
            var task = await _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger""}"));
            await _index.WaitTask(task["taskID"].ToString());
            var res = await _index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"]);
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"]);
        }

        [TestMethod]
        public async Task TestSaveObject()
        {
            await clearTest();
            var task = await _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger""}"), "à/go/?à");
            await _index.WaitTask(task["taskID"].ToString());
            task = await _index.SaveObject(JObject.Parse(@"{""firstname"":""Robert"", ""lastname"":""Barninger"", ""objectID"":""à/go/?à""}"));
            await _index.WaitTask(task["taskID"].ToString());
            var res = await _index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"]);
            Assert.AreEqual("Robert", res["hits"][0]["firstname"]);
        }

        [TestMethod]
        public async Task TestPartialUpdateObject()
        {
            await clearTest();
            var task = await _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger""}"), "à/go/?à");
            await _index.WaitTask(task["taskID"].ToString());
            task = await _index.SaveObject(JObject.Parse(@"{""firstname"":""Robert"", ""objectID"":""à/go/?à""}"));
            await _index.WaitTask(task["taskID"].ToString());
            var res = await _index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"]);
            Assert.AreEqual("Robert", res["hits"][0]["firstname"]);
        }

        [TestMethod]
        public async Task TaskAddObjects()
        {
            await clearTest();
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Barninger""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Speach""}"));
            var task = await _index.AddObjects(objs);
            await _index.WaitTask(task["taskID"].ToString());
            var res = await _index.Search(new Query(""));
            Assert.AreEqual(2, res["nbHits"]);
            Assert.AreEqual("Roger", res["hits"][0]["firstname"]);
        }

        [TestMethod]
        public async Task TaskSaveObjects()
        {
            await clearTest();
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Barninger"", ""objectID"":""à/go/?à1""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Speach"", ""objectID"":""à/go/?à2""}"));
            await _index.AddObjects(objs);
            objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", 
                          ""lastname"":""Barninger"", ""objectID"":""à/go/?à1""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", 
                          ""lastname"":""Speach"", ""objectID"":""à/go/?à2""}"));
            var task = await _index.SaveObjects(objs);
            await _index.WaitTask(task["taskID"].ToString());
            var res = await _index.Search(new Query(""));
            Assert.AreEqual(2, res["nbHits"]);
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"]);
        }

        [TestMethod]
        public async Task TaskPartialUpdateObjects()
        {
            await clearTest();
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Barninger"", ""objectID"":""à/go/?à1""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Speach"", ""objectID"":""à/go/?à2""}"));
            await _index.AddObjects(objs);
            objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", ""objectID"":""à/go/?à1""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", ""objectID"":""à/go/?à2""}"));
            var task = await _index.PartialUpdateObjects(objs);
            await _index.WaitTask(task["taskID"].ToString());
            var res = await _index.Search(new Query(""));
            Assert.AreEqual(2, res["nbHits"]);
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"]);
        }

        public Boolean IsPresent(JArray array, string attribute, string value)
        {
            for (int i = 0; i < array.Count; ++i)
            {
                if(array[i][attribute].ToString().Equals(value))
                    return true;
            }
            return false;
        }

        [TestMethod]
        public async Task TestDeleteIndex()
        {
            await clearTest();
            var task = await _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger""}"));
            await _index.WaitTask(task["taskID"].ToString());
            var res = await _index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"]);
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"]);
            res = await _client.ListIndexes();
            Assert.IsTrue(IsPresent((JArray)res["items"], "name", safe_name("àlgol?à-csharp")));
            await _client.DeleteIndex(safe_name("àlgol?à-csharp"));
            res = await _client.ListIndexes();
            Assert.IsFalse(IsPresent((JArray)res["items"], "name", safe_name("àlgol?à-csharp")));
        }

        [TestMethod]
        public async Task TestGetObject()
        {
            await clearTest();
            var task = await _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""à/go/?à""}"));
            await _index.WaitTask(task["taskID"].ToString());
            var res = await _index.GetObject("à/go/?à");
            Assert.AreEqual("1", res["objectID"]);
            Assert.AreEqual("Jimmie", res["firstname"]);
        }

        [TestMethod]
        public async Task TestDeleteObject()
        {
            await clearTest();
            await _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""à/go/?à""}"));
            var task = await _index.DeleteObject("à/go/?à");
            await _index.WaitTask(task["taskID"].ToString());
            var res = await _index.Search(new Query(""));
            Assert.AreEqual(0, res["nbHits"]);
        }

        [TestMethod]
        public async Task TestCopyIndex()
        {
            var index = _client.InitIndex(safe_name("àlgol?à-csharp2"));
            await clearTest();
            var task = await _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
            await _index.WaitTask(task["taskID"].ToString());
            task = await _client.CopyIndex(safe_name("àlgol?à-csharp"), safe_name("àlgol?à-csharp2"));
            var res = await index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"]);
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"]);
            await _client.DeleteIndex(safe_name("àlgol?à-csharp2"));
        }

        [TestMethod]
        public async Task TestMoveIndex()
        {
            var index = _client.InitIndex(safe_name("àlgol?à-csharp2"));
            await clearTest();
            var task = await _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
            await _index.WaitTask(task["taskID"].ToString());
            task = await _client.MoveIndex(safe_name("àlgol?à-csharp"), safe_name("àlgol?à-csharp2"));
            
            var res = await index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"]);
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"]);
            res = await _client.ListIndexes();
            Assert.IsFalse(IsPresent((JArray)res["items"], "name", safe_name("àlgol?à-csharp")));
            await _client.DeleteIndex(safe_name("àlgol?à-csharp2"));
        }

        [TestMethod]
        public async Task TestBrowse()
        {
            await clearTest();
            var task = await _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
            await _index.WaitTask(task["taskID"].ToString());
            var res = await _index.Browse(0);
            Assert.AreEqual(1, res["nbHits"]);
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"]);
        }

        [TestMethod]
        public async Task TestLogs()
        {
            var res = await _client.GetLogs();
            Assert.IsTrue(((JArray)res["logs"]).Count > 0);
        }

        [TestMethod]
        public async Task TestSearch()
        {
            await clearTest();
            // Add one object to be sure the test will not fail because index is empty
            var res = await _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["objectID"].ToString()));
            res = await _index.Search(new Query("san fran"));
            Assert.AreEqual("San Francisco", res["hits"][0]["name"]);
        }

        [TestMethod]
        public async Task TestSettings()
        {
            await clearTest();
            var res = await _index.SetSettings(JObject.Parse(@"{""customRanking"":[""desc(population)"", ""asc(name)""]}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
            res = await _index.GetSettings();
            System.Diagnostics.Debug.WriteLine(res);
        }

        [TestMethod]
        public async Task TestAddObject2()
        {
            await clearTest();
            var res = await _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["objectID"].ToString()));
            res = await _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
            Assert.AreEqual("myID", res["objectID"]);
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            await clearTest();
            var res = await _index.SaveObject(JObject.Parse(@"{""name"":""Los Angeles"", 
                                                              ""population"":3792621, 
                                                              ""objectID"":""myID""}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
            res = await _index.PartialUpdateObject(JObject.Parse(@"{""population"":3792621, 
                                                                   ""objectID"":""myID""}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
        }

        [TestMethod]
        public async Task TaskGetObject()
        {
            await clearTest();
            // Add one object to be sure the test will not fail because index is empty
            var res = await _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
            await _index.WaitTask(res["taskID"].ToString());
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["objectID"].ToString()));
            res = await _index.GetObject("myID");
            Assert.AreEqual("San Francisco", res["name"]);
            Assert.AreEqual(805235, res["population"]);
            Assert.AreEqual("myID", res["objectID"]);
            res = await _index.GetObject("myID", new String[] {"name", "population"});
            Assert.AreEqual("San Francisco", res["name"]);
            Assert.AreEqual(805235, res["population"]);
            Assert.AreEqual("myID", res["objectID"]);
            res = await _index.GetObject("myID", new String[] { "name" });
            Assert.AreEqual(null, res["population"]);
            Assert.AreEqual("San Francisco", res["name"]);
            Assert.AreEqual("myID", res["objectID"]);
        }

        [TestMethod]
        public async Task TaskDeleteObject()
        {
            await clearTest();
            // Add one object to be sure the test will not fail because index is empty
            var res = await _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
            await _index.WaitTask(res["taskID"].ToString());
            res = await _index.DeleteObject("myID");
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["deletedAt"].ToString()));
        }

        [TestMethod]
        public async Task TaskBatch()
        {
            await clearTest();
            
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
            objs.Add(JObject.Parse(@"{""name"":""Los Angeles"", ""population"":3792621}"));
            var res = await _index.AddObjects(objs);
            JArray objectIDs = (JArray)res["objectIDs"];
            Assert.AreEqual(objectIDs.Count, 2);
            List<JObject> objs2 = new List<JObject>();
            objs2.Add(JObject.Parse(@"{""name"":""San Francisco"", 
                          ""population"": 805235,
                          ""objectID"":""SFO""}"));
            objs2.Add(JObject.Parse(@"{""name"":""Los Angeles"", 
                          ""population"": 3792621,
                          ""objectID"": ""LA""}"));
            res = await _index.SaveObjects(objs2);
            objectIDs = (JArray)res["objectIDs"];
            Assert.AreEqual(objectIDs.Count, 2);
        }

        [TestMethod]
        public async Task TestListIndexes()
        {
            await clearTest();
            try
            {
                var result = await _client.ListIndexes();
                Assert.IsFalse(string.IsNullOrWhiteSpace(result["items"].ToString()));
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public async Task TaskACL()
        {
            await clearTest();
            var res = await _client.AddUserKey(new String[] { "search" });
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["key"].ToString()));
            var task = await _client.DeleteUserKey(res["key"].ToString());

            res = await _index.AddUserKey(new String[] { "search" });
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["key"].ToString()));
            task = await _index.DeleteUserKey(res["key"].ToString());
        }
    }
}

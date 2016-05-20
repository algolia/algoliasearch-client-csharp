using System.Threading.Tasks;
using NUnit.Framework;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using Algolia.Search;
using System.Threading;

namespace NUnit.Framework.Test
{

    [TestFixture]
    public class AlgoliaClientTest
    {
        private static string _testApplicationID = "";
        private static string _testApiKey = "";
        
        private AlgoliaClient _client;
        private Index _index;

        public static string safe_name(string name)
        {
            if (Environment.GetEnvironmentVariable("APPVEYOR") == null)
            {
                return name;
            }
            //String[] id = Environment.GetEnvironmentVariable("TRAVIS_JOB_NUMBER").Split('.');
            return name + "appveyor-" + Environment.GetEnvironmentVariable("APPVEYOR_BUILD_NUMBER");
        }

        public void clearTest()
        {
            try
            {
                _index.ClearIndex();
            }
            catch (Exception)
            {
                // Index not found
            }
        }

        [SetUp]
        public void TestInitialize()
        {
            _testApiKey = Environment.GetEnvironmentVariable("ALGOLIA_API_KEY");
            _testApplicationID = Environment.GetEnvironmentVariable("ALGOLIA_APPLICATION_ID");
            _client = new AlgoliaClient(_testApplicationID, _testApiKey);
            _index = _client.InitIndex(safe_name("àlgol?à-csharp"));
        }

        [TearDown]
        public void TestCleanup()
        {
            _client.DeleteIndex(safe_name("àlgol?à-csharp"));
            _client = null;
        }

        [Test]
        public void TestAddObject()
        {
            clearTest();
            var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger""}"));
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"].ToString());
        }

        [Test]
        public void TestSaveObject()
        {
            clearTest();
            var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger""}"), "àlgol?à");
            _index.WaitTask(task["taskID"].ToString());
            task = _index.SaveObject(JObject.Parse(@"{""firstname"":""Robert"", ""lastname"":""Barninger"", ""objectID"":""àlgol?à""}"));
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Robert", res["hits"][0]["firstname"].ToString());
        }

        [Test]
        public void TestPartialUpdateObject()
        {
            clearTest();
            var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger""}"), "àlgol?à");
            _index.WaitTask(task["taskID"].ToString());
            task = _index.PartialUpdateObject(JObject.Parse(@"{""firstname"":""Robert"", ""objectID"":""àlgol?à""}"));
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Robert", res["hits"][0]["firstname"].ToString());
        }

        [Test]
        public void TestPartialUpdateObjectNoCreate()
        {
            clearTest();
            var task = _index.PartialUpdateObject(JObject.Parse(@"{""firstname"":""Robert"", ""objectID"":""àlgol?à""}"), false);
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.Search(new Query(""));
            Assert.AreEqual(0, res["nbHits"].ToObject<int>());
        }

        [Test]
        public void TaskAddObjects()
        {
            clearTest();
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Barninger""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Speach""}"));
            var task = _index.AddObjects(objs);
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.Search(new Query(""));
            Assert.AreEqual(2, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Roger", res["hits"][0]["firstname"].ToString());
        }

        [Test]
        public void TaskSaveObjects()
        {
            clearTest();
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Barninger"", ""objectID"":""à/go/?à1""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Speach"", ""objectID"":""à/go/?à2""}"));
            _index.AddObjects(objs);
            objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", 
                          ""lastname"":""Barninger"", ""objectID"":""à/go/?à1""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", 
                          ""lastname"":""Speach"", ""objectID"":""à/go/?à2""}"));
            var task = _index.SaveObjects(objs);
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.Search(new Query(""));
            Assert.AreEqual(2, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"].ToString());
        }

        [Test]
        public void TaskPartialUpdateObjects()
        {
            clearTest();
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Barninger"", ""objectID"":""à/go/?à1""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Speach"", ""objectID"":""à/go/?à2""}"));
            _index.AddObjects(objs);
            objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", ""objectID"":""à/go/?à1""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", ""objectID"":""à/go/?à2""}"));
            var task = _index.PartialUpdateObjects(objs);
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.Search(new Query(""));
            Assert.AreEqual(2, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"].ToString());
        }

        [Test]
        public void TaskPartialUpdateObjectsNoCreate()
        {
            clearTest();
            var objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", ""objectID"":""à/go/?à1""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", ""objectID"":""à/go/?à2""}"));
            var task = _index.PartialUpdateObjects(objs, false);
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.Search(new Query(""));
            Assert.AreEqual(0, res["nbHits"].ToObject<int>());
        }

        [Test]
        public void TaskDeleteObjects()
        {
            clearTest();
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Barninger"", ""objectID"": ""à/go/?à""}"));
            objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Speach"", ""objectID"": ""à/go/?à2""}"));
            var task = _index.AddObjects(objs);
            _index.WaitTask(task["taskID"].ToString());
            List<String> ids = new List<string>();
            ids.Add("à/go/?à");
            ids.Add("à/go/?à2");
            task = _index.DeleteObjects(ids);
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.Search(new Query(""));
            Assert.AreEqual(0, res["nbHits"].ToObject<int>());
        }

        [Test]
        public void TaskBatchMultipleIndexes()
        {
            clearTest();
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""action"":""addObject"", ""indexName"": """ + safe_name("àlgol?à-csharp") + @""", ""body"": {""firstname"":""Roger"", ""lastname"":""Barninger""}}"));
            objs.Add(JObject.Parse(@"{""action"":""addObject"", ""indexName"": """ + safe_name("àlgol?à-csharp") + @""", ""body"": {""firstname"":""Roger"", ""lastname"":""Speach""}}"));
            var task = _client.Batch(objs);
            _index.WaitTask(task["taskID"][safe_name("àlgol?à-csharp")].ToString());
            var res = _index.Search(new Query(""));
            Assert.AreEqual(2, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Roger", res["hits"][0]["firstname"].ToString());
        }

        [Test]
        public void TaskDeleteByQuery()
        {
            clearTest();
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""name"":""San Francisco""}"));
            objs.Add(JObject.Parse(@"{""name"":""San Jose""}"));
            objs.Add(JObject.Parse(@"{""name"":""Washington""}"));
            var task = _index.AddObjects(objs);
            _index.WaitTask(task["taskID"].ToString());
            _index.DeleteByQuery(new Query("San"));
            var res = _index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
        }

        [Test]
        public void TestMissingObjectIDPartialUpdateObject()
        {
            clearTest();
            JObject obj = new JObject();
            try
            {
                _index.PartialUpdateObject(obj);
            }
            catch (AlgoliaException)
            {
                return;
            }
            Assert.True(false);
        }

        [Test]
        public void TestMissingObjectIDPartialUpdateObjects()
        {
            clearTest();
            List<JObject> objs = new List<JObject>();
            objs.Add(new JObject());
            try
            {
                _index.PartialUpdateObjects(objs);
            }
            catch (AlgoliaException)
            {
                return;
            }
            Assert.True(false);
        }

        [Test]
        public void TestMissingObjectIDSaveObject()
        {
            clearTest();
            JObject obj = new JObject();
            try
            {
                _index.SaveObject(obj);
            }
            catch (AlgoliaException)
            {
                return;
            }
            Assert.True(false);
        }

        [Test]
        public void TestMissingObjectIDSaveObjects()
        {
            clearTest();
            List<JObject> objs = new List<JObject>();
            objs.Add(new JObject());
            try
            {
                _index.SaveObjects(objs);
            }
            catch (AlgoliaException)
            {
                return;
            }
            Assert.True(false);
        }

        public Boolean Include(JArray array, string attribute, string value)
        {
            for (int i = 0; i < array.Count; ++i)
            {
                if(array[i][attribute].ToString().Equals(value))
                    return true;
            }
            return false;
        }
        
        [Test]
        public void TestDeleteIndex()
        {
            clearTest();
            var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger""}"));
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"].ToString());
            res = _client.ListIndexes();
            Assert.IsTrue(Include((JArray)res["items"], "name", safe_name("àlgol?à-csharp")));
            task = _client.DeleteIndex(safe_name("àlgol?à-csharp"));
            _index.WaitTask(task["taskID"].ToObject<String>());
            res = _client.ListIndexes();
            Assert.IsFalse(Include((JArray)res["items"], "name", safe_name("àlgol?à-csharp")));
        }

        [Test]
        public void TestGetObject()
        {
            clearTest();
            var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""àlgol?à""}"));
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.GetObject("àlgol?à");
            Assert.AreEqual("àlgol?à", res["objectID"].ToString());
            Assert.AreEqual("Jimmie", res["firstname"].ToString());
        }

        [Test]
        public void TestGetObjects()
        {
            clearTest();
            _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""objectID"":""1""}"));
            var task = _index.AddObject(JObject.Parse(@"{""name"":""Los Angeles"", ""objectID"":""2""}"));
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.GetObjects(new string[2]{"1", "2"});
            Assert.AreEqual("San Francisco", res["results"][0]["name"].ToString());
            Assert.AreEqual("Los Angeles", res["results"][1]["name"].ToString());
        }

        [Test]
        public void TestDeleteObject()
        {
            clearTest();
            _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""àlgol?à""}"));
            var task = _index.DeleteObject("àlgol?à");
            _index.WaitTask(task["taskID"].ToString());
            Query query = new Query();
            query.SetQueryString("");
            var res = _index.Search(query);
            Assert.AreEqual(0, res["nbHits"].ToObject<int>());
        }

        [Test]
        public void TestDeleteObjectWithoutID()
        {
            clearTest();
            _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""à/go/?à""}"));
            try
            {
                var task = _index.DeleteObject("");
                _index.WaitTask(task["taskID"].ToString());
                Assert.Fail();
            }
            catch (Exception)
            { }
        }

        [Test]
        public void TestCopyIndex()
        {
            var index = _client.InitIndex(safe_name("àlgol?à-csharp2"));
            clearTest();
            var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
            _index.WaitTask(task["taskID"].ToString());
            task = _client.CopyIndex(safe_name("àlgol?à-csharp"), safe_name("àlgol?à-csharp2"));
            _index.WaitTask(task["taskID"].ToString());
            var res = index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"].ToString());
            _client.DeleteIndex(safe_name("àlgol?à-csharp2"));
        }

        [Test]
        public void TestMoveIndex()
        {
            var index = _client.InitIndex(safe_name("àlgol?à-csharp2"));
            clearTest();
            var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
            _index.WaitTask(task["taskID"].ToString());
            task = _client.MoveIndex(safe_name("àlgol?à-csharp"), safe_name("àlgol?à-csharp2"));
            index.WaitTask(task["taskID"].ToString());
            var res = index.Search(new Query(""));
            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"].ToString());
            res = _client.ListIndexes();
            Assert.IsTrue(Include((JArray)res["items"], "name", safe_name("àlgol?à-csharp2")));
            Assert.IsFalse(Include((JArray)res["items"], "name", safe_name("àlgol?à-csharp")));
            _client.DeleteIndex(safe_name("àlgol?à-csharp2"));
        }

        [Test]
        public void TestBrowse()
        {
            clearTest();
            var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.Browse(0);
            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"].ToString());
            res = _index.Browse(0, 1);
            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Jimmie", res["hits"][0]["firstname"].ToString());

        }

        [Test]
        public void TestBrowseAll()
        {
            clearTest();
            var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
            _index.WaitTask(task["taskID"].ToString());
            var res = _index.BrowseAll(new Query());
            JArray hits = new JArray();
            foreach (var hit in res) {
                hits.Add(hit);
            }
            Assert.AreEqual(1, hits.Count);
            Assert.AreEqual("Jimmie", hits[0]["firstname"].ToString());
        }

        [Test]
        public void TestLogs()
        {
            var res = _client.GetLogs();
            Assert.IsTrue(((JArray)res["logs"]).Count > 0);
            res = _client.GetLogs(0, 1, false);
            Assert.AreEqual(1, ((JArray)res["logs"]).Count);
        }

        [Test]
        public void TestSearch()
        {
            clearTest();
            // Add one object to be sure the test will not fail because index is empty
            var task = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
            _index.WaitTask(task["taskID"].ToObject<String>());
            Assert.IsFalse(string.IsNullOrWhiteSpace(task["objectID"].ToString()));
            var res = _index.Search(new Query());
            Assert.AreEqual("1", res["nbHits"].ToObject<String>());
            Assert.AreEqual("San Francisco", res["hits"][0]["name"].ToString());
        }

        [Test]
        public void TestSettings()
        {
            clearTest();
            var res = _index.SetSettings(JObject.Parse(@"{""customRanking"":[""desc(population)"", ""asc(name)""]}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
            _index.WaitTask(res["taskID"].ToObject<String>());
            res = _index.GetSettings();
            _client.DeleteIndex(safe_name("àlgol?à-csharp"));
        }

        [Test]
        public void TestAddObject2()
        {
            clearTest();
            var res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["objectID"].ToString()));
            res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
            Assert.AreEqual("myID", res["objectID"].ToString());
        }

        [Test]
        public void TestUpdate()
        {
            clearTest();
            var res = _index.SaveObject(JObject.Parse(@"{""name"":""Los Angeles"", 
                                                              ""population"":3792621, 
                                                              ""objectID"":""myID""}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
            res = _index.PartialUpdateObject(JObject.Parse(@"{""population"":3792621, 
                                                                   ""objectID"":""myID""}"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
        }

        [Test]
        public void TaskGetObject()
        {
            clearTest();
            // Add one object to be sure the test will not fail because index is empty
            var res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
            _index.WaitTask(res["taskID"].ToString());
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["objectID"].ToString()));
            res = _index.GetObject("myID");
            Assert.AreEqual("San Francisco", res["name"].ToString());
            Assert.AreEqual(805235, res["population"].ToObject<int>());
            Assert.AreEqual("myID", res["objectID"].ToString());
            res = _index.GetObject("myID", new String[] {"name", "population"});
            Assert.AreEqual("San Francisco", res["name"].ToString());
            Assert.AreEqual(805235, res["population"].ToObject<int>());
            Assert.AreEqual("myID", res["objectID"].ToString());
            res = _index.GetObject("myID", new String[] { "name" });
            Assert.AreEqual(null, res["population"]);
            Assert.AreEqual("San Francisco", res["name"].ToString());
            Assert.AreEqual("myID", res["objectID"].ToString());
        }

        [Test]
        public void TaskDeleteObject()
        {
            clearTest();
            // Add one object to be sure the test will not fail because index is empty
            var res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
            _index.WaitTask(res["taskID"].ToString());
            res = _index.DeleteObject("myID");
            Assert.IsFalse(string.IsNullOrWhiteSpace(res["deletedAt"].ToString()));
        }

        [Test]
        public void TaskBatch()
        {
            clearTest();
            
            List<JObject> objs = new List<JObject>();
            objs.Add(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
            objs.Add(JObject.Parse(@"{""name"":""Los Angeles"", ""population"":3792621}"));
            var res = _index.AddObjects(objs);
            JArray objectIDs = (JArray)res["objectIDs"];
            Assert.AreEqual(objectIDs.Count, 2);
            List<JObject> objs2 = new List<JObject>();
            objs2.Add(JObject.Parse(@"{""name"":""San Francisco"", 
                          ""population"": 805235,
                          ""objectID"":""SFO""}"));
            objs2.Add(JObject.Parse(@"{""name"":""Los Angeles"", 
                          ""population"": 3792621,
                          ""objectID"": ""LA""}"));
            res = _index.SaveObjects(objs2);
            objectIDs = (JArray)res["objectIDs"];
            Assert.AreEqual(objectIDs.Count, 2);
        }

        [Test]
        public void TestListIndexes()
        {
            clearTest();
            try
            {
                var result = _client.ListIndexes();
                Assert.IsFalse(string.IsNullOrWhiteSpace(result["items"].ToString()));
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [Test]
        public void TaskACLClient()
        {
            clearTest();
            // Add one object to be sure the test will not fail because index is empty
            var res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
            _index.WaitTask(res["taskID"].ToString());
            var key = _client.AddUserKey(new String[] { "search" });
            System.Threading.Thread.Sleep(5000);
            Assert.IsFalse(string.IsNullOrWhiteSpace(key["key"].ToString()));
            var getKey = _client.GetUserKeyACL(key["key"].ToString());
            Assert.AreEqual(key["key"], getKey["value"]);
            var keys = _client.ListUserKeys();
            Assert.IsTrue(Include((JArray)keys["keys"], "value", key["key"].ToString()));
            _client.UpdateUserKey(key["key"].ToString(), new String[] { "addObject" });
            System.Threading.Thread.Sleep(5000);
            getKey = _client.GetUserKeyACL(key["key"].ToString());
            Assert.AreEqual((string)getKey["acl"][0], "addObject");
            _client.DeleteUserKey(key["key"].ToString());
            System.Threading.Thread.Sleep(5000);
            keys = _client.ListUserKeys();
            Assert.IsFalse(Include((JArray)keys["keys"], "value", key["key"].ToString()));
        }

        [Test]
        public void TaskACLIndex()
        {
            clearTest();
            // Add one object to be sure the test will not fail because index is empty
            var res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
            _index.WaitTask(res["taskID"].ToString());
            var key = _index.AddUserKey(new String[] { "search" });
            System.Threading.Thread.Sleep(5000);
            Assert.IsFalse(string.IsNullOrWhiteSpace(key["key"].ToString()));
            var getKey = _index.GetUserKeyACL(key["key"].ToString());
            Assert.AreEqual(key["key"], getKey["value"]);
            var keys = _index.ListUserKeys();
            Assert.IsTrue(Include((JArray)keys["keys"], "value", key["key"].ToString()));
            _index.UpdateUserKey(key["key"].ToString(), new String[] { "addObject" });
            System.Threading.Thread.Sleep(5000);
            getKey = _index.GetUserKeyACL(key["key"].ToString());
            Assert.AreEqual((string)getKey["acl"][0], "addObject");
            _index.DeleteUserKey(key["key"].ToString());
            System.Threading.Thread.Sleep(5000);
            keys = _index.ListUserKeys();
            Assert.IsFalse(Include((JArray)keys["keys"], "value", key["key"].ToString()));
        }

        [Test]
        public void BadIndexName()
        {
            var ind = _client.InitIndex("&&");
            try
            {
                ind.ClearIndex();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(new AlgoliaException("indexName is not valid").Message, e.Message);
            }
        }

        [Test]
        public void NetworkIssue()
        {
            try
            {
                new AlgoliaClient(_testApiKey, _testApiKey).ListIndexes(); // Should not find APIKEY.algolia.net

                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.True(e.Message.StartsWith("Hosts unreachable:"));
            }
        }

        [Test]
        public void BadClientCreation()
        {
            string[] _hosts = new string[] { "localhost.algolia.com:8080", "" };
            try
            {
                new AlgoliaClient("", _testApiKey);
                Assert.Fail();
            }
            catch (Exception)
            { }
            try
            {
                new AlgoliaClient(_testApplicationID, "");
                Assert.Fail();
            }
            catch (Exception)
            { }
            try
            {
                new AlgoliaClient(_testApplicationID, "", _hosts);
                Assert.Fail();
            }
            catch (Exception)
            { }
            try
            {
                new AlgoliaClient("", _testApiKey, _hosts);
                Assert.Fail();
            }
            catch (Exception)
            { }
            try
            {
                var badClient = new AlgoliaClient(_testApplicationID, _testApiKey, null);
                Assert.Fail();
            }
            catch (Exception)
            { }
            try
            {
                var badClient = new AlgoliaClient(_testApplicationID, _testApiKey, _hosts);
                badClient.ListIndexes();
                Assert.Fail();
            }
            catch (Exception)
            { }
        }

        [Test]
        public void TestBigQueryAll()
        {
            clearTest();
            _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie J""
                , ""Age"":42, ""lastname"":""Barninger"", ""_tags"": ""people""
                , ""_geoloc"":{""lat"":0.853409, ""lng"":0.348800}}"));
            var task = _index.SetSettings(JObject.Parse(@"{""attributesForFaceting"": [""_tags""]}"));
            _index.WaitTask(task["taskID"].ToString());
            Query query = new Query("Jimmie");
            query.SetPage(0);
            query.SetOptionalWords("J");
            query.SetNbHitsPerPage(1);
            string[] attr = { "firstname" };
            query.SetAttributesToHighlight(attr);
            query.SetMinWordSizeToAllowOneTypo(1);
            query.SetMinWordSizeToAllowTwoTypos(2);
            query.EnableDistinct(true);
            query.SetRemoveWordsIfNoResult(Query.RemoveWordsIfNoResult.FIRST_WORDS);
            query.GetRankingInfo(true);
            query.EnableTyposOnNumericTokens(false);
            query.SetAttributesToRetrieve(attr);
            query.SetAttributesToSnippet(attr);
            query.InsideBoundingBox(0, 0, 90, 90);
            query.AroundLatitudeLongitude(0, 0, 2000000000);
            string[] facetFilter = { "_tags:people" };
            string[] facets = { "_tags" };
            query.SetFacetFilters(facetFilter);
            query.SetFacets(facets);
            query.SetTagFilters("people");
            query.SetNumericFilters("Age>=42");
            query.SetQueryType(Query.QueryType.PREFIX_ALL);
            query.AddCustomParameter("facets", "_tags");
            var res = _index.Search(query);
            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Jimmie J", res["hits"][0]["firstname"].ToString());
            _client.DeleteIndex(safe_name("àlgol?à-csharp"));
        }

        [Test]
        public void TestBigQueryNone()
        {
            clearTest();
            _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie J""
                , ""Age"":42, ""lastname"":""Barninger"", ""_tags"": ""people""
                , ""_geoloc"":{""lat"":0.853409, ""lng"":0.348800}}"));
            var task = _index.SetSettings(JObject.Parse(@"{""attributesForFaceting"": [""_tags""]}"));
            _index.WaitTask(task["taskID"].ToString());
            Query query = new Query("Jimmie");
            query.SetPage(0);
            query.SetOptionalWords("J");
            query.SetNbHitsPerPage(1);
            string[] attr = { "firstname" };
            query.SetAttributesToHighlight(attr);
            query.SetMinWordSizeToAllowOneTypo(1);
            query.SetMinWordSizeToAllowTwoTypos(2);
            query.SetAnalyticsTags(new string[]{"tagIt"});
            query.EnableDistinct(true);
            query.EnableAnalytics(true);
            query.EnableAdvancedSyntax(true);
            query.GetRankingInfo(true);
            query.EnableTypoTolerance(false);
            query.SetAttributesToRetrieve(attr);
            query.SetAttributesToSnippet(attr);
            query.SetMaxValuesPerFacets(1);
            query.InsideBoundingBox(0, 0, 90, 90);
            query.AroundLatitudeLongitude(0, 0, 2000000000, 100);
            string[] facetFilter = { "_tags:people" };
            string[] facets = { "_tags" };
            query.SetFacetFilters(facetFilter);
            query.SetFacets(facets);
            query.SetTagFilters("people");
            query.SetNumericFilters("Age>=42");
            query.SetQueryType(Query.QueryType.PREFIX_NONE);
            query.SetRemoveWordsIfNoResult(Query.RemoveWordsIfNoResult.LAST_WORDS);
            var res = _index.Search(query);
            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Jimmie J", res["hits"][0]["firstname"].ToString());
            _client.DeleteIndex(safe_name("àlgol?à-csharp"));
        }

        [Test]
        public void TestMultipleQueries()
        {
            clearTest();
            var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger""}"));
            _index.WaitTask(task["taskID"].ToString());
            var indexQuery = new List<IndexQuery>();
            indexQuery.Add(new IndexQuery(safe_name("àlgol?à-csharp"), new Query("")));
            var res = _client.MultipleQueries(indexQuery);
            Assert.AreEqual(1, res["results"].ToObject<JArray>().Count);
            Assert.AreEqual(1, res["results"][0]["hits"].ToObject<JArray>().Count);
            Assert.AreEqual("Jimmie", res["results"][0]["hits"][0]["firstname"].ToString());
            _client.DeleteIndex(safe_name("àlgol?à-csharp"));
        }

        [Test]
        public void TestFacets()
        {
            clearTest();
            _index.SetSettings(JObject.Parse(@"{""attributesForFacetting"":[""city"", ""stars"", ""facilites""]}"));
            JObject task = _index.AddObjects(new JObject[] {
                JObject.Parse(@"{""name"": ""Hotel A"", ""stars"":""*"", ""facilities"":[""wifi"", ""bath"", ""spa""], ""city"": ""Paris""}"),
                JObject.Parse(@"{""name"": ""Hotel B"", ""stars"":""*"", ""facilities"":[""wifi""], ""city"": ""Paris""}"),
                JObject.Parse(@"{""name"": ""Hotel C"", ""stars"":""**"", ""facilities"":[""bath""], ""city"": ""San Francisco""}"),
                JObject.Parse(@"{""name"": ""Hotel D"", ""stars"":""****"", ""facilities"":[""spa""], ""city"": ""Paris""}"),
                JObject.Parse(@"{""name"": ""Hotel E"", ""stars"":""****"", ""facilities"":[""spa""], ""city"": ""New York""}")
            });
            _index.WaitTask((task["taskID"].ToString()));
            JObject res = _index.Search(new Query().SetFacetFilters(new String[] { "stars:****", "city:Paris" }).SetFacets(new String[] { "stars" }));
            Assert.AreEqual("1", res["nbHits"].ToString());
            Assert.AreEqual("Hotel D", res["hits"][0]["name"].ToString());
            res = _index.Search(new Query().SetFacetFilters("[\"stars:****\",\"city:Paris\"]").SetFacets(new String[] { "stars" }));
            Assert.AreEqual("1", res["nbHits"].ToString());
            Assert.AreEqual("Hotel D", res["hits"][0]["name"].ToString());
            res = _index.Search(new Query().SetFacetFilters(JArray.Parse(@"[""stars:****"",""city:Paris""]")).SetFacets(new String[] { "stars" }));
            Assert.AreEqual("1", res["nbHits"].ToString());
            Assert.AreEqual("Hotel D", res["hits"][0]["name"].ToString());
            res = _index.Search(new Query().SetFilters("stars:**** AND city:Paris").SetFacets(new String[] { "stars" }));
            Assert.AreEqual("1", res["nbHits"].ToString());
            Assert.AreEqual("Hotel D", res["hits"][0]["name"].ToString());
        }

        [Test]
        public void TestGenerateSecuredApiKey()
        {
            Assert.AreEqual("ZmI5YjQ5N2U3YjFkYjcxYTQ2YjE4OWFmNWUxNmVlNmVlNDkzNzYyYTFlYmE5NThhNjhhN2I5ZjhhN2NkYWNmMnRhZ0ZpbHRlcnM9KHB1YmxpYyUyQ3VzZXIxKQ==", _client.GenerateSecuredApiKey("182634d8894831d5dbce3b3185c50881", "(public,user1)"));
            Assert.AreEqual("ZmI5YjQ5N2U3YjFkYjcxYTQ2YjE4OWFmNWUxNmVlNmVlNDkzNzYyYTFlYmE5NThhNjhhN2I5ZjhhN2NkYWNmMnRhZ0ZpbHRlcnM9KHB1YmxpYyUyQ3VzZXIxKQ==", _client.GenerateSecuredApiKey("182634d8894831d5dbce3b3185c50881", new Query().SetTagFilters("(public,user1)")));
            Assert.AreEqual("MjgzZDFkNjliM2UwNGQ1MTBiODY0MTAwZjAyNjgxN2MzZmVhNTBkY2JkMzE5ODRhNmVjNzE0MGVlOTE0ZjVmZXRhZ0ZpbHRlcnM9KHB1YmxpYyUyQ3VzZXIxKSZ1c2VyVG9rZW49NDI=", _client.GenerateSecuredApiKey("182634d8894831d5dbce3b3185c50881", new Query().SetTagFilters("(public,user1)").SetUserToken("42")));
        }

        [Test]
        public void TestDisjunctiveFaceting()
        {
            clearTest();
            _index.SetSettings(JObject.Parse(@"{""attributesForFacetting"": [""city"", ""stars"", ""facilities""]}"));
            JObject task = _index.AddObjects(new JObject[]{
                JObject.Parse(@"{""name"":""Hotel A"", ""stars"":""*"", ""facilities"":[""wifi"", ""bath"", ""spa""], ""city"":""Paris""}"),
                JObject.Parse(@"{""name"":""Hotel B"", ""stars"":""*"", ""facilities"":[""wifi""], ""city"":""Paris""}"),
                JObject.Parse(@"{""name"":""Hotel C"", ""stars"":""**"", ""facilities"":[""bath""], ""city"":""San Fancisco""}"),
                JObject.Parse(@"{""name"":""Hotel D"", ""stars"":""****"", ""facilities"":[""spa""], ""city"":""Paris""}"),
                JObject.Parse(@"{""name"":""Hotel E"", ""stars"":""****"", ""facilities"":[""spa""], ""city"":""New York""}")
            });
            _index.WaitTask((task["taskID"].ToString()));
            Dictionary<string, IEnumerable<string>> refinements = new Dictionary<string, IEnumerable<string>>();
            JObject answer = _index.SearchDisjunctiveFaceting(new Query("h").SetFacets(new string[] { "city" }), new string[] { "stars", "facilities" });
            Assert.AreEqual(5, answer["nbHits"].ToObject<int>());
            Assert.AreEqual(1, answer["facets"].ToObject<JObject>().Count);
            Assert.AreEqual(2, answer["disjunctiveFacets"].ToObject<JObject>().Count);

            refinements.Add("stars", new string[] { "*" });
            answer = _index.SearchDisjunctiveFaceting(new Query("h").SetFacets(new string[] { "city" }), new string[] { "stars", "facilities" }, refinements);
            Assert.AreEqual(2, answer["nbHits"].ToObject<int>());
            Assert.AreEqual(1, answer["facets"].ToObject<JObject>().Count);
            Assert.AreEqual(2, answer["disjunctiveFacets"].ToObject<JObject>().Count);
            Assert.AreEqual(2, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["*"].ToObject<int>());
            Assert.AreEqual(1, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["**"].ToObject<int>());
            Assert.AreEqual(2, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["****"].ToObject<int>());

            refinements.Clear();
            refinements.Add("stars", new string[] { "*" });
            refinements.Add("city", new string[] { "Paris" });
            answer = _index.SearchDisjunctiveFaceting(new Query("h").SetFacets(new string[] { "city" }), new string[] { "stars", "facilities" }, refinements);
            Assert.AreEqual(2, answer["nbHits"].ToObject<int>());
            Assert.AreEqual(1, answer["facets"].ToObject<JObject>().Count);
            Assert.AreEqual(2, answer["disjunctiveFacets"].ToObject<JObject>().Count);
            Assert.AreEqual(2, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["*"].ToObject<int>());
            Assert.AreEqual(1, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["****"].ToObject<int>());

            refinements.Clear();
            refinements.Add("stars", new string[] { "*", "****" });
            refinements.Add("city", new string[] { "Paris" });
            answer = _index.SearchDisjunctiveFaceting(new Query("h").SetFacets(new string[] { "city" }), new string[] { "stars", "facilities" }, refinements);
            Assert.AreEqual(3, answer["nbHits"].ToObject<int>());
            Assert.AreEqual(1, answer["facets"].ToObject<JObject>().Count);
            Assert.AreEqual(2, answer["disjunctiveFacets"].ToObject<JObject>().Count);
            Assert.AreEqual(2, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["*"].ToObject<int>());
            Assert.AreEqual(1, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["****"].ToObject<int>());
        }

        [Test]
        public void TestCancellationToken()
        {
            CancellationTokenSource ct = new CancellationTokenSource();
            Task<JObject> task = _client.ListIndexesAsync(ct.Token);
            ct.Cancel();
            try
            {
                task.GetAwaiter().GetResult();
                Assert.Fail("Should thow an error");
            }
            catch (TaskCanceledException)
            {
                // Pass
            }
        }

        [Test]
        public void TestTimeoutHandling()
        {
            _client.setTimeout(0.001, 0.001);
            try
            {
                _client.ListIndexes();
                _client = new AlgoliaClient(_testApplicationID, _testApiKey);
                _index = _client.InitIndex(safe_name("àlgol?à-csharp"));
                Assert.Fail("Should throw an error");
            } 
            catch (AlgoliaException)
            {
                // Reset 
                _client = new AlgoliaClient(_testApplicationID, _testApiKey);
                _index = _client.InitIndex(safe_name("àlgol?à-csharp"));
            }
            catch (OperationCanceledException)
            {
                _client = new AlgoliaClient(_testApplicationID, _testApiKey);
                _index = _client.InitIndex(safe_name("àlgol?à-csharp"));
                Assert.Fail("Should throw an AlgoliaException");
            }
        }

        [Test]
        public void TestDnsTimeout()
        {
            var hosts = new List<string> {
                _testApplicationID + "-dsn.algolia.biz",
                _testApplicationID + "-dsn.algolia.net",
                _testApplicationID + "-1.algolianet.com",
                _testApplicationID + "-2.algolianet.com",
                _testApplicationID + "-3.algolianet.com"
            };

            var _client = new AlgoliaClient(_testApplicationID, _testApiKey, hosts);
            _client.setTimeout(0.5, 0.5);
            var startTime = DateTime.Now;
            var index = _client.ListIndexes();
            Assert.IsTrue(startTime.AddSeconds(0.5) < DateTime.Now);
        }
    }
}

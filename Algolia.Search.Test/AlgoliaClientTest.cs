using System;
using System.Collections;
using Xunit;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Algolia.Search;
using System.Threading;
using Algolia.Search.Models;
using RichardSzalay.MockHttp;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Xunit.Sdk;
using Algolia.Search.Iterators;
using System.Linq;

namespace Algolia.Search.Test
{
#pragma warning disable 0618
	public class AlgoliaClientTest : BaseTest
	{
		[Fact]
		public void TaskAddObjects()
		{
			ClearTest();
			List<JObject> objs = new List<JObject>();
			objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Barninger""}"));
			objs.Add(JObject.Parse(@"{""firstname"":""Roger"", 
                          ""lastname"":""Speach""}"));
			var task = _index.AddObjects(objs);
			_index.WaitTask(task["taskID"].ToString());
			var res = _index.Search(new Query(""));
			Assert.Equal(2, res["nbHits"].ToObject<int>());
			Assert.Equal("Roger", res["hits"][0]["firstname"].ToString());
		}

		[Fact]
		public void TaskSaveObjects()
		{
			ClearTest();
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
			Assert.Equal(2, res["nbHits"].ToObject<int>());
			Assert.Equal("Jimmie", res["hits"][0]["firstname"].ToString());
		}

		[Fact]
		public void TaskPartialUpdateObjects()
		{
			ClearTest();
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
			Assert.Equal(2, res["nbHits"].ToObject<int>());
			Assert.Equal("Jimmie", res["hits"][0]["firstname"].ToString());
		}

		[Fact]
		public void TaskPartialUpdateObjectsNoCreate()
		{
			ClearTest();
			var objs = new List<JObject>();
			objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", ""objectID"":""à/go/?à1""}"));
			objs.Add(JObject.Parse(@"{""firstname"":""Jimmie"", ""objectID"":""à/go/?à2""}"));
			var task = _index.PartialUpdateObjects(objs, false);
			_index.WaitTask(task["taskID"].ToString());
			var res = _index.Search(new Query(""));
			Assert.Equal(0, res["nbHits"].ToObject<int>());
		}

		[Fact]
		public void TaskDeleteObjects()
		{
			ClearTest();
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
			Assert.Equal(0, res["nbHits"].ToObject<int>());
		}

		[Fact]
		public void TaskBatchMultipleIndexes()
		{
			ClearTest();
			List<JObject> objs = new List<JObject>();
			objs.Add(JObject.Parse(@"{""action"":""addObject"", ""indexName"": """ + GetSafeName("àlgol?à-csharp") + @""", ""body"": {""firstname"":""Roger"", ""lastname"":""Barninger""}}"));
			objs.Add(JObject.Parse(@"{""action"":""addObject"", ""indexName"": """ + GetSafeName("àlgol?à-csharp") + @""", ""body"": {""firstname"":""Roger"", ""lastname"":""Speach""}}"));
			var task = _client.Batch(objs);
			_index.WaitTask(task["taskID"][GetSafeName("àlgol?à-csharp")].ToString());
			var res = _index.Search(new Query(""));
			Assert.Equal(2, res["nbHits"].ToObject<int>());
			Assert.Equal("Roger", res["hits"][0]["firstname"].ToString());
		}

		[Fact]
		public void TaskDeleteByQuery()
		{
			ClearTest();
			List<JObject> objs = new List<JObject>();
			objs.Add(JObject.Parse(@"{""name"":""San Francisco""}"));
			objs.Add(JObject.Parse(@"{""name"":""San Jose""}"));
			objs.Add(JObject.Parse(@"{""name"":""Washington""}"));
			var task = _index.AddObjects(objs);
			_index.WaitTask(task["taskID"].ToString());
			_index.DeleteByQuery(new Query("San"));
			var res = _index.Search(new Query(""));
			Assert.Equal(1, res["nbHits"].ToObject<int>());
		}

		[Fact]
		public void TaskDeleteBy()
		{
			ClearTest();
			List<JObject> objs = new List<JObject>();
			objs.Add(JObject.Parse(@"{""dummy"": 1}"));
			objs.Add(JObject.Parse(@"{""dummy"": 2}"));
			objs.Add(JObject.Parse(@"{""dummy"": 3}"));
			var task = _index.AddObjects(objs);
			_index.WaitTask(task["taskID"].ToString());
			var query = new Query();
			query.SetNumericFilters("dummy <= 2");
			var task2 = _index.DeleteBy(query);
			_index.WaitTask(task2["taskID"].ToString());
			var res = _index.Search(new Query(""));
			Assert.Equal(1, res["nbHits"].ToObject<int>());
		}

		[Fact]
		public void TestMissingObjectIDPartialUpdateObject()
		{
			ClearTest();
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

		[Fact]
		public void TestMissingObjectIDPartialUpdateObjects()
		{
			ClearTest();
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

		[Fact]
		public void TestMissingObjectIDSaveObject()
		{
			ClearTest();
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

		[Fact]
		public void TestMissingObjectIDSaveObjects()
		{
			ClearTest();
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
				if (array[i][attribute].ToString().Equals(value))
					return true;
			}
			return false;
		}

		[Fact]
		public void TestDeleteIndex()
		{
			ClearTest();
			var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger""}"));
			_index.WaitTask(task["taskID"].ToString());
			var res = _index.Search(new Query(""));
			Assert.Equal(1, res["nbHits"].ToObject<int>());
			Assert.Equal("Jimmie", res["hits"][0]["firstname"].ToString());
			res = _client.ListIndexes();
			Assert.True(Include((JArray)res["items"], "name", GetSafeName("àlgol?à-csharp")));
			task = _client.DeleteIndex(GetSafeName("àlgol?à-csharp"));
			_index.WaitTask(task["taskID"].ToObject<String>());
			res = _client.ListIndexes();
			Assert.False(Include((JArray)res["items"], "name", GetSafeName("àlgol?à-csharp")));
		}

		[Fact]
		public void TestGetObject()
		{
			ClearTest();
			var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""algolia""}"));
			_index.WaitTask(task["taskID"].ToString());
			var res = _index.GetObject("algolia");
			Assert.Equal("algolia", res["objectID"].ToString());
			Assert.Equal("Jimmie", res["firstname"].ToString());
		}

		[Fact]
		public void TestGetObjects()
		{
			ClearTest();
			_index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""objectID"":""1""}"));
			var task = _index.AddObject(JObject.Parse(@"{""name"":""Los Angeles"", ""objectID"":""2""}"));
			_index.WaitTask(task["taskID"].ToString());
			var res = _index.GetObjects(new string[2] { "1", "2" });
			Assert.Equal("San Francisco", res["results"][0]["name"].ToString());
			Assert.Equal("Los Angeles", res["results"][1]["name"].ToString());
		}

		[Fact]
		public void TestGetObjectsWithAttribute()
		{
			ClearTest();
			_index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""objectID"":""1"", ""nickname"":""SF""}"));
			var task = _index.AddObject(JObject.Parse(@"{""name"":""Los Angeles"", ""objectID"":""2"", ""nickname"":""SanF""}"));
			_index.WaitTask(task["taskID"].ToString());
			var res = _index.GetObjects(new string[2] { "1", "2" }, new List<string> { "nickname" });
			Assert.Equal("SF", res["results"][0]["nickname"].ToString());
			Assert.Equal("SanF", res["results"][1]["nickname"].ToString());
			Assert.Empty(res["results"][1]["name"]);
		}

		[Fact]
		public void TestDeleteObject()
		{
			ClearTest();
			_index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""algolia""}"));
			var task = _index.DeleteObject("algolia");
			_index.WaitTask(task["taskID"].ToString());
			Query query = new Query();
			query.SetQueryString("");
			var res = _index.Search(query);
			Assert.Equal(0, res["nbHits"].ToObject<int>());
		}

		[Fact]
		public void TestDeleteObjectWithoutID()
		{
			ClearTest();
			_index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""à/go/?à""}"));
			try
			{
				Assert.Throws<Exception>(() =>
				{
					var task = _index.DeleteObject("");
					_index.WaitTask(task["taskID"].ToString());
				});
			}
			catch (Exception)
			{ }
		}

		[Fact]
		public void TestScopedCopyIndex()
		{
			var dstIndexName = GetSafeName("àlgol?à-csharp2");
			var srcIndexName = GetSafeName("àlgol?à-csharp");
			ClearTest();
			_client.DeleteIndex(dstIndexName);
			var dstIndex = _client.InitIndex(GetSafeName(dstIndexName));

			var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
			_index.WaitTask(task["taskID"].ToString());
			task = _index.SetSettings(JObject.Parse(@"{""searchableAttributes"": [""name""]}"));
			_index.WaitTask(task["taskID"].ToString());

			string ruleId = "ruleID";
			JObject rule = generateRuleStub(ruleId);
			task = _index.SaveRule(rule);
			_index.WaitTask(task["taskID"].ToString());

			List<CopyScope> scopes = new List<CopyScope>() { CopyScope.SETTINGS };
			task = _client.CopyIndex(srcIndexName, dstIndexName, null, scopes);
			_index.WaitTask(task["taskID"].ToString());

			// The below would fail if we don't specify CopyScope.SETTINGS since there wouldn't be any settings.
			//var srcSettings = _index.GetSettings();
			//var dstSettings = dstIndex.GetSettings();
			var dstRules = dstIndex.SearchRules();
			var srcRules = _index.SearchRules();

			// Assert same settings
			//Assert.Equal(srcSettings["searchableAttributes"][0].ToString(), dstSettings["searchableAttributes"][0].ToString());
			// Assert different rules since they haven't been copied
			Assert.NotEqual((int)srcRules["nbHits"], (int)dstRules["nbHits"]);

			_client.DeleteIndex(dstIndexName);
		}

		[Fact]
		public void TestCopyIndex()
		{
			var index = _client.InitIndex(GetSafeName("àlgol?à-csharp2"));
			ClearTest();
			var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
			_index.WaitTask(task["taskID"].ToString());
			task = _client.CopyIndex(GetSafeName("àlgol?à-csharp"), GetSafeName("àlgol?à-csharp2"));
			_index.WaitTask(task["taskID"].ToString());
			var res = index.Search(new Query(""));
			Assert.Equal(1, res["nbHits"].ToObject<int>());
			Assert.Equal("Jimmie", res["hits"][0]["firstname"].ToString());
			_client.DeleteIndex(GetSafeName("àlgol?à-csharp2"));
		}

		[Fact]
		public void TestMoveIndex()
		{
			var index = _client.InitIndex(GetSafeName("àlgol?à-csharp2"));
			ClearTest();
			var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
			_index.WaitTask(task["taskID"].ToString());
			task = _client.MoveIndex(GetSafeName("àlgol?à-csharp"), GetSafeName("àlgol?à-csharp2"));
			index.WaitTask(task["taskID"].ToString());
			var res = index.Search(new Query(""));
			Assert.Equal(1, res["nbHits"].ToObject<int>());
			Assert.Equal("Jimmie", res["hits"][0]["firstname"].ToString());
			res = _client.ListIndexes();
			Assert.True(Include((JArray)res["items"], "name", GetSafeName("àlgol?à-csharp2")));
			Assert.False(Include((JArray)res["items"], "name", GetSafeName("àlgol?à-csharp")));
			_client.DeleteIndex(GetSafeName("àlgol?à-csharp2"));
		}

		[Fact]
		public void TestBrowse()
		{
			ClearTest();
			var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
			_index.WaitTask(task["taskID"].ToString());
			var res = _index.Browse(0);
			Assert.Equal(1, res["nbHits"].ToObject<int>());
			Assert.Equal("Jimmie", res["hits"][0]["firstname"].ToString());
			res = _index.Browse(0, 1);
			Assert.Equal(1, res["nbHits"].ToObject<int>());
			Assert.Equal("Jimmie", res["hits"][0]["firstname"].ToString());

		}

		[Fact]
		public void TestBrowseAll()
		{
			ClearTest();
			var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger"", ""objectID"":""1""}"));
			_index.WaitTask(task["taskID"].ToString());
			var res = _index.BrowseAll(new Query());
			JArray hits = new JArray();
			foreach (var hit in res)
			{
				hits.Add(hit);
			}
			Assert.Equal(1, hits.Count);
			Assert.Equal("Jimmie", hits[0]["firstname"].ToString());
		}

		[Fact]
		public void TestLogs()
		{
			var res = _client.GetLogs();
			Assert.True(((JArray)res["logs"]).Count > 0);
			res = _client.GetLogs(0, 1, false);
			Assert.Equal(1, ((JArray)res["logs"]).Count);
		}

		[Fact]
		public void TestSearch()
		{
			ClearTest();
			// Add one object to be sure the test will not fail because index is empty
			var task = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
			_index.WaitTask(task["taskID"].ToObject<String>());
			Assert.False(string.IsNullOrWhiteSpace(task["objectID"].ToString()));
			var res = _index.Search(new Query());
			Assert.Equal("1", res["nbHits"].ToObject<String>());
			Assert.Equal("San Francisco", res["hits"][0]["name"].ToString());
		}

		[Fact]
		public void TestSettings()
		{
			ClearTest();
			var res = _index.SetSettings(JObject.Parse(@"{""customRanking"":[""desc(population)"", ""asc(name)""]}"));
			Assert.False(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
			_index.WaitTask(res["taskID"].ToObject<String>());
			res = _index.GetSettings();
			_client.DeleteIndex(GetSafeName("àlgol?à-csharp"));
		}

		[Fact]
		public void TestSettingsWithReplicas()
		{
			ClearTest();
			var res = _index.SetSettings(JObject.Parse(@"{""customRanking"":[""desc(population)"", ""asc(name)""]}"), true);
			Assert.False(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
			_index.WaitTask(res["taskID"].ToObject<String>());
			res = _index.GetSettings();
			_client.DeleteIndex(GetSafeName("àlgol?à-csharp"));
		}

		[Fact]
		public void TestAddObject2()
		{
			ClearTest();
			var res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
			Assert.False(string.IsNullOrWhiteSpace(res["objectID"].ToString()));
			res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
			Assert.Equal("myID", res["objectID"].ToString());
		}

		[Fact]
		public void TestUpdate()
		{
			ClearTest();
			var res = _index.SaveObject(JObject.Parse(@"{""name"":""Los Angeles"", 
                                                              ""population"":3792621, 
                                                              ""objectID"":""myID""}"));
			Assert.False(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
			res = _index.PartialUpdateObject(JObject.Parse(@"{""population"":3792621, 
                                                                   ""objectID"":""myID""}"));
			Assert.False(string.IsNullOrWhiteSpace(res["updatedAt"].ToString()));
		}

		[Fact]
		public void TaskGetObject()
		{
			ClearTest();
			// Add one object to be sure the test will not fail because index is empty
			var res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
			_index.WaitTask(res["taskID"].ToString());
			Assert.False(string.IsNullOrWhiteSpace(res["objectID"].ToString()));
			res = _index.GetObject("myID");
			Assert.Equal("San Francisco", res["name"].ToString());
			Assert.Equal(805235, res["population"].ToObject<int>());
			Assert.Equal("myID", res["objectID"].ToString());
			res = _index.GetObject("myID", new String[] { "name", "population" });
			Assert.Equal("San Francisco", res["name"].ToString());
			Assert.Equal(805235, res["population"].ToObject<int>());
			Assert.Equal("myID", res["objectID"].ToString());
			res = _index.GetObject("myID", new String[] { "name" });
			Assert.Equal(null, res["population"]);
			Assert.Equal("San Francisco", res["name"].ToString());
			Assert.Equal("myID", res["objectID"].ToString());
		}

		[Fact]
		public void TaskDeleteObject()
		{
			ClearTest();
			// Add one object to be sure the test will not fail because index is empty
			var res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
			_index.WaitTask(res["taskID"].ToString());
			res = _index.DeleteObject("myID");
			Assert.False(string.IsNullOrWhiteSpace(res["deletedAt"].ToString()));
		}

		[Fact]
		public void TaskBatch()
		{
			ClearTest();

			List<JObject> objs = new List<JObject>();
			objs.Add(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"));
			objs.Add(JObject.Parse(@"{""name"":""Los Angeles"", ""population"":3792621}"));
			var res = _index.AddObjects(objs);
			JArray objectIDs = (JArray)res["objectIDs"];
			Assert.Equal(objectIDs.Count, 2);
			List<JObject> objs2 = new List<JObject>();
			objs2.Add(JObject.Parse(@"{""name"":""San Francisco"", 
                          ""population"": 805235,
                          ""objectID"":""SFO""}"));
			objs2.Add(JObject.Parse(@"{""name"":""Los Angeles"", 
                          ""population"": 3792621,
                          ""objectID"": ""LA""}"));
			res = _index.SaveObjects(objs2);
			objectIDs = (JArray)res["objectIDs"];
			Assert.Equal(objectIDs.Count, 2);
		}

		[Fact]
		public void TestListIndexes()
		{
			ClearTest();
			try
			{
				var result = _client.ListIndexes();
				Assert.False(string.IsNullOrWhiteSpace(result["items"].ToString()));
			}
			catch (ArgumentOutOfRangeException)
			{
			}
		}

		[Fact]
		public void TaskACLClient()
		{
			ClearTest();
			// Add one object to be sure the test will not fail because index is empty
			var res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
			_index.WaitTask(res["taskID"].ToString());

			var key = _client.AddUserKey(new String[] { "search" });
			System.Threading.Thread.Sleep(5000);
			Assert.False(string.IsNullOrWhiteSpace(key["key"].ToString()));

			key = _client.AddApiKey(new String[] { "search" });
			System.Threading.Thread.Sleep(5000);
			Assert.False(string.IsNullOrWhiteSpace(key["key"].ToString()));

			var getKey = _client.GetUserKeyACL(key["key"].ToString());
			Assert.Equal(key["key"], getKey["value"]);

			getKey = _client.GetApiKey(key["key"].ToString());
			Assert.Equal(key["key"], getKey["value"]);

			var keys = _client.ListUserKeys();
			Assert.True(Include((JArray)keys["keys"], "value", key["key"].ToString()));

			keys = _client.ListApiKeys();
			Assert.True(Include((JArray)keys["keys"], "value", key["key"].ToString()));

			_client.UpdateApiKey(key["key"].ToString(), new String[] { "addObject" });
			System.Threading.Thread.Sleep(5000);

			getKey = _client.GetUserKeyACL(key["key"].ToString());
			Assert.Equal((string)getKey["acl"][0], "addObject");

			getKey = _client.GetApiKey(key["key"].ToString());
			Assert.Equal((string)getKey["acl"][0], "addObject");

			_client.DeleteApiKey(key["key"].ToString());
			System.Threading.Thread.Sleep(5000);

			keys = _client.ListApiKeys();
			Assert.False(Include((JArray)keys["keys"], "value", key["key"].ToString()));
		}

		[Fact]
		public void TaskACLIndex()
		{
			ClearTest();
			// Add one object to be sure the test will not fail because index is empty
			var res = _index.AddObject(JObject.Parse(@"{""name"":""San Francisco"", ""population"":805235}"), "myID");
			_index.WaitTask(res["taskID"].ToString());

			var key = _index.AddUserKey(new String[] { "search" });
			WaitKey(_index, key);
			Assert.False(string.IsNullOrWhiteSpace(key["key"].ToString()));

			key = _index.AddApiKey(new String[] { "search" });
			WaitKey(_index, key);
			Assert.False(string.IsNullOrWhiteSpace(key["key"].ToString()));

			var getKey = _index.GetUserKeyACL(key["key"].ToString());
			Assert.Equal(key["key"], getKey["value"]);

			getKey = _index.GetApiKey(key["key"].ToString());
			Assert.Equal(key["key"], getKey["value"]);

			var keys = _index.ListUserKeys();
			Assert.True(Include((JArray)keys["keys"], "value", key["key"].ToString()));

			keys = _index.ListApiKeys();
			Assert.True(Include((JArray)keys["keys"], "value", key["key"].ToString()));

			_index.UpdateApiKey(key["key"].ToString(), new String[] { "addObject" });
			WaitKey(_index, key, "addObject");

			getKey = _index.GetUserKeyACL(key["key"].ToString());
			Assert.Equal((string)getKey["acl"][0], "addObject");

			getKey = _index.GetApiKey(key["key"].ToString());
			Assert.Equal((string)getKey["acl"][0], "addObject");

			_index.DeleteApiKey(key["key"].ToString());

			WaitKeyMissing(_index, key);
			keys = _index.ListUserKeys();
			Assert.False(Include((JArray)keys["keys"], "value", key["key"].ToString()));
		}

		[Fact]
		public void BadIndexName()
		{
			var ind = _client.InitIndex("&&");
			try
			{
				Assert.Throws<AlgoliaException>(() => ind.ClearIndex());
			}
			catch (Exception)
			{

			}
		}

		[Fact]
		public void NetworkIssue()
		{
			Assert.Throws<AlgoliaException>(() => new AlgoliaClient(_testApiKey, _testApiKey).ListIndexes());
		}

		[Fact]
		public void BadClientCreation()
		{
			string[] _hosts = new string[] { "localhost.algolia.com:8080", "" };
			try
			{
				Assert.Throws<AlgoliaException>(() => new AlgoliaClient("", _testApiKey));
			}
			catch (Exception)
			{ }
			try
			{
				Assert.Throws<AlgoliaException>(() => new AlgoliaClient(_testApplicationID, ""));
			}
			catch (Exception)
			{ }
			try
			{
				Assert.Throws<AlgoliaException>(() => new AlgoliaClient(_testApplicationID, "", _hosts));
			}
			catch (Exception)
			{ }
			try
			{
				Assert.Throws<AlgoliaException>(() => new AlgoliaClient("", _testApiKey, _hosts));
			}
			catch (Exception)
			{ }
			try
			{
				Assert.Throws<AlgoliaException>(() => new AlgoliaClient(_testApplicationID, _testApiKey, null));
			}
			catch (Exception)
			{ }
			try
			{

				var badClient = new AlgoliaClient(_testApplicationID, _testApiKey, _hosts);
				Assert.Throws<AlgoliaException>(() => badClient.ListIndexes());
			}
			catch (Exception)
			{ }
		}

		[Fact]
		public void TestBigQueryAll()
		{
			ClearTest();
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
			query.IgnorePlural(new IgnorePluralList { Ignored = "af,ar" });
			query.SetMinWordSizeToAllowOneTypo(1);
			query.SetMinWordSizeToAllowTwoTypos(2);
			query.AddInsidePolygon(0.01F, 0.1F);
			query.AddInsidePolygon(0.02F, 0.4F);
			query.EnableDistinct(true);
			query.SetRemoveWordsIfNoResult(Query.RemoveWordsIfNoResult.FIRST_WORDS);
			query.GetRankingInfo(true);
			query.EnableTyposOnNumericTokens(false);
			query.SetOffset(0);
			query.SetLength(1);
			query.SetAttributesToRetrieve(attr);
			query.SetAttributesToSnippet(attr);
			query.SetFieldsToRetrieve(new string[] { "hits", "nbHits" });
			query.InsideBoundingBox(0, 0, 90, 90);
			query.AroundLatitudeLongitude(0, 0, 2000000000);
			string[] facetFilter = { "_tags:people" };
			string[] facets = { "_tags" };
			query.SetFacetFilters(facetFilter);
			query.SetFacets(facets);
			query.SetTagFilters("people");
			query.SetNumericFilters("Age>=42");
			query.SetQueryType(Query.QueryType.PREFIX_ALL);
			query.SetSortFacetValuesBy(Query.SortFacetValuesBy.ALPHA);
			query.AddCustomParameter("facets", "_tags");
			var res = _index.Search(query);
			Assert.Equal(1, res["nbHits"].ToObject<int>());
			Assert.Equal(null, res["query"]);
			Assert.Equal("Jimmie J", res["hits"][0]["firstname"].ToString());
			_client.DeleteIndex(GetSafeName("àlgol?à-csharp"));
		}

		[Fact]
		public void TestBigQueryNone()
		{
			Console.WriteLine(GetSafeName("àlgol?à-csharp"));
			ClearTest();
			_index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie J""
                , ""Age"":42, ""lastname"":""Barninger"", ""_tags"": ""people""
                , ""_geoloc"":{""lat"":0.853409, ""lng"":0.348800}}"));
			var task = _index.SetSettings(JObject.Parse(@"{""attributesForFaceting"": [""_tags""]}"));
			_index.WaitTask(task["taskID"].ToString());
			Query query = new Query("Jimmie");
			query.SetPage(0);
			query.SetOptionalWords("J");
			query.SetNbHitsPerPage(1);
			query.EnablePercentileComputation(true);
			query.SetAroundRadius(new AllRadiusInt());
			string[] attr = { "firstname" };
			query.SetAttributesToHighlight(attr);
			query.SetOffset(0);
			query.SetLength(1);
			query.SetMinWordSizeToAllowOneTypo(1);
			query.SetMinWordSizeToAllowTwoTypos(2);
			query.SetAnalyticsTags(new string[] { "tagIt" });
			query.EnableDistinct(true);
			query.EnableAnalytics(true);
			query.EnableAdvancedSyntax(true);
			query.GetRankingInfo(true);
			query.EnableTypoTolerance(false);
			query.SetAttributesToRetrieve(attr);
			query.SetAttributesToSnippet(attr);
			query.SetMaxValuesPerFacets(1);
			query.SetMaxFacetHits(1);
			query.InsideBoundingBox(0, 0, 90, 90);
			query.AroundLatitudeLongitude(0, 0, 2000000000, 100);
			string[] facetFilter = { "_tags:people" };
			string[] facets = { "_tags" };
			query.SetFacetFilters(facetFilter);
			query.SetFacets(facets);
			query.SetTagFilters("people");
			query.SetNumericFilters("Age>=42");
			query.SetQueryType(Query.QueryType.PREFIX_NONE);
			query.SetSortFacetValuesBy(Query.SortFacetValuesBy.ALPHA);
			query.SetRemoveWordsIfNoResult(Query.RemoveWordsIfNoResult.LAST_WORDS);
			var res = _index.Search(query);
			Assert.Equal(1, res["nbHits"].ToObject<int>());
			Assert.Equal("Jimmie J", res["hits"][0]["firstname"].ToString());
			_client.DeleteIndex(GetSafeName("àlgol?à-csharp"));
		}

		[Fact]
		public void TestMultipleQueries()
		{
			ClearTest();
			var task = _index.AddObject(JObject.Parse(@"{""firstname"":""Jimmie"", ""lastname"":""Barninger""}"));
			_index.WaitTask(task["taskID"].ToString());
			var indexQuery = new List<IndexQuery>();
			indexQuery.Add(new IndexQuery(GetSafeName("àlgol?à-csharp"), new Query("")));
			var res = _client.MultipleQueries(indexQuery);
			Assert.Equal(1, res["results"].ToObject<JArray>().Count);
			Assert.Equal(1, res["results"][0]["hits"].ToObject<JArray>().Count);
			Assert.Equal("Jimmie", res["results"][0]["hits"][0]["firstname"].ToString());
			_client.DeleteIndex(GetSafeName("àlgol?à-csharp"));
		}

		[Fact]
		public void TestFacets()
		{
			ClearTest();
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
			Assert.Equal("1", res["nbHits"].ToString());
			Assert.Equal("Hotel D", res["hits"][0]["name"].ToString());
			res = _index.Search(new Query().SetFacetFilters("[\"stars:****\",\"city:Paris\"]").SetFacets(new String[] { "stars" }));
			Assert.Equal("1", res["nbHits"].ToString());
			Assert.Equal("Hotel D", res["hits"][0]["name"].ToString());
			res = _index.Search(new Query().SetFacetFilters(JArray.Parse(@"[""stars:****"",""city:Paris""]")).SetFacets(new String[] { "stars" }));
			Assert.Equal("1", res["nbHits"].ToString());
			Assert.Equal("Hotel D", res["hits"][0]["name"].ToString());
			res = _index.Search(new Query().SetFilters("stars:**** AND city:Paris").SetFacets(new String[] { "stars" }));
			Assert.Equal("1", res["nbHits"].ToString());
			Assert.Equal("Hotel D", res["hits"][0]["name"].ToString());
		}

		[Fact]
		public void TestGenerateSecuredApiKey()
		{
			Assert.Equal("ZmI5YjQ5N2U3YjFkYjcxYTQ2YjE4OWFmNWUxNmVlNmVlNDkzNzYyYTFlYmE5NThhNjhhN2I5ZjhhN2NkYWNmMnRhZ0ZpbHRlcnM9KHB1YmxpYyUyQ3VzZXIxKQ==", _client.GenerateSecuredApiKey("182634d8894831d5dbce3b3185c50881", "(public,user1)"));
			Assert.Equal("ZmI5YjQ5N2U3YjFkYjcxYTQ2YjE4OWFmNWUxNmVlNmVlNDkzNzYyYTFlYmE5NThhNjhhN2I5ZjhhN2NkYWNmMnRhZ0ZpbHRlcnM9KHB1YmxpYyUyQ3VzZXIxKQ==", _client.GenerateSecuredApiKey("182634d8894831d5dbce3b3185c50881", new Query().SetTagFilters("(public,user1)")));
			Assert.Equal("MjgzZDFkNjliM2UwNGQ1MTBiODY0MTAwZjAyNjgxN2MzZmVhNTBkY2JkMzE5ODRhNmVjNzE0MGVlOTE0ZjVmZXRhZ0ZpbHRlcnM9KHB1YmxpYyUyQ3VzZXIxKSZ1c2VyVG9rZW49NDI=", _client.GenerateSecuredApiKey("182634d8894831d5dbce3b3185c50881", new Query().SetTagFilters("(public,user1)").SetUserToken("42")));
		}

		[Fact]
		public void TestDisjunctiveFaceting()
		{
			ClearTest();
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
			Assert.Equal(5, answer["nbHits"].ToObject<int>());
			Assert.Equal(1, answer["facets"].ToObject<JObject>().Count);
			Assert.Equal(2, answer["disjunctiveFacets"].ToObject<JObject>().Count);

			refinements.Add("stars", new string[] { "*" });
			answer = _index.SearchDisjunctiveFaceting(new Query("h").SetFacets(new string[] { "city" }), new string[] { "stars", "facilities" }, refinements);
			Assert.Equal(2, answer["nbHits"].ToObject<int>());
			Assert.Equal(1, answer["facets"].ToObject<JObject>().Count);
			Assert.Equal(2, answer["disjunctiveFacets"].ToObject<JObject>().Count);
			Assert.Equal(2, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["*"].ToObject<int>());
			Assert.Equal(1, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["**"].ToObject<int>());
			Assert.Equal(2, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["****"].ToObject<int>());

			refinements.Clear();
			refinements.Add("stars", new string[] { "*" });
			refinements.Add("city", new string[] { "Paris" });
			answer = _index.SearchDisjunctiveFaceting(new Query("h").SetFacets(new string[] { "city" }), new string[] { "stars", "facilities" }, refinements);
			Assert.Equal(2, answer["nbHits"].ToObject<int>());
			Assert.Equal(1, answer["facets"].ToObject<JObject>().Count);
			Assert.Equal(2, answer["disjunctiveFacets"].ToObject<JObject>().Count);
			Assert.Equal(2, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["*"].ToObject<int>());
			Assert.Equal(1, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["****"].ToObject<int>());

			refinements.Clear();
			refinements.Add("stars", new string[] { "*", "****" });
			refinements.Add("city", new string[] { "Paris" });
			answer = _index.SearchDisjunctiveFaceting(new Query("h").SetFacets(new string[] { "city" }), new string[] { "stars", "facilities" }, refinements);
			Assert.Equal(3, answer["nbHits"].ToObject<int>());
			Assert.Equal(1, answer["facets"].ToObject<JObject>().Count);
			Assert.Equal(2, answer["disjunctiveFacets"].ToObject<JObject>().Count);
			Assert.Equal(2, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["*"].ToObject<int>());
			Assert.Equal(1, answer["disjunctiveFacets"].ToObject<JObject>()["stars"].ToObject<JObject>()["****"].ToObject<int>());
		}

		[Fact]
		public void TestCancellationToken()
		{
			CancellationTokenSource ct = new CancellationTokenSource();
			Task<JObject> task = _client.ListIndexesAsync(ct.Token);
			ct.Cancel();
			try
			{
				Assert.Throws<TaskCanceledException>(() => task.GetAwaiter().GetResult());
			}
			catch (TaskCanceledException)
			{
				// Pass
			}
		}

		[Fact]
		public void TestTimeoutHandling()
		{
			_client.setTimeout(0.001, 0.001);
			try
			{
				_client.ListIndexes();
				_client = new AlgoliaClient(_testApplicationID, _testApiKey);
				Assert.Throws<Exception>(() =>
				{
					_index = _client.InitIndex(GetSafeName("àlgol?à-csharp"));
				});
			}
			catch (AlgoliaException)
			{
				// Reset 
				_client = new AlgoliaClient(_testApplicationID, _testApiKey);
				_index = _client.InitIndex(GetSafeName("àlgol?à-csharp"));
			}
			catch (OperationCanceledException)
			{
				_client = new AlgoliaClient(_testApplicationID, _testApiKey);
				_index = _client.InitIndex(GetSafeName("àlgol?à-csharp"));
				Assert.Throws<AlgoliaException>(() =>
				{
					_index = _client.InitIndex(GetSafeName("àlgol?à-csharp"));
				});
			}
		}

		[Fact]
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
			_client.setTimeout(1, 1);
			var startTime = DateTime.Now;
			var index = _client.ListIndexes();
			Assert.True((DateTime.Now - startTime).TotalSeconds < 2);
			Assert.True(index != null);
		}

		private void WaitKey(Index index, JObject newIndexKey, string updatedACL = null)
		{
			var isUpdate = !string.IsNullOrEmpty(updatedACL);
			for (var i = 0; i <= 10; i++)
			{
				try
				{
					var key = index.GetApiKey(newIndexKey["key"].ToString());
					if (isUpdate && key["acl"][0].ToString() != updatedACL)
					{
						throw new Exception();
					}
					return;
				}
				catch (Exception)
				{
					Thread.Sleep(1000);
					continue;
				}
			}
		}

		private void WaitKeyMissing(Index index, JObject newIndexKey)
		{
			for (var i = 0; i <= 10; i++)
			{
				try
				{
					var key = index.GetApiKey(newIndexKey["key"].ToString());
					Thread.Sleep(1000);
				}
				catch (Exception)
				{
					return;
				}
			}
		}

		[Fact]
		public void TestSynonyms()
		{
			ClearTest();
			var res = _index.AddObject(JObject.Parse(@"{""name"":""589 Howard St., San Francisco""}"));
			res = _index.BatchSynonyms(JArray.Parse(@"[{""objectID"":""city"", ""type"": ""synonym"", ""synonyms"":[""San Francisco"", ""SF""]}, {""objectID"":""street"", ""type"":""altCorrection1"", ""word"":""Street"", ""corrections"":[""St""]}]"));
			_index.WaitTask(res["taskID"].ToString());
			res = _index.GetSynonym("city");
			Assert.Equal("city", res["objectID"].ToObject<string>());
			res = _index.Search(new Query("Howard Street SF"));
			Assert.Equal(1, res["nbHits"].ToObject<int>());
			res = _index.DeleteSynonym("street");
			_index.WaitTask(res["taskID"].ToString());
			res = _index.SearchSynonyms("", new Index.SynonymType[] { Index.SynonymType.SYNONYM }, 0, 5);
			Assert.Equal(1, res["nbHits"].ToObject<int>());
			res = _index.ClearSynonyms();
			_index.WaitTask(res["taskID"].ToString());
			res = _index.SearchSynonyms("", new Index.SynonymType[] { }, 0, 5);
			Assert.Equal(0, res["nbHits"].ToObject<int>());
		}

		[Fact]
		public void SearchForFacets()
		{
			ClearTest();
			_index.SetSettings(JObject.Parse(@"{""attributesForFacetting"":[""searchable(city)"", ""facilities""]}"));
			JObject task = _index.AddObjects(new JObject[] {
				JObject.Parse(@"{""name"": ""Hotel A"", ""stars"":""*"", ""facilities"":[""wifi"", ""bath"", ""spa""], ""city"": ""Paris"", ""rooms"": 10}"),
				JObject.Parse(@"{""name"": ""Hotel B"", ""stars"":""*"", ""facilities"":[""bath""], ""city"": ""Paris"", ""rooms"": 50}"),
				JObject.Parse(@"{""name"": ""Hotel C"", ""stars"":""**"", ""facilities"":[""bath""], ""city"": ""San Francisco"", ""rooms"": 3}"),
				JObject.Parse(@"{""name"": ""Hotel D"", ""stars"":""****"", ""facilities"":[""wifi""], ""city"": ""Paris"", ""rooms"": 300}"),
				JObject.Parse(@"{""name"": ""Hotel E"", ""stars"":""****"", ""facilities"":[""spa""], ""city"": ""New York"", ""rooms"": 125}")
			});
			_index.WaitTask((task["taskID"].ToString()));
			var res = _index.SearchForFacetValues("city", "pari");
			Assert.Equal(1, res["facetHits"].ToObject<JArray>().Count);

			string[] facetFilter = { "facilities:wifi" };
			var query = new Query().SetFacetFilters(facetFilter).SetNumericFilters("rooms>200");
			res = _index.SearchForFacetValues("city", "pari", query);
			Assert.Equal(1, res["facetHits"].ToObject<JArray>().Count);
		}

		[Fact]
		public void TestRetryStrategy_Build()
		{
			var applicationId = "test";
			var mockHttp = new MockHttpMessageHandler();
			var hosts = new string[] { applicationId + "-1.algolianet.com", applicationId + "-2.algolianet.com" };

			mockHttp.When(HttpMethod.Get, "https://" + hosts[0] + "/1/indexes/").Respond(HttpStatusCode.RequestTimeout);
			mockHttp.When(HttpMethod.Get, "https://" + hosts[0] + "/1/indexes/test/settings").Respond("application/json", "{\"fromFirstHost\":[]}");
			mockHttp.When(HttpMethod.Get, "https://" + hosts[0] + "/1/indexes/test/browse").Respond("application/json", "{\"fromFirstHost\":[]}");

			mockHttp.When(HttpMethod.Get, "https://" + hosts[1] + "/1/indexes/").Respond("application/json", "{\"fromSecondHost\":[]}");
			mockHttp.When(HttpMethod.Get, "https://" + hosts[1] + "/1/indexes/test/settings").Respond("application/json", "{\"fromSecondHost\":[]}");
			mockHttp.When(HttpMethod.Get, "https://" + hosts[1] + "/1/indexes/test/browse").Respond("application/json", "{\"fromSecondHost\":[]}");

			var client = new AlgoliaClient("test", "test", hosts, mockHttp);
			client._dsnInternalTimeout = 2;
			Assert.Equal(JObject.Parse("{\"fromSecondHost\":[]}").ToString(), client.ListIndexes().ToString());

			//first host back up again but no retry because lastModified < _dsnInternalTimeout, stick with second host
			Assert.Equal(JObject.Parse("{\"fromSecondHost\":[]}").ToString(), client.InitIndex("test").GetSettings().ToString());
			Thread.Sleep(10000);
			//lastModified > _dsnInternalTimeout, retry on first host
			Assert.Equal(JObject.Parse("{\"fromFirstHost\":[]}").ToString(), client.InitIndex("test").Browse().ToString());
		}

		[Fact]
		public void TestRequestOptions()
		{
			ClearTest();

			var attributesToRetrieve = new List<string> { "firstname" };
			RequestOptions requestOptions = new RequestOptions();
			requestOptions.SetForwardedFor("ForwardedFor");
			requestOptions.AddExtraHeader("Header", "headerValue");
			requestOptions.AddExtraQueryParameters("ExtraQueryParamKey", "ExtraQueryParamValue");

			// A Request without url parameters 
			var task = _index.AddObject(JObject.Parse(@"{""firstname"":""bob"", ""lastname"":""snow"", ""objectID"":""ananas""}"), requestOptions, null);
			_index.WaitTask(task["taskID"].ToString());
			// A request with url parameters
			var res = _index.GetObject("ananas", requestOptions, attributesToRetrieve);

			Assert.Equal("ananas", res["objectID"].ToString());
			Assert.Equal("bob", res["firstname"].ToString());
		}

		// Query Rule tests

		private JObject generateRuleStub(string objectId = "my-rule")
		{
			JObject condition = new JObject();
			condition.Add("pattern", "some text");
			condition.Add("anchoring", "is");

			JObject consequence = new JObject();
			JObject parameters = new JObject();
			parameters.Add("query", "other text");
			consequence.Add("params", parameters);

			JObject rule = new JObject();
			rule.Add("objectID", objectId);
			rule.Add("condition", condition);
			rule.Add("consequence", consequence);

			return rule;
		}

		private static void AreEqualByJson(JObject expected, JObject actual)
		{
			var expectedJson = JsonConvert.SerializeObject(expected);
			var actualJson = JsonConvert.SerializeObject(actual);
			Assert.Equal(expectedJson, actualJson);
		}

		[Fact]
		public void TestSaveAndGetRule()
		{
			ClearTest();

			string ruleId = "ruleID";
			JObject rule = generateRuleStub(ruleId);
			var task = _index.SaveRule(rule);
			_index.WaitTask(task["taskID"].ToString());
			AreEqualByJson(rule, _index.GetRule(ruleId));
		}

		[Fact]
		public void TestDeleteRule()
		{
			ClearTest();

			string ruleId = "ruleID";
			JObject rule = generateRuleStub(ruleId);

			var task = _index.SaveRule(rule);
			_index.WaitTask(task["taskID"].ToString());

			var task2 = _index.DeleteRule(ruleId);
			_index.WaitTask(task2["taskID"].ToString());

			try
			{
				_index.GetRule(ruleId);
			}
			catch (AlgoliaException)
			{
				return;
			}
			Assert.True(false);
		}

		[Fact]
		public void TestSearchRules()
		{
			ClearTest();

			string ruleId1 = "ruleID1";
			string ruleId2 = "ruleID2";
			JObject rule1 = generateRuleStub(ruleId1);
			JObject rule2 = generateRuleStub(ruleId2);
			var task1 = _index.SaveRule(rule1);
			_index.WaitTask(task1["taskID"].ToString());
			var task2 = _index.SaveRule(rule2);
			_index.WaitTask(task2["taskID"].ToString());

			var rules = _index.SearchRules();
			Assert.Equal(2, (int)rules["nbHits"]);
		}

		[Fact]
		public void TestClearRules()
		{
			ClearTest();

			string ruleId = "ruleID3";
			JObject rule = generateRuleStub(ruleId);
			var task = _index.SaveRule(rule);
			_index.WaitTask(task["taskID"].ToString());
			var task2 = _index.ClearRules();
			_index.WaitTask(task2["taskID"].ToString());

			var rules = _index.SearchRules();

			Assert.Equal(rules["nbHits"], 0);
		}

		[Fact]
		public void TestBatchRules()
		{
			ClearTest();

			string ruleId1 = "ruleID4";
			string ruleId2 = "ruleID5";
			JObject rule1 = generateRuleStub(ruleId1);
			JObject rule2 = generateRuleStub(ruleId2);
			var task = _index.BatchRules(new List<JObject>() { rule1, rule2 });
			_index.WaitTask(task["taskID"].ToString());

			var rules = _index.SearchRules();
			Assert.Equal(2, (int)rules["nbHits"]);
		}

		[Fact]
		public void TestExtractRules_ContainsManyPages()
		{
			ClearTest();
			var rulesToPush = new List<JObject>();
			for (int i = 0; i < 10; i++)
			{
				rulesToPush.Add(generateRuleStub("id_" + i));
			}
			var task = _index.BatchRules(rulesToPush);
			_index.WaitTask(task["taskID"].ToString());

			var rulesIterator = new RulesIterator(_index, 3);

			var rulesFetched = rulesIterator.ToList();
			Assert.Equal(10, rulesFetched.Count);
			Assert.Contains("id_", rulesFetched[0]["objectID"].ToObject<String>());
			Assert.Null(rulesFetched[0]["_highlightResult"]);
		}

		[Fact]
		public void TestExtractRules_ContainsOnePage()
		{
			ClearTest();
			var rulesToPush = new List<JObject>();
			for (int i = 0; i < 10; i++)
			{
				rulesToPush.Add(generateRuleStub("id_" + i));
			}
			var task = _index.BatchRules(rulesToPush);
			_index.WaitTask(task["taskID"].ToString());

			var rulesIterator = new RulesIterator(_index, 1000);

			var rulesFetched = rulesIterator.ToList();
			Assert.Equal(10, rulesFetched.Count);
			Assert.Contains("id_", rulesFetched[0]["objectID"].ToObject<String>());
			Assert.Null(rulesFetched[0]["_highlightResult"]);
		}

		[Fact]
		public void TestExtractRules_NoRules()
		{
			ClearTest();

			var rulesIterator = new RulesIterator(_index, 3);
			var rulesFetched = rulesIterator.ToList();
			Assert.Equal(0, rulesFetched.Count);
		}

		[Fact]
		public void TestExtractSynonyms_ContainsManyPages()
		{
			ClearTest();
			var synonymsToPush = new List<JObject>();
			for (int i = 0; i < 10; i++)
			{
				var id = "synonymid_" + i;
				var synonym = JObject.Parse(
						@"{
                            ""objectID"":""" + id + @""",
                            ""type"": ""synonym"",
                            ""synonyms"": [
                            ""car"",
                            ""vehicle"",
                            ""auto""
                            ]
                        }"
					);
				synonymsToPush.Add(synonym);
			}
			var task = _index.BatchSynonyms(synonymsToPush);
			_index.WaitTask(task["taskID"].ToString());

			var synonymsIterator = new SynonymsIterator(_index, 3);

			var synonymsFetched = synonymsIterator.ToList();
			Assert.Equal(10, synonymsFetched.Count);
			Assert.Contains("synonymid_", synonymsFetched[0]["objectID"].ToObject<String>());
			Assert.Null(synonymsFetched[0]["_highlightResult"]);
		}

		[Fact]
		public void TestExtractSynonyms_ContainsOnePage()
		{
			ClearTest();
			var synonymsToPush = new List<JObject>();
			for (int i = 0; i < 10; i++)
			{
				var id = "synonymid_" + i;
				var synonym = JObject.Parse(
						@"{
                            ""objectID"":""" + id + @""",
                            ""type"": ""synonym"",
                            ""synonyms"": [
                            ""car"",
                            ""vehicle"",
                            ""auto""
                            ]
                        }"
					);
				synonymsToPush.Add(synonym);
			}
			var task = _index.BatchSynonyms(synonymsToPush);
			_index.WaitTask(task["taskID"].ToString());

			var synonymsIterator = new SynonymsIterator(_index, 1000);

			var synonymsFetched = synonymsIterator.ToList();
			Assert.Equal(10, synonymsFetched.Count);
			Assert.Contains("synonymid_", synonymsFetched[0]["objectID"].ToObject<String>());
			Assert.Null(synonymsFetched[0]["_highlightResult"]);
		}

		[Fact]
		public void TestExtractSynonyms_NoRules()
		{
			ClearTest();

			var synonymIterator = new SynonymsIterator(_index, 3);
			var synonymFetched = synonymIterator.ToList();
			Assert.Equal(0, synonymFetched.Count);
		}

		// MCM tests

		[Fact]
		public void TestListClusters()
		{
			var answer = _clientMCM.ListClusters();

			Assert.True(answer["clusters"] != null);
			Assert.True(((JArray)answer["clusters"]).Count > 0);
			Assert.True(answer["clusters"][0]["clusterName"] != null);
			Assert.True(answer["clusters"][0]["nbRecords"] != null);
			Assert.True(answer["clusters"][0]["nbUserIDs"] != null);
			Assert.True(answer["clusters"][0]["dataSize"] != null);
		}

		[Fact]
		public void TestAssignUserID()
		{
			var clusters = _clientMCM.ListClusters();
			var clusterName = (string) (clusters["clusters"][0]["clusterName"]);
			var answer = _clientMCM.AssignUserID(_userID, clusterName);

			Assert.True(answer["createdAt"] != null);
			Thread.Sleep(5000); // Sleep to let the cluster publish the change
		}

		[Fact]
		public void TestListUserIds()
		{
			var answer = _clientMCM.ListUserIDs();

			Assert.True(answer["userIDs"] != null);
			Assert.True(((JArray)answer["userIDs"]).Count > 0);
			Assert.True(answer["userIDs"][0]["userID"] != null);
			Assert.True(answer["userIDs"][0]["clusterName"] != null);
			Assert.True(answer["userIDs"][0]["nbRecords"] != null);
			Assert.True(answer["userIDs"][0]["dataSize"] != null);
		}

		[Fact]
		public void TestGetTopUserID()
		{
			var clusters = _clientMCM.ListClusters();
			var clusterName = (string)(clusters["clusters"][0]["clusterName"]);
			var answer = _clientMCM.GetTopUserID();

			Assert.True(answer["topUsers"] != null);
			Assert.True(((JArray)answer["topUsers"][clusterName]).Count > 0);
			Assert.True(answer["topUsers"][clusterName][0]["userID"] != null);
			Assert.True(answer["topUsers"][clusterName][0]["nbRecords"] != null);
			Assert.True(answer["topUsers"][clusterName][0]["dataSize"] != null);
		}

		[Fact]
		public void TestGetUserID()
		{
			var answer = _clientMCM.GetUserID(_userID);

			Assert.True(answer["userID"] != null);
			Assert.True(answer["clusterName"] != null);
			Assert.True(answer["nbRecords"] != null);
			Assert.True(answer["dataSize"] != null);
		}

		[Fact]
		public void TestSearchUserIDs()
		{
			var clusters = _clientMCM.ListClusters();
			var clusterName = (string)(clusters["clusters"][0]["clusterName"]);
			var answer = _clientMCM.SearchUserIds(_userID, clusterName, 0, 1000);

			Assert.True(answer["hits"] != null);
			Assert.True(answer["nbHits"] != null);
			Assert.True(answer["page"] != null);
			Assert.True(answer["hitsPerPage"] != null);
			Assert.True(answer["hits"][0]["userID"] != null);
			Assert.True(answer["hits"][0]["clusterName"] != null);
			Assert.True(answer["hits"][0]["nbRecords"] != null);
			Assert.True(answer["hits"][0]["dataSize"] != null);
		}

		[Fact]
		public void TestRemoveUserID()
		{
			var answer = _clientMCM.RemoveUserID(_userID);

			Assert.True(answer["deletedAt"] != null);
		}

	}
}
#pragma warning restore 0618
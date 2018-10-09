﻿using System;
using System.Collections.Generic;

namespace Algolia.Search.Test
{
    public class BaseTest
    {
        public static string _testApplicationID = "";
        public static string _testApiKey = "";

        public AlgoliaClient _client;

        public Index _index;
        public Analytics _analytics;
        public IndexHelper<TestModel> _indexHelper;

        // MCM specific
        public static string _testApplicationIDMCM = "";
        public static string _testApiKeyMCM = "";
        public AlgoliaClient _clientMCM;
        public string _userID;

        public BaseTest()
        {
            _testApiKey = Environment.GetEnvironmentVariable("ALGOLIA_API_KEY");
            _testApplicationID = Environment.GetEnvironmentVariable("ALGOLIA_APPLICATION_ID");
            _client = new AlgoliaClient(_testApplicationID, _testApiKey);
            _index = _client.InitIndex(GetSafeName("àlgol?à-csharp"));
            _analytics = new Analytics(_client);
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("àlgol?à-csharp"));

            _testApiKeyMCM = Environment.GetEnvironmentVariable("ALGOLIA_API_KEY_MCM");
            _testApplicationIDMCM = Environment.GetEnvironmentVariable("ALGOLIA_APPLICATION_ID_MCM");
            _clientMCM = new AlgoliaClient(_testApplicationIDMCM, _testApiKeyMCM);
            _userID = GetUniqueUserID("csharp-client");
        }

        public static string GetSafeName(string name)
        {
            if (Environment.GetEnvironmentVariable("APPVEYOR") == null)
            {
                return name;
            }
            //String[] id = Environment.GetEnvironmentVariable("TRAVIS_JOB_NUMBER").Split('.');
            return name + "appveyor-" + Environment.GetEnvironmentVariable("APPVEYOR_BUILD_NUMBER");
        }

        public static string GetUniqueUserID(string name)
        {
            return GetSafeName(name);
        }

        public void ClearTest()
        {
            try
            {
                _index.ClearIndex();
                var clearTask = _index.ClearRules();
                _index.WaitTask(clearTask["taskID"].ToString());
                var taskSynonym = _index.ClearSynonyms();
                _index.WaitTask(taskSynonym["taskID"].ToString());
            }
            catch (Exception)
            {
                // Index not found
            }
        }

        public void TestCleanup()
        {
            _client.DeleteIndex(GetSafeName("àlgol?à-csharp"));
            _client = null;

        }

        public TestModel BuildTestModel()
        {
            return new TestModel() { Id = 5, TestModelId = 10, FirstName = "Scott", LastName = "Smith" };
        }

        public List<TestModel> BuildTestModelList()
        {
            var list = new List<TestModel>();

            list.Add(new TestModel() { Id = 1, TestModelId = 5, FirstName = "Nicolas", LastName = "Dessaigne" });
            list.Add(new TestModel() { Id = 2, TestModelId = 6, FirstName = "Julien", LastName = "Lemoine" });
            list.Add(new TestModel() { Id = 3, TestModelId = 7, FirstName = "Kevin", LastName = "Granger" });
            list.Add(new TestModel() { Id = 4, TestModelId = 8, FirstName = "Sylvain", LastName = "Utard" });

            return list;
        }

        public List<TestModel> BuildTestModelList(int count)
        {
            var list = new List<TestModel>();

            for (int i = 6; i < count + 6; i++)
            {
                list.Add(new TestModel() { Id = i, TestModelId = i + count, FirstName = "FirstName" + i, LastName = "LastName" + i });
            }

            return list;
        }
    }
    public class TestModel
    {
        public int Id { get; set; }
        public int TestModelId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

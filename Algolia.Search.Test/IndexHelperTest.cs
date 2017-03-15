using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Algolia.Search.Test
{
    [TestFixture]
    public class IndexHelperTest
    {
        private AlgoliaClient _client;
        private IndexHelper<TestModel> _indexHelper;

        public static string GetSafeName(string name)
        {
            if (Environment.GetEnvironmentVariable("APPVEYOR") == null)
            {
                return name;
            }
            return name + "appveyor-" + Environment.GetEnvironmentVariable("APPVEYOR_BUILD_NUMBER");
        }

        public void ClearTest()
        {
            try
            {
                _indexHelper.ClearIndex();
            }
            catch (Exception)
            {
                // Index not found
            }
        }

        [SetUp]
        public void TestInitialize()
        {
            var testApiKey = Environment.GetEnvironmentVariable("ALGOLIA_API_KEY") ?? "MY-ALGOLIA-API-KEY";
            var testApplicationID = Environment.GetEnvironmentVariable("ALGOLIA_APPLICATION_ID") ?? "MY-ALGOLIA-APPLICATION-ID";
            _client = new AlgoliaClient(testApplicationID, testApiKey);
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("algolia-csharp"));
        }

        [TearDown]
        public void TestCleanup()
        {
            _client.DeleteIndex(GetSafeName("algolia-csharp"));
            _client = null;

        }

        private TestModel BuildTestModel()
        {
            return new TestModel() { Id = 5, TestModelId = 10, FirstName = "Scott", LastName = "Smith" };
        }

        private List<TestModel> BuildTestModelList()
        {
            var list = new List<TestModel>();

            list.Add(new TestModel() { Id = 1, TestModelId = 5, FirstName = "Nicolas", LastName = "Dessaigne" });
            list.Add(new TestModel() { Id = 2, TestModelId = 6, FirstName = "Julien", LastName = "Lemoine" });
            list.Add(new TestModel() { Id = 3, TestModelId = 7, FirstName = "Kevin", LastName = "Granger" });
            list.Add(new TestModel() { Id = 4, TestModelId = 8, FirstName = "Sylvain", LastName = "Utard" });

            return list;
        }

        private List<TestModel> BuildTestModelList(int count)
        {
            var list = new List<TestModel>();

            for (int i = 6; i < count + 6; i++)
            {
                list.Add(new TestModel() { Id = i, TestModelId = i + count, FirstName = "FirstName" + i, LastName = "LastName" + i });
            }

            return list;
        }

        [Test]
        public void TestOverwriteIndex()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList();
            task = _indexHelper.OverwriteIndex(models);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(4, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("4", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestOverwriteIndexLarge()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList(8966);
            task = _indexHelper.OverwriteIndex(models);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(8966, res["nbHits"].ToObject<int>());
            Assert.AreEqual("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("999", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestOverwriteIndexLargeMax()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList(8966);
            task = _indexHelper.OverwriteIndex(models, 10000);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(8966, res["nbHits"].ToObject<int>());
            Assert.AreEqual("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("999", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestOverwriteIndexObjectId()
        {
            ClearTest();
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("algolia-csharp"), "TestModelId");

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList();
            task = _indexHelper.OverwriteIndex(models);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(4, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("8", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestSaveObjects()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList();
            var tasks = _indexHelper.SaveObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(5, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("5", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestSaveObjectsLarge()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList(8966);
            var tasks = _indexHelper.SaveObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(8967, res["nbHits"].ToObject<int>());
            Assert.AreEqual("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("999", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestSaveObjectsLargeMax()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList(8966);
            var tasks = _indexHelper.SaveObjects(models, 10000);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(8967, res["nbHits"].ToObject<int>());
            Assert.AreEqual("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("999", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestSaveObjectsObjectId()
        {
            ClearTest();
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("algolia-csharp"), "TestModelId");

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList();
            var tasks = _indexHelper.SaveObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(5, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("8", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestSaveObject()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("5", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestSaveObjectObjectId()
        {
            ClearTest();
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("algolia-csharp"), "TestModelId");

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("10", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestDeleteObjects()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList();
            var tasks = _indexHelper.SaveObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(5, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("5", res["hits"][0]["objectID"].ToString());

            tasks = _indexHelper.DeleteObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("5", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestDeleteObjectsLarge()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList(8966);
            var tasks = _indexHelper.SaveObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(8967, res["nbHits"].ToObject<int>());
            Assert.AreEqual("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("999", res["hits"][0]["objectID"].ToString());

            tasks = _indexHelper.DeleteObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("5", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestDeleteObjectsLargeMax()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList(8966);
            var tasks = _indexHelper.SaveObjects(models, 10000);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(8967, res["nbHits"].ToObject<int>());
            Assert.AreEqual("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("999", res["hits"][0]["objectID"].ToString());

            tasks = _indexHelper.DeleteObjects(models, 10000);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("5", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestDeleteObjectsObjectId()
        {
            ClearTest();
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("algolia-csharp"), "TestModelId");

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList();
            var tasks = _indexHelper.SaveObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(5, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("8", res["hits"][0]["objectID"].ToString());

            tasks = _indexHelper.DeleteObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(1, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("10", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestDeleteObject()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList();
            var tasks = _indexHelper.SaveObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(5, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("5", res["hits"][0]["objectID"].ToString());

            task = _indexHelper.DeleteObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(4, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("4", res["hits"][0]["objectID"].ToString());
        }

        [Test]
        public void TestDeleteObjectObjectId()
        {
            ClearTest();
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("algolia-csharp"), "TestModelId");

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList();
            var tasks = _indexHelper.SaveObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            var res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(5, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("8", res["hits"][0]["objectID"].ToString());

            task = _indexHelper.DeleteObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            res = _indexHelper.Search(new Query(""));

            Assert.AreEqual(4, res["nbHits"].ToObject<int>());
            Assert.AreEqual("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.AreEqual("8", res["hits"][0]["objectID"].ToString());
        }

        public class TestModel
        {
            public int Id { get; set; }
            public int TestModelId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}

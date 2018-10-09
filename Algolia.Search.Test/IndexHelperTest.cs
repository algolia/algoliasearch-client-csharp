using Xunit;

namespace Algolia.Search.Test
{
    public class IndexHelperTest : BaseTest
    {

        [Fact]
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

            Assert.Equal(4, res["nbHits"].ToObject<int>());
            Assert.Equal("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("4", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
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

            Assert.Equal(8966, res["nbHits"].ToObject<int>());
            Assert.Equal("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("999", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
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

            Assert.Equal(8966, res["nbHits"].ToObject<int>());
            Assert.Equal("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("999", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
        public void TestOverwriteIndexObjectId()
        {
            ClearTest();
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("àlgol?à-csharp"), "TestModelId");

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var models = BuildTestModelList();
            task = _indexHelper.OverwriteIndex(models);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var res = _indexHelper.Search(new Query(""));

            Assert.Equal(4, res["nbHits"].ToObject<int>());
            Assert.Equal("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("8", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
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

            Assert.Equal(5, res["nbHits"].ToObject<int>());
            Assert.Equal("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("5", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
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

            Assert.Equal(8967, res["nbHits"].ToObject<int>());
            Assert.Equal("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("999", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
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

            Assert.Equal(8967, res["nbHits"].ToObject<int>());
            Assert.Equal("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("999", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
        public void TestSaveObjectsObjectId()
        {
            ClearTest();
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("àlgol?à-csharp"), "TestModelId");

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

            Assert.Equal(5, res["nbHits"].ToObject<int>());
            Assert.Equal("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("8", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
        public void TestSaveObject()
        {
            ClearTest();

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var res = _indexHelper.Search(new Query(""));

            Assert.Equal(1, res["nbHits"].ToObject<int>());
            Assert.Equal("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("5", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
        public void TestSaveObjectObjectId()
        {
            ClearTest();
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("àlgol?à-csharp"), "TestModelId");

            var model = BuildTestModel();
            var task = _indexHelper.SaveObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            var res = _indexHelper.Search(new Query(""));

            Assert.Equal(1, res["nbHits"].ToObject<int>());
            Assert.Equal("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("10", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
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

            Assert.Equal(5, res["nbHits"].ToObject<int>());
            Assert.Equal("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("5", res["hits"][0]["objectID"].ToString());

            tasks = _indexHelper.DeleteObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            res = _indexHelper.Search(new Query(""));

            Assert.Equal(1, res["nbHits"].ToObject<int>());
            Assert.Equal("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("5", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
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

            Assert.Equal(8967, res["nbHits"].ToObject<int>());
            Assert.Equal("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("999", res["hits"][0]["objectID"].ToString());

            tasks = _indexHelper.DeleteObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            res = _indexHelper.Search(new Query(""));

            Assert.Equal(1, res["nbHits"].ToObject<int>());
            Assert.Equal("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("5", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
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

            Assert.Equal(8967, res["nbHits"].ToObject<int>());
            Assert.Equal("FirstName999", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("999", res["hits"][0]["objectID"].ToString());

            tasks = _indexHelper.DeleteObjects(models, 10000);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            res = _indexHelper.Search(new Query(""));

            Assert.Equal(1, res["nbHits"].ToObject<int>());
            Assert.Equal("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("5", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
        public void TestDeleteObjectsObjectId()
        {
            ClearTest();
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("àlgol?à-csharp"), "TestModelId");

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

            Assert.Equal(5, res["nbHits"].ToObject<int>());
            Assert.Equal("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("8", res["hits"][0]["objectID"].ToString());

            tasks = _indexHelper.DeleteObjects(models);
            foreach (var item in tasks)
            {
                _indexHelper.WaitTask(item["taskID"].ToString());
            }

            res = _indexHelper.Search(new Query(""));

            Assert.Equal(1, res["nbHits"].ToObject<int>());
            Assert.Equal("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("10", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
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

            Assert.Equal(5, res["nbHits"].ToObject<int>());
            Assert.Equal("Scott", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("5", res["hits"][0]["objectID"].ToString());

            task = _indexHelper.DeleteObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            res = _indexHelper.Search(new Query(""));

            Assert.Equal(4, res["nbHits"].ToObject<int>());
            Assert.Equal("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("4", res["hits"][0]["objectID"].ToString());
        }

        [Fact]
        public void TestDeleteObjectObjectId()
        {
            ClearTest();
            _indexHelper = new IndexHelper<TestModel>(_client, GetSafeName("àlgol?à-csharp"), "TestModelId");

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

            Assert.Equal(5, res["nbHits"].ToObject<int>());
            Assert.Equal("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("8", res["hits"][0]["objectID"].ToString());

            task = _indexHelper.DeleteObject(model);
            _indexHelper.WaitTask(task["taskID"].ToString());

            res = _indexHelper.Search(new Query(""));

            Assert.Equal(4, res["nbHits"].ToObject<int>());
            Assert.Equal("Sylvain", res["hits"][0]["FirstName"].ToString());
            Assert.Equal("8", res["hits"][0]["objectID"].ToString());
        }
    }
}
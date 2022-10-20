using Algolia.Search.Utils;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Algolia.Search.Test.Utils;

class TestRecord
{
    public string ObjectID { get; set; }
    public string Name { get; set; }
}

[TestFixture]
[Parallelizable]
public class AlgoliaHelperTest
{
    [Test]
    [Parallelizable]
    public void TestGetObjectIdFromClass()
    {
        var objectId = "1-2_1-2";
        var record = new TestRecord
        {
            ObjectID = objectId,
            Name = "this is a test"
        };
        Assert.AreEqual(objectId, AlgoliaHelper.GetObjectID(record));
    }

    [Test]
    [Parallelizable]
    public void TestGetObjectIdFromJObject()
    {
        var objectId = "1-2_1-2";
        var record = new JObject
        {
            { "objectID", objectId },
            { "name", "this is a test" }
        };
        Assert.AreEqual(objectId, AlgoliaHelper.GetObjectID(record));
    }
}

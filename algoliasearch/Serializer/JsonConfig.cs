using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Algolia.Search.Serializer
{
  /// <summary>
  /// Used to ensure that all the properties are serialized and deserialized well (because of Pascal and Camel Casing)
  /// </summary>
  internal static class JsonConfig
  {
    public const string JsonContentType = "application/json";

    public static JsonSerializerSettings AlgoliaJsonSerializerSettings => new JsonSerializerSettings
    {
      Formatting = Formatting.None,
      NullValueHandling = NullValueHandling.Ignore,
      ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
      DateParseHandling = DateParseHandling.DateTime
    };
  }
}

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

    private static readonly DefaultContractResolver Resolver = new() { NamingStrategy = new CamelCaseNamingStrategy() };

    public static readonly JsonSerializerSettings AlgoliaJsonSerializerSettings = new()
    {
      Formatting = Formatting.None,
      NullValueHandling = NullValueHandling.Ignore,
      ContractResolver = Resolver,
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
      DateParseHandling = DateParseHandling.DateTime,
      ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
      MissingMemberHandling = MissingMemberHandling.Ignore,
    };

    // When DeserializeOneOfSettings is used, we set MissingMemberHandling to Error to throw an exception if a property is missing
    public static readonly JsonSerializerSettings DeserializeOneOfSettings = new()
    {
      Formatting = Formatting.None,
      NullValueHandling = NullValueHandling.Ignore,
      ContractResolver = Resolver,
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
      DateParseHandling = DateParseHandling.DateTime,
      ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
      MissingMemberHandling = MissingMemberHandling.Error,
    };
  }
}

using System.Text.Json;
using System.Text.Json.Serialization;
using Algolia.Search.Models.Search;

namespace Algolia.Search.Serializer;

/// <summary>
/// Used to ensure that all the properties are serialized and deserialized well (because of Pascal and Camel Casing)
/// </summary>
internal static class JsonConfig
{
  public const string JsonContentType = "application/json";

  public static readonly JsonSerializerOptions Options = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = false,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
    Converters =
    {
      new JsonStringEnumConverterFactory(),
      new SearchResultConverterFactory()
    }
  };
}

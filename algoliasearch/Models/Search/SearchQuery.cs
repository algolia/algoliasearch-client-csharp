//
// Code generated by OpenAPI Generator (https://openapi-generator.tech), manual changes will be lost - read more on https://github.com/algolia/api-clients-automation. DO NOT EDIT.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Algolia.Search.Models.Common;
using Algolia.Search.Serializer;

namespace Algolia.Search.Models.Search;

/// <summary>
/// SearchQuery
/// </summary>
[JsonConverter(typeof(SearchQueryJsonConverter))]
public partial class SearchQuery : AbstractSchema
{
  /// <summary>
  /// Initializes a new instance of the SearchQuery class
  /// with a SearchForFacets
  /// </summary>
  /// <param name="actualInstance">An instance of SearchForFacets.</param>
  public SearchQuery(SearchForFacets actualInstance)
  {
    ActualInstance =
      actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }

  /// <summary>
  /// Initializes a new instance of the SearchQuery class
  /// with a SearchForHits
  /// </summary>
  /// <param name="actualInstance">An instance of SearchForHits.</param>
  public SearchQuery(SearchForHits actualInstance)
  {
    ActualInstance =
      actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }

  /// <summary>
  /// Gets or Sets ActualInstance
  /// </summary>
  public sealed override object ActualInstance { get; set; }

  /// <summary>
  /// Get the actual instance of `SearchForFacets`. If the actual instance is not `SearchForFacets`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of SearchForFacets</returns>
  public SearchForFacets AsSearchForFacets()
  {
    return (SearchForFacets)ActualInstance;
  }

  /// <summary>
  /// Get the actual instance of `SearchForHits`. If the actual instance is not `SearchForHits`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of SearchForHits</returns>
  public SearchForHits AsSearchForHits()
  {
    return (SearchForHits)ActualInstance;
  }

  /// <summary>
  /// Check if the actual instance is of `SearchForFacets` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsSearchForFacets()
  {
    return ActualInstance.GetType() == typeof(SearchForFacets);
  }

  /// <summary>
  /// Check if the actual instance is of `SearchForHits` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsSearchForHits()
  {
    return ActualInstance.GetType() == typeof(SearchForHits);
  }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("class SearchQuery {\n");
    sb.Append("  ActualInstance: ").Append(ActualInstance).Append("\n");
    sb.Append("}\n");
    return sb.ToString();
  }

  /// <summary>
  /// Returns the JSON string presentation of the object
  /// </summary>
  /// <returns>JSON string presentation of the object</returns>
  public override string ToJson()
  {
    return JsonSerializer.Serialize(ActualInstance, JsonConfig.Options);
  }

  /// <summary>
  /// Returns true if objects are equal
  /// </summary>
  /// <param name="obj">Object to be compared</param>
  /// <returns>Boolean</returns>
  public override bool Equals(object obj)
  {
    if (obj is not SearchQuery input)
    {
      return false;
    }

    return ActualInstance.Equals(input.ActualInstance);
  }

  /// <summary>
  /// Gets the hash code
  /// </summary>
  /// <returns>Hash code</returns>
  public override int GetHashCode()
  {
    unchecked // Overflow is fine, just wrap
    {
      int hashCode = 41;
      if (ActualInstance != null)
        hashCode = hashCode * 59 + ActualInstance.GetHashCode();
      return hashCode;
    }
  }
}

/// <summary>
/// Custom JSON converter for SearchQuery
/// </summary>
public class SearchQueryJsonConverter : JsonConverter<SearchQuery>
{
  /// <summary>
  /// Check if the object can be converted
  /// </summary>
  /// <param name="objectType">Object type</param>
  /// <returns>True if the object can be converted</returns>
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof(SearchQuery);
  }

  /// <summary>
  /// To convert a JSON string into an object
  /// </summary>
  /// <param name="reader">JSON reader</param>
  /// <param name="typeToConvert">Object type</param>
  /// <param name="options">Serializer options</param>
  /// <returns>The object converted from the JSON string</returns>
  public override SearchQuery Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options
  )
  {
    var jsonDocument = JsonDocument.ParseValue(ref reader);
    var root = jsonDocument.RootElement;
    if (
      root.ValueKind == JsonValueKind.Object
      && root.TryGetProperty("facet", out _)
      && root.TryGetProperty("type", out _)
    )
    {
      try
      {
        return new SearchQuery(jsonDocument.Deserialize<SearchForFacets>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine(
          $"Failed to deserialize into SearchForFacets: {exception}"
        );
      }
    }
    if (root.ValueKind == JsonValueKind.Object)
    {
      try
      {
        return new SearchQuery(jsonDocument.Deserialize<SearchForHits>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine(
          $"Failed to deserialize into SearchForHits: {exception}"
        );
      }
    }
    throw new InvalidDataException(
      $"The JSON string cannot be deserialized into any schema defined."
    );
  }

  /// <summary>
  /// To write the JSON string
  /// </summary>
  /// <param name="writer">JSON writer</param>
  /// <param name="value">SearchQuery to be converted into a JSON string</param>
  /// <param name="options">JSON Serializer options</param>
  public override void Write(
    Utf8JsonWriter writer,
    SearchQuery value,
    JsonSerializerOptions options
  )
  {
    writer.WriteRawValue(value.ToJson());
  }
}

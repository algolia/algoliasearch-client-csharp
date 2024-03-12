//
// Code generated by OpenAPI Generator (https://openapi-generator.tech), manual changes will be lost - read more on https://github.com/algolia/api-clients-automation. DO NOT EDIT.
//
using System;
using System.Text;
using System.Linq;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Algolia.Search.Serializer;
using System.Text.Json;
using System.IO;
using System.Reflection;
using Algolia.Search.Models.Common;

namespace Algolia.Search.Models.Recommend;

/// <summary>
/// Determines how many records of a group are included in the search results.  Records with the same value for the `attributeForDistinct` attribute are considered a group. The `distinct` setting controls how many members of the group are returned. This is useful for [deduplication and grouping](https://www.algolia.com/doc/guides/managing-results/refine-results/grouping/#introducing-algolias-distinct-feature).  The `distinct` setting is ignored if `attributeForDistinct` is not set. 
/// </summary>
[JsonConverter(typeof(DistinctJsonConverter))]
public partial class Distinct : AbstractSchema
{
  /// <summary>
  /// Initializes a new instance of the Distinct class
  /// with a bool
  /// </summary>
  /// <param name="actualInstance">An instance of bool.</param>
  public Distinct(bool actualInstance)
  {
    ActualInstance = actualInstance;
  }

  /// <summary>
  /// Initializes a new instance of the Distinct class
  /// with a int
  /// </summary>
  /// <param name="actualInstance">An instance of int.</param>
  public Distinct(int actualInstance)
  {
    ActualInstance = actualInstance;
  }


  /// <summary>
  /// Gets or Sets ActualInstance
  /// </summary>
  public sealed override object ActualInstance { get; set; }

  /// <summary>
  /// Get the actual instance of `bool`. If the actual instance is not `bool`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of bool</returns>
  public bool AsBool()
  {
    return (bool)ActualInstance;
  }

  /// <summary>
  /// Get the actual instance of `int`. If the actual instance is not `int`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of int</returns>
  public int AsInt()
  {
    return (int)ActualInstance;
  }


  /// <summary>
  /// Check if the actual instance is of `bool` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsBool()
  {
    return ActualInstance.GetType() == typeof(bool);
  }

  /// <summary>
  /// Check if the actual instance is of `int` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsInt()
  {
    return ActualInstance.GetType() == typeof(int);
  }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("class Distinct {\n");
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
    if (obj is not Distinct input)
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
/// Custom JSON converter for Distinct
/// </summary>
public class DistinctJsonConverter : JsonConverter<Distinct>
{

  /// <summary>
  /// Check if the object can be converted
  /// </summary>
  /// <param name="objectType">Object type</param>
  /// <returns>True if the object can be converted</returns>
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof(Distinct);
  }

  /// <summary>
  /// To convert a JSON string into an object
  /// </summary>
  /// <param name="reader">JSON reader</param>
  /// <param name="typeToConvert">Object type</param>
  /// <param name="options">Serializer options</param>
  /// <returns>The object converted from the JSON string</returns>
  public override Distinct Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonDocument = JsonDocument.ParseValue(ref reader);
    var root = jsonDocument.RootElement;
    if (root.ValueKind == JsonValueKind.True || root.ValueKind == JsonValueKind.False)
    {
      try
      {
        return new Distinct(jsonDocument.Deserialize<bool>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into bool: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.Number)
    {
      try
      {
        return new Distinct(jsonDocument.Deserialize<int>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into int: {exception}");
      }
    }
    throw new InvalidDataException($"The JSON string cannot be deserialized into any schema defined.");
  }

  /// <summary>
  /// To write the JSON string
  /// </summary>
  /// <param name="writer">JSON writer</param>
  /// <param name="value">Distinct to be converted into a JSON string</param>
  /// <param name="options">JSON Serializer options</param>
  public override void Write(Utf8JsonWriter writer, Distinct value, JsonSerializerOptions options)
  {
    writer.WriteRawValue(value.ToJson());
  }
}


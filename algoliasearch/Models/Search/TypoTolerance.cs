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

namespace Algolia.Search.Models.Search;

/// <summary>
/// Controls whether [typo tolerance](https://www.algolia.com/doc/guides/managing-results/optimize-search-results/typo-tolerance/) is enabled and how it is applied.
/// </summary>
[JsonConverter(typeof(TypoToleranceJsonConverter))]
public partial class TypoTolerance : AbstractSchema
{
  /// <summary>
  /// Initializes a new instance of the TypoTolerance class
  /// with a bool
  /// </summary>
  /// <param name="actualInstance">An instance of bool.</param>
  public TypoTolerance(bool actualInstance)
  {
    ActualInstance = actualInstance;
  }

  /// <summary>
  /// Initializes a new instance of the TypoTolerance class
  /// with a TypoToleranceEnum
  /// </summary>
  /// <param name="actualInstance">An instance of TypoToleranceEnum.</param>
  public TypoTolerance(TypoToleranceEnum actualInstance)
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
  /// Get the actual instance of `TypoToleranceEnum`. If the actual instance is not `TypoToleranceEnum`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of TypoToleranceEnum</returns>
  public TypoToleranceEnum AsTypoToleranceEnum()
  {
    return (TypoToleranceEnum)ActualInstance;
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
  /// Check if the actual instance is of `TypoToleranceEnum` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsTypoToleranceEnum()
  {
    return ActualInstance.GetType() == typeof(TypoToleranceEnum);
  }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("class TypoTolerance {\n");
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
    if (obj is not TypoTolerance input)
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
/// Custom JSON converter for TypoTolerance
/// </summary>
public class TypoToleranceJsonConverter : JsonConverter<TypoTolerance>
{

  /// <summary>
  /// Check if the object can be converted
  /// </summary>
  /// <param name="objectType">Object type</param>
  /// <returns>True if the object can be converted</returns>
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof(TypoTolerance);
  }

  /// <summary>
  /// To convert a JSON string into an object
  /// </summary>
  /// <param name="reader">JSON reader</param>
  /// <param name="typeToConvert">Object type</param>
  /// <param name="options">Serializer options</param>
  /// <returns>The object converted from the JSON string</returns>
  public override TypoTolerance Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonDocument = JsonDocument.ParseValue(ref reader);
    var root = jsonDocument.RootElement;
    if (root.ValueKind == JsonValueKind.True || root.ValueKind == JsonValueKind.False)
    {
      try
      {
        return new TypoTolerance(jsonDocument.Deserialize<bool>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into bool: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.String)
    {
      try
      {
        return new TypoTolerance(jsonDocument.Deserialize<TypoToleranceEnum>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into TypoToleranceEnum: {exception}");
      }
    }
    throw new InvalidDataException($"The JSON string cannot be deserialized into any schema defined.");
  }

  /// <summary>
  /// To write the JSON string
  /// </summary>
  /// <param name="writer">JSON writer</param>
  /// <param name="value">TypoTolerance to be converted into a JSON string</param>
  /// <param name="options">JSON Serializer options</param>
  public override void Write(Utf8JsonWriter writer, TypoTolerance value, JsonSerializerOptions options)
  {
    writer.WriteRawValue(value.ToJson());
  }
}

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

namespace Algolia.Search.Models.Ingestion;

/// <summary>
/// PlatformWithNone
/// </summary>
[JsonConverter(typeof(PlatformWithNoneJsonConverter))]
public partial class PlatformWithNone : AbstractSchema
{
  /// <summary>
  /// Initializes a new instance of the PlatformWithNone class
  /// with a Platform
  /// </summary>
  /// <param name="actualInstance">An instance of Platform.</param>
  public PlatformWithNone(Platform actualInstance)
  {
    ActualInstance = actualInstance;
  }

  /// <summary>
  /// Initializes a new instance of the PlatformWithNone class
  /// with a PlatformNone
  /// </summary>
  /// <param name="actualInstance">An instance of PlatformNone.</param>
  public PlatformWithNone(PlatformNone actualInstance)
  {
    ActualInstance = actualInstance;
  }

  /// <summary>
  /// Gets or Sets ActualInstance
  /// </summary>
  public sealed override object ActualInstance { get; set; }

  /// <summary>
  /// Get the actual instance of `Platform`. If the actual instance is not `Platform`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of Platform</returns>
  public Platform AsPlatform()
  {
    return (Platform)ActualInstance;
  }

  /// <summary>
  /// Get the actual instance of `PlatformNone`. If the actual instance is not `PlatformNone`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of PlatformNone</returns>
  public PlatformNone AsPlatformNone()
  {
    return (PlatformNone)ActualInstance;
  }

  /// <summary>
  /// Check if the actual instance is of `Platform` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsPlatform()
  {
    return ActualInstance.GetType() == typeof(Platform);
  }

  /// <summary>
  /// Check if the actual instance is of `PlatformNone` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsPlatformNone()
  {
    return ActualInstance.GetType() == typeof(PlatformNone);
  }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("class PlatformWithNone {\n");
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
    if (obj is not PlatformWithNone input)
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
/// Custom JSON converter for PlatformWithNone
/// </summary>
public class PlatformWithNoneJsonConverter : JsonConverter<PlatformWithNone>
{
  /// <summary>
  /// Check if the object can be converted
  /// </summary>
  /// <param name="objectType">Object type</param>
  /// <returns>True if the object can be converted</returns>
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof(PlatformWithNone);
  }

  /// <summary>
  /// To convert a JSON string into an object
  /// </summary>
  /// <param name="reader">JSON reader</param>
  /// <param name="typeToConvert">Object type</param>
  /// <param name="options">Serializer options</param>
  /// <returns>The object converted from the JSON string</returns>
  public override PlatformWithNone Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options
  )
  {
    var jsonDocument = JsonDocument.ParseValue(ref reader);
    var root = jsonDocument.RootElement;
    if (root.ValueKind == JsonValueKind.String)
    {
      try
      {
        return new PlatformWithNone(jsonDocument.Deserialize<Platform>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into Platform: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.String)
    {
      try
      {
        return new PlatformWithNone(jsonDocument.Deserialize<PlatformNone>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into PlatformNone: {exception}");
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
  /// <param name="value">PlatformWithNone to be converted into a JSON string</param>
  /// <param name="options">JSON Serializer options</param>
  public override void Write(
    Utf8JsonWriter writer,
    PlatformWithNone value,
    JsonSerializerOptions options
  )
  {
    writer.WriteRawValue(value.ToJson());
  }
}

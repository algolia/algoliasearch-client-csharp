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
/// Promote
/// </summary>
[JsonConverter(typeof(PromoteJsonConverter))]
public partial class Promote : AbstractSchema
{
  /// <summary>
  /// Initializes a new instance of the Promote class
  /// with a PromoteObjectIDs
  /// </summary>
  /// <param name="actualInstance">An instance of PromoteObjectIDs.</param>
  public Promote(PromoteObjectIDs actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }

  /// <summary>
  /// Initializes a new instance of the Promote class
  /// with a PromoteObjectID
  /// </summary>
  /// <param name="actualInstance">An instance of PromoteObjectID.</param>
  public Promote(PromoteObjectID actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }


  /// <summary>
  /// Gets or Sets ActualInstance
  /// </summary>
  public sealed override object ActualInstance { get; set; }

  /// <summary>
  /// Get the actual instance of `PromoteObjectIDs`. If the actual instance is not `PromoteObjectIDs`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of PromoteObjectIDs</returns>
  public PromoteObjectIDs AsPromoteObjectIDs()
  {
    return (PromoteObjectIDs)ActualInstance;
  }

  /// <summary>
  /// Get the actual instance of `PromoteObjectID`. If the actual instance is not `PromoteObjectID`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of PromoteObjectID</returns>
  public PromoteObjectID AsPromoteObjectID()
  {
    return (PromoteObjectID)ActualInstance;
  }


  /// <summary>
  /// Check if the actual instance is of `PromoteObjectIDs` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsPromoteObjectIDs()
  {
    return ActualInstance.GetType() == typeof(PromoteObjectIDs);
  }

  /// <summary>
  /// Check if the actual instance is of `PromoteObjectID` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsPromoteObjectID()
  {
    return ActualInstance.GetType() == typeof(PromoteObjectID);
  }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("class Promote {\n");
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
    if (obj is not Promote input)
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
/// Custom JSON converter for Promote
/// </summary>
public class PromoteJsonConverter : JsonConverter<Promote>
{

  /// <summary>
  /// Check if the object can be converted
  /// </summary>
  /// <param name="objectType">Object type</param>
  /// <returns>True if the object can be converted</returns>
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof(Promote);
  }

  /// <summary>
  /// To convert a JSON string into an object
  /// </summary>
  /// <param name="reader">JSON reader</param>
  /// <param name="typeToConvert">Object type</param>
  /// <param name="options">Serializer options</param>
  /// <returns>The object converted from the JSON string</returns>
  public override Promote Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonDocument = JsonDocument.ParseValue(ref reader);
    var root = jsonDocument.RootElement;
    if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("objectIDs", out _))
    {
      try
      {
        return new Promote(jsonDocument.Deserialize<PromoteObjectIDs>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into PromoteObjectIDs: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("objectID", out _))
    {
      try
      {
        return new Promote(jsonDocument.Deserialize<PromoteObjectID>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into PromoteObjectID: {exception}");
      }
    }
    throw new InvalidDataException($"The JSON string cannot be deserialized into any schema defined.");
  }

  /// <summary>
  /// To write the JSON string
  /// </summary>
  /// <param name="writer">JSON writer</param>
  /// <param name="value">Promote to be converted into a JSON string</param>
  /// <param name="options">JSON Serializer options</param>
  public override void Write(Utf8JsonWriter writer, Promote value, JsonSerializerOptions options)
  {
    writer.WriteRawValue(value.ToJson());
  }
}

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

namespace Algolia.Search.Models.Ingestion;

/// <summary>
/// SourceUpdateInput
/// </summary>
[JsonConverter(typeof(SourceUpdateInputJsonConverter))]
public partial class SourceUpdateInput : AbstractSchema
{
  /// <summary>
  /// Initializes a new instance of the SourceUpdateInput class
  /// with a SourceBigQuery
  /// </summary>
  /// <param name="actualInstance">An instance of SourceBigQuery.</param>
  public SourceUpdateInput(SourceBigQuery actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }

  /// <summary>
  /// Initializes a new instance of the SourceUpdateInput class
  /// with a SourceUpdateCommercetools
  /// </summary>
  /// <param name="actualInstance">An instance of SourceUpdateCommercetools.</param>
  public SourceUpdateInput(SourceUpdateCommercetools actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }

  /// <summary>
  /// Initializes a new instance of the SourceUpdateInput class
  /// with a SourceJSON
  /// </summary>
  /// <param name="actualInstance">An instance of SourceJSON.</param>
  public SourceUpdateInput(SourceJSON actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }

  /// <summary>
  /// Initializes a new instance of the SourceUpdateInput class
  /// with a SourceCSV
  /// </summary>
  /// <param name="actualInstance">An instance of SourceCSV.</param>
  public SourceUpdateInput(SourceCSV actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }

  /// <summary>
  /// Initializes a new instance of the SourceUpdateInput class
  /// with a SourceUpdateDocker
  /// </summary>
  /// <param name="actualInstance">An instance of SourceUpdateDocker.</param>
  public SourceUpdateInput(SourceUpdateDocker actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }


  /// <summary>
  /// Gets or Sets ActualInstance
  /// </summary>
  public sealed override object ActualInstance { get; set; }

  /// <summary>
  /// Get the actual instance of `SourceBigQuery`. If the actual instance is not `SourceBigQuery`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of SourceBigQuery</returns>
  public SourceBigQuery AsSourceBigQuery()
  {
    return (SourceBigQuery)ActualInstance;
  }

  /// <summary>
  /// Get the actual instance of `SourceUpdateCommercetools`. If the actual instance is not `SourceUpdateCommercetools`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of SourceUpdateCommercetools</returns>
  public SourceUpdateCommercetools AsSourceUpdateCommercetools()
  {
    return (SourceUpdateCommercetools)ActualInstance;
  }

  /// <summary>
  /// Get the actual instance of `SourceJSON`. If the actual instance is not `SourceJSON`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of SourceJSON</returns>
  public SourceJSON AsSourceJSON()
  {
    return (SourceJSON)ActualInstance;
  }

  /// <summary>
  /// Get the actual instance of `SourceCSV`. If the actual instance is not `SourceCSV`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of SourceCSV</returns>
  public SourceCSV AsSourceCSV()
  {
    return (SourceCSV)ActualInstance;
  }

  /// <summary>
  /// Get the actual instance of `SourceUpdateDocker`. If the actual instance is not `SourceUpdateDocker`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of SourceUpdateDocker</returns>
  public SourceUpdateDocker AsSourceUpdateDocker()
  {
    return (SourceUpdateDocker)ActualInstance;
  }


  /// <summary>
  /// Check if the actual instance is of `SourceBigQuery` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsSourceBigQuery()
  {
    return ActualInstance.GetType() == typeof(SourceBigQuery);
  }

  /// <summary>
  /// Check if the actual instance is of `SourceUpdateCommercetools` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsSourceUpdateCommercetools()
  {
    return ActualInstance.GetType() == typeof(SourceUpdateCommercetools);
  }

  /// <summary>
  /// Check if the actual instance is of `SourceJSON` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsSourceJSON()
  {
    return ActualInstance.GetType() == typeof(SourceJSON);
  }

  /// <summary>
  /// Check if the actual instance is of `SourceCSV` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsSourceCSV()
  {
    return ActualInstance.GetType() == typeof(SourceCSV);
  }

  /// <summary>
  /// Check if the actual instance is of `SourceUpdateDocker` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsSourceUpdateDocker()
  {
    return ActualInstance.GetType() == typeof(SourceUpdateDocker);
  }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("class SourceUpdateInput {\n");
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
    if (obj is not SourceUpdateInput input)
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
/// Custom JSON converter for SourceUpdateInput
/// </summary>
public class SourceUpdateInputJsonConverter : JsonConverter<SourceUpdateInput>
{

  /// <summary>
  /// Check if the object can be converted
  /// </summary>
  /// <param name="objectType">Object type</param>
  /// <returns>True if the object can be converted</returns>
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof(SourceUpdateInput);
  }

  /// <summary>
  /// To convert a JSON string into an object
  /// </summary>
  /// <param name="reader">JSON reader</param>
  /// <param name="typeToConvert">Object type</param>
  /// <param name="options">Serializer options</param>
  /// <returns>The object converted from the JSON string</returns>
  public override SourceUpdateInput Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonDocument = JsonDocument.ParseValue(ref reader);
    var root = jsonDocument.RootElement;
    if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("projectID", out _))
    {
      try
      {
        return new SourceUpdateInput(jsonDocument.Deserialize<SourceBigQuery>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into SourceBigQuery: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.Object)
    {
      try
      {
        return new SourceUpdateInput(jsonDocument.Deserialize<SourceUpdateCommercetools>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into SourceUpdateCommercetools: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.Object)
    {
      try
      {
        return new SourceUpdateInput(jsonDocument.Deserialize<SourceJSON>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into SourceJSON: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.Object)
    {
      try
      {
        return new SourceUpdateInput(jsonDocument.Deserialize<SourceCSV>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into SourceCSV: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.Object)
    {
      try
      {
        return new SourceUpdateInput(jsonDocument.Deserialize<SourceUpdateDocker>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into SourceUpdateDocker: {exception}");
      }
    }
    throw new InvalidDataException($"The JSON string cannot be deserialized into any schema defined.");
  }

  /// <summary>
  /// To write the JSON string
  /// </summary>
  /// <param name="writer">JSON writer</param>
  /// <param name="value">SourceUpdateInput to be converted into a JSON string</param>
  /// <param name="options">JSON Serializer options</param>
  public override void Write(Utf8JsonWriter writer, SourceUpdateInput value, JsonSerializerOptions options)
  {
    writer.WriteRawValue(value.ToJson());
  }
}


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
/// InsideBoundingBox
/// </summary>
[JsonConverter(typeof(InsideBoundingBoxJsonConverter))]
public partial class InsideBoundingBox : AbstractSchema
{
  /// <summary>
  /// Initializes a new instance of the InsideBoundingBox class.
  /// </summary>
  public InsideBoundingBox()
  {
  }

  /// <summary>
  /// Initializes a new instance of the InsideBoundingBox class
  /// with a string
  /// </summary>
  /// <param name="actualInstance">An instance of string.</param>
  public InsideBoundingBox(string actualInstance)
  {
    ActualInstance = actualInstance;
  }

  /// <summary>
  /// Initializes a new instance of the InsideBoundingBox class
  /// with a List{List{double}}
  /// </summary>
  /// <param name="actualInstance">An instance of List<List<double>>.</param>
  public InsideBoundingBox(List<List<double>> actualInstance)
  {
    ActualInstance = actualInstance;
  }


  /// <summary>
  /// Gets or Sets ActualInstance
  /// </summary>
  public sealed override object ActualInstance { get; set; }

  /// <summary>
  /// Get the actual instance of `string`. If the actual instance is not `string`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of string</returns>
  public string AsString()
  {
    return (string)ActualInstance;
  }

  /// <summary>
  /// Get the actual instance of `List{List{double}}`. If the actual instance is not `List{List{double}}`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of List&lt;List&lt;double&gt;&gt;</returns>
  public List<List<double>> AsListListDouble()
  {
    return (List<List<double>>)ActualInstance;
  }


  /// <summary>
  /// Check if the actual instance is of `string` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsString()
  {
    return ActualInstance.GetType() == typeof(string);
  }

  /// <summary>
  /// Check if the actual instance is of `List{List{double}}` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsListListDouble()
  {
    return ActualInstance.GetType() == typeof(List<List<double>>);
  }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("class InsideBoundingBox {\n");
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
    if (obj is not InsideBoundingBox input)
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
/// Custom JSON converter for InsideBoundingBox
/// </summary>
public class InsideBoundingBoxJsonConverter : JsonConverter<InsideBoundingBox>
{

  /// <summary>
  /// Check if the object can be converted
  /// </summary>
  /// <param name="objectType">Object type</param>
  /// <returns>True if the object can be converted</returns>
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof(InsideBoundingBox);
  }

  /// <summary>
  /// To convert a JSON string into an object
  /// </summary>
  /// <param name="reader">JSON reader</param>
  /// <param name="typeToConvert">Object type</param>
  /// <param name="options">Serializer options</param>
  /// <returns>The object converted from the JSON string</returns>
  public override InsideBoundingBox Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonDocument = JsonDocument.ParseValue(ref reader);
    var root = jsonDocument.RootElement;
    if (root.ValueKind == JsonValueKind.String)
    {
      try
      {
        return new InsideBoundingBox(jsonDocument.Deserialize<string>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into string: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.Array)
    {
      try
      {
        return new InsideBoundingBox(jsonDocument.Deserialize<List<List<double>>>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into List<List<double>>: {exception}");
      }
    }
    throw new InvalidDataException($"The JSON string cannot be deserialized into any schema defined.");
  }

  /// <summary>
  /// To write the JSON string
  /// </summary>
  /// <param name="writer">JSON writer</param>
  /// <param name="value">InsideBoundingBox to be converted into a JSON string</param>
  /// <param name="options">JSON Serializer options</param>
  public override void Write(Utf8JsonWriter writer, InsideBoundingBox value, JsonSerializerOptions options)
  {
    writer.WriteRawValue(value.ToJson());
  }
}


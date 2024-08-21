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
/// Precision of a coordinate-based search in meters to group results with similar distances.  The Geo ranking criterion considers all matches within the same range of distances to be equal. 
/// </summary>
[JsonConverter(typeof(AroundPrecisionJsonConverter))]
public partial class AroundPrecision : AbstractSchema
{
  /// <summary>
  /// Initializes a new instance of the AroundPrecision class
  /// with a int
  /// </summary>
  /// <param name="actualInstance">An instance of int.</param>
  public AroundPrecision(int actualInstance)
  {
    ActualInstance = actualInstance;
  }

  /// <summary>
  /// Initializes a new instance of the AroundPrecision class
  /// with a List{Range}
  /// </summary>
  /// <param name="actualInstance">An instance of List&lt;Range&gt;.</param>
  public AroundPrecision(List<Range> actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }


  /// <summary>
  /// Gets or Sets ActualInstance
  /// </summary>
  public sealed override object ActualInstance { get; set; }

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
  /// Get the actual instance of `List{Range}`. If the actual instance is not `List{Range}`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of List&lt;Range&gt;</returns>
  public List<Range> AsListRange()
  {
    return (List<Range>)ActualInstance;
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
  /// Check if the actual instance is of `List{Range}` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsListRange()
  {
    return ActualInstance.GetType() == typeof(List<Range>);
  }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("class AroundPrecision {\n");
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
    if (obj is not AroundPrecision input)
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
/// Custom JSON converter for AroundPrecision
/// </summary>
public class AroundPrecisionJsonConverter : JsonConverter<AroundPrecision>
{

  /// <summary>
  /// Check if the object can be converted
  /// </summary>
  /// <param name="objectType">Object type</param>
  /// <returns>True if the object can be converted</returns>
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof(AroundPrecision);
  }

  /// <summary>
  /// To convert a JSON string into an object
  /// </summary>
  /// <param name="reader">JSON reader</param>
  /// <param name="typeToConvert">Object type</param>
  /// <param name="options">Serializer options</param>
  /// <returns>The object converted from the JSON string</returns>
  public override AroundPrecision Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonDocument = JsonDocument.ParseValue(ref reader);
    var root = jsonDocument.RootElement;
    if (root.ValueKind == JsonValueKind.Number)
    {
      try
      {
        return new AroundPrecision(jsonDocument.Deserialize<int>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into int: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.Array)
    {
      try
      {
        return new AroundPrecision(jsonDocument.Deserialize<List<Range>>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into List<Range>: {exception}");
      }
    }
    throw new InvalidDataException($"The JSON string cannot be deserialized into any schema defined.");
  }

  /// <summary>
  /// To write the JSON string
  /// </summary>
  /// <param name="writer">JSON writer</param>
  /// <param name="value">AroundPrecision to be converted into a JSON string</param>
  /// <param name="options">JSON Serializer options</param>
  public override void Write(Utf8JsonWriter writer, AroundPrecision value, JsonSerializerOptions options)
  {
    writer.WriteRawValue(value.ToJson());
  }
}

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

namespace Algolia.Search.Models.Analytics;

/// <summary>
/// GetTopSearchesResponse
/// </summary>
[JsonConverter(typeof(GetTopSearchesResponseJsonConverter))]
public partial class GetTopSearchesResponse : AbstractSchema
{
  /// <summary>
  /// Initializes a new instance of the GetTopSearchesResponse class
  /// with a TopSearchesResponse
  /// </summary>
  /// <param name="actualInstance">An instance of TopSearchesResponse.</param>
  public GetTopSearchesResponse(TopSearchesResponse actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }

  /// <summary>
  /// Initializes a new instance of the GetTopSearchesResponse class
  /// with a TopSearchesResponseWithAnalytics
  /// </summary>
  /// <param name="actualInstance">An instance of TopSearchesResponseWithAnalytics.</param>
  public GetTopSearchesResponse(TopSearchesResponseWithAnalytics actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }

  /// <summary>
  /// Initializes a new instance of the GetTopSearchesResponse class
  /// with a TopSearchesResponseWithRevenueAnalytics
  /// </summary>
  /// <param name="actualInstance">An instance of TopSearchesResponseWithRevenueAnalytics.</param>
  public GetTopSearchesResponse(TopSearchesResponseWithRevenueAnalytics actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }


  /// <summary>
  /// Gets or Sets ActualInstance
  /// </summary>
  public sealed override object ActualInstance { get; set; }

  /// <summary>
  /// Get the actual instance of `TopSearchesResponse`. If the actual instance is not `TopSearchesResponse`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of TopSearchesResponse</returns>
  public TopSearchesResponse AsTopSearchesResponse()
  {
    return (TopSearchesResponse)ActualInstance;
  }

  /// <summary>
  /// Get the actual instance of `TopSearchesResponseWithAnalytics`. If the actual instance is not `TopSearchesResponseWithAnalytics`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of TopSearchesResponseWithAnalytics</returns>
  public TopSearchesResponseWithAnalytics AsTopSearchesResponseWithAnalytics()
  {
    return (TopSearchesResponseWithAnalytics)ActualInstance;
  }

  /// <summary>
  /// Get the actual instance of `TopSearchesResponseWithRevenueAnalytics`. If the actual instance is not `TopSearchesResponseWithRevenueAnalytics`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of TopSearchesResponseWithRevenueAnalytics</returns>
  public TopSearchesResponseWithRevenueAnalytics AsTopSearchesResponseWithRevenueAnalytics()
  {
    return (TopSearchesResponseWithRevenueAnalytics)ActualInstance;
  }


  /// <summary>
  /// Check if the actual instance is of `TopSearchesResponse` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsTopSearchesResponse()
  {
    return ActualInstance.GetType() == typeof(TopSearchesResponse);
  }

  /// <summary>
  /// Check if the actual instance is of `TopSearchesResponseWithAnalytics` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsTopSearchesResponseWithAnalytics()
  {
    return ActualInstance.GetType() == typeof(TopSearchesResponseWithAnalytics);
  }

  /// <summary>
  /// Check if the actual instance is of `TopSearchesResponseWithRevenueAnalytics` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsTopSearchesResponseWithRevenueAnalytics()
  {
    return ActualInstance.GetType() == typeof(TopSearchesResponseWithRevenueAnalytics);
  }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("class GetTopSearchesResponse {\n");
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
    if (obj is not GetTopSearchesResponse input)
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
/// Custom JSON converter for GetTopSearchesResponse
/// </summary>
public class GetTopSearchesResponseJsonConverter : JsonConverter<GetTopSearchesResponse>
{

  /// <summary>
  /// Check if the object can be converted
  /// </summary>
  /// <param name="objectType">Object type</param>
  /// <returns>True if the object can be converted</returns>
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof(GetTopSearchesResponse);
  }

  /// <summary>
  /// To convert a JSON string into an object
  /// </summary>
  /// <param name="reader">JSON reader</param>
  /// <param name="typeToConvert">Object type</param>
  /// <param name="options">Serializer options</param>
  /// <returns>The object converted from the JSON string</returns>
  public override GetTopSearchesResponse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonDocument = JsonDocument.ParseValue(ref reader);
    var root = jsonDocument.RootElement;
    if (root.ValueKind == JsonValueKind.Object)
    {
      try
      {
        return new GetTopSearchesResponse(jsonDocument.Deserialize<TopSearchesResponse>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into TopSearchesResponse: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.Object)
    {
      try
      {
        return new GetTopSearchesResponse(jsonDocument.Deserialize<TopSearchesResponseWithAnalytics>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into TopSearchesResponseWithAnalytics: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.Object)
    {
      try
      {
        return new GetTopSearchesResponse(jsonDocument.Deserialize<TopSearchesResponseWithRevenueAnalytics>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into TopSearchesResponseWithRevenueAnalytics: {exception}");
      }
    }
    throw new InvalidDataException($"The JSON string cannot be deserialized into any schema defined.");
  }

  /// <summary>
  /// To write the JSON string
  /// </summary>
  /// <param name="writer">JSON writer</param>
  /// <param name="value">GetTopSearchesResponse to be converted into a JSON string</param>
  /// <param name="options">JSON Serializer options</param>
  public override void Write(Utf8JsonWriter writer, GetTopSearchesResponse value, JsonSerializerOptions options)
  {
    writer.WriteRawValue(value.ToJson());
  }
}

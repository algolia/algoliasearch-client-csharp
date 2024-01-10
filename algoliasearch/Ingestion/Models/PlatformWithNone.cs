//
// Code generated by OpenAPI Generator (https://openapi-generator.tech), manual changes will be lost - read more on https://github.com/algolia/api-clients-automation. DO NOT EDIT.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Algolia.Search.Models;

namespace Algolia.Search.Models.Ingestion
{
  /// <summary>
  /// PlatformWithNone
  /// </summary>
  [JsonConverter(typeof(PlatformWithNoneJsonConverter))]
  [DataContract(Name = "platformWithNone")]
  public partial class PlatformWithNone : AbstractSchema
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="PlatformWithNone" /> class
    /// with the <see cref="Platform" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of Platform.</param>
    public PlatformWithNone(Platform actualInstance)
    {
      IsNullable = false;
      SchemaType = "oneOf";
      ActualInstance = actualInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlatformWithNone" /> class
    /// with the <see cref="PlatformNone" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of PlatformNone.</param>
    public PlatformWithNone(PlatformNone actualInstance)
    {
      IsNullable = false;
      SchemaType = "oneOf";
      ActualInstance = actualInstance;
    }


    private Object _actualInstance;

    /// <summary>
    /// Gets or Sets ActualInstance
    /// </summary>
    public override Object ActualInstance
    {
      get
      {
        return _actualInstance;
      }
      set
      {
        this._actualInstance = value;
      }
    }

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
      return JsonConvert.SerializeObject(ActualInstance, SerializerSettings);
    }

    /// <summary>
    /// Converts the JSON string into an instance of PlatformWithNone
    /// </summary>
    /// <param name="jsonString">JSON string</param>
    /// <returns>An instance of PlatformWithNone</returns>
    public static PlatformWithNone FromJson(string jsonString)
    {
      PlatformWithNone newPlatformWithNone = null;

      if (string.IsNullOrEmpty(jsonString))
      {
        return newPlatformWithNone;
      }
      try
      {
        return new PlatformWithNone(JsonConvert.DeserializeObject<Platform>(jsonString, AdditionalPropertiesSerializerSettings));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine(string.Format("Failed to deserialize `{0}` into Platform: {1}", jsonString, exception.ToString()));
      }
      try
      {
        return new PlatformWithNone(JsonConvert.DeserializeObject<PlatformNone>(jsonString, AdditionalPropertiesSerializerSettings));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine(string.Format("Failed to deserialize `{0}` into PlatformNone: {1}", jsonString, exception.ToString()));
      }

      throw new InvalidDataException("The JSON string `" + jsonString + "` cannot be deserialized into any schema defined.");
    }

  }

  /// <summary>
  /// Custom JSON converter for PlatformWithNone
  /// </summary>
  public class PlatformWithNoneJsonConverter : JsonConverter
  {
    /// <summary>
    /// To write the JSON string
    /// </summary>
    /// <param name="writer">JSON writer</param>
    /// <param name="value">Object to be converted into a JSON string</param>
    /// <param name="serializer">JSON Serializer</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteRawValue((string)(typeof(PlatformWithNone).GetMethod("ToJson").Invoke(value, null)));
    }

    /// <summary>
    /// To convert a JSON string into an object
    /// </summary>
    /// <param name="reader">JSON reader</param>
    /// <param name="objectType">Object type</param>
    /// <param name="existingValue">Existing value</param>
    /// <param name="serializer">JSON Serializer</param>
    /// <returns>The object converted from the JSON string</returns>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType != JsonToken.Null)
      {
        return objectType.GetMethod("FromJson").Invoke(null, new[] { JObject.Load(reader).ToString(Formatting.None) });
      }
      return null;
    }

    /// <summary>
    /// Check if the object can be converted
    /// </summary>
    /// <param name="objectType">Object type</param>
    /// <returns>True if the object can be converted</returns>
    public override bool CanConvert(Type objectType)
    {
      return false;
    }
  }

}

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

namespace Algolia.Search.Search.Models
{
  /// <summary>
  /// AttributeToUpdate
  /// </summary>
  [JsonConverter(typeof(AttributeToUpdateJsonConverter))]
  [DataContract(Name = "attributeToUpdate")]
  public partial class AttributeToUpdate : AbstractSchema
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeToUpdate" /> class
    /// with the <see cref="string" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of string.</param>
    public AttributeToUpdate(string actualInstance)
    {
      this.IsNullable = false;
      this.SchemaType = "oneOf";
      this.ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AttributeToUpdate" /> class
    /// with the <see cref="BuiltInOperation" /> class
    /// </summary>
    /// <param name="actualInstance">An instance of BuiltInOperation.</param>
    public AttributeToUpdate(BuiltInOperation actualInstance)
    {
      this.IsNullable = false;
      this.SchemaType = "oneOf";
      this.ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
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
        if (value.GetType() == typeof(BuiltInOperation))
        {
          this._actualInstance = value;
        }
        else if (value.GetType() == typeof(string))
        {
          this._actualInstance = value;
        }
        else
        {
          throw new ArgumentException("Invalid instance found. Must be the following types: BuiltInOperation, string");
        }
      }
    }

    /// <summary>
    /// Get the actual instance of `string`. If the actual instance is not `string`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of string</returns>
    public string GetterString()
    {
      return (string)this.ActualInstance;
    }

    /// <summary>
    /// Get the actual instance of `BuiltInOperation`. If the actual instance is not `BuiltInOperation`,
    /// the InvalidClassException will be thrown
    /// </summary>
    /// <returns>An instance of BuiltInOperation</returns>
    public BuiltInOperation GetterBuiltInOperation()
    {
      return (BuiltInOperation)this.ActualInstance;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("class AttributeToUpdate {\n");
      sb.Append("  ActualInstance: ").Append(this.ActualInstance).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public override string ToJson()
    {
      return JsonConvert.SerializeObject(this.ActualInstance, AttributeToUpdate.SerializerSettings);
    }

    /// <summary>
    /// Converts the JSON string into an instance of AttributeToUpdate
    /// </summary>
    /// <param name="jsonString">JSON string</param>
    /// <returns>An instance of AttributeToUpdate</returns>
    public static AttributeToUpdate FromJson(string jsonString)
    {
      AttributeToUpdate newAttributeToUpdate = null;

      if (string.IsNullOrEmpty(jsonString))
      {
        return newAttributeToUpdate;
      }
      int match = 0;
      List<string> matchedTypes = new List<string>();

      try
      {
        // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
        if (typeof(BuiltInOperation).GetProperty("AdditionalProperties") == null)
        {
          newAttributeToUpdate = new AttributeToUpdate(JsonConvert.DeserializeObject<BuiltInOperation>(jsonString, AttributeToUpdate.SerializerSettings));
        }
        else
        {
          newAttributeToUpdate = new AttributeToUpdate(JsonConvert.DeserializeObject<BuiltInOperation>(jsonString, AttributeToUpdate.AdditionalPropertiesSerializerSettings));
        }
        matchedTypes.Add("BuiltInOperation");
        match++;
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine(string.Format("Failed to deserialize `{0}` into BuiltInOperation: {1}", jsonString, exception.ToString()));
      }

      try
      {
        // if it does not contains "AdditionalProperties", use SerializerSettings to deserialize
        if (typeof(string).GetProperty("AdditionalProperties") == null)
        {
          newAttributeToUpdate = new AttributeToUpdate(JsonConvert.DeserializeObject<string>(jsonString, AttributeToUpdate.SerializerSettings));
        }
        else
        {
          newAttributeToUpdate = new AttributeToUpdate(JsonConvert.DeserializeObject<string>(jsonString, AttributeToUpdate.AdditionalPropertiesSerializerSettings));
        }
        matchedTypes.Add("string");
        match++;
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine(string.Format("Failed to deserialize `{0}` into string: {1}", jsonString, exception.ToString()));
      }

      if (match == 0)
      {
        throw new InvalidDataException("The JSON string `" + jsonString + "` cannot be deserialized into any schema defined.");
      }
      else if (match > 1)
      {
        throw new InvalidDataException("The JSON string `" + jsonString + "` incorrectly matches more than one schema (should be exactly one match): " + String.Join(",", matchedTypes));
      }

      // deserialization is considered successful at this point if no exception has been thrown.
      return newAttributeToUpdate;
    }

  }

  /// <summary>
  /// Custom JSON converter for AttributeToUpdate
  /// </summary>
  public class AttributeToUpdateJsonConverter : JsonConverter
  {
    /// <summary>
    /// To write the JSON string
    /// </summary>
    /// <param name="writer">JSON writer</param>
    /// <param name="value">Object to be converted into a JSON string</param>
    /// <param name="serializer">JSON Serializer</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteRawValue((string)(typeof(AttributeToUpdate).GetMethod("ToJson").Invoke(value, null)));
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
        return AttributeToUpdate.FromJson(JObject.Load(reader).ToString(Formatting.None));
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
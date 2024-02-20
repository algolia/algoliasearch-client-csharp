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
/// Removes stop (common) words from the query before executing it. `removeStopWords` is used in conjunction with the `queryLanguages` setting. _list_: language ISO codes for which stop words should be enabled. This list will override any values that you may have set in `queryLanguages`. _true_: enables the stop words feature, ensuring that stop words are removed from consideration in a search. The languages supported here are either [every language](https://www.algolia.com/doc/guides/managing-results/optimize-search-results/handling-natural-languages-nlp/in-depth/supported-languages/) (this is the default) or those set by `queryLanguages`. _false_: turns off the stop words feature, allowing stop words to be taken into account in a search. 
/// </summary>
[JsonConverter(typeof(RemoveStopWordsJsonConverter))]
public partial class RemoveStopWords : AbstractSchema
{
  /// <summary>
  /// Initializes a new instance of the RemoveStopWords class
  /// with a List{String}
  /// </summary>
  /// <param name="actualInstance">An instance of List&lt;string&gt;.</param>
  public RemoveStopWords(List<string> actualInstance)
  {
    ActualInstance = actualInstance ?? throw new ArgumentException("Invalid instance found. Must not be null.");
  }

  /// <summary>
  /// Initializes a new instance of the RemoveStopWords class
  /// with a bool
  /// </summary>
  /// <param name="actualInstance">An instance of bool.</param>
  public RemoveStopWords(bool actualInstance)
  {
    ActualInstance = actualInstance;
  }


  /// <summary>
  /// Gets or Sets ActualInstance
  /// </summary>
  public sealed override object ActualInstance { get; set; }

  /// <summary>
  /// Get the actual instance of `List{string}`. If the actual instance is not `List{string}`,
  /// the InvalidClassException will be thrown
  /// </summary>
  /// <returns>An instance of List&lt;string&gt;</returns>
  public List<string> AsListString()
  {
    return (List<string>)ActualInstance;
  }

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
  /// Check if the actual instance is of `List{string}` type.
  /// </summary>
  /// <returns>Whether or not the instance is the type</returns>
  public bool IsListString()
  {
    return ActualInstance.GetType() == typeof(List<string>);
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
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.Append("class RemoveStopWords {\n");
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
    if (obj is not RemoveStopWords input)
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
/// Custom JSON converter for RemoveStopWords
/// </summary>
public class RemoveStopWordsJsonConverter : JsonConverter<RemoveStopWords>
{

  /// <summary>
  /// Check if the object can be converted
  /// </summary>
  /// <param name="objectType">Object type</param>
  /// <returns>True if the object can be converted</returns>
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof(RemoveStopWords);
  }

  /// <summary>
  /// To convert a JSON string into an object
  /// </summary>
  /// <param name="reader">JSON reader</param>
  /// <param name="typeToConvert">Object type</param>
  /// <param name="options">Serializer options</param>
  /// <returns>The object converted from the JSON string</returns>
  public override RemoveStopWords Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var jsonDocument = JsonDocument.ParseValue(ref reader);
    var root = jsonDocument.RootElement;
    if (root.ValueKind == JsonValueKind.Array)
    {
      try
      {
        return new RemoveStopWords(jsonDocument.Deserialize<List<string>>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into List<string>: {exception}");
      }
    }
    if (root.ValueKind == JsonValueKind.True || root.ValueKind == JsonValueKind.False)
    {
      try
      {
        return new RemoveStopWords(jsonDocument.Deserialize<bool>(JsonConfig.Options));
      }
      catch (Exception exception)
      {
        // deserialization failed, try the next one
        System.Diagnostics.Debug.WriteLine($"Failed to deserialize into bool: {exception}");
      }
    }
    throw new InvalidDataException($"The JSON string cannot be deserialized into any schema defined.");
  }

  /// <summary>
  /// To write the JSON string
  /// </summary>
  /// <param name="writer">JSON writer</param>
  /// <param name="value">RemoveStopWords to be converted into a JSON string</param>
  /// <param name="options">JSON Serializer options</param>
  public override void Write(Utf8JsonWriter writer, RemoveStopWords value, JsonSerializerOptions options)
  {
    writer.WriteRawValue(value.ToJson());
  }
}


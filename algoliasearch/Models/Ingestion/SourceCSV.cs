//
// Code generated by OpenAPI Generator (https://openapi-generator.tech), manual changes will be lost - read more on https://github.com/algolia/api-clients-automation. DO NOT EDIT.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Algolia.Search.Serializer;

namespace Algolia.Search.Models.Ingestion;

/// <summary>
/// SourceCSV
/// </summary>
public partial class SourceCSV
{
  /// <summary>
  /// Gets or Sets Method
  /// </summary>
  [JsonPropertyName("method")]
  public MethodType? Method { get; set; }

  /// <summary>
  /// Initializes a new instance of the SourceCSV class.
  /// </summary>
  [JsonConstructor]
  public SourceCSV() { }

  /// <summary>
  /// Initializes a new instance of the SourceCSV class.
  /// </summary>
  /// <param name="url">URL of the file. (required).</param>
  public SourceCSV(string url)
  {
    Url = url ?? throw new ArgumentNullException(nameof(url));
  }

  /// <summary>
  /// URL of the file.
  /// </summary>
  /// <value>URL of the file.</value>
  [JsonPropertyName("url")]
  public string Url { get; set; }

  /// <summary>
  /// Name of a column that contains a unique ID which will be used as `objectID` in Algolia.
  /// </summary>
  /// <value>Name of a column that contains a unique ID which will be used as `objectID` in Algolia.</value>
  [JsonPropertyName("uniqueIDColumn")]
  public string UniqueIDColumn { get; set; }

  /// <summary>
  /// Key-value pairs of column names and their expected types.
  /// </summary>
  /// <value>Key-value pairs of column names and their expected types. </value>
  [JsonPropertyName("mapping")]
  public Dictionary<string, MappingTypeCSV> Mapping { get; set; }

  /// <summary>
  /// The character used to split the value on each line, default to a comma (\\r, \\n, 0xFFFD, and space are forbidden).
  /// </summary>
  /// <value>The character used to split the value on each line, default to a comma (\\r, \\n, 0xFFFD, and space are forbidden).</value>
  [JsonPropertyName("delimiter")]
  public string Delimiter { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class SourceCSV {\n");
    sb.Append("  Url: ").Append(Url).Append("\n");
    sb.Append("  UniqueIDColumn: ").Append(UniqueIDColumn).Append("\n");
    sb.Append("  Mapping: ").Append(Mapping).Append("\n");
    sb.Append("  Method: ").Append(Method).Append("\n");
    sb.Append("  Delimiter: ").Append(Delimiter).Append("\n");
    sb.Append("}\n");
    return sb.ToString();
  }

  /// <summary>
  /// Returns the JSON string presentation of the object
  /// </summary>
  /// <returns>JSON string presentation of the object</returns>
  public virtual string ToJson()
  {
    return JsonSerializer.Serialize(this, JsonConfig.Options);
  }

  /// <summary>
  /// Returns true if objects are equal
  /// </summary>
  /// <param name="obj">Object to be compared</param>
  /// <returns>Boolean</returns>
  public override bool Equals(object obj)
  {
    if (obj is not SourceCSV input)
    {
      return false;
    }

    return (Url == input.Url || (Url != null && Url.Equals(input.Url)))
      && (
        UniqueIDColumn == input.UniqueIDColumn
        || (UniqueIDColumn != null && UniqueIDColumn.Equals(input.UniqueIDColumn))
      )
      && (
        Mapping == input.Mapping
        || Mapping != null && input.Mapping != null && Mapping.SequenceEqual(input.Mapping)
      )
      && (Method == input.Method || Method.Equals(input.Method))
      && (Delimiter == input.Delimiter || (Delimiter != null && Delimiter.Equals(input.Delimiter)));
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
      if (Url != null)
      {
        hashCode = (hashCode * 59) + Url.GetHashCode();
      }
      if (UniqueIDColumn != null)
      {
        hashCode = (hashCode * 59) + UniqueIDColumn.GetHashCode();
      }
      if (Mapping != null)
      {
        hashCode = (hashCode * 59) + Mapping.GetHashCode();
      }
      hashCode = (hashCode * 59) + Method.GetHashCode();
      if (Delimiter != null)
      {
        hashCode = (hashCode * 59) + Delimiter.GetHashCode();
      }
      return hashCode;
    }
  }
}

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

namespace Algolia.Search.Models.Ingestion;

/// <summary>
/// TransformationSearch
/// </summary>
public partial class TransformationSearch
{
  /// <summary>
  /// Initializes a new instance of the TransformationSearch class.
  /// </summary>
  [JsonConstructor]
  public TransformationSearch() { }
  /// <summary>
  /// Initializes a new instance of the TransformationSearch class.
  /// </summary>
  /// <param name="transformationsIDs">transformationsIDs (required).</param>
  public TransformationSearch(List<string> transformationsIDs)
  {
    TransformationsIDs = transformationsIDs ?? throw new ArgumentNullException(nameof(transformationsIDs));
  }

  /// <summary>
  /// Gets or Sets TransformationsIDs
  /// </summary>
  [JsonPropertyName("transformationsIDs")]
  public List<string> TransformationsIDs { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class TransformationSearch {\n");
    sb.Append("  TransformationsIDs: ").Append(TransformationsIDs).Append("\n");
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
    if (obj is not TransformationSearch input)
    {
      return false;
    }

    return
        (TransformationsIDs == input.TransformationsIDs || TransformationsIDs != null && input.TransformationsIDs != null && TransformationsIDs.SequenceEqual(input.TransformationsIDs));
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
      if (TransformationsIDs != null)
      {
        hashCode = (hashCode * 59) + TransformationsIDs.GetHashCode();
      }
      return hashCode;
    }
  }

}


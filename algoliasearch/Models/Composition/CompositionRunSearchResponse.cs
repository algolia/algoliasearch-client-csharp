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

namespace Algolia.Search.Models.Composition;

/// <summary>
/// CompositionRunSearchResponse
/// </summary>
public partial class CompositionRunSearchResponse
{
  /// <summary>
  /// Initializes a new instance of the CompositionRunSearchResponse class.
  /// </summary>
  [JsonConstructor]
  public CompositionRunSearchResponse()
  {
    AdditionalProperties = new Dictionary<string, object>();
  }
  /// <summary>
  /// Initializes a new instance of the CompositionRunSearchResponse class.
  /// </summary>
  /// <param name="objectID">Unique record identifier. (required).</param>
  public CompositionRunSearchResponse(string objectID)
  {
    ObjectID = objectID ?? throw new ArgumentNullException(nameof(objectID));
    AdditionalProperties = new Dictionary<string, object>();
  }

  /// <summary>
  /// Unique record identifier.
  /// </summary>
  /// <value>Unique record identifier.</value>
  [JsonPropertyName("objectID")]
  public string ObjectID { get; set; }

  /// <summary>
  /// Gets or Sets AppliedRules
  /// </summary>
  [JsonPropertyName("appliedRules")]
  public List<CompositionRunAppliedRules> AppliedRules { get; set; }

  /// <summary>
  /// Gets or Sets additional properties
  /// </summary>
  [JsonExtensionData]
  public IDictionary<string, object> AdditionalProperties { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class CompositionRunSearchResponse {\n");
    sb.Append("  ObjectID: ").Append(ObjectID).Append("\n");
    sb.Append("  AppliedRules: ").Append(AppliedRules).Append("\n");
    sb.Append("  AdditionalProperties: ").Append(AdditionalProperties).Append("\n");
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
    if (obj is not CompositionRunSearchResponse input)
    {
      return false;
    }

    return
        (ObjectID == input.ObjectID || (ObjectID != null && ObjectID.Equals(input.ObjectID))) &&
        (AppliedRules == input.AppliedRules || AppliedRules != null && input.AppliedRules != null && AppliedRules.SequenceEqual(input.AppliedRules))
        && (AdditionalProperties.Count == input.AdditionalProperties.Count && !AdditionalProperties.Except(input.AdditionalProperties).Any());
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
      if (ObjectID != null)
      {
        hashCode = (hashCode * 59) + ObjectID.GetHashCode();
      }
      if (AppliedRules != null)
      {
        hashCode = (hashCode * 59) + AppliedRules.GetHashCode();
      }
      if (AdditionalProperties != null)
      {
        hashCode = (hashCode * 59) + AdditionalProperties.GetHashCode();
      }
      return hashCode;
    }
  }

}


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

namespace Algolia.Search.Models.Monitoring;

/// <summary>
/// IncidentsResponse
/// </summary>
public partial class IncidentsResponse
{
  /// <summary>
  /// Initializes a new instance of the IncidentsResponse class.
  /// </summary>
  public IncidentsResponse()
  {
  }

  /// <summary>
  /// Gets or Sets Incidents
  /// </summary>
  [JsonPropertyName("incidents")]
  public Dictionary<string, List<IncidentEntry>> Incidents { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class IncidentsResponse {\n");
    sb.Append("  Incidents: ").Append(Incidents).Append("\n");
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
    if (obj is not IncidentsResponse input)
    {
      return false;
    }

    return
        (Incidents == input.Incidents || Incidents != null && input.Incidents != null && Incidents.SequenceEqual(input.Incidents));
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
      if (Incidents != null)
      {
        hashCode = (hashCode * 59) + Incidents.GetHashCode();
      }
      return hashCode;
    }
  }

}

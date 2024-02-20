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
/// ListAuthenticationsResponse
/// </summary>
public partial class ListAuthenticationsResponse
{
  /// <summary>
  /// Initializes a new instance of the ListAuthenticationsResponse class.
  /// </summary>
  [JsonConstructor]
  public ListAuthenticationsResponse() { }
  /// <summary>
  /// Initializes a new instance of the ListAuthenticationsResponse class.
  /// </summary>
  /// <param name="authentications">authentications (required).</param>
  /// <param name="pagination">pagination (required).</param>
  public ListAuthenticationsResponse(List<Authentication> authentications, Pagination pagination)
  {
    Authentications = authentications ?? throw new ArgumentNullException(nameof(authentications));
    Pagination = pagination ?? throw new ArgumentNullException(nameof(pagination));
  }

  /// <summary>
  /// Gets or Sets Authentications
  /// </summary>
  [JsonPropertyName("authentications")]
  public List<Authentication> Authentications { get; set; }

  /// <summary>
  /// Gets or Sets Pagination
  /// </summary>
  [JsonPropertyName("pagination")]
  public Pagination Pagination { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class ListAuthenticationsResponse {\n");
    sb.Append("  Authentications: ").Append(Authentications).Append("\n");
    sb.Append("  Pagination: ").Append(Pagination).Append("\n");
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
    if (obj is not ListAuthenticationsResponse input)
    {
      return false;
    }

    return
        (Authentications == input.Authentications || Authentications != null && input.Authentications != null && Authentications.SequenceEqual(input.Authentications)) &&
        (Pagination == input.Pagination || (Pagination != null && Pagination.Equals(input.Pagination)));
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
      if (Authentications != null)
      {
        hashCode = (hashCode * 59) + Authentications.GetHashCode();
      }
      if (Pagination != null)
      {
        hashCode = (hashCode * 59) + Pagination.GetHashCode();
      }
      return hashCode;
    }
  }

}

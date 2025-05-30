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
/// API response for the successful creation of an authentication resource.
/// </summary>
public partial class AuthenticationCreateResponse
{
  /// <summary>
  /// Initializes a new instance of the AuthenticationCreateResponse class.
  /// </summary>
  [JsonConstructor]
  public AuthenticationCreateResponse() { }

  /// <summary>
  /// Initializes a new instance of the AuthenticationCreateResponse class.
  /// </summary>
  /// <param name="authenticationID">Universally unique identifier (UUID) of an authentication resource. (required).</param>
  /// <param name="name">Descriptive name for the resource. (required).</param>
  /// <param name="createdAt">Date of creation in RFC 3339 format. (required).</param>
  public AuthenticationCreateResponse(string authenticationID, string name, string createdAt)
  {
    AuthenticationID =
      authenticationID ?? throw new ArgumentNullException(nameof(authenticationID));
    Name = name ?? throw new ArgumentNullException(nameof(name));
    CreatedAt = createdAt ?? throw new ArgumentNullException(nameof(createdAt));
  }

  /// <summary>
  /// Universally unique identifier (UUID) of an authentication resource.
  /// </summary>
  /// <value>Universally unique identifier (UUID) of an authentication resource.</value>
  [JsonPropertyName("authenticationID")]
  public string AuthenticationID { get; set; }

  /// <summary>
  /// Descriptive name for the resource.
  /// </summary>
  /// <value>Descriptive name for the resource.</value>
  [JsonPropertyName("name")]
  public string Name { get; set; }

  /// <summary>
  /// Date of creation in RFC 3339 format.
  /// </summary>
  /// <value>Date of creation in RFC 3339 format.</value>
  [JsonPropertyName("createdAt")]
  public string CreatedAt { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class AuthenticationCreateResponse {\n");
    sb.Append("  AuthenticationID: ").Append(AuthenticationID).Append("\n");
    sb.Append("  Name: ").Append(Name).Append("\n");
    sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
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
    if (obj is not AuthenticationCreateResponse input)
    {
      return false;
    }

    return (
        AuthenticationID == input.AuthenticationID
        || (AuthenticationID != null && AuthenticationID.Equals(input.AuthenticationID))
      )
      && (Name == input.Name || (Name != null && Name.Equals(input.Name)))
      && (CreatedAt == input.CreatedAt || (CreatedAt != null && CreatedAt.Equals(input.CreatedAt)));
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
      if (AuthenticationID != null)
      {
        hashCode = (hashCode * 59) + AuthenticationID.GetHashCode();
      }
      if (Name != null)
      {
        hashCode = (hashCode * 59) + Name.GetHashCode();
      }
      if (CreatedAt != null)
      {
        hashCode = (hashCode * 59) + CreatedAt.GetHashCode();
      }
      return hashCode;
    }
  }
}

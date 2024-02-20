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
/// Authentication input for OAuth login.
/// </summary>
public partial class AuthOAuthPartial
{
  /// <summary>
  /// Initializes a new instance of the AuthOAuthPartial class.
  /// </summary>
  public AuthOAuthPartial()
  {
  }

  /// <summary>
  /// The OAuth endpoint URL.
  /// </summary>
  /// <value>The OAuth endpoint URL.</value>
  [JsonPropertyName("url")]
  public string Url { get; set; }

  /// <summary>
  /// The clientID.
  /// </summary>
  /// <value>The clientID.</value>
  [JsonPropertyName("client_id")]
  public string ClientId { get; set; }

  /// <summary>
  /// The secret.
  /// </summary>
  /// <value>The secret.</value>
  [JsonPropertyName("client_secret")]
  public string ClientSecret { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class AuthOAuthPartial {\n");
    sb.Append("  Url: ").Append(Url).Append("\n");
    sb.Append("  ClientId: ").Append(ClientId).Append("\n");
    sb.Append("  ClientSecret: ").Append(ClientSecret).Append("\n");
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
    if (obj is not AuthOAuthPartial input)
    {
      return false;
    }

    return
        (Url == input.Url || (Url != null && Url.Equals(input.Url))) &&
        (ClientId == input.ClientId || (ClientId != null && ClientId.Equals(input.ClientId))) &&
        (ClientSecret == input.ClientSecret || (ClientSecret != null && ClientSecret.Equals(input.ClientSecret)));
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
      if (ClientId != null)
      {
        hashCode = (hashCode * 59) + ClientId.GetHashCode();
      }
      if (ClientSecret != null)
      {
        hashCode = (hashCode * 59) + ClientSecret.GetHashCode();
      }
      return hashCode;
    }
  }

}

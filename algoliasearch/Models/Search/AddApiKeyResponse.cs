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

namespace Algolia.Search.Models.Search;

/// <summary>
/// AddApiKeyResponse
/// </summary>
public partial class AddApiKeyResponse
{
  /// <summary>
  /// Initializes a new instance of the AddApiKeyResponse class.
  /// </summary>
  [JsonConstructor]
  public AddApiKeyResponse() { }

  /// <summary>
  /// Initializes a new instance of the AddApiKeyResponse class.
  /// </summary>
  /// <param name="key">API key. (required).</param>
  /// <param name="createdAt">Date and time when the object was created, in RFC 3339 format. (required).</param>
  public AddApiKeyResponse(string key, string createdAt)
  {
    Key = key ?? throw new ArgumentNullException(nameof(key));
    CreatedAt = createdAt ?? throw new ArgumentNullException(nameof(createdAt));
  }

  /// <summary>
  /// API key.
  /// </summary>
  /// <value>API key.</value>
  [JsonPropertyName("key")]
  public string Key { get; set; }

  /// <summary>
  /// Date and time when the object was created, in RFC 3339 format.
  /// </summary>
  /// <value>Date and time when the object was created, in RFC 3339 format.</value>
  [JsonPropertyName("createdAt")]
  public string CreatedAt { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class AddApiKeyResponse {\n");
    sb.Append("  Key: ").Append(Key).Append("\n");
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
    if (obj is not AddApiKeyResponse input)
    {
      return false;
    }

    return (Key == input.Key || (Key != null && Key.Equals(input.Key)))
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
      if (Key != null)
      {
        hashCode = (hashCode * 59) + Key.GetHashCode();
      }
      if (CreatedAt != null)
      {
        hashCode = (hashCode * 59) + CreatedAt.GetHashCode();
      }
      return hashCode;
    }
  }
}

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
/// GenerateTransformationCodePayload
/// </summary>
public partial class GenerateTransformationCodePayload
{
  /// <summary>
  /// Initializes a new instance of the GenerateTransformationCodePayload class.
  /// </summary>
  [JsonConstructor]
  public GenerateTransformationCodePayload() { }
  /// <summary>
  /// Initializes a new instance of the GenerateTransformationCodePayload class.
  /// </summary>
  /// <param name="id">id (required).</param>
  /// <param name="userPrompt">userPrompt (required).</param>
  public GenerateTransformationCodePayload(string id, string userPrompt)
  {
    Id = id ?? throw new ArgumentNullException(nameof(id));
    UserPrompt = userPrompt ?? throw new ArgumentNullException(nameof(userPrompt));
  }

  /// <summary>
  /// Gets or Sets Id
  /// </summary>
  [JsonPropertyName("id")]
  public string Id { get; set; }

  /// <summary>
  /// Gets or Sets SystemPrompt
  /// </summary>
  [JsonPropertyName("systemPrompt")]
  public string SystemPrompt { get; set; }

  /// <summary>
  /// Gets or Sets UserPrompt
  /// </summary>
  [JsonPropertyName("userPrompt")]
  public string UserPrompt { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class GenerateTransformationCodePayload {\n");
    sb.Append("  Id: ").Append(Id).Append("\n");
    sb.Append("  SystemPrompt: ").Append(SystemPrompt).Append("\n");
    sb.Append("  UserPrompt: ").Append(UserPrompt).Append("\n");
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
    if (obj is not GenerateTransformationCodePayload input)
    {
      return false;
    }

    return
        (Id == input.Id || (Id != null && Id.Equals(input.Id))) &&
        (SystemPrompt == input.SystemPrompt || (SystemPrompt != null && SystemPrompt.Equals(input.SystemPrompt))) &&
        (UserPrompt == input.UserPrompt || (UserPrompt != null && UserPrompt.Equals(input.UserPrompt)));
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
      if (Id != null)
      {
        hashCode = (hashCode * 59) + Id.GetHashCode();
      }
      if (SystemPrompt != null)
      {
        hashCode = (hashCode * 59) + SystemPrompt.GetHashCode();
      }
      if (UserPrompt != null)
      {
        hashCode = (hashCode * 59) + UserPrompt.GetHashCode();
      }
      return hashCode;
    }
  }

}


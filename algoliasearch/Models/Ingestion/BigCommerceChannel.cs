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
/// BigCommerceChannel
/// </summary>
public partial class BigCommerceChannel
{
  /// <summary>
  /// Initializes a new instance of the BigCommerceChannel class.
  /// </summary>
  [JsonConstructor]
  public BigCommerceChannel() { }

  /// <summary>
  /// Initializes a new instance of the BigCommerceChannel class.
  /// </summary>
  /// <param name="id">ID of the BigCommerce channel. (required).</param>
  public BigCommerceChannel(int id)
  {
    Id = id;
  }

  /// <summary>
  /// ID of the BigCommerce channel.
  /// </summary>
  /// <value>ID of the BigCommerce channel.</value>
  [JsonPropertyName("id")]
  public int Id { get; set; }

  /// <summary>
  /// Currencies for the given channel.
  /// </summary>
  /// <value>Currencies for the given channel.</value>
  [JsonPropertyName("currencies")]
  public List<string> Currencies { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class BigCommerceChannel {\n");
    sb.Append("  Id: ").Append(Id).Append("\n");
    sb.Append("  Currencies: ").Append(Currencies).Append("\n");
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
    if (obj is not BigCommerceChannel input)
    {
      return false;
    }

    return (Id == input.Id || Id.Equals(input.Id))
      && (
        Currencies == input.Currencies
        || Currencies != null
          && input.Currencies != null
          && Currencies.SequenceEqual(input.Currencies)
      );
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
      hashCode = (hashCode * 59) + Id.GetHashCode();
      if (Currencies != null)
      {
        hashCode = (hashCode * 59) + Currencies.GetHashCode();
      }
      return hashCode;
    }
  }
}

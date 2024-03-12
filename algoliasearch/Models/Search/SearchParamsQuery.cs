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

namespace Algolia.Search.Models.Search;

/// <summary>
/// SearchParamsQuery
/// </summary>
public partial class SearchParamsQuery
{
  /// <summary>
  /// Initializes a new instance of the SearchParamsQuery class.
  /// </summary>
  public SearchParamsQuery()
  {
  }

  /// <summary>
  /// Search query.
  /// </summary>
  /// <value>Search query.</value>
  [JsonPropertyName("query")]
  public string Query { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class SearchParamsQuery {\n");
    sb.Append("  Query: ").Append(Query).Append("\n");
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
    if (obj is not SearchParamsQuery input)
    {
      return false;
    }

    return
        (Query == input.Query || (Query != null && Query.Equals(input.Query)));
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
      if (Query != null)
      {
        hashCode = (hashCode * 59) + Query.GetHashCode();
      }
      return hashCode;
    }
  }

}


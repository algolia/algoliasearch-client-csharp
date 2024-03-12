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
/// LogQuery
/// </summary>
public partial class LogQuery
{
  /// <summary>
  /// Initializes a new instance of the LogQuery class.
  /// </summary>
  public LogQuery()
  {
  }

  /// <summary>
  /// Index targeted by the query.
  /// </summary>
  /// <value>Index targeted by the query.</value>
  [JsonPropertyName("index_name")]
  public string IndexName { get; set; }

  /// <summary>
  /// A user identifier.
  /// </summary>
  /// <value>A user identifier.</value>
  [JsonPropertyName("user_token")]
  public string UserToken { get; set; }

  /// <summary>
  /// Unique query identifier.
  /// </summary>
  /// <value>Unique query identifier.</value>
  [JsonPropertyName("query_id")]
  public string QueryId { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class LogQuery {\n");
    sb.Append("  IndexName: ").Append(IndexName).Append("\n");
    sb.Append("  UserToken: ").Append(UserToken).Append("\n");
    sb.Append("  QueryId: ").Append(QueryId).Append("\n");
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
    if (obj is not LogQuery input)
    {
      return false;
    }

    return
        (IndexName == input.IndexName || (IndexName != null && IndexName.Equals(input.IndexName))) &&
        (UserToken == input.UserToken || (UserToken != null && UserToken.Equals(input.UserToken))) &&
        (QueryId == input.QueryId || (QueryId != null && QueryId.Equals(input.QueryId)));
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
      if (IndexName != null)
      {
        hashCode = (hashCode * 59) + IndexName.GetHashCode();
      }
      if (UserToken != null)
      {
        hashCode = (hashCode * 59) + UserToken.GetHashCode();
      }
      if (QueryId != null)
      {
        hashCode = (hashCode * 59) + QueryId.GetHashCode();
      }
      return hashCode;
    }
  }

}


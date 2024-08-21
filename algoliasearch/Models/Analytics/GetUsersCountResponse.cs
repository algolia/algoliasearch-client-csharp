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

namespace Algolia.Search.Models.Analytics;

/// <summary>
/// GetUsersCountResponse
/// </summary>
public partial class GetUsersCountResponse
{
  /// <summary>
  /// Initializes a new instance of the GetUsersCountResponse class.
  /// </summary>
  [JsonConstructor]
  public GetUsersCountResponse() { }
  /// <summary>
  /// Initializes a new instance of the GetUsersCountResponse class.
  /// </summary>
  /// <param name="count">Number of unique users. (required).</param>
  /// <param name="dates">Daily number of unique users. (required).</param>
  public GetUsersCountResponse(int count, List<DailyUsers> dates)
  {
    Count = count;
    Dates = dates ?? throw new ArgumentNullException(nameof(dates));
  }

  /// <summary>
  /// Number of unique users.
  /// </summary>
  /// <value>Number of unique users.</value>
  [JsonPropertyName("count")]
  public int Count { get; set; }

  /// <summary>
  /// Daily number of unique users.
  /// </summary>
  /// <value>Daily number of unique users.</value>
  [JsonPropertyName("dates")]
  public List<DailyUsers> Dates { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class GetUsersCountResponse {\n");
    sb.Append("  Count: ").Append(Count).Append("\n");
    sb.Append("  Dates: ").Append(Dates).Append("\n");
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
    if (obj is not GetUsersCountResponse input)
    {
      return false;
    }

    return
        (Count == input.Count || Count.Equals(input.Count)) &&
        (Dates == input.Dates || Dates != null && input.Dates != null && Dates.SequenceEqual(input.Dates));
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
      hashCode = (hashCode * 59) + Count.GetHashCode();
      if (Dates != null)
      {
        hashCode = (hashCode * 59) + Dates.GetHashCode();
      }
      return hashCode;
    }
  }

}

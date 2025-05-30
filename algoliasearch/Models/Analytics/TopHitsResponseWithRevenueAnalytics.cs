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

namespace Algolia.Search.Models.Analytics;

/// <summary>
/// TopHitsResponseWithRevenueAnalytics
/// </summary>
public partial class TopHitsResponseWithRevenueAnalytics
{
  /// <summary>
  /// Initializes a new instance of the TopHitsResponseWithRevenueAnalytics class.
  /// </summary>
  [JsonConstructor]
  public TopHitsResponseWithRevenueAnalytics() { }

  /// <summary>
  /// Initializes a new instance of the TopHitsResponseWithRevenueAnalytics class.
  /// </summary>
  /// <param name="hits">Most frequent search results with click, conversion, and revenue metrics. (required).</param>
  public TopHitsResponseWithRevenueAnalytics(List<TopHitWithRevenueAnalytics> hits)
  {
    Hits = hits ?? throw new ArgumentNullException(nameof(hits));
  }

  /// <summary>
  /// Most frequent search results with click, conversion, and revenue metrics.
  /// </summary>
  /// <value>Most frequent search results with click, conversion, and revenue metrics.</value>
  [JsonPropertyName("hits")]
  public List<TopHitWithRevenueAnalytics> Hits { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class TopHitsResponseWithRevenueAnalytics {\n");
    sb.Append("  Hits: ").Append(Hits).Append("\n");
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
    if (obj is not TopHitsResponseWithRevenueAnalytics input)
    {
      return false;
    }

    return (
      Hits == input.Hits || Hits != null && input.Hits != null && Hits.SequenceEqual(input.Hits)
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
      if (Hits != null)
      {
        hashCode = (hashCode * 59) + Hits.GetHashCode();
      }
      return hashCode;
    }
  }
}

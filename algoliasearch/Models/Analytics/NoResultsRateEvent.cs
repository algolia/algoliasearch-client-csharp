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
/// NoResultsRateEvent
/// </summary>
public partial class NoResultsRateEvent
{
  /// <summary>
  /// Initializes a new instance of the NoResultsRateEvent class.
  /// </summary>
  [JsonConstructor]
  public NoResultsRateEvent() { }
  /// <summary>
  /// Initializes a new instance of the NoResultsRateEvent class.
  /// </summary>
  /// <param name="date">Date of the event in the format YYYY-MM-DD. (required).</param>
  /// <param name="noResultCount">Number of occurences. (required).</param>
  /// <param name="count">Number of tracked _and_ untracked searches (where the &#x60;clickAnalytics&#x60; parameter isn&#39;t &#x60;true&#x60;). (required).</param>
  /// <param name="rate">[Click-through rate (CTR)](https://www.algolia.com/doc/guides/search-analytics/concepts/metrics/#click-through-rate).  (required).</param>
  public NoResultsRateEvent(string date, int noResultCount, int count, double rate)
  {
    Date = date ?? throw new ArgumentNullException(nameof(date));
    NoResultCount = noResultCount;
    Count = count;
    Rate = rate;
  }

  /// <summary>
  /// Date of the event in the format YYYY-MM-DD.
  /// </summary>
  /// <value>Date of the event in the format YYYY-MM-DD.</value>
  [JsonPropertyName("date")]
  public string Date { get; set; }

  /// <summary>
  /// Number of occurences.
  /// </summary>
  /// <value>Number of occurences.</value>
  [JsonPropertyName("noResultCount")]
  public int NoResultCount { get; set; }

  /// <summary>
  /// Number of tracked _and_ untracked searches (where the `clickAnalytics` parameter isn't `true`).
  /// </summary>
  /// <value>Number of tracked _and_ untracked searches (where the `clickAnalytics` parameter isn't `true`).</value>
  [JsonPropertyName("count")]
  public int Count { get; set; }

  /// <summary>
  /// [Click-through rate (CTR)](https://www.algolia.com/doc/guides/search-analytics/concepts/metrics/#click-through-rate). 
  /// </summary>
  /// <value>[Click-through rate (CTR)](https://www.algolia.com/doc/guides/search-analytics/concepts/metrics/#click-through-rate). </value>
  [JsonPropertyName("rate")]
  public double Rate { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class NoResultsRateEvent {\n");
    sb.Append("  Date: ").Append(Date).Append("\n");
    sb.Append("  NoResultCount: ").Append(NoResultCount).Append("\n");
    sb.Append("  Count: ").Append(Count).Append("\n");
    sb.Append("  Rate: ").Append(Rate).Append("\n");
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
    if (obj is not NoResultsRateEvent input)
    {
      return false;
    }

    return
        (Date == input.Date || (Date != null && Date.Equals(input.Date))) &&
        (NoResultCount == input.NoResultCount || NoResultCount.Equals(input.NoResultCount)) &&
        (Count == input.Count || Count.Equals(input.Count)) &&
        (Rate == input.Rate || Rate.Equals(input.Rate));
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
      if (Date != null)
      {
        hashCode = (hashCode * 59) + Date.GetHashCode();
      }
      hashCode = (hashCode * 59) + NoResultCount.GetHashCode();
      hashCode = (hashCode * 59) + Count.GetHashCode();
      hashCode = (hashCode * 59) + Rate.GetHashCode();
      return hashCode;
    }
  }

}

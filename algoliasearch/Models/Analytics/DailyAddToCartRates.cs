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
/// DailyAddToCartRates
/// </summary>
public partial class DailyAddToCartRates
{
  /// <summary>
  /// Initializes a new instance of the DailyAddToCartRates class.
  /// </summary>
  [JsonConstructor]
  public DailyAddToCartRates() { }
  /// <summary>
  /// Initializes a new instance of the DailyAddToCartRates class.
  /// </summary>
  /// <param name="rate">Add-to-cart rate, calculated as number of tracked searches with at least one add-to-cart event divided by the number of tracked searches. If null, Algolia didn&#39;t receive any search requests with &#x60;clickAnalytics&#x60; set to true.  (required).</param>
  /// <param name="trackedSearchCount">Number of tracked searches. Tracked searches are search requests where the &#x60;clickAnalytics&#x60; parameter is true. (required) (default to 0).</param>
  /// <param name="addToCartCount">Number of add-to-cart events from this search. (required) (default to 0).</param>
  /// <param name="date">Date in the format YYYY-MM-DD. (required).</param>
  public DailyAddToCartRates(double? rate, int trackedSearchCount, int addToCartCount, string date)
  {
    Rate = rate ?? throw new ArgumentNullException(nameof(rate));
    TrackedSearchCount = trackedSearchCount;
    AddToCartCount = addToCartCount;
    Date = date ?? throw new ArgumentNullException(nameof(date));
  }

  /// <summary>
  /// Add-to-cart rate, calculated as number of tracked searches with at least one add-to-cart event divided by the number of tracked searches. If null, Algolia didn't receive any search requests with `clickAnalytics` set to true. 
  /// </summary>
  /// <value>Add-to-cart rate, calculated as number of tracked searches with at least one add-to-cart event divided by the number of tracked searches. If null, Algolia didn't receive any search requests with `clickAnalytics` set to true. </value>
  [JsonPropertyName("rate")]
  public double? Rate { get; set; }

  /// <summary>
  /// Number of tracked searches. Tracked searches are search requests where the `clickAnalytics` parameter is true.
  /// </summary>
  /// <value>Number of tracked searches. Tracked searches are search requests where the `clickAnalytics` parameter is true.</value>
  [JsonPropertyName("trackedSearchCount")]
  public int TrackedSearchCount { get; set; }

  /// <summary>
  /// Number of add-to-cart events from this search.
  /// </summary>
  /// <value>Number of add-to-cart events from this search.</value>
  [JsonPropertyName("addToCartCount")]
  public int AddToCartCount { get; set; }

  /// <summary>
  /// Date in the format YYYY-MM-DD.
  /// </summary>
  /// <value>Date in the format YYYY-MM-DD.</value>
  [JsonPropertyName("date")]
  public string Date { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class DailyAddToCartRates {\n");
    sb.Append("  Rate: ").Append(Rate).Append("\n");
    sb.Append("  TrackedSearchCount: ").Append(TrackedSearchCount).Append("\n");
    sb.Append("  AddToCartCount: ").Append(AddToCartCount).Append("\n");
    sb.Append("  Date: ").Append(Date).Append("\n");
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
    if (obj is not DailyAddToCartRates input)
    {
      return false;
    }

    return
        (Rate == input.Rate || (Rate != null && Rate.Equals(input.Rate))) &&
        (TrackedSearchCount == input.TrackedSearchCount || TrackedSearchCount.Equals(input.TrackedSearchCount)) &&
        (AddToCartCount == input.AddToCartCount || AddToCartCount.Equals(input.AddToCartCount)) &&
        (Date == input.Date || (Date != null && Date.Equals(input.Date)));
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
      if (Rate != null)
      {
        hashCode = (hashCode * 59) + Rate.GetHashCode();
      }
      hashCode = (hashCode * 59) + TrackedSearchCount.GetHashCode();
      hashCode = (hashCode * 59) + AddToCartCount.GetHashCode();
      if (Date != null)
      {
        hashCode = (hashCode * 59) + Date.GetHashCode();
      }
      return hashCode;
    }
  }

}


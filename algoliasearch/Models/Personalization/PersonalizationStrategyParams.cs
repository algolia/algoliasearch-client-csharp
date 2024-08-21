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

namespace Algolia.Search.Models.Personalization;

/// <summary>
/// PersonalizationStrategyParams
/// </summary>
public partial class PersonalizationStrategyParams
{
  /// <summary>
  /// Initializes a new instance of the PersonalizationStrategyParams class.
  /// </summary>
  [JsonConstructor]
  public PersonalizationStrategyParams() { }
  /// <summary>
  /// Initializes a new instance of the PersonalizationStrategyParams class.
  /// </summary>
  /// <param name="eventScoring">Scores associated with each event.  The higher the scores, the higher the impact of those events on the personalization of search results.  (required).</param>
  /// <param name="facetScoring">Scores associated with each facet.  The higher the scores, the higher the impact of those events on the personalization of search results.  (required).</param>
  /// <param name="personalizationImpact">Impact of personalization on the search results.  If set to 0, personalization has no impact on the search results.  (required).</param>
  public PersonalizationStrategyParams(List<EventScoring> eventScoring, List<FacetScoring> facetScoring, int personalizationImpact)
  {
    EventScoring = eventScoring ?? throw new ArgumentNullException(nameof(eventScoring));
    FacetScoring = facetScoring ?? throw new ArgumentNullException(nameof(facetScoring));
    PersonalizationImpact = personalizationImpact;
  }

  /// <summary>
  /// Scores associated with each event.  The higher the scores, the higher the impact of those events on the personalization of search results. 
  /// </summary>
  /// <value>Scores associated with each event.  The higher the scores, the higher the impact of those events on the personalization of search results. </value>
  [JsonPropertyName("eventScoring")]
  public List<EventScoring> EventScoring { get; set; }

  /// <summary>
  /// Scores associated with each facet.  The higher the scores, the higher the impact of those events on the personalization of search results. 
  /// </summary>
  /// <value>Scores associated with each facet.  The higher the scores, the higher the impact of those events on the personalization of search results. </value>
  [JsonPropertyName("facetScoring")]
  public List<FacetScoring> FacetScoring { get; set; }

  /// <summary>
  /// Impact of personalization on the search results.  If set to 0, personalization has no impact on the search results. 
  /// </summary>
  /// <value>Impact of personalization on the search results.  If set to 0, personalization has no impact on the search results. </value>
  [JsonPropertyName("personalizationImpact")]
  public int PersonalizationImpact { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class PersonalizationStrategyParams {\n");
    sb.Append("  EventScoring: ").Append(EventScoring).Append("\n");
    sb.Append("  FacetScoring: ").Append(FacetScoring).Append("\n");
    sb.Append("  PersonalizationImpact: ").Append(PersonalizationImpact).Append("\n");
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
    if (obj is not PersonalizationStrategyParams input)
    {
      return false;
    }

    return
        (EventScoring == input.EventScoring || EventScoring != null && input.EventScoring != null && EventScoring.SequenceEqual(input.EventScoring)) &&
        (FacetScoring == input.FacetScoring || FacetScoring != null && input.FacetScoring != null && FacetScoring.SequenceEqual(input.FacetScoring)) &&
        (PersonalizationImpact == input.PersonalizationImpact || PersonalizationImpact.Equals(input.PersonalizationImpact));
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
      if (EventScoring != null)
      {
        hashCode = (hashCode * 59) + EventScoring.GetHashCode();
      }
      if (FacetScoring != null)
      {
        hashCode = (hashCode * 59) + FacetScoring.GetHashCode();
      }
      hashCode = (hashCode * 59) + PersonalizationImpact.GetHashCode();
      return hashCode;
    }
  }

}

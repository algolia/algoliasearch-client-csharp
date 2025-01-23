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

namespace Algolia.Search.Models.Recommend;

/// <summary>
/// TrendingFacetsQuery
/// </summary>
public partial class TrendingFacetsQuery
{

  /// <summary>
  /// Gets or Sets Model
  /// </summary>
  [JsonPropertyName("model")]
  public TrendingFacetsModel? Model { get; set; }
  /// <summary>
  /// Initializes a new instance of the TrendingFacetsQuery class.
  /// </summary>
  [JsonConstructor]
  public TrendingFacetsQuery() { }
  /// <summary>
  /// Initializes a new instance of the TrendingFacetsQuery class.
  /// </summary>
  /// <param name="indexName">Index name (case-sensitive). (required).</param>
  /// <param name="threshold">Minimum score a recommendation must have to be included in the response. (required).</param>
  /// <param name="facetName">Facet attribute for which to retrieve trending facet values. (required).</param>
  /// <param name="model">model (required).</param>
  public TrendingFacetsQuery(string indexName, double threshold, string facetName, TrendingFacetsModel? model)
  {
    IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
    Threshold = threshold;
    FacetName = facetName ?? throw new ArgumentNullException(nameof(facetName));
    Model = model;
  }

  /// <summary>
  /// Index name (case-sensitive).
  /// </summary>
  /// <value>Index name (case-sensitive).</value>
  [JsonPropertyName("indexName")]
  public string IndexName { get; set; }

  /// <summary>
  /// Minimum score a recommendation must have to be included in the response.
  /// </summary>
  /// <value>Minimum score a recommendation must have to be included in the response.</value>
  [JsonPropertyName("threshold")]
  public double Threshold { get; set; }

  /// <summary>
  /// Maximum number of recommendations to retrieve. By default, all recommendations are returned and no fallback request is made. Depending on the available recommendations and the other request parameters, the actual number of recommendations may be lower than this value. 
  /// </summary>
  /// <value>Maximum number of recommendations to retrieve. By default, all recommendations are returned and no fallback request is made. Depending on the available recommendations and the other request parameters, the actual number of recommendations may be lower than this value. </value>
  [JsonPropertyName("maxRecommendations")]
  public int? MaxRecommendations { get; set; }

  /// <summary>
  /// Gets or Sets QueryParameters
  /// </summary>
  [JsonPropertyName("queryParameters")]
  public RecommendSearchParams QueryParameters { get; set; }

  /// <summary>
  /// Facet attribute for which to retrieve trending facet values.
  /// </summary>
  /// <value>Facet attribute for which to retrieve trending facet values.</value>
  [JsonPropertyName("facetName")]
  public string FacetName { get; set; }

  /// <summary>
  /// Gets or Sets FallbackParameters
  /// </summary>
  [JsonPropertyName("fallbackParameters")]
  public FallbackParams FallbackParameters { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class TrendingFacetsQuery {\n");
    sb.Append("  IndexName: ").Append(IndexName).Append("\n");
    sb.Append("  Threshold: ").Append(Threshold).Append("\n");
    sb.Append("  MaxRecommendations: ").Append(MaxRecommendations).Append("\n");
    sb.Append("  QueryParameters: ").Append(QueryParameters).Append("\n");
    sb.Append("  FacetName: ").Append(FacetName).Append("\n");
    sb.Append("  Model: ").Append(Model).Append("\n");
    sb.Append("  FallbackParameters: ").Append(FallbackParameters).Append("\n");
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
    if (obj is not TrendingFacetsQuery input)
    {
      return false;
    }

    return
        (IndexName == input.IndexName || (IndexName != null && IndexName.Equals(input.IndexName))) &&
        (Threshold == input.Threshold || Threshold.Equals(input.Threshold)) &&
        (MaxRecommendations == input.MaxRecommendations || MaxRecommendations.Equals(input.MaxRecommendations)) &&
        (QueryParameters == input.QueryParameters || (QueryParameters != null && QueryParameters.Equals(input.QueryParameters))) &&
        (FacetName == input.FacetName || (FacetName != null && FacetName.Equals(input.FacetName))) &&
        (Model == input.Model || Model.Equals(input.Model)) &&
        (FallbackParameters == input.FallbackParameters || (FallbackParameters != null && FallbackParameters.Equals(input.FallbackParameters)));
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
      hashCode = (hashCode * 59) + Threshold.GetHashCode();
      hashCode = (hashCode * 59) + MaxRecommendations.GetHashCode();
      if (QueryParameters != null)
      {
        hashCode = (hashCode * 59) + QueryParameters.GetHashCode();
      }
      if (FacetName != null)
      {
        hashCode = (hashCode * 59) + FacetName.GetHashCode();
      }
      hashCode = (hashCode * 59) + Model.GetHashCode();
      if (FallbackParameters != null)
      {
        hashCode = (hashCode * 59) + FallbackParameters.GetHashCode();
      }
      return hashCode;
    }
  }

}


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

namespace Algolia.Search.Models.Search;

/// <summary>
/// Filter or optional filter to be applied to the search.
/// </summary>
public partial class AutomaticFacetFilter
{
  /// <summary>
  /// Initializes a new instance of the AutomaticFacetFilter class.
  /// </summary>
  [JsonConstructor]
  public AutomaticFacetFilter() { }

  /// <summary>
  /// Initializes a new instance of the AutomaticFacetFilter class.
  /// </summary>
  /// <param name="facet">Facet name to be applied as filter. The name must match placeholders in the `pattern` parameter. For example, with `pattern: {facet:genre}`, `automaticFacetFilters` must be `genre`.  (required).</param>
  public AutomaticFacetFilter(string facet)
  {
    Facet = facet ?? throw new ArgumentNullException(nameof(facet));
  }

  /// <summary>
  /// Facet name to be applied as filter. The name must match placeholders in the `pattern` parameter. For example, with `pattern: {facet:genre}`, `automaticFacetFilters` must be `genre`.
  /// </summary>
  /// <value>Facet name to be applied as filter. The name must match placeholders in the `pattern` parameter. For example, with `pattern: {facet:genre}`, `automaticFacetFilters` must be `genre`. </value>
  [JsonPropertyName("facet")]
  public string Facet { get; set; }

  /// <summary>
  /// Filter scores to give different weights to individual filters.
  /// </summary>
  /// <value>Filter scores to give different weights to individual filters.</value>
  [JsonPropertyName("score")]
  public int? Score { get; set; }

  /// <summary>
  /// Whether the filter is disjunctive or conjunctive.  If true the filter has multiple matches, multiple occurences are combined with the logical `OR` operation. If false, multiple occurences are combined with the logical `AND` operation.
  /// </summary>
  /// <value>Whether the filter is disjunctive or conjunctive.  If true the filter has multiple matches, multiple occurences are combined with the logical `OR` operation. If false, multiple occurences are combined with the logical `AND` operation. </value>
  [JsonPropertyName("disjunctive")]
  public bool? Disjunctive { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class AutomaticFacetFilter {\n");
    sb.Append("  Facet: ").Append(Facet).Append("\n");
    sb.Append("  Score: ").Append(Score).Append("\n");
    sb.Append("  Disjunctive: ").Append(Disjunctive).Append("\n");
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
    if (obj is not AutomaticFacetFilter input)
    {
      return false;
    }

    return (Facet == input.Facet || (Facet != null && Facet.Equals(input.Facet)))
      && (Score == input.Score || Score.Equals(input.Score))
      && (Disjunctive == input.Disjunctive || Disjunctive.Equals(input.Disjunctive));
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
      if (Facet != null)
      {
        hashCode = (hashCode * 59) + Facet.GetHashCode();
      }
      hashCode = (hashCode * 59) + Score.GetHashCode();
      hashCode = (hashCode * 59) + Disjunctive.GetHashCode();
      return hashCode;
    }
  }
}

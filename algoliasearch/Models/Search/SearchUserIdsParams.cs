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
/// OK
/// </summary>
public partial class SearchUserIdsParams
{
  /// <summary>
  /// Initializes a new instance of the SearchUserIdsParams class.
  /// </summary>
  [JsonConstructor]
  public SearchUserIdsParams() { }
  /// <summary>
  /// Initializes a new instance of the SearchUserIdsParams class.
  /// </summary>
  /// <param name="query">Query to search. The search is a prefix search with [typo tolerance](https://www.algolia.com/doc/guides/managing-results/optimize-search-results/typo-tolerance/) enabled. An empty query will retrieve all users. (required).</param>
  public SearchUserIdsParams(string query)
  {
    Query = query ?? throw new ArgumentNullException(nameof(query));
  }

  /// <summary>
  /// Query to search. The search is a prefix search with [typo tolerance](https://www.algolia.com/doc/guides/managing-results/optimize-search-results/typo-tolerance/) enabled. An empty query will retrieve all users.
  /// </summary>
  /// <value>Query to search. The search is a prefix search with [typo tolerance](https://www.algolia.com/doc/guides/managing-results/optimize-search-results/typo-tolerance/) enabled. An empty query will retrieve all users.</value>
  [JsonPropertyName("query")]
  public string Query { get; set; }

  /// <summary>
  /// Cluster name.
  /// </summary>
  /// <value>Cluster name.</value>
  [JsonPropertyName("clusterName")]
  public string ClusterName { get; set; }

  /// <summary>
  /// Page of search results to retrieve.
  /// </summary>
  /// <value>Page of search results to retrieve.</value>
  [JsonPropertyName("page")]
  public int? Page { get; set; }

  /// <summary>
  /// Number of hits per page.
  /// </summary>
  /// <value>Number of hits per page.</value>
  [JsonPropertyName("hitsPerPage")]
  public int? HitsPerPage { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class SearchUserIdsParams {\n");
    sb.Append("  Query: ").Append(Query).Append("\n");
    sb.Append("  ClusterName: ").Append(ClusterName).Append("\n");
    sb.Append("  Page: ").Append(Page).Append("\n");
    sb.Append("  HitsPerPage: ").Append(HitsPerPage).Append("\n");
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
    if (obj is not SearchUserIdsParams input)
    {
      return false;
    }

    return
        (Query == input.Query || (Query != null && Query.Equals(input.Query))) &&
        (ClusterName == input.ClusterName || (ClusterName != null && ClusterName.Equals(input.ClusterName))) &&
        (Page == input.Page || Page.Equals(input.Page)) &&
        (HitsPerPage == input.HitsPerPage || HitsPerPage.Equals(input.HitsPerPage));
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
      if (ClusterName != null)
      {
        hashCode = (hashCode * 59) + ClusterName.GetHashCode();
      }
      hashCode = (hashCode * 59) + Page.GetHashCode();
      hashCode = (hashCode * 59) + HitsPerPage.GetHashCode();
      return hashCode;
    }
  }

}

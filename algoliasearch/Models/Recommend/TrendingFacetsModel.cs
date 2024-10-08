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
/// Trending facet values model.  This model recommends trending facet values for the specified facet attribute. 
/// </summary>
/// <value>Trending facet values model.  This model recommends trending facet values for the specified facet attribute. </value>
[JsonConverter(typeof(Serializer.JsonStringEnumConverter<TrendingFacetsModel>))]
public enum TrendingFacetsModel
{
  /// <summary>
  /// Enum TrendingFacets for value: trending-facets
  /// </summary>
  [JsonPropertyName("trending-facets")]
  TrendingFacets = 1
}


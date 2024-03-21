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
/// Character that characterizes how the filter is applied.  For example, for a facet filter `facet:value`, `:` is the operator. For a numeric filter `count>50`, `>` is the operator. 
/// </summary>
/// <value>Character that characterizes how the filter is applied.  For example, for a facet filter `facet:value`, `:` is the operator. For a numeric filter `count>50`, `>` is the operator. </value>
public enum Operator
{
  /// <summary>
  /// Enum Colon for value: :
  /// </summary>
  [JsonPropertyName(":")]
  Colon = 1,

  /// <summary>
  /// Enum LessThan for value: &lt;
  /// </summary>
  [JsonPropertyName("<")]
  LessThan = 2,

  /// <summary>
  /// Enum LessThanOrEqualTo for value: &lt;&#x3D;
  /// </summary>
  [JsonPropertyName("<=")]
  LessThanOrEqualTo = 3,

  /// <summary>
  /// Enum Equal for value: &#x3D;
  /// </summary>
  [JsonPropertyName("=")]
  Equal = 4,

  /// <summary>
  /// Enum NotEqual for value: !&#x3D;
  /// </summary>
  [JsonPropertyName("!=")]
  NotEqual = 5,

  /// <summary>
  /// Enum GreaterThan for value: &gt;
  /// </summary>
  [JsonPropertyName(">")]
  GreaterThan = 6,

  /// <summary>
  /// Enum GreaterThanOrEqualTo for value: &gt;&#x3D;
  /// </summary>
  [JsonPropertyName(">=")]
  GreaterThanOrEqualTo = 7
}


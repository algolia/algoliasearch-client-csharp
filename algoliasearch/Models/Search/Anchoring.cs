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
/// Whether the pattern parameter matches the beginning (`startsWith`) or end (`endsWith`) of the query string, is an exact match (`is`), or a partial match (`contains`).
/// </summary>
/// <value>Whether the pattern parameter matches the beginning (`startsWith`) or end (`endsWith`) of the query string, is an exact match (`is`), or a partial match (`contains`).</value>
public enum Anchoring
{
  /// <summary>
  /// Enum Is for value: is
  /// </summary>
  [JsonPropertyName("is")]
  Is = 1,

  /// <summary>
  /// Enum StartsWith for value: startsWith
  /// </summary>
  [JsonPropertyName("startsWith")]
  StartsWith = 2,

  /// <summary>
  /// Enum EndsWith for value: endsWith
  /// </summary>
  [JsonPropertyName("endsWith")]
  EndsWith = 3,

  /// <summary>
  /// Enum Contains for value: contains
  /// </summary>
  [JsonPropertyName("contains")]
  Contains = 4
}

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

namespace Algolia.Search.Models.Recommend;

/// <summary>
/// Defines booleanString
/// </summary>
[JsonConverter(typeof(Serializer.JsonStringEnumConverter<BooleanString>))]
public enum BooleanString
{
  /// <summary>
  /// Enum True for value: true
  /// </summary>
  [JsonPropertyName("true")]
  True = 1,

  /// <summary>
  /// Enum False for value: false
  /// </summary>
  [JsonPropertyName("false")]
  False = 2,
}

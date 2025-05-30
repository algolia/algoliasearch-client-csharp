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

namespace Algolia.Search.Models.Ingestion;

/// <summary>
/// Defines MappingTypeCSV
/// </summary>
[JsonConverter(typeof(Serializer.JsonStringEnumConverter<MappingTypeCSV>))]
public enum MappingTypeCSV
{
  /// <summary>
  /// Enum String for value: string
  /// </summary>
  [JsonPropertyName("string")]
  String = 1,

  /// <summary>
  /// Enum Integer for value: integer
  /// </summary>
  [JsonPropertyName("integer")]
  Integer = 2,

  /// <summary>
  /// Enum Float for value: float
  /// </summary>
  [JsonPropertyName("float")]
  Float = 3,

  /// <summary>
  /// Enum Boolean for value: boolean
  /// </summary>
  [JsonPropertyName("boolean")]
  Boolean = 4,

  /// <summary>
  /// Enum Json for value: json
  /// </summary>
  [JsonPropertyName("json")]
  Json = 5,
}

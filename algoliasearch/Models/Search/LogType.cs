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
/// Defines logType
/// </summary>
[JsonConverter(typeof(Serializer.JsonStringEnumConverter<LogType>))]
public enum LogType
{
  /// <summary>
  /// Enum All for value: all
  /// </summary>
  [JsonPropertyName("all")]
  All = 1,

  /// <summary>
  /// Enum Query for value: query
  /// </summary>
  [JsonPropertyName("query")]
  Query = 2,

  /// <summary>
  /// Enum Build for value: build
  /// </summary>
  [JsonPropertyName("build")]
  Build = 3,

  /// <summary>
  /// Enum Error for value: error
  /// </summary>
  [JsonPropertyName("error")]
  Error = 4,
}

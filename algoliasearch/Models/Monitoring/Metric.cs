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

namespace Algolia.Search.Models.Monitoring;

/// <summary>
/// Defines Metric
/// </summary>
[JsonConverter(typeof(Serializer.JsonStringEnumConverter<Metric>))]
public enum Metric
{
  /// <summary>
  /// Enum AvgBuildTime for value: avg_build_time
  /// </summary>
  [JsonPropertyName("avg_build_time")]
  AvgBuildTime = 1,

  /// <summary>
  /// Enum SsdUsage for value: ssd_usage
  /// </summary>
  [JsonPropertyName("ssd_usage")]
  SsdUsage = 2,

  /// <summary>
  /// Enum RamSearchUsage for value: ram_search_usage
  /// </summary>
  [JsonPropertyName("ram_search_usage")]
  RamSearchUsage = 3,

  /// <summary>
  /// Enum RamIndexingUsage for value: ram_indexing_usage
  /// </summary>
  [JsonPropertyName("ram_indexing_usage")]
  RamIndexingUsage = 4,

  /// <summary>
  /// Enum CpuUsage for value: cpu_usage
  /// </summary>
  [JsonPropertyName("cpu_usage")]
  CpuUsage = 5,

  /// <summary>
  /// Enum Star for value: *
  /// </summary>
  [JsonPropertyName("*")]
  Star = 6,
}

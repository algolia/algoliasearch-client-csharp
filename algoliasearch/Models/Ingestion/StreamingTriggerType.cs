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

namespace Algolia.Search.Models.Ingestion;

/// <summary>
/// Task runs continuously.
/// </summary>
/// <value>Task runs continuously.</value>
[JsonConverter(typeof(Serializer.JsonStringEnumConverter<StreamingTriggerType>))]
public enum StreamingTriggerType
{
  /// <summary>
  /// Enum Streaming for value: streaming
  /// </summary>
  [JsonPropertyName("streaming")]
  Streaming = 1
}


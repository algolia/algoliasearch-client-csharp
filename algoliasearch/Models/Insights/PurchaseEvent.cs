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

namespace Algolia.Search.Models.Insights;

/// <summary>
/// Defines PurchaseEvent
/// </summary>
[JsonConverter(typeof(Serializer.JsonStringEnumConverter<PurchaseEvent>))]
public enum PurchaseEvent
{
  /// <summary>
  /// Enum Purchase for value: purchase
  /// </summary>
  [JsonPropertyName("purchase")]
  Purchase = 1
}


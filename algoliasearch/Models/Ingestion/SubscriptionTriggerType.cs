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
/// A task which is triggered by an external subscription (e.g. Webhook).
/// </summary>
/// <value>A task which is triggered by an external subscription (e.g. Webhook).</value>
public enum SubscriptionTriggerType
{
  /// <summary>
  /// Enum Subscription for value: subscription
  /// </summary>
  [JsonPropertyName("subscription")]
  Subscription = 1
}

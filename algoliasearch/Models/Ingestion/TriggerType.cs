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
/// The type of the task reflect how it can be used:   - onDemand: a task that runs manually   - schedule: a task that runs regularly, following a given cron expression   - subscription: a task that runs after a subscription event is received from an integration (e.g. Webhook). 
/// </summary>
/// <value>The type of the task reflect how it can be used:   - onDemand: a task that runs manually   - schedule: a task that runs regularly, following a given cron expression   - subscription: a task that runs after a subscription event is received from an integration (e.g. Webhook). </value>
public enum TriggerType
{
  /// <summary>
  /// Enum OnDemand for value: onDemand
  /// </summary>
  [JsonPropertyName("onDemand")]
  OnDemand = 1,

  /// <summary>
  /// Enum Schedule for value: schedule
  /// </summary>
  [JsonPropertyName("schedule")]
  Schedule = 2,

  /// <summary>
  /// Enum Subscription for value: subscription
  /// </summary>
  [JsonPropertyName("subscription")]
  Subscription = 3
}

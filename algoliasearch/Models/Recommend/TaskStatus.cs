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

namespace Algolia.Search.Models.Recommend;

/// <summary>
/// _published_ if the task has been processed, _notPublished_ otherwise.
/// </summary>
/// <value>_published_ if the task has been processed, _notPublished_ otherwise.</value>
public enum TaskStatus
{
  /// <summary>
  /// Enum Published for value: published
  /// </summary>
  [JsonPropertyName("published")]
  Published = 1,

  /// <summary>
  /// Enum NotPublished for value: notPublished
  /// </summary>
  [JsonPropertyName("notPublished")]
  NotPublished = 2
}

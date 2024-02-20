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
/// Operation to perform (_move_ or _copy_).
/// </summary>
/// <value>Operation to perform (_move_ or _copy_).</value>
public enum OperationType
{
  /// <summary>
  /// Enum Move for value: move
  /// </summary>
  [JsonPropertyName("move")]
  Move = 1,

  /// <summary>
  /// Enum Copy for value: copy
  /// </summary>
  [JsonPropertyName("copy")]
  Copy = 2
}

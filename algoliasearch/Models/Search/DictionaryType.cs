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
/// Defines dictionaryType
/// </summary>
public enum DictionaryType
{
  /// <summary>
  /// Enum Plurals for value: plurals
  /// </summary>
  [JsonPropertyName("plurals")]
  Plurals = 1,

  /// <summary>
  /// Enum Stopwords for value: stopwords
  /// </summary>
  [JsonPropertyName("stopwords")]
  Stopwords = 2,

  /// <summary>
  /// Enum Compounds for value: compounds
  /// </summary>
  [JsonPropertyName("compounds")]
  Compounds = 3
}

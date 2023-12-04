//
// Code generated by OpenAPI Generator (https://openapi-generator.tech), manual changes will be lost - read more on https://github.com/algolia/api-clients-automation. DO NOT EDIT.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using FileParameter = Algolia.Search.Ingestion.Client.FileParameter;
using OpenAPIDateConverter = Algolia.Search.Ingestion.Client.OpenAPIDateConverter;

namespace Algolia.Search.Ingestion.Models
{
  /// <summary>
  /// Used to sort the Source list endpoint.
  /// </summary>
  /// <value>Used to sort the Source list endpoint.</value>
  [JsonConverter(typeof(StringEnumConverter))]
  public enum SourceSortKeys
  {
    /// <summary>
    /// Enum Name for value: name
    /// </summary>
    [EnumMember(Value = "name")]
    Name = 1,

    /// <summary>
    /// Enum Type for value: type
    /// </summary>
    [EnumMember(Value = "type")]
    Type = 2,

    /// <summary>
    /// Enum UpdatedAt for value: updatedAt
    /// </summary>
    [EnumMember(Value = "updatedAt")]
    UpdatedAt = 3,

    /// <summary>
    /// Enum CreatedAt for value: createdAt
    /// </summary>
    [EnumMember(Value = "createdAt")]
    CreatedAt = 4
  }

}

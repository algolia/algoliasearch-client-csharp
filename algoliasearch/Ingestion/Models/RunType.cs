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
  /// Defines RunType
  /// </summary>
  [JsonConverter(typeof(StringEnumConverter))]
  public enum RunType
  {
    /// <summary>
    /// Enum Reindex for value: reindex
    /// </summary>
    [EnumMember(Value = "reindex")]
    Reindex = 1,

    /// <summary>
    /// Enum Update for value: update
    /// </summary>
    [EnumMember(Value = "update")]
    Update = 2,

    /// <summary>
    /// Enum Discover for value: discover
    /// </summary>
    [EnumMember(Value = "discover")]
    Discover = 3
  }

}

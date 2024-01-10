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
using Algolia.Search.Models;

namespace Algolia.Search.Models.Search
{
  /// <summary>
  /// Actions to perform.
  /// </summary>
  /// <value>Actions to perform.</value>
  [JsonConverter(typeof(StringEnumConverter))]
  public enum DictionaryAction
  {
    /// <summary>
    /// Enum AddEntry for value: addEntry
    /// </summary>
    [EnumMember(Value = "addEntry")]
    AddEntry = 1,

    /// <summary>
    /// Enum DeleteEntry for value: deleteEntry
    /// </summary>
    [EnumMember(Value = "deleteEntry")]
    DeleteEntry = 2
  }

}

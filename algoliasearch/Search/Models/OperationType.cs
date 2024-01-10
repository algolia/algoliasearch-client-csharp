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
  /// Operation to perform (_move_ or _copy_).
  /// </summary>
  /// <value>Operation to perform (_move_ or _copy_).</value>
  [JsonConverter(typeof(StringEnumConverter))]
  public enum OperationType
  {
    /// <summary>
    /// Enum Move for value: move
    /// </summary>
    [EnumMember(Value = "move")]
    Move = 1,

    /// <summary>
    /// Enum Copy for value: copy
    /// </summary>
    [EnumMember(Value = "copy")]
    Copy = 2
  }

}

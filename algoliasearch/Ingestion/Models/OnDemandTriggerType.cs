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

namespace Algolia.Search.Ingestion.Models
{
  /// <summary>
  /// A task which is manually executed via the run task endpoint.
  /// </summary>
  /// <value>A task which is manually executed via the run task endpoint.</value>
  [JsonConverter(typeof(StringEnumConverter))]
  public enum OnDemandTriggerType
  {
    /// <summary>
    /// Enum OnDemand for value: onDemand
    /// </summary>
    [EnumMember(Value = "onDemand")]
    OnDemand = 1
  }

}
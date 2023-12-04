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
using FileParameter = Algolia.Search.Search.Client.FileParameter;
using OpenAPIDateConverter = Algolia.Search.Search.Client.OpenAPIDateConverter;

namespace Algolia.Search.Search.Models
{
  /// <summary>
  /// - &#x60;none&#x60;: executes all queries. - &#x60;stopIfEnoughMatches&#x60;: executes queries one by one, stopping further query execution as soon as a query matches at least the &#x60;hitsPerPage&#x60; number of results.  
  /// </summary>
  /// <value>- &#x60;none&#x60;: executes all queries. - &#x60;stopIfEnoughMatches&#x60;: executes queries one by one, stopping further query execution as soon as a query matches at least the &#x60;hitsPerPage&#x60; number of results.  </value>
  [JsonConverter(typeof(StringEnumConverter))]
  public enum SearchStrategy
  {
    /// <summary>
    /// Enum None for value: none
    /// </summary>
    [EnumMember(Value = "none")]
    None = 1,

    /// <summary>
    /// Enum StopIfEnoughMatches for value: stopIfEnoughMatches
    /// </summary>
    [EnumMember(Value = "stopIfEnoughMatches")]
    StopIfEnoughMatches = 2
  }

}

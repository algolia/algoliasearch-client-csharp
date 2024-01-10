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

namespace Algolia.Search.Models.Analytics
{
  /// <summary>
  /// GetSearchesCountResponse
  /// </summary>
  [DataContract(Name = "getSearchesCountResponse")]
  public partial class GetSearchesCountResponse
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSearchesCountResponse" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    public GetSearchesCountResponse() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="GetSearchesCountResponse" /> class.
    /// </summary>
    /// <param name="count">Number of occurrences. (required).</param>
    /// <param name="dates">Search events with their associated dates and hit counts. (required).</param>
    public GetSearchesCountResponse(int count, List<SearchEvent> dates)
    {
      this.Count = count;
      this.Dates = dates ?? throw new ArgumentNullException("dates is a required property for GetSearchesCountResponse and cannot be null");
    }

    /// <summary>
    /// Number of occurrences.
    /// </summary>
    /// <value>Number of occurrences.</value>
    [DataMember(Name = "count", IsRequired = true, EmitDefaultValue = true)]
    public int Count { get; set; }

    /// <summary>
    /// Search events with their associated dates and hit counts.
    /// </summary>
    /// <value>Search events with their associated dates and hit counts.</value>
    [DataMember(Name = "dates", IsRequired = true, EmitDefaultValue = true)]
    public List<SearchEvent> Dates { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class GetSearchesCountResponse {\n");
      sb.Append("  Count: ").Append(Count).Append("\n");
      sb.Append("  Dates: ").Append(Dates).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public virtual string ToJson()
    {
      return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }

  }

}

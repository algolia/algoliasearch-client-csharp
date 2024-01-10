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

namespace Algolia.Search.Models.Ingestion
{
  /// <summary>
  /// ListEventsResponse
  /// </summary>
  [DataContract(Name = "listEventsResponse")]
  public partial class ListEventsResponse
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ListEventsResponse" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    public ListEventsResponse() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ListEventsResponse" /> class.
    /// </summary>
    /// <param name="events">events (required).</param>
    /// <param name="pagination">pagination (required).</param>
    /// <param name="window">window (required).</param>
    public ListEventsResponse(List<Event> events, Pagination pagination, Window window)
    {
      this.Events = events ?? throw new ArgumentNullException("events is a required property for ListEventsResponse and cannot be null");
      this.Pagination = pagination ?? throw new ArgumentNullException("pagination is a required property for ListEventsResponse and cannot be null");
      this.Window = window ?? throw new ArgumentNullException("window is a required property for ListEventsResponse and cannot be null");
    }

    /// <summary>
    /// Gets or Sets Events
    /// </summary>
    [DataMember(Name = "events", IsRequired = true, EmitDefaultValue = true)]
    public List<Event> Events { get; set; }

    /// <summary>
    /// Gets or Sets Pagination
    /// </summary>
    [DataMember(Name = "pagination", IsRequired = true, EmitDefaultValue = true)]
    public Pagination Pagination { get; set; }

    /// <summary>
    /// Gets or Sets Window
    /// </summary>
    [DataMember(Name = "window", IsRequired = true, EmitDefaultValue = true)]
    public Window Window { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class ListEventsResponse {\n");
      sb.Append("  Events: ").Append(Events).Append("\n");
      sb.Append("  Pagination: ").Append(Pagination).Append("\n");
      sb.Append("  Window: ").Append(Window).Append("\n");
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

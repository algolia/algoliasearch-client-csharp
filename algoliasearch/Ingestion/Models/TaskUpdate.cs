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
  /// The payload for a task update.
  /// </summary>
  [DataContract(Name = "TaskUpdate")]
  public partial class TaskUpdate
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskUpdate" /> class.
    /// </summary>
    /// <param name="destinationID">The destination UUID..</param>
    /// <param name="trigger">trigger.</param>
    /// <param name="input">input.</param>
    /// <param name="enabled">Whether the task is enabled or not..</param>
    /// <param name="failureThreshold">A percentage representing the accepted failure threshold to determine if a &#x60;run&#x60; succeeded or not..</param>
    public TaskUpdate(string destinationID = default(string), TriggerUpdateInput trigger = default(TriggerUpdateInput), TaskInput input = default(TaskInput), bool enabled = default(bool), int failureThreshold = default(int))
    {
      this.DestinationID = destinationID;
      this.Trigger = trigger;
      this.Input = input;
      this.Enabled = enabled;
      this.FailureThreshold = failureThreshold;
    }

    /// <summary>
    /// The destination UUID.
    /// </summary>
    /// <value>The destination UUID.</value>
    [DataMember(Name = "destinationID", EmitDefaultValue = false)]
    public string DestinationID { get; set; }

    /// <summary>
    /// Gets or Sets Trigger
    /// </summary>
    [DataMember(Name = "trigger", EmitDefaultValue = false)]
    public TriggerUpdateInput Trigger { get; set; }

    /// <summary>
    /// Gets or Sets Input
    /// </summary>
    [DataMember(Name = "input", EmitDefaultValue = false)]
    public TaskInput Input { get; set; }

    /// <summary>
    /// Whether the task is enabled or not.
    /// </summary>
    /// <value>Whether the task is enabled or not.</value>
    [DataMember(Name = "enabled", EmitDefaultValue = true)]
    public bool Enabled { get; set; }

    /// <summary>
    /// A percentage representing the accepted failure threshold to determine if a &#x60;run&#x60; succeeded or not.
    /// </summary>
    /// <value>A percentage representing the accepted failure threshold to determine if a &#x60;run&#x60; succeeded or not.</value>
    [DataMember(Name = "failureThreshold", EmitDefaultValue = false)]
    public int FailureThreshold { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class TaskUpdate {\n");
      sb.Append("  DestinationID: ").Append(DestinationID).Append("\n");
      sb.Append("  Trigger: ").Append(Trigger).Append("\n");
      sb.Append("  Input: ").Append(Input).Append("\n");
      sb.Append("  Enabled: ").Append(Enabled).Append("\n");
      sb.Append("  FailureThreshold: ").Append(FailureThreshold).Append("\n");
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
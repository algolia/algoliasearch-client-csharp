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

namespace Algolia.Search.Models.Ingestion;

/// <summary>
/// The payload for a task creation.
/// </summary>
public partial class TaskCreate
{

  /// <summary>
  /// Gets or Sets Action
  /// </summary>
  [JsonPropertyName("action")]
  public ActionType? Action { get; set; }
  /// <summary>
  /// Initializes a new instance of the TaskCreate class.
  /// </summary>
  [JsonConstructor]
  public TaskCreate() { }
  /// <summary>
  /// Initializes a new instance of the TaskCreate class.
  /// </summary>
  /// <param name="sourceID">The source UUID. (required).</param>
  /// <param name="destinationID">The destination UUID. (required).</param>
  /// <param name="trigger">trigger (required).</param>
  /// <param name="action">action (required).</param>
  public TaskCreate(string sourceID, string destinationID, TaskCreateTrigger trigger, ActionType? action)
  {
    SourceID = sourceID ?? throw new ArgumentNullException(nameof(sourceID));
    DestinationID = destinationID ?? throw new ArgumentNullException(nameof(destinationID));
    Trigger = trigger ?? throw new ArgumentNullException(nameof(trigger));
    Action = action;
  }

  /// <summary>
  /// The source UUID.
  /// </summary>
  /// <value>The source UUID.</value>
  [JsonPropertyName("sourceID")]
  public string SourceID { get; set; }

  /// <summary>
  /// The destination UUID.
  /// </summary>
  /// <value>The destination UUID.</value>
  [JsonPropertyName("destinationID")]
  public string DestinationID { get; set; }

  /// <summary>
  /// Gets or Sets Trigger
  /// </summary>
  [JsonPropertyName("trigger")]
  public TaskCreateTrigger Trigger { get; set; }

  /// <summary>
  /// Whether the task is enabled or not.
  /// </summary>
  /// <value>Whether the task is enabled or not.</value>
  [JsonPropertyName("enabled")]
  public bool? Enabled { get; set; }

  /// <summary>
  /// A percentage representing the accepted failure threshold to determine if a `run` succeeded or not.
  /// </summary>
  /// <value>A percentage representing the accepted failure threshold to determine if a `run` succeeded or not.</value>
  [JsonPropertyName("failureThreshold")]
  public int? FailureThreshold { get; set; }

  /// <summary>
  /// Gets or Sets Input
  /// </summary>
  [JsonPropertyName("input")]
  public TaskInput Input { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class TaskCreate {\n");
    sb.Append("  SourceID: ").Append(SourceID).Append("\n");
    sb.Append("  DestinationID: ").Append(DestinationID).Append("\n");
    sb.Append("  Trigger: ").Append(Trigger).Append("\n");
    sb.Append("  Action: ").Append(Action).Append("\n");
    sb.Append("  Enabled: ").Append(Enabled).Append("\n");
    sb.Append("  FailureThreshold: ").Append(FailureThreshold).Append("\n");
    sb.Append("  Input: ").Append(Input).Append("\n");
    sb.Append("}\n");
    return sb.ToString();
  }

  /// <summary>
  /// Returns the JSON string presentation of the object
  /// </summary>
  /// <returns>JSON string presentation of the object</returns>
  public virtual string ToJson()
  {
    return JsonSerializer.Serialize(this, JsonConfig.Options);
  }

  /// <summary>
  /// Returns true if objects are equal
  /// </summary>
  /// <param name="obj">Object to be compared</param>
  /// <returns>Boolean</returns>
  public override bool Equals(object obj)
  {
    if (obj is not TaskCreate input)
    {
      return false;
    }

    return
        (SourceID == input.SourceID || (SourceID != null && SourceID.Equals(input.SourceID))) &&
        (DestinationID == input.DestinationID || (DestinationID != null && DestinationID.Equals(input.DestinationID))) &&
        (Trigger == input.Trigger || (Trigger != null && Trigger.Equals(input.Trigger))) &&
        (Action == input.Action || Action.Equals(input.Action)) &&
        (Enabled == input.Enabled || Enabled.Equals(input.Enabled)) &&
        (FailureThreshold == input.FailureThreshold || FailureThreshold.Equals(input.FailureThreshold)) &&
        (Input == input.Input || (Input != null && Input.Equals(input.Input)));
  }

  /// <summary>
  /// Gets the hash code
  /// </summary>
  /// <returns>Hash code</returns>
  public override int GetHashCode()
  {
    unchecked // Overflow is fine, just wrap
    {
      int hashCode = 41;
      if (SourceID != null)
      {
        hashCode = (hashCode * 59) + SourceID.GetHashCode();
      }
      if (DestinationID != null)
      {
        hashCode = (hashCode * 59) + DestinationID.GetHashCode();
      }
      if (Trigger != null)
      {
        hashCode = (hashCode * 59) + Trigger.GetHashCode();
      }
      hashCode = (hashCode * 59) + Action.GetHashCode();
      hashCode = (hashCode * 59) + Enabled.GetHashCode();
      hashCode = (hashCode * 59) + FailureThreshold.GetHashCode();
      if (Input != null)
      {
        hashCode = (hashCode * 59) + Input.GetHashCode();
      }
      return hashCode;
    }
  }

}

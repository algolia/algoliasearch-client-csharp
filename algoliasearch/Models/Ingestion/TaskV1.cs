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
/// The V1 task object, please use methods and types that don't contain the V1 suffix.
/// </summary>
public partial class TaskV1
{

  /// <summary>
  /// Gets or Sets Action
  /// </summary>
  [JsonPropertyName("action")]
  public ActionType? Action { get; set; }
  /// <summary>
  /// Initializes a new instance of the TaskV1 class.
  /// </summary>
  [JsonConstructor]
  public TaskV1() { }
  /// <summary>
  /// Initializes a new instance of the TaskV1 class.
  /// </summary>
  /// <param name="taskID">Universally unique identifier (UUID) of a task. (required).</param>
  /// <param name="sourceID">Universally uniqud identifier (UUID) of a source. (required).</param>
  /// <param name="destinationID">Universally unique identifier (UUID) of a destination resource. (required).</param>
  /// <param name="trigger">trigger (required).</param>
  /// <param name="enabled">Whether the task is enabled. (required) (default to true).</param>
  /// <param name="createdAt">Date of creation in RFC 3339 format. (required).</param>
  /// <param name="updatedAt">Date of last update in RFC 3339 format. (required).</param>
  public TaskV1(string taskID, string sourceID, string destinationID, Trigger trigger, bool enabled, string createdAt, string updatedAt)
  {
    TaskID = taskID ?? throw new ArgumentNullException(nameof(taskID));
    SourceID = sourceID ?? throw new ArgumentNullException(nameof(sourceID));
    DestinationID = destinationID ?? throw new ArgumentNullException(nameof(destinationID));
    Trigger = trigger ?? throw new ArgumentNullException(nameof(trigger));
    Enabled = enabled;
    CreatedAt = createdAt ?? throw new ArgumentNullException(nameof(createdAt));
    UpdatedAt = updatedAt ?? throw new ArgumentNullException(nameof(updatedAt));
  }

  /// <summary>
  /// Universally unique identifier (UUID) of a task.
  /// </summary>
  /// <value>Universally unique identifier (UUID) of a task.</value>
  [JsonPropertyName("taskID")]
  public string TaskID { get; set; }

  /// <summary>
  /// Universally uniqud identifier (UUID) of a source.
  /// </summary>
  /// <value>Universally uniqud identifier (UUID) of a source.</value>
  [JsonPropertyName("sourceID")]
  public string SourceID { get; set; }

  /// <summary>
  /// Universally unique identifier (UUID) of a destination resource.
  /// </summary>
  /// <value>Universally unique identifier (UUID) of a destination resource.</value>
  [JsonPropertyName("destinationID")]
  public string DestinationID { get; set; }

  /// <summary>
  /// Gets or Sets Trigger
  /// </summary>
  [JsonPropertyName("trigger")]
  public Trigger Trigger { get; set; }

  /// <summary>
  /// Gets or Sets Input
  /// </summary>
  [JsonPropertyName("input")]
  public TaskInput Input { get; set; }

  /// <summary>
  /// Whether the task is enabled.
  /// </summary>
  /// <value>Whether the task is enabled.</value>
  [JsonPropertyName("enabled")]
  public bool Enabled { get; set; }

  /// <summary>
  /// Maximum accepted percentage of failures for a task run to finish successfully.
  /// </summary>
  /// <value>Maximum accepted percentage of failures for a task run to finish successfully.</value>
  [JsonPropertyName("failureThreshold")]
  public int? FailureThreshold { get; set; }

  /// <summary>
  /// Date of the last cursor in RFC 3339 format.
  /// </summary>
  /// <value>Date of the last cursor in RFC 3339 format.</value>
  [JsonPropertyName("cursor")]
  public string Cursor { get; set; }

  /// <summary>
  /// Gets or Sets Notifications
  /// </summary>
  [JsonPropertyName("notifications")]
  public Notifications Notifications { get; set; }

  /// <summary>
  /// Gets or Sets Policies
  /// </summary>
  [JsonPropertyName("policies")]
  public Policies Policies { get; set; }

  /// <summary>
  /// Date of creation in RFC 3339 format.
  /// </summary>
  /// <value>Date of creation in RFC 3339 format.</value>
  [JsonPropertyName("createdAt")]
  public string CreatedAt { get; set; }

  /// <summary>
  /// Date of last update in RFC 3339 format.
  /// </summary>
  /// <value>Date of last update in RFC 3339 format.</value>
  [JsonPropertyName("updatedAt")]
  public string UpdatedAt { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class TaskV1 {\n");
    sb.Append("  TaskID: ").Append(TaskID).Append("\n");
    sb.Append("  SourceID: ").Append(SourceID).Append("\n");
    sb.Append("  DestinationID: ").Append(DestinationID).Append("\n");
    sb.Append("  Trigger: ").Append(Trigger).Append("\n");
    sb.Append("  Input: ").Append(Input).Append("\n");
    sb.Append("  Enabled: ").Append(Enabled).Append("\n");
    sb.Append("  FailureThreshold: ").Append(FailureThreshold).Append("\n");
    sb.Append("  Action: ").Append(Action).Append("\n");
    sb.Append("  Cursor: ").Append(Cursor).Append("\n");
    sb.Append("  Notifications: ").Append(Notifications).Append("\n");
    sb.Append("  Policies: ").Append(Policies).Append("\n");
    sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
    sb.Append("  UpdatedAt: ").Append(UpdatedAt).Append("\n");
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
    if (obj is not TaskV1 input)
    {
      return false;
    }

    return
        (TaskID == input.TaskID || (TaskID != null && TaskID.Equals(input.TaskID))) &&
        (SourceID == input.SourceID || (SourceID != null && SourceID.Equals(input.SourceID))) &&
        (DestinationID == input.DestinationID || (DestinationID != null && DestinationID.Equals(input.DestinationID))) &&
        (Trigger == input.Trigger || (Trigger != null && Trigger.Equals(input.Trigger))) &&
        (Input == input.Input || (Input != null && Input.Equals(input.Input))) &&
        (Enabled == input.Enabled || Enabled.Equals(input.Enabled)) &&
        (FailureThreshold == input.FailureThreshold || FailureThreshold.Equals(input.FailureThreshold)) &&
        (Action == input.Action || Action.Equals(input.Action)) &&
        (Cursor == input.Cursor || (Cursor != null && Cursor.Equals(input.Cursor))) &&
        (Notifications == input.Notifications || (Notifications != null && Notifications.Equals(input.Notifications))) &&
        (Policies == input.Policies || (Policies != null && Policies.Equals(input.Policies))) &&
        (CreatedAt == input.CreatedAt || (CreatedAt != null && CreatedAt.Equals(input.CreatedAt))) &&
        (UpdatedAt == input.UpdatedAt || (UpdatedAt != null && UpdatedAt.Equals(input.UpdatedAt)));
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
      if (TaskID != null)
      {
        hashCode = (hashCode * 59) + TaskID.GetHashCode();
      }
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
      if (Input != null)
      {
        hashCode = (hashCode * 59) + Input.GetHashCode();
      }
      hashCode = (hashCode * 59) + Enabled.GetHashCode();
      hashCode = (hashCode * 59) + FailureThreshold.GetHashCode();
      hashCode = (hashCode * 59) + Action.GetHashCode();
      if (Cursor != null)
      {
        hashCode = (hashCode * 59) + Cursor.GetHashCode();
      }
      if (Notifications != null)
      {
        hashCode = (hashCode * 59) + Notifications.GetHashCode();
      }
      if (Policies != null)
      {
        hashCode = (hashCode * 59) + Policies.GetHashCode();
      }
      if (CreatedAt != null)
      {
        hashCode = (hashCode * 59) + CreatedAt.GetHashCode();
      }
      if (UpdatedAt != null)
      {
        hashCode = (hashCode * 59) + UpdatedAt.GetHashCode();
      }
      return hashCode;
    }
  }

}


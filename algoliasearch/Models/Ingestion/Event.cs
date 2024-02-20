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
/// An event describe a step of the task execution flow..
/// </summary>
public partial class Event
{

  /// <summary>
  /// Gets or Sets Status
  /// </summary>
  [JsonPropertyName("status")]
  public EventStatus? Status { get; set; }

  /// <summary>
  /// Gets or Sets Type
  /// </summary>
  [JsonPropertyName("type")]
  public EventType? Type { get; set; }
  /// <summary>
  /// Initializes a new instance of the Event class.
  /// </summary>
  [JsonConstructor]
  public Event() { }
  /// <summary>
  /// Initializes a new instance of the Event class.
  /// </summary>
  /// <param name="eventID">The event UUID. (required).</param>
  /// <param name="runID">The run UUID. (required).</param>
  /// <param name="status">status (required).</param>
  /// <param name="type">type (required).</param>
  /// <param name="batchSize">The extracted record batch size. (required).</param>
  /// <param name="publishedAt">Date of publish (RFC3339 format). (required).</param>
  public Event(string eventID, string runID, EventStatus? status, EventType? type, int batchSize, string publishedAt)
  {
    EventID = eventID ?? throw new ArgumentNullException(nameof(eventID));
    RunID = runID ?? throw new ArgumentNullException(nameof(runID));
    Status = status;
    Type = type;
    BatchSize = batchSize;
    PublishedAt = publishedAt ?? throw new ArgumentNullException(nameof(publishedAt));
  }

  /// <summary>
  /// The event UUID.
  /// </summary>
  /// <value>The event UUID.</value>
  [JsonPropertyName("eventID")]
  public string EventID { get; set; }

  /// <summary>
  /// The run UUID.
  /// </summary>
  /// <value>The run UUID.</value>
  [JsonPropertyName("runID")]
  public string RunID { get; set; }

  /// <summary>
  /// The parent event, the cause of this event.
  /// </summary>
  /// <value>The parent event, the cause of this event.</value>
  [JsonPropertyName("parentID")]
  public string ParentID { get; set; }

  /// <summary>
  /// The extracted record batch size.
  /// </summary>
  /// <value>The extracted record batch size.</value>
  [JsonPropertyName("batchSize")]
  public int BatchSize { get; set; }

  /// <summary>
  /// Gets or Sets Data
  /// </summary>
  [JsonPropertyName("data")]
  public Dictionary<string, object> Data { get; set; }

  /// <summary>
  /// Date of publish (RFC3339 format).
  /// </summary>
  /// <value>Date of publish (RFC3339 format).</value>
  [JsonPropertyName("publishedAt")]
  public string PublishedAt { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class Event {\n");
    sb.Append("  EventID: ").Append(EventID).Append("\n");
    sb.Append("  RunID: ").Append(RunID).Append("\n");
    sb.Append("  ParentID: ").Append(ParentID).Append("\n");
    sb.Append("  Status: ").Append(Status).Append("\n");
    sb.Append("  Type: ").Append(Type).Append("\n");
    sb.Append("  BatchSize: ").Append(BatchSize).Append("\n");
    sb.Append("  Data: ").Append(Data).Append("\n");
    sb.Append("  PublishedAt: ").Append(PublishedAt).Append("\n");
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
    if (obj is not Event input)
    {
      return false;
    }

    return
        (EventID == input.EventID || (EventID != null && EventID.Equals(input.EventID))) &&
        (RunID == input.RunID || (RunID != null && RunID.Equals(input.RunID))) &&
        (ParentID == input.ParentID || (ParentID != null && ParentID.Equals(input.ParentID))) &&
        (Status == input.Status || Status.Equals(input.Status)) &&
        (Type == input.Type || Type.Equals(input.Type)) &&
        (BatchSize == input.BatchSize || BatchSize.Equals(input.BatchSize)) &&
        (Data == input.Data || Data != null && input.Data != null && Data.SequenceEqual(input.Data)) &&
        (PublishedAt == input.PublishedAt || (PublishedAt != null && PublishedAt.Equals(input.PublishedAt)));
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
      if (EventID != null)
      {
        hashCode = (hashCode * 59) + EventID.GetHashCode();
      }
      if (RunID != null)
      {
        hashCode = (hashCode * 59) + RunID.GetHashCode();
      }
      if (ParentID != null)
      {
        hashCode = (hashCode * 59) + ParentID.GetHashCode();
      }
      hashCode = (hashCode * 59) + Status.GetHashCode();
      hashCode = (hashCode * 59) + Type.GetHashCode();
      hashCode = (hashCode * 59) + BatchSize.GetHashCode();
      if (Data != null)
      {
        hashCode = (hashCode * 59) + Data.GetHashCode();
      }
      if (PublishedAt != null)
      {
        hashCode = (hashCode * 59) + PublishedAt.GetHashCode();
      }
      return hashCode;
    }
  }

}

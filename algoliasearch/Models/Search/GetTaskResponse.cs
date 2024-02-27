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

namespace Algolia.Search.Models.Search;

/// <summary>
/// GetTaskResponse
/// </summary>
public partial class GetTaskResponse
{

  /// <summary>
  /// Gets or Sets Status
  /// </summary>
  [JsonPropertyName("status")]
  public TaskStatus? Status { get; set; }
  /// <summary>
  /// Initializes a new instance of the GetTaskResponse class.
  /// </summary>
  [JsonConstructor]
  public GetTaskResponse() { }
  /// <summary>
  /// Initializes a new instance of the GetTaskResponse class.
  /// </summary>
  /// <param name="status">status (required).</param>
  public GetTaskResponse(TaskStatus? status)
  {
    Status = status;
  }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class GetTaskResponse {\n");
    sb.Append("  Status: ").Append(Status).Append("\n");
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
    if (obj is not GetTaskResponse input)
    {
      return false;
    }

    return
        (Status == input.Status || Status.Equals(input.Status));
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
      hashCode = (hashCode * 59) + Status.GetHashCode();
      return hashCode;
    }
  }

}

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
/// MultipleBatchRequest
/// </summary>
public partial class MultipleBatchRequest
{

  /// <summary>
  /// Gets or Sets Action
  /// </summary>
  [JsonPropertyName("action")]
  public Action? Action { get; set; }
  /// <summary>
  /// Initializes a new instance of the MultipleBatchRequest class.
  /// </summary>
  [JsonConstructor]
  public MultipleBatchRequest() { }
  /// <summary>
  /// Initializes a new instance of the MultipleBatchRequest class.
  /// </summary>
  /// <param name="action">action (required).</param>
  /// <param name="indexName">Index name (case-sensitive). (required).</param>
  public MultipleBatchRequest(Action? action, string indexName)
  {
    Action = action;
    IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
  }

  /// <summary>
  /// Operation arguments (varies with specified `action`).
  /// </summary>
  /// <value>Operation arguments (varies with specified `action`).</value>
  [JsonPropertyName("body")]
  public object Body { get; set; }

  /// <summary>
  /// Index name (case-sensitive).
  /// </summary>
  /// <value>Index name (case-sensitive).</value>
  [JsonPropertyName("indexName")]
  public string IndexName { get; set; }

  /// <summary>
  /// Returns the string presentation of the object
  /// </summary>
  /// <returns>String presentation of the object</returns>
  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("class MultipleBatchRequest {\n");
    sb.Append("  Action: ").Append(Action).Append("\n");
    sb.Append("  Body: ").Append(Body).Append("\n");
    sb.Append("  IndexName: ").Append(IndexName).Append("\n");
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
    if (obj is not MultipleBatchRequest input)
    {
      return false;
    }

    return
        (Action == input.Action || Action.Equals(input.Action)) &&
        (Body == input.Body || (Body != null && Body.Equals(input.Body))) &&
        (IndexName == input.IndexName || (IndexName != null && IndexName.Equals(input.IndexName)));
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
      hashCode = (hashCode * 59) + Action.GetHashCode();
      if (Body != null)
      {
        hashCode = (hashCode * 59) + Body.GetHashCode();
      }
      if (IndexName != null)
      {
        hashCode = (hashCode * 59) + IndexName.GetHashCode();
      }
      return hashCode;
    }
  }

}

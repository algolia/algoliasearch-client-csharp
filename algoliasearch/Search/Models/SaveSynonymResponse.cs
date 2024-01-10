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

namespace Algolia.Search.Models.Search
{
  /// <summary>
  /// SaveSynonymResponse
  /// </summary>
  [DataContract(Name = "saveSynonymResponse")]
  public partial class SaveSynonymResponse
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveSynonymResponse" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    public SaveSynonymResponse() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveSynonymResponse" /> class.
    /// </summary>
    /// <param name="taskID">Unique identifier of a task. A successful API response means that a task was added to a queue. It might not run immediately. You can check the task&#39;s progress with the &#x60;task&#x60; operation and this &#x60;taskID&#x60;.  (required).</param>
    /// <param name="updatedAt">Timestamp of the last update in [ISO 8601](https://wikipedia.org/wiki/ISO_8601) format. (required).</param>
    /// <param name="id">Unique identifier of a synonym object. (required).</param>
    public SaveSynonymResponse(long taskID, string updatedAt, string id)
    {
      this.TaskID = taskID;
      this.UpdatedAt = updatedAt ?? throw new ArgumentNullException("updatedAt is a required property for SaveSynonymResponse and cannot be null");
      this.Id = id ?? throw new ArgumentNullException("id is a required property for SaveSynonymResponse and cannot be null");
    }

    /// <summary>
    /// Unique identifier of a task. A successful API response means that a task was added to a queue. It might not run immediately. You can check the task&#39;s progress with the &#x60;task&#x60; operation and this &#x60;taskID&#x60;. 
    /// </summary>
    /// <value>Unique identifier of a task. A successful API response means that a task was added to a queue. It might not run immediately. You can check the task&#39;s progress with the &#x60;task&#x60; operation and this &#x60;taskID&#x60;. </value>
    [DataMember(Name = "taskID", IsRequired = true, EmitDefaultValue = true)]
    public long TaskID { get; set; }

    /// <summary>
    /// Timestamp of the last update in [ISO 8601](https://wikipedia.org/wiki/ISO_8601) format.
    /// </summary>
    /// <value>Timestamp of the last update in [ISO 8601](https://wikipedia.org/wiki/ISO_8601) format.</value>
    [DataMember(Name = "updatedAt", IsRequired = true, EmitDefaultValue = true)]
    public string UpdatedAt { get; set; }

    /// <summary>
    /// Unique identifier of a synonym object.
    /// </summary>
    /// <value>Unique identifier of a synonym object.</value>
    [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
    public string Id { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class SaveSynonymResponse {\n");
      sb.Append("  TaskID: ").Append(TaskID).Append("\n");
      sb.Append("  UpdatedAt: ").Append(UpdatedAt).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
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

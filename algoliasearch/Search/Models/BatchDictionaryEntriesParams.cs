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
  /// &#x60;batchDictionaryEntries&#x60; parameters. 
  /// </summary>
  [DataContract(Name = "batchDictionaryEntriesParams")]
  public partial class BatchDictionaryEntriesParams
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="BatchDictionaryEntriesParams" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    public BatchDictionaryEntriesParams() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="BatchDictionaryEntriesParams" /> class.
    /// </summary>
    /// <param name="requests">Operations to batch. (required).</param>
    public BatchDictionaryEntriesParams(List<BatchDictionaryEntriesRequest> requests)
    {
      this.Requests = requests ?? throw new ArgumentNullException("requests is a required property for BatchDictionaryEntriesParams and cannot be null");
    }

    /// <summary>
    /// Incidates whether to replace all custom entries in the dictionary with the ones sent with this request.
    /// </summary>
    /// <value>Incidates whether to replace all custom entries in the dictionary with the ones sent with this request.</value>
    [DataMember(Name = "clearExistingDictionaryEntries", EmitDefaultValue = true)]
    public bool? ClearExistingDictionaryEntries { get; set; }

    /// <summary>
    /// Operations to batch.
    /// </summary>
    /// <value>Operations to batch.</value>
    [DataMember(Name = "requests", IsRequired = true, EmitDefaultValue = true)]
    public List<BatchDictionaryEntriesRequest> Requests { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class BatchDictionaryEntriesParams {\n");
      sb.Append("  ClearExistingDictionaryEntries: ").Append(ClearExistingDictionaryEntries).Append("\n");
      sb.Append("  Requests: ").Append(Requests).Append("\n");
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

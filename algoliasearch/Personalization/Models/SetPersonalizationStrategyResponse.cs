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

namespace Algolia.Search.Personalization.Models
{
  /// <summary>
  /// SetPersonalizationStrategyResponse
  /// </summary>
  [DataContract(Name = "setPersonalizationStrategyResponse")]
  public partial class SetPersonalizationStrategyResponse
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SetPersonalizationStrategyResponse" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected SetPersonalizationStrategyResponse() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="SetPersonalizationStrategyResponse" /> class.
    /// </summary>
    /// <param name="message">A message confirming the strategy update. (required).</param>
    public SetPersonalizationStrategyResponse(string message = default(string))
    {
      // to ensure "message" is required (not null)
      if (message == null)
      {
        throw new ArgumentNullException("message is a required property for SetPersonalizationStrategyResponse and cannot be null");
      }
      this.Message = message;
    }

    /// <summary>
    /// A message confirming the strategy update.
    /// </summary>
    /// <value>A message confirming the strategy update.</value>
    [DataMember(Name = "message", IsRequired = true, EmitDefaultValue = true)]
    public string Message { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class SetPersonalizationStrategyResponse {\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
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
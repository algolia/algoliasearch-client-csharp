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
using System.ComponentModel.DataAnnotations;
using FileParameter = Algolia.Search.Monitoring.Client.FileParameter;
using OpenAPIDateConverter = Algolia.Search.Monitoring.Client.OpenAPIDateConverter;

namespace Algolia.Search.Monitoring.Models
{
  /// <summary>
  /// Error.
  /// </summary>
  [DataContract(Name = "ErrorBase")]
  public partial class ErrorBase : Dictionary<String, Object>, IEquatable<ErrorBase>, IValidatableObject
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorBase" /> class.
    /// </summary>
    /// <param name="message">message.</param>
    public ErrorBase(string message = default(string)) : base()
    {
      this.Message = message;
      this.AdditionalProperties = new Dictionary<string, object>();
    }

    /// <summary>
    /// Gets or Sets Message
    /// </summary>
    [DataMember(Name = "message", EmitDefaultValue = false)]
    public string Message { get; set; }

    /// <summary>
    /// Gets or Sets additional properties
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class ErrorBase {\n");
      sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
      sb.Append("  AdditionalProperties: ").Append(AdditionalProperties).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
      return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
      return this.Equals(input as ErrorBase);
    }

    /// <summary>
    /// Returns true if ErrorBase instances are equal
    /// </summary>
    /// <param name="input">Instance of ErrorBase to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(ErrorBase input)
    {
      if (input == null)
      {
        return false;
      }
      return base.Equals(input) &&
          (
              this.Message == input.Message ||
              (this.Message != null &&
              this.Message.Equals(input.Message))
          )
          && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && !this.AdditionalProperties.Except(input.AdditionalProperties).Any());
    }

    /// <summary>
    /// Gets the hash code
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
      unchecked // Overflow is fine, just wrap
      {
        int hashCode = base.GetHashCode();
        if (this.Message != null)
        {
          hashCode = (hashCode * 59) + this.Message.GetHashCode();
        }
        if (this.AdditionalProperties != null)
        {
          hashCode = (hashCode * 59) + this.AdditionalProperties.GetHashCode();
        }
        return hashCode;
      }
    }

    /// <summary>
    /// To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
      return this.BaseValidate(validationContext);
    }

    /// <summary>
    /// To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    protected IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> BaseValidate(ValidationContext validationContext)
    {
      yield break;
    }
  }

}

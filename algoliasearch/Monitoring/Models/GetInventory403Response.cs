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
  /// GetInventory403Response
  /// </summary>
  [DataContract(Name = "getInventory_403_response")]
  public partial class GetInventory403Response : IEquatable<GetInventory403Response>, IValidatableObject
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="GetInventory403Response" /> class.
    /// </summary>
    /// <param name="reason">reason.</param>
    public GetInventory403Response(string reason = default(string))
    {
      this.Reason = reason;
    }

    /// <summary>
    /// Gets or Sets Reason
    /// </summary>
    [DataMember(Name = "reason", EmitDefaultValue = false)]
    public string Reason { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class GetInventory403Response {\n");
      sb.Append("  Reason: ").Append(Reason).Append("\n");
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

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
      return this.Equals(input as GetInventory403Response);
    }

    /// <summary>
    /// Returns true if GetInventory403Response instances are equal
    /// </summary>
    /// <param name="input">Instance of GetInventory403Response to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GetInventory403Response input)
    {
      if (input == null)
      {
        return false;
      }
      return
          (
              this.Reason == input.Reason ||
              (this.Reason != null &&
              this.Reason.Equals(input.Reason))
          );
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
        if (this.Reason != null)
        {
          hashCode = (hashCode * 59) + this.Reason.GetHashCode();
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
      yield break;
    }
  }

}

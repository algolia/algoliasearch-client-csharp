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
using FileParameter = Algolia.Search.Recommend.Client.FileParameter;
using OpenAPIDateConverter = Algolia.Search.Recommend.Client.OpenAPIDateConverter;

namespace Algolia.Search.Recommend.Models
{
  /// <summary>
  /// GetRecommendTaskResponse
  /// </summary>
  [DataContract(Name = "getRecommendTaskResponse")]
  public partial class GetRecommendTaskResponse : IEquatable<GetRecommendTaskResponse>, IValidatableObject
  {

    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name = "status", IsRequired = true, EmitDefaultValue = true)]
    public TaskStatus Status { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="GetRecommendTaskResponse" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected GetRecommendTaskResponse() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="GetRecommendTaskResponse" /> class.
    /// </summary>
    /// <param name="status">status (required).</param>
    public GetRecommendTaskResponse(TaskStatus status = default(TaskStatus))
    {
      this.Status = status;
    }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class GetRecommendTaskResponse {\n");
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
      return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }

    /// <summary>
    /// Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input)
    {
      return this.Equals(input as GetRecommendTaskResponse);
    }

    /// <summary>
    /// Returns true if GetRecommendTaskResponse instances are equal
    /// </summary>
    /// <param name="input">Instance of GetRecommendTaskResponse to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(GetRecommendTaskResponse input)
    {
      if (input == null)
      {
        return false;
      }
      return
          (
              this.Status == input.Status ||
              this.Status.Equals(input.Status)
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
        hashCode = (hashCode * 59) + this.Status.GetHashCode();
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

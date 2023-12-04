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
  /// LatencyResponseMetrics
  /// </summary>
  [DataContract(Name = "LatencyResponse_metrics")]
  public partial class LatencyResponseMetrics : IEquatable<LatencyResponseMetrics>, IValidatableObject
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="LatencyResponseMetrics" /> class.
    /// </summary>
    /// <param name="latency">latency.</param>
    public LatencyResponseMetrics(Dictionary<string, List<TimeInner>> latency = default(Dictionary<string, List<TimeInner>>))
    {
      this.Latency = latency;
    }

    /// <summary>
    /// Gets or Sets Latency
    /// </summary>
    [DataMember(Name = "latency", EmitDefaultValue = false)]
    public Dictionary<string, List<TimeInner>> Latency { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class LatencyResponseMetrics {\n");
      sb.Append("  Latency: ").Append(Latency).Append("\n");
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
      return this.Equals(input as LatencyResponseMetrics);
    }

    /// <summary>
    /// Returns true if LatencyResponseMetrics instances are equal
    /// </summary>
    /// <param name="input">Instance of LatencyResponseMetrics to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(LatencyResponseMetrics input)
    {
      if (input == null)
      {
        return false;
      }
      return
          (
              this.Latency == input.Latency ||
              this.Latency != null &&
              input.Latency != null &&
              this.Latency.SequenceEqual(input.Latency)
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
        if (this.Latency != null)
        {
          hashCode = (hashCode * 59) + this.Latency.GetHashCode();
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

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
using FileParameter = Algolia.Search.Search.Client.FileParameter;
using OpenAPIDateConverter = Algolia.Search.Search.Client.OpenAPIDateConverter;

namespace Algolia.Search.Search.Models
{
  /// <summary>
  /// Unique user ID.
  /// </summary>
  [DataContract(Name = "userId")]
  public partial class UserId : IEquatable<UserId>, IValidatableObject
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="UserId" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected UserId() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="UserId" /> class.
    /// </summary>
    /// <param name="varUserID">userID of the user. (required).</param>
    /// <param name="clusterName">Cluster to which the user is assigned. (required).</param>
    /// <param name="nbRecords">Number of records belonging to the user. (required).</param>
    /// <param name="dataSize">Data size used by the user. (required).</param>
    public UserId(string varUserID = default(string), string clusterName = default(string), int nbRecords = default(int), int dataSize = default(int))
    {
      // to ensure "varUserID" is required (not null)
      if (varUserID == null)
      {
        throw new ArgumentNullException("varUserID is a required property for UserId and cannot be null");
      }
      this.VarUserID = varUserID;
      // to ensure "clusterName" is required (not null)
      if (clusterName == null)
      {
        throw new ArgumentNullException("clusterName is a required property for UserId and cannot be null");
      }
      this.ClusterName = clusterName;
      this.NbRecords = nbRecords;
      this.DataSize = dataSize;
    }

    /// <summary>
    /// userID of the user.
    /// </summary>
    /// <value>userID of the user.</value>
    [DataMember(Name = "userID", IsRequired = true, EmitDefaultValue = true)]
    public string VarUserID { get; set; }

    /// <summary>
    /// Cluster to which the user is assigned.
    /// </summary>
    /// <value>Cluster to which the user is assigned.</value>
    [DataMember(Name = "clusterName", IsRequired = true, EmitDefaultValue = true)]
    public string ClusterName { get; set; }

    /// <summary>
    /// Number of records belonging to the user.
    /// </summary>
    /// <value>Number of records belonging to the user.</value>
    [DataMember(Name = "nbRecords", IsRequired = true, EmitDefaultValue = true)]
    public int NbRecords { get; set; }

    /// <summary>
    /// Data size used by the user.
    /// </summary>
    /// <value>Data size used by the user.</value>
    [DataMember(Name = "dataSize", IsRequired = true, EmitDefaultValue = true)]
    public int DataSize { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class UserId {\n");
      sb.Append("  VarUserID: ").Append(VarUserID).Append("\n");
      sb.Append("  ClusterName: ").Append(ClusterName).Append("\n");
      sb.Append("  NbRecords: ").Append(NbRecords).Append("\n");
      sb.Append("  DataSize: ").Append(DataSize).Append("\n");
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
      return this.Equals(input as UserId);
    }

    /// <summary>
    /// Returns true if UserId instances are equal
    /// </summary>
    /// <param name="input">Instance of UserId to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(UserId input)
    {
      if (input == null)
      {
        return false;
      }
      return
          (
              this.VarUserID == input.VarUserID ||
              (this.VarUserID != null &&
              this.VarUserID.Equals(input.VarUserID))
          ) &&
          (
              this.ClusterName == input.ClusterName ||
              (this.ClusterName != null &&
              this.ClusterName.Equals(input.ClusterName))
          ) &&
          (
              this.NbRecords == input.NbRecords ||
              this.NbRecords.Equals(input.NbRecords)
          ) &&
          (
              this.DataSize == input.DataSize ||
              this.DataSize.Equals(input.DataSize)
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
        if (this.VarUserID != null)
        {
          hashCode = (hashCode * 59) + this.VarUserID.GetHashCode();
        }
        if (this.ClusterName != null)
        {
          hashCode = (hashCode * 59) + this.ClusterName.GetHashCode();
        }
        hashCode = (hashCode * 59) + this.NbRecords.GetHashCode();
        hashCode = (hashCode * 59) + this.DataSize.GetHashCode();
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
      if (this.VarUserID != null)
      {
        // VarUserID (string) pattern
        Regex regexVarUserID = new Regex(@"^[a-zA-Z0-9 \-*.]+$", RegexOptions.CultureInvariant);
        if (!regexVarUserID.Match(this.VarUserID).Success)
        {
          yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for VarUserID, must match a pattern of " + regexVarUserID, new[] { "VarUserID" });
        }
      }

      yield break;
    }
  }

}

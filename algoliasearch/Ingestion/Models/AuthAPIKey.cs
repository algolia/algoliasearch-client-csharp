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
using FileParameter = Algolia.Search.Ingestion.Client.FileParameter;
using OpenAPIDateConverter = Algolia.Search.Ingestion.Client.OpenAPIDateConverter;

namespace Algolia.Search.Ingestion.Models
{
  /// <summary>
  /// Authentication input used for token credentials.
  /// </summary>
  [DataContract(Name = "AuthAPIKey")]
  public partial class AuthAPIKey : IEquatable<AuthAPIKey>, IValidatableObject
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthAPIKey" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected AuthAPIKey() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthAPIKey" /> class.
    /// </summary>
    /// <param name="key">key (required).</param>
    public AuthAPIKey(string key = default(string))
    {
      // to ensure "key" is required (not null)
      if (key == null)
      {
        throw new ArgumentNullException("key is a required property for AuthAPIKey and cannot be null");
      }
      this.Key = key;
    }

    /// <summary>
    /// Gets or Sets Key
    /// </summary>
    [DataMember(Name = "key", IsRequired = true, EmitDefaultValue = true)]
    public string Key { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class AuthAPIKey {\n");
      sb.Append("  Key: ").Append(Key).Append("\n");
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
      return this.Equals(input as AuthAPIKey);
    }

    /// <summary>
    /// Returns true if AuthAPIKey instances are equal
    /// </summary>
    /// <param name="input">Instance of AuthAPIKey to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(AuthAPIKey input)
    {
      if (input == null)
      {
        return false;
      }
      return
          (
              this.Key == input.Key ||
              (this.Key != null &&
              this.Key.Equals(input.Key))
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
        if (this.Key != null)
        {
          hashCode = (hashCode * 59) + this.Key.GetHashCode();
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

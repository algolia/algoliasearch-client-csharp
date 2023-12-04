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
  /// ListApiKeysResponse
  /// </summary>
  [DataContract(Name = "listApiKeysResponse")]
  public partial class ListApiKeysResponse : IEquatable<ListApiKeysResponse>, IValidatableObject
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ListApiKeysResponse" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected ListApiKeysResponse() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ListApiKeysResponse" /> class.
    /// </summary>
    /// <param name="keys">API keys. (required).</param>
    public ListApiKeysResponse(List<GetApiKeyResponse> keys = default(List<GetApiKeyResponse>))
    {
      // to ensure "keys" is required (not null)
      if (keys == null)
      {
        throw new ArgumentNullException("keys is a required property for ListApiKeysResponse and cannot be null");
      }
      this.Keys = keys;
    }

    /// <summary>
    /// API keys.
    /// </summary>
    /// <value>API keys.</value>
    [DataMember(Name = "keys", IsRequired = true, EmitDefaultValue = true)]
    public List<GetApiKeyResponse> Keys { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class ListApiKeysResponse {\n");
      sb.Append("  Keys: ").Append(Keys).Append("\n");
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
      return this.Equals(input as ListApiKeysResponse);
    }

    /// <summary>
    /// Returns true if ListApiKeysResponse instances are equal
    /// </summary>
    /// <param name="input">Instance of ListApiKeysResponse to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(ListApiKeysResponse input)
    {
      if (input == null)
      {
        return false;
      }
      return
          (
              this.Keys == input.Keys ||
              this.Keys != null &&
              input.Keys != null &&
              this.Keys.SequenceEqual(input.Keys)
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
        if (this.Keys != null)
        {
          hashCode = (hashCode * 59) + this.Keys.GetHashCode();
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

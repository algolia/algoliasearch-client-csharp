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

namespace Algolia.Search.Models.Recommend
{
  /// <summary>
  /// Unique identifier of the record to hide.
  /// </summary>
  [DataContract(Name = "consequenceHide")]
  public partial class ConsequenceHide
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsequenceHide" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    public ConsequenceHide() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsequenceHide" /> class.
    /// </summary>
    /// <param name="objectID">Unique object identifier. (required).</param>
    public ConsequenceHide(string objectID)
    {
      this.ObjectID = objectID ?? throw new ArgumentNullException("objectID is a required property for ConsequenceHide and cannot be null");
    }

    /// <summary>
    /// Unique object identifier.
    /// </summary>
    /// <value>Unique object identifier.</value>
    [DataMember(Name = "objectID", IsRequired = true, EmitDefaultValue = true)]
    public string ObjectID { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class ConsequenceHide {\n");
      sb.Append("  ObjectID: ").Append(ObjectID).Append("\n");
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

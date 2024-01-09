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

namespace Algolia.Search.Search.Models
{
  /// <summary>
  /// Custom entries for a dictionary.
  /// </summary>
  [DataContract(Name = "dictionaryLanguage")]
  public partial class DictionaryLanguage
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DictionaryLanguage" /> class.
    /// </summary>
    /// <param name="nbCustomEntries">If &#x60;0&#x60;, the dictionary hasn&#39;t been customized and only contains standard entries provided by Algolia. If &#x60;null&#x60;, that feature isn&#39;t available or isn&#39;t supported for that language. .</param>
    public DictionaryLanguage(int nbCustomEntries = default(int))
    {
      this.NbCustomEntries = nbCustomEntries;
    }

    /// <summary>
    /// If &#x60;0&#x60;, the dictionary hasn&#39;t been customized and only contains standard entries provided by Algolia. If &#x60;null&#x60;, that feature isn&#39;t available or isn&#39;t supported for that language. 
    /// </summary>
    /// <value>If &#x60;0&#x60;, the dictionary hasn&#39;t been customized and only contains standard entries provided by Algolia. If &#x60;null&#x60;, that feature isn&#39;t available or isn&#39;t supported for that language. </value>
    [DataMember(Name = "nbCustomEntries", EmitDefaultValue = false)]
    public int NbCustomEntries { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class DictionaryLanguage {\n");
      sb.Append("  NbCustomEntries: ").Append(NbCustomEntries).Append("\n");
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
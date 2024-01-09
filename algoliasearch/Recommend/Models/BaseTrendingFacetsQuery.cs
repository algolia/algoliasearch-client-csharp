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

namespace Algolia.Search.Recommend.Models
{
  /// <summary>
  /// BaseTrendingFacetsQuery
  /// </summary>
  [DataContract(Name = "baseTrendingFacetsQuery")]
  public partial class BaseTrendingFacetsQuery
  {

    /// <summary>
    /// Gets or Sets Model
    /// </summary>
    [DataMember(Name = "model", EmitDefaultValue = false)]
    public TrendingFacetsModel? Model { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTrendingFacetsQuery" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected BaseTrendingFacetsQuery() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTrendingFacetsQuery" /> class.
    /// </summary>
    /// <param name="facetName">Facet name for trending models. (required).</param>
    /// <param name="model">model.</param>
    public BaseTrendingFacetsQuery(string facetName = default(string), TrendingFacetsModel? model = default(TrendingFacetsModel?))
    {
      // to ensure "facetName" is required (not null)
      if (facetName == null)
      {
        throw new ArgumentNullException("facetName is a required property for BaseTrendingFacetsQuery and cannot be null");
      }
      this.FacetName = facetName;
      this.Model = model;
    }

    /// <summary>
    /// Facet name for trending models.
    /// </summary>
    /// <value>Facet name for trending models.</value>
    [DataMember(Name = "facetName", IsRequired = true, EmitDefaultValue = true)]
    public string FacetName { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class BaseTrendingFacetsQuery {\n");
      sb.Append("  FacetName: ").Append(FacetName).Append("\n");
      sb.Append("  Model: ").Append(Model).Append("\n");
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
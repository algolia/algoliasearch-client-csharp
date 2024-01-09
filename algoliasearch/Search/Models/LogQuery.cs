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
  /// LogQuery
  /// </summary>
  [DataContract(Name = "logQuery")]
  public partial class LogQuery
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="LogQuery" /> class.
    /// </summary>
    /// <param name="indexName">Index targeted by the query..</param>
    /// <param name="userToken">User identifier..</param>
    /// <param name="queryId">Unique query identifier..</param>
    public LogQuery(string indexName = default(string), string userToken = default(string), string queryId = default(string))
    {
      this.IndexName = indexName;
      this.UserToken = userToken;
      this.QueryId = queryId;
    }

    /// <summary>
    /// Index targeted by the query.
    /// </summary>
    /// <value>Index targeted by the query.</value>
    [DataMember(Name = "index_name", EmitDefaultValue = false)]
    public string IndexName { get; set; }

    /// <summary>
    /// User identifier.
    /// </summary>
    /// <value>User identifier.</value>
    [DataMember(Name = "user_token", EmitDefaultValue = false)]
    public string UserToken { get; set; }

    /// <summary>
    /// Unique query identifier.
    /// </summary>
    /// <value>Unique query identifier.</value>
    [DataMember(Name = "query_id", EmitDefaultValue = false)]
    public string QueryId { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class LogQuery {\n");
      sb.Append("  IndexName: ").Append(IndexName).Append("\n");
      sb.Append("  UserToken: ").Append(UserToken).Append("\n");
      sb.Append("  QueryId: ").Append(QueryId).Append("\n");
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
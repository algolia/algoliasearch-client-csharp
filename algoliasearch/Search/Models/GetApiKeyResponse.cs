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
  /// GetApiKeyResponse
  /// </summary>
  [DataContract(Name = "getApiKeyResponse")]
  public partial class GetApiKeyResponse
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="GetApiKeyResponse" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected GetApiKeyResponse() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="GetApiKeyResponse" /> class.
    /// </summary>
    /// <param name="value">API key..</param>
    /// <param name="createdAt">Timestamp of creation in milliseconds in [Unix epoch time](https://wikipedia.org/wiki/Unix_time). (required).</param>
    /// <param name="acl">[Permissions](https://www.algolia.com/doc/guides/security/api-keys/#access-control-list-acl) associated with the key.  (required).</param>
    /// <param name="description">Description of an API key for you and your team members. (default to &quot;&quot;).</param>
    /// <param name="indexes">Restricts this API key to a list of indices or index patterns. If the list is empty, all indices are allowed. Specify either an exact index name or a pattern with a leading or trailing wildcard character (or both). For example: - &#x60;dev_*&#x60; matches all indices starting with \&quot;dev_\&quot; - &#x60;*_dev&#x60; matches all indices ending with \&quot;_dev\&quot; - &#x60;*_products_*&#x60; matches all indices containing \&quot;_products_\&quot;. .</param>
    /// <param name="maxHitsPerQuery">Maximum number of hits this API key can retrieve in one query. If zero, no limit is enforced. &gt; **Note**: Use this parameter to protect you from third-party attempts to retrieve your entire content by massively querying the index.  (default to 0).</param>
    /// <param name="maxQueriesPerIPPerHour">Maximum number of API calls per hour allowed from a given IP address or [user token](https://www.algolia.com/doc/guides/sending-events/concepts/usertoken/). Each time an API call is performed with this key, a check is performed. If there were more than the specified number of calls within the last hour, the API returns an error with the status code &#x60;429&#x60; (Too Many Requests).  &gt; **Note**: Use this parameter to protect you from third-party attempts to retrieve your entire content by massively querying the index.  (default to 0).</param>
    /// <param name="queryParameters">Force some [query parameters](https://www.algolia.com/doc/api-reference/api-parameters/) to be applied for each query made with this API key. It&#39;s a URL-encoded query string.  (default to &quot;&quot;).</param>
    /// <param name="referers">Restrict this API key to specific [referrers](https://www.algolia.com/doc/guides/security/api-keys/in-depth/api-key-restrictions/#http-referrers). If empty, all referrers are allowed. For example: - &#x60;https://algolia.com/_*&#x60; matches all referrers starting with \&quot;https://algolia.com/\&quot; - &#x60;*.algolia.com&#x60; matches all referrers ending with \&quot;.algolia.com\&quot; - &#x60;*algolia.com*&#x60; allows everything in the domain \&quot;algolia.com\&quot;. .</param>
    /// <param name="validity">Validity duration of a key (in seconds).  The key will automatically be removed after this time has expired. The default value of 0 never expires. Short-lived API keys are useful to grant temporary access to your data. For example, in mobile apps, you can&#39;t [control when users update your app](https://www.algolia.com/doc/guides/security/security-best-practices/#use-secured-api-keys-in-mobile-apps). So instead of encoding keys into your app as you would for a web app, you should dynamically fetch them from your mobile app&#39;s backend.  (default to 0).</param>
    public GetApiKeyResponse(string value = default(string), long createdAt = default(long), List<Acl> acl = default(List<Acl>), string description = @"", List<string> indexes = default(List<string>), int maxHitsPerQuery = 0, int maxQueriesPerIPPerHour = 0, string queryParameters = @"", List<string> referers = default(List<string>), int validity = 0)
    {
      this.CreatedAt = createdAt;
      // to ensure "acl" is required (not null)
      if (acl == null)
      {
        throw new ArgumentNullException("acl is a required property for GetApiKeyResponse and cannot be null");
      }
      this.Acl = acl;
      this.Value = value;
      // use default value if no "description" provided
      this.Description = description ?? @"";
      this.Indexes = indexes;
      this.MaxHitsPerQuery = maxHitsPerQuery;
      this.MaxQueriesPerIPPerHour = maxQueriesPerIPPerHour;
      // use default value if no "queryParameters" provided
      this.QueryParameters = queryParameters ?? @"";
      this.Referers = referers;
      this.Validity = validity;
    }

    /// <summary>
    /// API key.
    /// </summary>
    /// <value>API key.</value>
    [DataMember(Name = "value", EmitDefaultValue = false)]
    public string Value { get; set; }

    /// <summary>
    /// Timestamp of creation in milliseconds in [Unix epoch time](https://wikipedia.org/wiki/Unix_time).
    /// </summary>
    /// <value>Timestamp of creation in milliseconds in [Unix epoch time](https://wikipedia.org/wiki/Unix_time).</value>
    [DataMember(Name = "createdAt", IsRequired = true, EmitDefaultValue = true)]
    public long CreatedAt { get; set; }

    /// <summary>
    /// [Permissions](https://www.algolia.com/doc/guides/security/api-keys/#access-control-list-acl) associated with the key. 
    /// </summary>
    /// <value>[Permissions](https://www.algolia.com/doc/guides/security/api-keys/#access-control-list-acl) associated with the key. </value>
    [DataMember(Name = "acl", IsRequired = true, EmitDefaultValue = true)]
    public List<Acl> Acl { get; set; }

    /// <summary>
    /// Description of an API key for you and your team members.
    /// </summary>
    /// <value>Description of an API key for you and your team members.</value>
    [DataMember(Name = "description", EmitDefaultValue = false)]
    public string Description { get; set; }

    /// <summary>
    /// Restricts this API key to a list of indices or index patterns. If the list is empty, all indices are allowed. Specify either an exact index name or a pattern with a leading or trailing wildcard character (or both). For example: - &#x60;dev_*&#x60; matches all indices starting with \&quot;dev_\&quot; - &#x60;*_dev&#x60; matches all indices ending with \&quot;_dev\&quot; - &#x60;*_products_*&#x60; matches all indices containing \&quot;_products_\&quot;. 
    /// </summary>
    /// <value>Restricts this API key to a list of indices or index patterns. If the list is empty, all indices are allowed. Specify either an exact index name or a pattern with a leading or trailing wildcard character (or both). For example: - &#x60;dev_*&#x60; matches all indices starting with \&quot;dev_\&quot; - &#x60;*_dev&#x60; matches all indices ending with \&quot;_dev\&quot; - &#x60;*_products_*&#x60; matches all indices containing \&quot;_products_\&quot;. </value>
    [DataMember(Name = "indexes", EmitDefaultValue = false)]
    public List<string> Indexes { get; set; }

    /// <summary>
    /// Maximum number of hits this API key can retrieve in one query. If zero, no limit is enforced. &gt; **Note**: Use this parameter to protect you from third-party attempts to retrieve your entire content by massively querying the index. 
    /// </summary>
    /// <value>Maximum number of hits this API key can retrieve in one query. If zero, no limit is enforced. &gt; **Note**: Use this parameter to protect you from third-party attempts to retrieve your entire content by massively querying the index. </value>
    [DataMember(Name = "maxHitsPerQuery", EmitDefaultValue = false)]
    public int MaxHitsPerQuery { get; set; }

    /// <summary>
    /// Maximum number of API calls per hour allowed from a given IP address or [user token](https://www.algolia.com/doc/guides/sending-events/concepts/usertoken/). Each time an API call is performed with this key, a check is performed. If there were more than the specified number of calls within the last hour, the API returns an error with the status code &#x60;429&#x60; (Too Many Requests).  &gt; **Note**: Use this parameter to protect you from third-party attempts to retrieve your entire content by massively querying the index. 
    /// </summary>
    /// <value>Maximum number of API calls per hour allowed from a given IP address or [user token](https://www.algolia.com/doc/guides/sending-events/concepts/usertoken/). Each time an API call is performed with this key, a check is performed. If there were more than the specified number of calls within the last hour, the API returns an error with the status code &#x60;429&#x60; (Too Many Requests).  &gt; **Note**: Use this parameter to protect you from third-party attempts to retrieve your entire content by massively querying the index. </value>
    [DataMember(Name = "maxQueriesPerIPPerHour", EmitDefaultValue = false)]
    public int MaxQueriesPerIPPerHour { get; set; }

    /// <summary>
    /// Force some [query parameters](https://www.algolia.com/doc/api-reference/api-parameters/) to be applied for each query made with this API key. It&#39;s a URL-encoded query string. 
    /// </summary>
    /// <value>Force some [query parameters](https://www.algolia.com/doc/api-reference/api-parameters/) to be applied for each query made with this API key. It&#39;s a URL-encoded query string. </value>
    [DataMember(Name = "queryParameters", EmitDefaultValue = false)]
    public string QueryParameters { get; set; }

    /// <summary>
    /// Restrict this API key to specific [referrers](https://www.algolia.com/doc/guides/security/api-keys/in-depth/api-key-restrictions/#http-referrers). If empty, all referrers are allowed. For example: - &#x60;https://algolia.com/_*&#x60; matches all referrers starting with \&quot;https://algolia.com/\&quot; - &#x60;*.algolia.com&#x60; matches all referrers ending with \&quot;.algolia.com\&quot; - &#x60;*algolia.com*&#x60; allows everything in the domain \&quot;algolia.com\&quot;. 
    /// </summary>
    /// <value>Restrict this API key to specific [referrers](https://www.algolia.com/doc/guides/security/api-keys/in-depth/api-key-restrictions/#http-referrers). If empty, all referrers are allowed. For example: - &#x60;https://algolia.com/_*&#x60; matches all referrers starting with \&quot;https://algolia.com/\&quot; - &#x60;*.algolia.com&#x60; matches all referrers ending with \&quot;.algolia.com\&quot; - &#x60;*algolia.com*&#x60; allows everything in the domain \&quot;algolia.com\&quot;. </value>
    [DataMember(Name = "referers", EmitDefaultValue = false)]
    public List<string> Referers { get; set; }

    /// <summary>
    /// Validity duration of a key (in seconds).  The key will automatically be removed after this time has expired. The default value of 0 never expires. Short-lived API keys are useful to grant temporary access to your data. For example, in mobile apps, you can&#39;t [control when users update your app](https://www.algolia.com/doc/guides/security/security-best-practices/#use-secured-api-keys-in-mobile-apps). So instead of encoding keys into your app as you would for a web app, you should dynamically fetch them from your mobile app&#39;s backend. 
    /// </summary>
    /// <value>Validity duration of a key (in seconds).  The key will automatically be removed after this time has expired. The default value of 0 never expires. Short-lived API keys are useful to grant temporary access to your data. For example, in mobile apps, you can&#39;t [control when users update your app](https://www.algolia.com/doc/guides/security/security-best-practices/#use-secured-api-keys-in-mobile-apps). So instead of encoding keys into your app as you would for a web app, you should dynamically fetch them from your mobile app&#39;s backend. </value>
    [DataMember(Name = "validity", EmitDefaultValue = false)]
    public int Validity { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class GetApiKeyResponse {\n");
      sb.Append("  Value: ").Append(Value).Append("\n");
      sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
      sb.Append("  Acl: ").Append(Acl).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  Indexes: ").Append(Indexes).Append("\n");
      sb.Append("  MaxHitsPerQuery: ").Append(MaxHitsPerQuery).Append("\n");
      sb.Append("  MaxQueriesPerIPPerHour: ").Append(MaxQueriesPerIPPerHour).Append("\n");
      sb.Append("  QueryParameters: ").Append(QueryParameters).Append("\n");
      sb.Append("  Referers: ").Append(Referers).Append("\n");
      sb.Append("  Validity: ").Append(Validity).Append("\n");
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
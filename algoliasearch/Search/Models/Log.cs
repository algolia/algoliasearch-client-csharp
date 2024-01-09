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
  /// Log
  /// </summary>
  [DataContract(Name = "log")]
  public partial class Log
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Log" /> class.
    /// </summary>
    [JsonConstructorAttribute]
    protected Log() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="Log" /> class.
    /// </summary>
    /// <param name="timestamp">Timestamp in [ISO 8601](https://wikipedia.org/wiki/ISO_8601) format. (required).</param>
    /// <param name="method">HTTP method of the performed request. (required).</param>
    /// <param name="answerCode">HTTP response code. (required).</param>
    /// <param name="queryBody">Request body. Truncated after 1,000 characters. (required).</param>
    /// <param name="answer">Answer body. Truncated after 1,000 characters. (required).</param>
    /// <param name="url">Request URL. (required).</param>
    /// <param name="ip">IP address of the client that performed the request. (required).</param>
    /// <param name="queryHeaders">Request headers (API key is obfuscated). (required).</param>
    /// <param name="sha1">SHA1 signature of the log entry. (required).</param>
    /// <param name="nbApiCalls">Number of API calls. (required).</param>
    /// <param name="processingTimeMs">Processing time for the query. Doesn&#39;t include network time. (required).</param>
    /// <param name="index">Index targeted by the query..</param>
    /// <param name="queryParams">Query parameters sent with the request..</param>
    /// <param name="queryNbHits">Number of hits returned for the query..</param>
    /// <param name="innerQueries">Performed queries for the given request..</param>
    public Log(string timestamp = default(string), string method = default(string), string answerCode = default(string), string queryBody = default(string), string answer = default(string), string url = default(string), string ip = default(string), string queryHeaders = default(string), string sha1 = default(string), string nbApiCalls = default(string), string processingTimeMs = default(string), string index = default(string), string queryParams = default(string), string queryNbHits = default(string), List<LogQuery> innerQueries = default(List<LogQuery>))
    {
      // to ensure "timestamp" is required (not null)
      if (timestamp == null)
      {
        throw new ArgumentNullException("timestamp is a required property for Log and cannot be null");
      }
      this.Timestamp = timestamp;
      // to ensure "method" is required (not null)
      if (method == null)
      {
        throw new ArgumentNullException("method is a required property for Log and cannot be null");
      }
      this.Method = method;
      // to ensure "answerCode" is required (not null)
      if (answerCode == null)
      {
        throw new ArgumentNullException("answerCode is a required property for Log and cannot be null");
      }
      this.AnswerCode = answerCode;
      // to ensure "queryBody" is required (not null)
      if (queryBody == null)
      {
        throw new ArgumentNullException("queryBody is a required property for Log and cannot be null");
      }
      this.QueryBody = queryBody;
      // to ensure "answer" is required (not null)
      if (answer == null)
      {
        throw new ArgumentNullException("answer is a required property for Log and cannot be null");
      }
      this.Answer = answer;
      // to ensure "url" is required (not null)
      if (url == null)
      {
        throw new ArgumentNullException("url is a required property for Log and cannot be null");
      }
      this.Url = url;
      // to ensure "ip" is required (not null)
      if (ip == null)
      {
        throw new ArgumentNullException("ip is a required property for Log and cannot be null");
      }
      this.Ip = ip;
      // to ensure "queryHeaders" is required (not null)
      if (queryHeaders == null)
      {
        throw new ArgumentNullException("queryHeaders is a required property for Log and cannot be null");
      }
      this.QueryHeaders = queryHeaders;
      // to ensure "sha1" is required (not null)
      if (sha1 == null)
      {
        throw new ArgumentNullException("sha1 is a required property for Log and cannot be null");
      }
      this.Sha1 = sha1;
      // to ensure "nbApiCalls" is required (not null)
      if (nbApiCalls == null)
      {
        throw new ArgumentNullException("nbApiCalls is a required property for Log and cannot be null");
      }
      this.NbApiCalls = nbApiCalls;
      // to ensure "processingTimeMs" is required (not null)
      if (processingTimeMs == null)
      {
        throw new ArgumentNullException("processingTimeMs is a required property for Log and cannot be null");
      }
      this.ProcessingTimeMs = processingTimeMs;
      this.Index = index;
      this.QueryParams = queryParams;
      this.QueryNbHits = queryNbHits;
      this.InnerQueries = innerQueries;
    }

    /// <summary>
    /// Timestamp in [ISO 8601](https://wikipedia.org/wiki/ISO_8601) format.
    /// </summary>
    /// <value>Timestamp in [ISO 8601](https://wikipedia.org/wiki/ISO_8601) format.</value>
    [DataMember(Name = "timestamp", IsRequired = true, EmitDefaultValue = true)]
    public string Timestamp { get; set; }

    /// <summary>
    /// HTTP method of the performed request.
    /// </summary>
    /// <value>HTTP method of the performed request.</value>
    [DataMember(Name = "method", IsRequired = true, EmitDefaultValue = true)]
    public string Method { get; set; }

    /// <summary>
    /// HTTP response code.
    /// </summary>
    /// <value>HTTP response code.</value>
    [DataMember(Name = "answer_code", IsRequired = true, EmitDefaultValue = true)]
    public string AnswerCode { get; set; }

    /// <summary>
    /// Request body. Truncated after 1,000 characters.
    /// </summary>
    /// <value>Request body. Truncated after 1,000 characters.</value>
    [DataMember(Name = "query_body", IsRequired = true, EmitDefaultValue = true)]
    public string QueryBody { get; set; }

    /// <summary>
    /// Answer body. Truncated after 1,000 characters.
    /// </summary>
    /// <value>Answer body. Truncated after 1,000 characters.</value>
    [DataMember(Name = "answer", IsRequired = true, EmitDefaultValue = true)]
    public string Answer { get; set; }

    /// <summary>
    /// Request URL.
    /// </summary>
    /// <value>Request URL.</value>
    [DataMember(Name = "url", IsRequired = true, EmitDefaultValue = true)]
    public string Url { get; set; }

    /// <summary>
    /// IP address of the client that performed the request.
    /// </summary>
    /// <value>IP address of the client that performed the request.</value>
    [DataMember(Name = "ip", IsRequired = true, EmitDefaultValue = true)]
    public string Ip { get; set; }

    /// <summary>
    /// Request headers (API key is obfuscated).
    /// </summary>
    /// <value>Request headers (API key is obfuscated).</value>
    [DataMember(Name = "query_headers", IsRequired = true, EmitDefaultValue = true)]
    public string QueryHeaders { get; set; }

    /// <summary>
    /// SHA1 signature of the log entry.
    /// </summary>
    /// <value>SHA1 signature of the log entry.</value>
    [DataMember(Name = "sha1", IsRequired = true, EmitDefaultValue = true)]
    public string Sha1 { get; set; }

    /// <summary>
    /// Number of API calls.
    /// </summary>
    /// <value>Number of API calls.</value>
    [DataMember(Name = "nb_api_calls", IsRequired = true, EmitDefaultValue = true)]
    public string NbApiCalls { get; set; }

    /// <summary>
    /// Processing time for the query. Doesn&#39;t include network time.
    /// </summary>
    /// <value>Processing time for the query. Doesn&#39;t include network time.</value>
    [DataMember(Name = "processing_time_ms", IsRequired = true, EmitDefaultValue = true)]
    public string ProcessingTimeMs { get; set; }

    /// <summary>
    /// Index targeted by the query.
    /// </summary>
    /// <value>Index targeted by the query.</value>
    [DataMember(Name = "index", EmitDefaultValue = false)]
    public string Index { get; set; }

    /// <summary>
    /// Query parameters sent with the request.
    /// </summary>
    /// <value>Query parameters sent with the request.</value>
    [DataMember(Name = "query_params", EmitDefaultValue = false)]
    public string QueryParams { get; set; }

    /// <summary>
    /// Number of hits returned for the query.
    /// </summary>
    /// <value>Number of hits returned for the query.</value>
    [DataMember(Name = "query_nb_hits", EmitDefaultValue = false)]
    public string QueryNbHits { get; set; }

    /// <summary>
    /// Performed queries for the given request.
    /// </summary>
    /// <value>Performed queries for the given request.</value>
    [DataMember(Name = "inner_queries", EmitDefaultValue = false)]
    public List<LogQuery> InnerQueries { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("class Log {\n");
      sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
      sb.Append("  Method: ").Append(Method).Append("\n");
      sb.Append("  AnswerCode: ").Append(AnswerCode).Append("\n");
      sb.Append("  QueryBody: ").Append(QueryBody).Append("\n");
      sb.Append("  Answer: ").Append(Answer).Append("\n");
      sb.Append("  Url: ").Append(Url).Append("\n");
      sb.Append("  Ip: ").Append(Ip).Append("\n");
      sb.Append("  QueryHeaders: ").Append(QueryHeaders).Append("\n");
      sb.Append("  Sha1: ").Append(Sha1).Append("\n");
      sb.Append("  NbApiCalls: ").Append(NbApiCalls).Append("\n");
      sb.Append("  ProcessingTimeMs: ").Append(ProcessingTimeMs).Append("\n");
      sb.Append("  Index: ").Append(Index).Append("\n");
      sb.Append("  QueryParams: ").Append(QueryParams).Append("\n");
      sb.Append("  QueryNbHits: ").Append(QueryNbHits).Append("\n");
      sb.Append("  InnerQueries: ").Append(InnerQueries).Append("\n");
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
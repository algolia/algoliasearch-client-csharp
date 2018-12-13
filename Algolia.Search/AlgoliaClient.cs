/*
* Copyright (c) 2013 Algolia
* http://www.algolia.com/
* Based on the first version developed by Christopher Maneu under the same license:
*  https://github.com/cmaneu/algoliasearch-client-csharp
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
using Algolia.Search.Models;
using Algolia.Search.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search
{

    /// <summary>
    /// Client for the Algolia Search cloud API.
    /// You should instantiate a Client object with your ApplicationID, ApiKey and Hosts to start using Algolia Search API
    /// </summary>
    public class AlgoliaClient
    {
        private string[] _readHosts;
        private string[] _writeHosts;
        private string _applicationId;
        private string _apiKey;
        private HttpClient _searchHttpClient;
        private HttpClient _buildHttpClient;
        private HttpMessageHandler _mock;
        private bool _continueOnCapturedContext;
        private ArrayUtils<string> _arrayUtils;
        private Dictionary<string, HostStatus> _readHostsStatus = new Dictionary<string, HostStatus>();
        private Dictionary<string, HostStatus> _writeHostsStatus = new Dictionary<string, HostStatus>();
        public int _dsnInternalTimeout = 60 * 5;

        /// <summary>
        /// Algolia Search initialization
        /// </summary>
        /// <param name="applicationId">The application ID you have in your admin interface</param>
        /// <param name="apiKey">A valid API key for the service</param>
        /// <param name="hosts">The list of hosts that you have received for the service</param>
        /// <param name="mock">Mocking object for controlling HTTP message handler</param>
        public AlgoliaClient(string applicationId, string apiKey, IEnumerable<string> hosts = null, HttpMessageHandler mock = null)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
                throw new ArgumentOutOfRangeException("applicationId", "An application Id is required.");

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentOutOfRangeException("apiKey", "An API key is required.");

            if (hosts != null && hosts.Count() == 0)
                throw new ArgumentOutOfRangeException("hosts", "At least one host is required.");

            if (hosts != null)
            {
                foreach (var host in hosts)
                {
                    if (string.IsNullOrWhiteSpace(host))
                        throw new ArgumentOutOfRangeException("hosts", "Each host is required.");
                }
                _readHosts = _writeHosts = hosts.ToArray();
            }
            else
            {
                _arrayUtils = new ArrayUtils<string>();

                var baseReadHosts = applicationId + "-dsn.algolia.net";
                var shuffledReadHosts = new List<string> { applicationId + "-1.algolianet.com", applicationId + "-2.algolianet.com", applicationId + "-3.algolianet.com" };
                _readHosts = getHosts(baseReadHosts, shuffledReadHosts);

                var baseWriteHosts = applicationId + ".algolia.net";
                var shuffledWriteHosts = new List<string> { applicationId + "-1.algolianet.com", applicationId + "-2.algolianet.com", applicationId + "-3.algolianet.com" };
                _writeHosts = getHosts(baseWriteHosts, shuffledWriteHosts);
            }

            _applicationId = applicationId;
            _apiKey = apiKey;
            _mock = mock;
            // randomize elements of hostsArray (act as a kind of load-balancer)


            HttpClient.DefaultRequestHeaders.Add("X-Algolia-Application-Id", applicationId);
            HttpClient.DefaultRequestHeaders.Add("X-Algolia-API-Key", apiKey);
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Algolia for Csharp 4.0.0");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SearchHttpClient.DefaultRequestHeaders.Add("X-Algolia-Application-Id", applicationId);
            SearchHttpClient.DefaultRequestHeaders.Add("X-Algolia-API-Key", apiKey);
            SearchHttpClient.DefaultRequestHeaders.Add("User-Agent", "Algolia for Csharp 4.0.0");
            SearchHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SearchHttpClient.Timeout = TimeSpan.FromSeconds(5);
            HttpClient.Timeout = TimeSpan.FromSeconds(30);

            _continueOnCapturedContext = false;
        }

        /// <summary>
        /// return the hosts array with the last 3 elements shuffled
        /// </summary>
        /// <param name="baseHost"></param>
        /// <param name="hosts"></param>
        /// <returns></returns>
        public string[] getHosts(string baseHost, IEnumerable<string> hosts)
        {
            var result = new List<string> { baseHost };
            //shuffling all but not the first one
            var shuffledHosts = _arrayUtils.Shuffle(hosts);
            result.AddRange(shuffledHosts);
            return result.ToArray();
        }

        public HostStatus setHostStatus(bool up)
        {
            return new HostStatus { Up = up, LastModified = DateTime.Now };
        }

        public string[] filterOnActiveHosts(string[] _hosts, bool isQuery)
        {

            var validHosts = new List<string> { };
            var statusHosts = isQuery ? _readHostsStatus : _writeHostsStatus;
            foreach (var host in _hosts)
            {
                if (statusHosts.ContainsKey(host))
                {
                    var canRetry = (DateTime.Now - statusHosts[host].LastModified).TotalSeconds > _dsnInternalTimeout;
                    if (statusHosts[host].Up || canRetry)
                    {
                        validHosts.Add(host);
                    }
                }
                else
                {
                    validHosts.Add(host);
                }

            }

            return validHosts.Count > 0 ? validHosts.ToArray() : _hosts;
        }

        /// <summary>
        /// Configure the await in the library. Useful to avoid a deadlock with ASP.NET projects.
        /// </summary>
        /// <param name="continueOnCapturedContext">Set to false to turn it off and avoid deadlocks</param>
        public void ConfigureAwait(bool continueOnCapturedContext)
        {
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>
        /// Get the context
        /// </summary>
        /// <returns></returns>
        public bool getContinueOnCapturedContext()
        {
            return _continueOnCapturedContext;
        }

        /// <summary>
        /// Set the read timeout for the search and for the build operation
        /// This method should be called before any api call.
        /// </summary>
        public void setTimeout(double searchTimeout, double writeTimeout)
        {
            SearchHttpClient.Timeout = TimeSpan.FromSeconds(searchTimeout);
            HttpClient.Timeout = TimeSpan.FromSeconds(writeTimeout);
        }

        /// <summary>
        /// Add security tag header (see http://www.algolia.com/doc/guides/csharp#SecurityUser for more details)
        /// </summary>
        /// <param name="tag"></param>
        public void SetSecurityTags(string tag)
        {
            HttpClient.DefaultRequestHeaders.Add("X-Algolia-TagFilters", tag);
            SearchHttpClient.DefaultRequestHeaders.Add("X-Algolia-TagFilters", tag);
        }

        /// <summary>
        /// Add user-token header (see http://www.algolia.com/doc/guides/csharp#SecurityUser for more details)
        /// </summary>
        /// <param name="userToken"></param>
        public void SetUserToken(string userToken)
        {
            HttpClient.DefaultRequestHeaders.Add("X-Algolia-UserToken", userToken);
            SearchHttpClient.DefaultRequestHeaders.Add("X-Algolia-UserToken", userToken);
        }

        /// <summary>
        /// Set extra HTTP request headers
        /// </summary>
        /// <param name="key">The header key</param>
        /// <param name="value">The header value</param>
        public void SetExtraHeader(string key, string value)
        {
            HttpClient.DefaultRequestHeaders.Add(key, value);
            SearchHttpClient.DefaultRequestHeaders.Add(key, value);
        }

        /// <summary>
        /// This method allows querying multiple indexes with one API call
        /// </summary>
        /// <param name="queries">List of queries per index</param>
        /// <param name="requestOptions"></param>
        /// <param name="strategy">Strategy applied on the sequence of queries</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<JObject> MultipleQueriesAsync(List<IndexQuery> queries, RequestOptions requestOptions, string strategy = "none", CancellationToken token = default(CancellationToken))
        {
            List<Dictionary<string, object>> body = new List<Dictionary<string, object>>();
            foreach (IndexQuery indexQuery in queries)
            {
                Dictionary<string, object> request = new Dictionary<string, object>();
                request.Add("indexName", indexQuery.Index);
                request.Add("params", indexQuery.Query.GetQueryString());
                body.Add(request);
            }
            Dictionary<string, object> requests = new Dictionary<string, object>();
            requests.Add("requests", body);
            requests.Add("strategy", strategy);
            return ExecuteRequest(callType.Search, "POST", "/1/indexes/*/queries", requests, token, requestOptions);

        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.MultipleQueriesAsync"/>
        /// </summary>
        /// <param name="queries">List of queries per index</param>
        /// <param name="requestOptions"></param>
        /// <param name="strategy"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public JObject MultipleQueries(List<IndexQuery> queries, RequestOptions requestOptions, string strategy = "none", CancellationToken token = default(CancellationToken))
        {
            return MultipleQueriesAsync(queries, requestOptions, strategy, token).GetAwaiter().GetResult();
        }

        /// <summary>
        /// List all existing indexes.
        /// </summary>
        /// <returns>An object in the form:
        ///    {"items": [ {"name": "contacts", "createdAt": "2013-01-18T15:33:13.556Z"},
        ///                {"name": "notes", "createdAt": "2013-01-18T15:33:13.556Z"} ] }
        /// </returns>
        public Task<JObject> ListIndexesAsync(RequestOptions requestOptions, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(callType.Read, "GET", "/1/indexes/", null, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListIndexesAsync"/> 
        /// </summary>
        /// <returns>An object in the form:
        ///    {"items": [ {"name": "contacts", "createdAt": "2013-01-18T15:33:13.556Z"},
        ///                {"name": "notes", "createdAt": "2013-01-18T15:33:13.556Z"} ] }
        /// </returns>
        public JObject ListIndexes(RequestOptions requestOptions)
        {
            return ListIndexesAsync(requestOptions, default(CancellationToken)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete an index.
        /// </summary>
        /// <param name="indexName">The name of index to delete</param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns>An object containing a "deletedAt" attribute</returns>
        public Task<JObject> DeleteIndexAsync(string indexName, RequestOptions requestOptions, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(callType.Write, "DELETE", "/1/indexes/" + WebUtility.UrlEncode(indexName), null, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteIndexAsync"/> 
        /// </summary>
        /// <returns>An object containing a "deletedAt" attribute</returns>
        public JObject DeleteIndex(string indexName, RequestOptions requestOptions)
        {
            return DeleteIndexAsync(indexName, requestOptions, default(CancellationToken)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Move an existing index.
        /// </summary>
        /// <param name="srcIndexName">The name of index to move.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        public Task<JObject> MoveIndexAsync(string srcIndexName, string dstIndexName, RequestOptions requestOptions, CancellationToken token = default(CancellationToken))
        {
            Dictionary<string, object> operation = new Dictionary<string, object>();
            operation["operation"] = "move";
            operation["destination"] = dstIndexName;
            return ExecuteRequest(callType.Write, "POST", string.Format("/1/indexes/{0}/operation", WebUtility.UrlEncode(srcIndexName)), operation, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.MoveIndexAsync"/>
        /// </summary>
        /// <param name="srcIndexName">The name of index to move.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        /// <param name="requestOptions"></param>
        public JObject MoveIndex(string srcIndexName, string dstIndexName, RequestOptions requestOptions)
        {
            return MoveIndexAsync(srcIndexName, dstIndexName, requestOptions, default(CancellationToken)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Copy an existing index.
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <param name="scopes">the scope of the copy, as a list.</param>
        public Task<JObject> CopyIndexAsync(string srcIndexName, string dstIndexName, RequestOptions requestOptions, CancellationToken token = default(CancellationToken), List<CopyScope> scopes = null)
        {
            Dictionary<string, object> operation = new Dictionary<string, object>();
            operation["operation"] = "copy";
            operation["destination"] = dstIndexName;

            if (scopes != null)
            {
                operation["scope"] = scopes;
            }

            return ExecuteRequest(callType.Write, "POST", string.Format("/1/indexes/{0}/operation", WebUtility.UrlEncode(srcIndexName)), operation, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.CopyIndexAsync"/> 
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        /// <param name="requestOptions"></param>
        /// <param name="scopes">the scope of the copy, as a list.</param>
        public JObject CopyIndex(string srcIndexName, string dstIndexName, RequestOptions requestOptions, List<CopyScope> scopes = null)
        {
            return CopyIndexAsync(srcIndexName, dstIndexName, requestOptions, default(CancellationToken), scopes).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.CopyIndexAsync"/> 
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        public JObject CopyIndex(string srcIndexName, string dstIndexName, RequestOptions requestOptions)
        {
            return CopyIndexAsync(srcIndexName, dstIndexName, requestOptions, default(CancellationToken)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// The type of log
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// All build logs
            /// </summary>
            LOG_BUILD,
            /// <summary>
            /// All query logs
            /// </summary>
            LOG_QUERY,
            /// <summary>
            /// All error logs
            /// </summary>
            LOG_ERROR,
            /// <summary>
            /// All logs
            /// </summary>
            LOG_ALL
        }

        /// <summary>
        /// Return last logs entries.
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="onlyErrors">If set to true, the answer will only contain API calls with errors.</param>
        public Task<JObject> GetLogsAsync(RequestOptions requestOptions, int offset = 0, int length = 10, bool onlyErrors = false)
        {
            return GetLogsAsync(requestOptions, offset, length, onlyErrors ? LogType.LOG_ERROR : LogType.LOG_ALL, default(CancellationToken));
        }

        /// <summary>
        /// Return last logs entries.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="logType">Specify the type of logs to include.</param>
        /// <param name="token"></param>
        public Task<JObject> GetLogsAsync(RequestOptions requestOptions, int offset = 0, int length = 10, LogType logType = LogType.LOG_ALL, CancellationToken token = default(CancellationToken))
        {
            string param = "";
            if (offset != 0)
                param += string.Format("?offset={0}", offset);

            if (length != 10)
            {
                if (param.Length == 0)
                    param += string.Format("?length={0}", length);
                else
                    param += string.Format("&length={0}", length);
            }
            if (logType != LogType.LOG_ALL)
            {
                string type = "";
                switch (logType)
                {
                    case LogType.LOG_BUILD:
                        type = "build";
                        break;
                    case LogType.LOG_QUERY:
                        type = "query";
                        break;
                    case LogType.LOG_ERROR:
                        type = "error";
                        break;
                    case LogType.LOG_ALL:
                        type = "all";
                        break;
                }
                if (param.Length == 0)
                    param += string.Format("?type={0}", type);
                else
                    param += string.Format("&type={0}", type);
            }
            return ExecuteRequest(callType.Write, "GET", String.Format("/1/logs{0}", param), null, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetLogsAsync(int, int, bool)"/>
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="onlyErrors">If set to true, the answer will only contain API calls with errors.</param>
        public JObject GetLogs(RequestOptions requestOptions, int offset = 0, int length = 10, bool onlyErrors = false)
        {
            return GetLogsAsync(requestOptions, offset, length, onlyErrors).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetLogsAsync(int, int, Algolia.Search.AlgoliaClient.LogType)"/>
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="logType">Specify the type of logs to include.</param>
        /// <param name="requestOptions"></param>
        public JObject GetLogs(int offset, int length, LogType logType, RequestOptions requestOptions)
        {
            return GetLogsAsync(requestOptions, offset, length, logType, default(CancellationToken)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get the index object initialized (no server call needed for initialization).
        /// </summary>
        /// <param name="indexName">The name of the index</param>
        /// <returns>An instance of the Index object that exposes Index actions</returns>
        public Index InitIndex(string indexName)
        {
            return new Index(this, indexName);
        }

        /// <summary>
        /// Get the analytics object initialized (no server call needed for initialization).
        /// </summary>
        /// <returns>An instance of the analytics object that exposes Index actions</returns>
        public Analytics InitAnalytics()
        {
            return new Analytics(this);
        }

        /// <summary>
        /// List all existing api keys with their associated ACLs.
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        public Task<JObject> ListApiKeysAsync(RequestOptions requestOptions, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(callType.Read, "GET", "/1/keys", null, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListApiKeysAsync"/> 
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        public JObject ListApiKeys(RequestOptions requestOptions)
        {
            return ListApiKeysAsync(requestOptions, default(CancellationToken)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get ACL for an existing user key.
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        public Task<JObject> GetApiKeyAsync(string key, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(callType.Read, "GET", "/1/keys/" + key, null, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetApiKeyAsync"/> 
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        public JObject GetApiKey(string key, RequestOptions requestOptions = null)
        {
            return GetApiKeyAsync(key, requestOptions, default(CancellationToken)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete an existing user key.
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        public Task<JObject> DeleteApiKeyAsync(string key, RequestOptions requestOptions, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(callType.Write, "DELETE", "/1/keys/" + key, null, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteApiKeyAsync"/> 
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        public JObject DeleteApiKey(string key, RequestOptions requestOptions)
        {
            return DeleteApiKeyAsync(key, requestOptions, default(CancellationToken)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create a new api key.
        /// </summary>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionnary that 
        ///     can contains the following values:
        ///     - acl: array of string
        ///     - indices: array of string
        ///     - validity: int
        ///     - referers: array of string
        ///     - description: string
        ///     - maxHitsPerQuery: integer
        ///     - queryParameters: string
        ///     - maxQueriesPerIPPerHour: integer
        ///     <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        public Task<JObject> AddApiKeyAsync(Dictionary<string, object> parameters, RequestOptions requestOptions, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(callType.Write, "POST", "/1/keys", parameters, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.AddApiKeyAsync"/>
        /// </summary>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionnary that 
        ///     can contains the following values:
        ///     - acl: array of string
        ///     - indices: array of string
        ///     - validity: int
        ///     - referers: array of string
        ///     - description: string
        ///     - maxHitsPerQuery: integer
        ///     - queryParameters: string
        ///     - maxQueriesPerIPPerHour: integer
        ///     <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        /// <param name="requestOptions"></param>
        public JObject AddApiKey(Dictionary<string, object> parameters, RequestOptions requestOptions)
        {
            return AddApiKeyAsync(parameters, requestOptions, default(CancellationToken)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create a new api key.
        /// </summary>
        /// <param name="acls">The list of ACL for this key. Defined by an array of strings that can contains the following values:
        ///     - search: allow searching (https and http)
        ///     - addObject: allow adding/updating an object in the index (https only)
        ///     - deleteObject : allow deleting an existing object (https only)
        ///     - deleteIndex : allow deleting an index (https only)
        ///     - settings : allow getting index settings (https only)
        ///     - editSettings : allow changing index settings (https only)</param>
        /// <param name="requestOptions"></param>
        /// <param name="validity">The number of seconds after which the key will be automatically removed (0 means no time limit for this key).</param>
        /// <param name="maxQueriesPerIPPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> AddApiKeyAsync(IEnumerable<string> acls, RequestOptions requestOptions, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            if (indexes == null)
            {
                indexes = new string[0] { };
            }
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIPPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            content["indexes"] = indexes;
            return AddApiKeyAsync(content, requestOptions, default(CancellationToken));
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.AddApiKeyAsync"/>
        /// </summary>
        /// <param name="acls">The list of ACL for this key. Defined by an array of strings that can contains the following values:
        ///     - search: allow searching (https and http)
        ///     - addObject: allow adding/updating an object in the index (https only)
        ///     - deleteObject : allow deleting an existing object (https only)
        ///     - deleteIndex : allow deleting an index (https only)
        ///     - settings : allow getting index settings (https only)
        ///     - editSettings : allow changing index settings (https only)</param>
        /// <param name="requestOptions"></param>
        /// <param name="validity">The number of seconds after which the key will be automatically removed (0 means no time limit for this key).</param>
        /// <param name="maxQueriesPerIPPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public JObject AddApiKey(IEnumerable<string> acls, RequestOptions requestOptions, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            return AddApiKeyAsync(acls, requestOptions, validity, maxQueriesPerIPPerHour, maxHitsPerQuery, indexes).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Update an api key.
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionnary that 
        ///     can contains the following values:
        ///     - acl: array of string
        ///     - indices: array of string
        ///     - validity: int
        ///     - referers: array of string
        ///     - description: string
        ///     - maxHitsPerQuery: integer
        ///     - queryParameters: string
        ///     - maxQueriesPerIPPerHour: integer
        ///     <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        public Task<JObject> UpdateApiKeyAsync(string key, Dictionary<string, object> parameters, RequestOptions requestOptions, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(callType.Write, "PUT", "/1/keys/" + key, parameters, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.UpdateApiKeyAsync"/>
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionnary that 
        ///     can contains the following values:
        ///     - acl: array of string
        ///     - indices: array of string
        ///     - validity: int
        ///     - referers: array of string
        ///     - description: string
        ///     - maxHitsPerQuery: integer
        ///     - queryParameters: string
        ///     - maxQueriesPerIPPerHour: integer
        ///     <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        /// <param name="requestOptions"></param>
        public JObject UpdateApiKey(string key, Dictionary<string, object> parameters, RequestOptions requestOptions)
        {
            return UpdateApiKeyAsync(key, parameters, requestOptions, default(CancellationToken)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Update an api key.
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="acls">The list of ACL for this key. Defined by an array of strings that can contains the following values:
        ///     - search: allow searching (https and http)
        ///     - addObject: allow adding/updating an object in the index (https only)
        ///     - deleteObject : allow deleting an existing object (https only)
        ///     - deleteIndex : allow deleting an index (https only)
        ///     - settings : allow getting index settings (https only)
        ///     - editSettings : allow changing index settings (https only)</param>
        /// <param name="requestOptions"></param>
        /// <param name="validity">The number of seconds after which the key will be automatically removed (0 means no time limit for this key).</param>
        /// <param name="maxQueriesPerIPPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> UpdateApiKeyAsync(string key, IEnumerable<string> acls, RequestOptions requestOptions, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            if (indexes == null)
            {
                indexes = new string[0] { };
            }
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIPPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            content["indexes"] = indexes;
            return UpdateApiKeyAsync(key, content, requestOptions, default(CancellationToken));
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.UpdateApiKeyAsync"/>
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="acls">The list of ACL for this key. Defined by an array of strings that can contains the following values:
        ///     - search: allow searching (https and http)
        ///     - addObject: allow adding/updating an object in the index (https only)
        ///     - deleteObject : allow deleting an existing object (https only)
        ///     - deleteIndex : allow deleting an index (https only)
        ///     - settings : allow getting index settings (https only)
        ///     - editSettings : allow changing index settings (https only)</param>
        /// <param name="requestOptions"></param>
        /// <param name="validity">The number of seconds after which the key will be automatically removed (0 means no time limit for this key).</param>
        /// <param name="maxQueriesPerIPPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public JObject UpdateApiKey(string key, IEnumerable<string> acls, RequestOptions requestOptions, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            return UpdateApiKeyAsync(key, acls, requestOptions, validity, maxQueriesPerIPPerHour, maxHitsPerQuery, indexes).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send a batch targeting multiple indices
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <param name="actions">An array of action to send.</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string) and a dictionary for the taskIDs.</returns>
        public Task<JObject> BatchAsync(IEnumerable<object> requests, RequestOptions requestOptions, CancellationToken token = default(CancellationToken))
        {
            Dictionary<string, object> batch = new Dictionary<string, object>();
            batch["requests"] = requests;
            return ExecuteRequest(AlgoliaClient.callType.Write, "POST", "/1/indexes/*/batch", batch, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.BatchAsync"/>
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="requestOptions"></param>
        /// <param name="actions">An array of action to send.</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string) and a dictionary for the taskIDs.</returns>
        public JObject Batch(IEnumerable<object> requests, RequestOptions requestOptions)
        {
            return BatchAsync(requests, requestOptions, default(CancellationToken)).GetAwaiter().GetResult();
        }



        /// <summary>
        /// Generates a secured and public API Key from a query parameters and an optional user token identifying the current user
        /// </summary>
        /// <param name="privateApiKey">Your private API Key</param>
        /// <param name="query">The query parameters applied to the query (used as security)</param>
        /// <param name="userToken">An optional token identifying the current user</param>
        /// <returns></returns>
        public string GenerateSecuredApiKey(String privateApiKey, Query query, String userToken = null)
        {
            if (userToken != null)
                query.SetUserToken(userToken);

            string queryStr = query.GetQueryString();
            var hash = Hmac(privateApiKey, queryStr);
            byte[] content = Encoding.UTF8.GetBytes(string.Format("{0}{1}", hash, queryStr));
            return Convert.ToBase64String(content);
        }

        /// <summary>
        /// Generates a secured and public API Key from a list of tagFilters and an optional user token identifying the current user
        /// </summary>
        /// <param name="privateApiKey">Your private API Key</param>
        /// <param name="tagFilter">The list of tags applied to the query (used as security)</param>
        /// <param name="userToken">An optional token identifying the current user</param>
        /// <returns></returns>
        public string GenerateSecuredApiKey(String privateApiKey, String tagFilter, String userToken = null)
        {
            if (!tagFilter.Contains("="))
                return GenerateSecuredApiKey(privateApiKey, new Query().SetTagFilters(tagFilter), userToken);
            else
            {
                if (userToken != null && userToken.Length > 0)
                    tagFilter = string.Format("{0}&userToken={1}", tagFilter, WebUtility.UrlEncode(userToken));
                byte[] content = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}{1}", Hmac(privateApiKey, tagFilter), tagFilter));
                return System.Convert.ToBase64String(content);
            }
        }

        private string Hmac(string key, string data)
        {
            return HmacSha256(key, data);
        }

        public string HmacSha256(string key, string data)
        {
            string hash;
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] code = encoder.GetBytes(key);
            using (HMACSHA256 hmac = new HMACSHA256(code))
            {
                byte[] hmBytes = hmac.ComputeHash(encoder.GetBytes(data));
                hash = ToHexString(hmBytes);
            }
            return hash;

        }

        public static string ToHexString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        /// <summary>
        /// Main HTTP client
        /// </summary>
        protected HttpClient HttpClient
        {
            get
            {
                if (_buildHttpClient == null)
                {
                    if (_mock == null)
                        _buildHttpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
                    else
                        _buildHttpClient = new HttpClient(_mock);
                }
                return _buildHttpClient;
            }
        }

        /// <summary>
        /// Search HTTP client
        /// </summary>
        protected HttpClient SearchHttpClient
        {
            get
            {
                if (_searchHttpClient == null)
                {
                    if (_mock == null)
                        _searchHttpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
                    else
                        _searchHttpClient = new HttpClient(_mock);
                }
                return _searchHttpClient;
            }
        }

        public enum callType
        {
            Search,
            Write,
            Read,
            Analytics,
            Insights
        };

        /// <summary>
        /// Used to execute the search request
        /// </summary>
        /// <param name="method">HTTP method</param>
        /// <param name="requestUrl">URL to request</param>
        /// <param name="content">The content</param>
        /// <param name="requestOptions">The additional request options</param>
        /// <returns></returns>
        public async Task<JObject> ExecuteRequest(callType type, string method, string requestUrl, object content, CancellationToken token, RequestOptions requestOptions)
        {
            string[] hosts = null;
            string requestExtraQueryParams = "";
            HttpClient client = null;
            if (type == callType.Search)
            {
                hosts = filterOnActiveHosts(_readHosts, true);
                client = _searchHttpClient;
            }
            else if (type == callType.Analytics)
            {
                hosts = new string[] { "analytics.algolia.com" };
                client = _buildHttpClient;
            }
            else if (type == callType.Insights)
            {
                hosts = filterOnActiveHosts(_readHosts, true);
                client = _buildHttpClient;
            }
            else
            {
                hosts = type == callType.Read
                    ? filterOnActiveHosts(_readHosts, true)
                    : filterOnActiveHosts(_writeHosts, false);
                client = _buildHttpClient;
            }

            if (requestOptions != null)
            {
                requestExtraQueryParams = buildExtraQueryParamsUrlString(requestOptions.GenerateExtraQueryParams());
            }

            Dictionary<string, string> errors = new Dictionary<string, string>();
            foreach (string host in hosts)
            {
                try
                {
                    try
                    {
                        string url;
                        if (String.IsNullOrEmpty(requestExtraQueryParams))
                        {
                            url = string.Format("https://{0}{1}", host, requestUrl);
                        }
                        else
                        {
                            url = requestUrl.Contains("?") // check if we already had query parameters added to the requestUrl
                                ? string.Format("https://{0}{1}&{2}", host, requestUrl, requestExtraQueryParams)
                                : string.Format("https://{0}{1}?{2}", host, requestUrl, requestExtraQueryParams);
                        }

                        HttpRequestMessage httpRequestMessage = null;
                        switch (method)
                        {
                            case "GET":
                                httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                                break;
                            case "POST":
                                httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                                if (content != null)
                                {
                                    httpRequestMessage.Content =
                                        new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content, new Newtonsoft.Json.Converters.StringEnumConverter()));
                                }
                                break;
                            case "PUT":
                                httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, url);
                                if (content != null)
                                {
                                    httpRequestMessage.Content =
                                        new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content, new Newtonsoft.Json.Converters.StringEnumConverter()));
                                }
                                break;
                            case "DELETE":
                                httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
                                break;
                        }

                        if (requestOptions != null)
                        {
                            foreach (var header in requestOptions.GenerateExtraHeaders())
                            {
                                httpRequestMessage.Headers.Add(header.Key, header.Value);
                            }
                        }

                        HttpResponseMessage responseMsg = await client.SendAsync(httpRequestMessage, token)
                            .ConfigureAwait(_continueOnCapturedContext);

                        if (responseMsg.IsSuccessStatusCode)
                        {
                            string serializedJSON = await responseMsg.Content.ReadAsStringAsync().ConfigureAwait(_continueOnCapturedContext);

                            JObject obj = JObject.Parse(serializedJSON);
                            if (type == callType.Search || type == callType.Read)
                            {
                                _readHostsStatus[host] = setHostStatus(true);
                            }
                            else
                            {
                                _writeHostsStatus[host] = setHostStatus(true);
                            }
                            return obj;
                        }
                        else
                        {
                            string serializedJSON = await responseMsg.Content.ReadAsStringAsync().ConfigureAwait(_continueOnCapturedContext);
                            string message = "Internal Error";
                            string status = "0";
                            try
                            {
                                JObject obj = JObject.Parse(serializedJSON);
                                message = obj["message"].ToString();
                                status = obj["status"].ToString();
                                if (obj["status"].ToObject<int>() / 100 == 4)
                                {
                                    throw new AlgoliaException(message, responseMsg.StatusCode);
                                }
                            }
                            catch (JsonReaderException)
                            {
                                message = responseMsg.ReasonPhrase;
                                status = "0";
                            }

                            errors.Add(host + '(' + status + ')', message);
                        }
                    }
                    catch (AlgoliaException)
                    {
                        if (type == callType.Search || type == callType.Read)
                        {
                            _readHostsStatus[host] = setHostStatus(false);
                        }
                        else
                        {
                            _writeHostsStatus[host] = setHostStatus(false);
                        }
                        throw;
                    }
                    catch (TaskCanceledException e)
                    {
                        if (token.IsCancellationRequested)
                        {
                            throw e;
                        }
                        if (type == callType.Search || type == callType.Read)
                        {
                            _readHostsStatus[host] = setHostStatus(false);
                        }
                        else
                        {
                            _writeHostsStatus[host] = setHostStatus(false);
                        }
                        errors.Add(host, "Timeout expired");
                    }
                    catch (Exception ex)
                    {
                        if (type == callType.Search || type == callType.Read)
                        {
                            _readHostsStatus[host] = setHostStatus(false);
                        }
                        else
                        {
                            _writeHostsStatus[host] = setHostStatus(false);
                        }
                        errors.Add(host, ex.Message);
                    }

                }
                catch (AlgoliaException)
                {
                    throw;
                }

            }
            throw new AlgoliaException("Hosts unreachable: " + string.Join(", ", errors.Select(x => x.Key + "=" + x.Value).ToArray()));
        }

        private string buildExtraQueryParamsUrlString(Dictionary<string, string> extraQueryParams)
        {
            if (extraQueryParams == null || extraQueryParams.Count == 0)
            {
                return "";
            }

            string stringBuilder = "";

            foreach (var queryParam in extraQueryParams)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder += '&';
                }

                stringBuilder += WebUtility.UrlEncode(queryParam.Key) + "=" + WebUtility.UrlEncode(queryParam.Value);
            }

            return stringBuilder;
        }

        /* 
         * These are overloaded methods of everything above in order to avoid binary incompatibility 
         * when adding all the requestOptions parameters
         */

        /// <summary>
        /// This method allows querying multiple indexes with one API call
        /// </summary>
        /// <param name="queries">List of queries per index</param>
        /// <param name="strategy">Strategy applied on the sequence of queries</param>
        /// <returns></returns>
        public Task<JObject> MultipleQueriesAsync(List<IndexQuery> queries, string strategy = "none", CancellationToken token = default(CancellationToken))
        { return MultipleQueriesAsync(queries, null, strategy, token); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.MultipleQueriesAsync"/>
        /// </summary>
        /// <param name="queries">List of queries per index</param>
        /// <returns></returns>
        public JObject MultipleQueries(List<IndexQuery> queries, string strategy = "none", CancellationToken token = default(CancellationToken))
        { return MultipleQueries(queries, null, strategy, token); }
        /// </returns>
        public Task<JObject> ListIndexesAsync(CancellationToken token = default(CancellationToken))
        { return ListIndexesAsync(null, token); }
        /// </returns>
        public JObject ListIndexes()
        {
            return ListIndexes(null);
        }

        /// <summary>
        /// Delete an index.
        /// </summary>
        /// <param name="indexName">The name of index to delete</param>
        /// <returns>An object containing a "deletedAt" attribute</returns>
        public Task<JObject> DeleteIndexAsync(string indexName, CancellationToken token = default(CancellationToken))
        { return DeleteIndexAsync(indexName, null, token); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteIndexAsync"/> 
        /// </summary>
        /// <returns>An object containing a "deletedAt" attribute</returns>
        public JObject DeleteIndex(string indexName)
        { return DeleteIndex(indexName, null); }

        /// <summary>
        /// Move an existing index.
        /// </summary>
        /// <param name="srcIndexName">The name of index to move.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        public Task<JObject> MoveIndexAsync(string srcIndexName, string dstIndexName, CancellationToken token = default(CancellationToken))
        { return MoveIndexAsync(srcIndexName, dstIndexName, null, token); }
        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.MoveIndexAsync"/>
        /// </summary>
        /// <param name="srcIndexName">The name of index to move.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        public JObject MoveIndex(string srcIndexName, string dstIndexName)
        { return MoveIndex(srcIndexName, dstIndexName, null); }

        /// <summary>
        /// Copy an existing index.
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        public Task<JObject> CopyIndexAsync(string srcIndexName, string dstIndexName, CancellationToken token = default(CancellationToken))
        { return CopyIndexAsync(srcIndexName, dstIndexName, null, token); }
        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.CopyIndexAsync"/> 
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        public JObject CopyIndex(string srcIndexName, string dstIndexName)
        { return CopyIndex(srcIndexName, dstIndexName, null); }

        /// <summary>
        /// Return last logs entries.
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="onlyErrors">If set to true, the answer will only contain API calls with errors.</param>
        public Task<JObject> GetLogsAsync(int offset = 0, int length = 10, bool onlyErrors = false)
        { return GetLogsAsync(null, offset, length, onlyErrors); }

        /// <summary>
        /// Return last logs entries.
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="logType">Specify the type of logs to include.</param>
        public Task<JObject> GetLogsAsync(int offset = 0, int length = 10, LogType logType = LogType.LOG_ALL, CancellationToken token = default(CancellationToken))
        { return GetLogsAsync(null, offset, length, logType, token); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetLogsAsync(int, int, bool)"/>
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="onlyErrors">If set to true, the answer will only contain API calls with errors.</param>
        public JObject GetLogs(int offset = 0, int length = 10, bool onlyErrors = false)
        { return GetLogs(null, offset, length, onlyErrors); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetLogsAsync(int, int, Algolia.Search.AlgoliaClient.LogType)"/>
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="logType">Specify the type of logs to include.</param>
        public JObject GetLogs(int offset, int length, LogType logType)
        { return GetLogs(offset, length, logType, null); }

        /// <summary>
        /// List all existing api keys with their associated ACLs.
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        public Task<JObject> ListApiKeysAsync(CancellationToken token = default(CancellationToken))
        { return ListApiKeysAsync(null, token); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListApiKeysAsync"/> 
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        public JObject ListApiKeys()
        {
            return ListApiKeys(null);
        }

        /// <summary>
        /// Get ACL for an existing api key.
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        public Task<JObject> GetApiKeyACLAsync(string key, CancellationToken token = default(CancellationToken))
        { return GetApiKeyAsync(key, null, token); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetApiKeyACLAsync"/> 
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        public JObject GetApiKeyACL(string key)
        { return GetApiKey(key, null); }

        /// <summary>
        /// Delete an existing user key.
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        public Task<JObject> DeleteApiKeyAsync(string key, CancellationToken token = default(CancellationToken))
        { return DeleteApiKeyAsync(key, null, token); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteApiKeyAsync"/> 
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        public JObject DeleteApiKey(string key)
        { return DeleteApiKey(key, null); }

        /// <summary>
        /// Create a new api key.
        /// </summary>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionnary that 
        /// can contains the following values:
        ///   - acl: array of string
        ///   - indices: array of string
        ///   - validity: int
        ///   - referers: array of string
        ///   - description: string
        ///   - maxHitsPerQuery: integer
        ///   - queryParameters: string
        ///   - maxQueriesPerIPPerHour: integer
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> AddApiKeyAsync(Dictionary<string, object> parameters, CancellationToken token = default(CancellationToken))
        { return AddApiKeyAsync(parameters, null, token); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.AddApiKeyAsync"/>
        /// </summary>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionnary that 
        /// can contains the following values:
        ///   - acl: array of string
        ///   - indices: array of string
        ///   - validity: int
        ///   - referers: array of string
        ///   - description: string
        ///   - maxHitsPerQuery: integer
        ///   - queryParameters: string
        ///   - maxQueriesPerIPPerHour: integer
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public JObject AddApiKey(Dictionary<string, object> parameters)
        { return AddApiKey(parameters, null); }

        /// <summary>
        /// Create a new api key.
        /// </summary>
        /// <param name="acls">The list of ACL for this key. Defined by an array of strings that can contains the following values:
        ///   - search: allow searching (https and http)
        ///   - addObject: allow adding/updating an object in the index (https only)
        ///   - deleteObject : allow deleting an existing object (https only)
        ///   - deleteIndex : allow deleting an index (https only)
        ///   - settings : allow getting index settings (https only)
        ///   - editSettings : allow changing index settings (https only)</param>
        /// <param name="validity">The number of seconds after which the key will be automatically removed (0 means no time limit for this key).</param>
        /// <param name="maxQueriesPerIPPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> AddApiKeyAsync(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        { return AddApiKeyAsync(acls, null, validity, maxQueriesPerIPPerHour, maxHitsPerQuery, indexes); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.AddApiKeyAsync"/>
        /// </summary>
        /// <param name="acls">The list of ACL for this key. Defined by an array of strings that can contains the following values:
        ///   - search: allow searching (https and http)
        ///   - addObject: allow adding/updating an object in the index (https only)
        ///   - deleteObject : allow deleting an existing object (https only)
        ///   - deleteIndex : allow deleting an index (https only)
        ///   - settings : allow getting index settings (https only)
        ///   - editSettings : allow changing index settings (https only)</param>
        /// <param name="validity">The number of seconds after which the key will be automatically removed (0 means no time limit for this key).</param>
        /// <param name="maxQueriesPerIPPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public JObject AddApiKey(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        { return AddApiKey(acls, null, validity, maxQueriesPerIPPerHour, maxHitsPerQuery, indexes); }

        /// <summary>
        /// Update an api key.
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionnary that 
        /// can contains the following values:
        ///   - acl: array of string
        ///   - indices: array of string
        ///   - validity: int
        ///   - referers: array of string
        ///   - description: string
        ///   - maxHitsPerQuery: integer
        ///   - queryParameters: string
        ///   - maxQueriesPerIPPerHour: integer
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> UpdateApiKeyAsync(string key, Dictionary<string, object> parameters, CancellationToken token = default(CancellationToken))
        { return UpdateApiKeyAsync(key, parameters, null, token); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.UpdateApiKeyAsync"/>
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionnary that 
        /// can contains the following values:
        ///   - acl: array of string
        ///   - indices: array of string
        ///   - validity: int
        ///   - referers: array of string
        ///   - description: string
        ///   - maxHitsPerQuery: integer
        ///   - queryParameters: string
        ///   - maxQueriesPerIPPerHour: integer
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public JObject UpdateApiKey(string key, Dictionary<string, object> parameters)
        { return UpdateApiKey(key, parameters, null); }

        /// <summary>
        /// Update an api key.
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="acls">The list of ACL for this key. Defined by an array of strings that can contains the following values:
        ///   - search: allow searching (https and http)
        ///   - addObject: allow adding/updating an object in the index (https only)
        ///   - deleteObject : allow deleting an existing object (https only)
        ///   - deleteIndex : allow deleting an index (https only)
        ///   - settings : allow getting index settings (https only)
        ///   - editSettings : allow changing index settings (https only)</param>
        /// <param name="validity">The number of seconds after which the key will be automatically removed (0 means no time limit for this key).</param>
        /// <param name="maxQueriesPerIPPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> UpdateApiKeyAsync(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        { return UpdateApiKeyAsync(key, acls, null, validity, maxQueriesPerIPPerHour, maxHitsPerQuery, indexes); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.UpdateApiKeyAsync"/>
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="acls">The list of ACL for this key. Defined by an array of strings that can contains the following values:
        ///   - search: allow searching (https and http)
        ///   - addObject: allow adding/updating an object in the index (https only)
        ///   - deleteObject : allow deleting an existing object (https only)
        ///   - deleteIndex : allow deleting an index (https only)
        ///   - settings : allow getting index settings (https only)
        ///   - editSettings : allow changing index settings (https only)</param>
        /// <param name="validity">The number of seconds after which the key will be automatically removed (0 means no time limit for this key).</param>
        /// <param name="maxQueriesPerIPPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public JObject UpdateApiKey(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        { return UpdateApiKey(key, acls, null, validity, maxQueriesPerIPPerHour, maxHitsPerQuery, indexes); }

        /// <summary>
        /// Send a batch targeting multiple indices
        /// </summary>
        /// <param name="actions">An array of action to send.</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string) and a dictionary for the taskIDs.</returns>
        public Task<JObject> BatchAsync(IEnumerable<object> requests, CancellationToken token = default(CancellationToken))
        { return BatchAsync(requests, null, token); }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.BatchAsync"/>
        /// </summary>
        /// <param name="actions">An array of action to send.</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string) and a dictionary for the taskIDs.</returns>
        public JObject Batch(IEnumerable<object> requests)
        { return Batch(requests, null); }

        // ***** METHODS RELATED TO MCM ***** \\

        /// <summary>
        /// Add a userID to the mapping
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="clusterName"></param>
        /// <param name="requestOptions"></param>
        /// <returns>an objecct containing a "updateAt" attribute {'updatedAt': 'XXXX'}</returns>
        public Task<JObject> AssignUserIDAsync(string userID, string clusterName, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            requestOptions = requestOptions ?? new RequestOptions();
            requestOptions.AddExtraHeader("X-Algolia-User-ID", userID);

            Dictionary<string, object> body = new Dictionary<string, object>();
            body.Add("cluster", clusterName);

            return ExecuteRequest(callType.Write, "POST", "/1/clusters/mapping", body, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.AssignUserIDAsync"/>
        /// </summary>
        public JObject AssignUserID(string userID, string clusterName, RequestOptions requestOptions = null)
        {
            return AssignUserIDAsync(userID, clusterName, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Remove a userID from the mapping
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="clusterName"></param>
        /// <param name="requestOptions"></param>
        /// <returns>an objecct containing a "deletedAt" attribute {'deletedAt': 'XXXX'}</returns>
        public Task<JObject> RemoveUserIDAsync(string userID, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            requestOptions = requestOptions ?? new RequestOptions();
            requestOptions.AddExtraHeader("X-Algolia-User-ID", userID);

            return ExecuteRequest(callType.Write, "DELETE", "/1/clusters/mapping", null, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.RemoveUserIDAsync"/>
        /// </summary>
        public JObject RemoveUserID(string userID, RequestOptions requestOptions = null)
        {
            return RemoveUserIDAsync(userID, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// List available cluster in the mapping
        /// Return an object in the form:
        ///{'clusters': [{
        ///		"clusterName": "XXXX",
        ///		"nbRecords": 0,
        ///		"nbUserIDs": 0,
        ///		"dataSize": 0
        ///}	]}
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<JObject> ListClustersAsync(RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(callType.Read, "GET", "/1/clusters", null, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListClustersAsync"/>
        /// </summary>
        public JObject ListClusters(RequestOptions requestOptions = null)
        {
            return ListClustersAsync(requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get one userID in the mapping
        /// returns an object in the form:
        /// {
        /// "userID": "XXXX",
        /// "clusterName": "XXXX",
        /// "nbRecords": 0,
        /// "dataSize": 0
        ///}
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<JObject> GetUserIDAsync(string userID, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(callType.Read, "GET", "/1/clusters/mapping/" + WebUtility.UrlEncode(userID), null, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetUserIDAsync"/>
        /// </summary>
        public JObject GetUserID(string userID, RequestOptions requestOptions = null)
        {
            return GetUserIDAsync(userID, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get top userID in the mapping
        /// returns an object in the form:
        /// {
        /// "userIDs": [{
        ///		 "userID": "userName",
        ///		"clusterName": "name",
        ///		"nbRecords": 0,
        ///		"dataSize": 0
        /// }],
        /// "page": 0,
        /// "hitsPerPage": 20
        ///}
        /// </summary>
        /// <param name="page"></param>
        /// <param name="hitsPerPage"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<JObject> ListUserIDsAsync(int page = 0, int hitsPerPage = 20, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(callType.Read, "GET", "/1/clusters/mapping?page=" + page + "&hitsPerPage=" + hitsPerPage, null, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListUserIDsAsync"/>
        /// </summary>
        public JObject ListUserIDs(int page = 0, int hitsPerPage = 20, RequestOptions requestOptions = null)
        {
            return ListUserIDsAsync(page, hitsPerPage, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get top userID in the mapping
        /// returns an object in the form:
        /// {
        ///"topUsers": {
        ///"XXXX": [{
        ///		"userID": "userName",
        ///		"nbRecords": 0,
        ///		"dataSize": 0
        ///}]
        ////},
        ///"page": 0,
        ///"hitsPerPage": 20
        ///}
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<JObject> GetTopUserIDAsync(RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(callType.Read, "GET", "/1/clusters/mapping/top", null, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetTopUserIDAsync"/>
        /// </summary>
        public JObject GetTopUserID(RequestOptions requestOptions = null)
        {
            return GetTopUserIDAsync(requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        ///  Search userIDs in the mapping
        ///  returns an object in the form:
        /// "hits": [{
        ///		"userID": "userName",
        ///		"clusterName": "name",
        ///		"nbRecords": 0,
        ///		"dataSize": 0
        /// }],
        /// "nbHits":0,
        /// "page": 0,
        /// "hitsPerPage": 20
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="clusterName"></param>
        /// <param name="requestOptions"></param>
        /// <returns>an objecct containing a "updateAt" attribute {'updatedAt': 'XXXX'}</returns>
        public Task<JObject> SearchUserIdsAsync(string query, string clusterName = null, int? page = null, int? hitsPerPage = null, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {

            Dictionary<string, object> body = new Dictionary<string, object>();

            if (query != null)
            {
                body.Add("query", query);
            }

            if (clusterName != null)
            {
                body.Add("cluster", clusterName);
            }

            if (page != null)
            {
                body.Add("page", page);
            }

            if (hitsPerPage != null)
            {
                body.Add("hitsPerPage", hitsPerPage);
            }

            return ExecuteRequest(callType.Read, "POST", "/1/clusters/mapping/search", body, token, requestOptions);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.SearchUserIdsAsync"/>
        /// </summary>
        public JObject SearchUserIds(string query, string clusterName = null, int? page = null, int? hitsPerPage = null, RequestOptions requestOptions = null)
        {
            return SearchUserIdsAsync(query, clusterName, page, hitsPerPage, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the personalization strategy
        /// </summary>
        /// <param name="requestOptions">Request options for the query</param>
        /// <returns></returns>
        public PersonalizationGetStrategyResponse GetPersonalizationStrategy(RequestOptions requestOptions = null)
        {
            return GetPersonalizationStrategyAsync(requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the personalization strategy
        /// </summary>
        /// <param name="requestOptions">Request options for the query</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        public async Task<PersonalizationGetStrategyResponse> GetPersonalizationStrategyAsync(RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            JObject response = await ExecuteRequest(AlgoliaClient.callType.Read, "GET", "/1/recommendation/personalization/strategy", null, token, requestOptions);

            return response.ToObject<PersonalizationGetStrategyResponse>();
        }

        /// <summary>
        /// This command configures the personalization strategy
        /// </summary>
        /// <param name="request">Your personalization strategy</param>
        /// <param name="requestOptions">Request options for the query</param>
        /// <returns></returns>
        public PersonalizationSetStrategyResponse SetPersonalizationStrategy(PersonalizationStrategyRequest request, RequestOptions requestOptions = null)
        {
            return SetPersonalizationStrategyAsync(request, requestOptions).GetAwaiter().GetResult();
        }

        /// <summary>
        /// This command configures the personalization strategy
        /// </summary>
        /// <param name="request">Your personalization strategy</param>
        /// <param name="requestOptions">Request options for the query</param>
        /// <param name="token">The cancellation token</param>
        /// <returns></returns>
        public async Task<PersonalizationSetStrategyResponse> SetPersonalizationStrategyAsync(PersonalizationStrategyRequest request, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken))
        {
            JObject response = await ExecuteRequest(AlgoliaClient.callType.Write, "POST", "/1/recommendation/personalization/strategy", request, token, requestOptions);
            return response.ToObject<PersonalizationSetStrategyResponse>();
        }
    }
}

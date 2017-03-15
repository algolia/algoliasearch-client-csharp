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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Algolia.Search.Utils;
using Algolia.Search.Models;
using System.Security.Cryptography;
using System.Text;

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
        public int DsnInternalTimeout = 60*5;

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
                _readHosts = GetHosts(baseReadHosts, shuffledReadHosts);

                var baseWriteHosts = applicationId + ".algolia.net";
                var shuffledWriteHosts = new List<string> { applicationId + "-1.algolianet.com", applicationId + "-2.algolianet.com", applicationId + "-3.algolianet.com" };
                _writeHosts = GetHosts(baseWriteHosts, shuffledWriteHosts);
            }

            _applicationId = applicationId;
            _apiKey = apiKey;
            _mock = mock;
            // randomize elements of hostsArray (act as a kind of load-balancer)


            HttpClient.DefaultRequestHeaders.Add("X-Algolia-Application-Id", applicationId);
            HttpClient.DefaultRequestHeaders.Add("X-Algolia-API-Key", apiKey);
            //HttpClient.DefaultRequestHeaders.Add("User-Agent", "Algolia for Csharp " + AssemblyInfo.AssemblyVersion);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SearchHttpClient.DefaultRequestHeaders.Add("X-Algolia-Application-Id", applicationId);
            SearchHttpClient.DefaultRequestHeaders.Add("X-Algolia-API-Key", apiKey);
            //SearchHttpClient.DefaultRequestHeaders.Add("User-Agent", "Algolia for Csharp " + AssemblyInfo.AssemblyVersion);
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
        public string[] GetHosts(string baseHost, IEnumerable<string> hosts)
        {
            var result = new List<string> { baseHost };
            //shuffling all but not the first one
            var shuffledHosts = _arrayUtils.Shuffle(hosts);
            result.AddRange(shuffledHosts);
            return result.ToArray();
        }

        public HostStatus SetHostStatus(bool up)
        {
            return new HostStatus { Up = up, LastModified = DateTime.Now };
        }

        public string[] FilterOnActiveHosts(string[] hosts, bool isQuery)
        {

            var validHosts = new List<string> { };
            var statusHosts = isQuery ? _readHostsStatus : _writeHostsStatus;
            foreach (var host in hosts)
            {
                if(statusHosts.ContainsKey(host))
                {
                    var canRetry = (DateTime.Now - statusHosts[host].LastModified).TotalSeconds > DsnInternalTimeout;
                    if (statusHosts[host].Up || canRetry)
                    {
                        validHosts.Add(host);
                    }
                } else
                {
                    validHosts.Add(host);
                }

            }

            return validHosts.Count > 0 ? validHosts.ToArray() : hosts;
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
        public bool GetContinueOnCapturedContext()
        {
            return _continueOnCapturedContext;
        }

        /// <summary>
        /// Set the read timeout for the search and for the build operation
        /// This method should be called before any api call.
        /// </summary>
        public void SetTimeout(double searchTimeout, double writeTimeout)
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
        /// <param name="strategy">Strategy applied on the sequence of queries</param>
        /// <returns></returns>
        public Task<JObject> MultipleQueriesAsync(List<IndexQuery> queries, string strategy = "none", CancellationToken token = default(CancellationToken))
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
            return ExecuteRequest(CallType.Search, "POST", "/1/indexes/*/queries", requests, token);

        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.MultipleQueriesAsync"/>
        /// </summary>
        /// <param name="queries">List of queries per index</param>
        /// <returns></returns>
        public JObject MultipleQueries(List<IndexQuery> queries, string strategy = "none", CancellationToken token = default(CancellationToken))
        {
            return MultipleQueriesAsync(queries, strategy).GetAwaiter().GetResult();
        }

        /// <summary>
        /// List all existing indexes.
        /// </summary>
        /// <returns>An object in the form:
        ///    {"items": [ {"name": "contacts", "createdAt": "2013-01-18T15:33:13.556Z"},
        ///                {"name": "notes", "createdAt": "2013-01-18T15:33:13.556Z"} ] }
        /// </returns>
        public Task<JObject> ListIndexesAsync(CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(CallType.Read, "GET", "/1/indexes/", null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListIndexesAsync"/>
        /// </summary>
        /// <returns>An object in the form:
        ///    {"items": [ {"name": "contacts", "createdAt": "2013-01-18T15:33:13.556Z"},
        ///                {"name": "notes", "createdAt": "2013-01-18T15:33:13.556Z"} ] }
        /// </returns>
        public JObject ListIndexes()
        {
            return ListIndexesAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete an index.
        /// </summary>
        /// <param name="indexName">The name of index to delete</param>
        /// <returns>An object containing a "deletedAt" attribute</returns>
        public Task<JObject> DeleteIndexAsync(string indexName, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(CallType.Write, "DELETE", "/1/indexes/" + Uri.EscapeDataString(indexName), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteIndexAsync"/>
        /// </summary>
        /// <returns>An object containing a "deletedAt" attribute</returns>
        public JObject DeleteIndex(string indexName)
        {
            return DeleteIndexAsync(indexName).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Move an existing index.
        /// </summary>
        /// <param name="srcIndexName">The name of index to move.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        public Task<JObject> MoveIndexAsync(string srcIndexName, string dstIndexName, CancellationToken token = default(CancellationToken))
        {
            Dictionary<string, object> operation = new Dictionary<string, object>();
            operation["operation"] = "move";
            operation["destination"] = dstIndexName;
            return ExecuteRequest(CallType.Write, "POST", string.Format("/1/indexes/{0}/operation", Uri.EscapeDataString(srcIndexName)), operation, token);
        }
        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.MoveIndexAsync"/>
        /// </summary>
        /// <param name="srcIndexName">The name of index to move.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        public JObject MoveIndex(string srcIndexName, string dstIndexName)
        {
            return MoveIndexAsync(srcIndexName, dstIndexName).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Copy an existing index.
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        public Task<JObject> CopyIndexAsync(string srcIndexName, string dstIndexName, CancellationToken token = default(CancellationToken))
        {
            Dictionary<string, object> operation = new Dictionary<string, object>();
            operation["operation"] = "copy";
            operation["destination"] = dstIndexName;
            return ExecuteRequest(CallType.Write, "POST", string.Format("/1/indexes/{0}/operation", Uri.EscapeDataString(srcIndexName)), operation, token);
        }
        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.CopyIndexAsync"/>
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        public JObject CopyIndex(string srcIndexName, string dstIndexName)
        {
            return CopyIndexAsync(srcIndexName, dstIndexName).GetAwaiter().GetResult();
        }

        /// <summary>
        /// The type of log
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// All build logs
            /// </summary>
            LogBuild,
            /// <summary>
            /// All query logs
            /// </summary>
            LogQuery,
            /// <summary>
            /// All error logs
            /// </summary>
            LogError,
            /// <summary>
            /// All logs
            /// </summary>
            LogAll
        }

        /// <summary>
        /// Return last logs entries.
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="onlyErrors">If set to true, the answer will only contain API calls with errors.</param>
        public Task<JObject> GetLogsAsync(int offset = 0, int length = 10, bool onlyErrors = false)
        {
            return GetLogsAsync(offset, length, onlyErrors ? LogType.LogError : LogType.LogAll);
        }

        /// <summary>
        /// Return last logs entries.
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="logType">Specify the type of logs to include.</param>
        public Task<JObject> GetLogsAsync(int offset = 0, int length = 10, LogType logType = LogType.LogAll, CancellationToken token = default(CancellationToken))
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
            if (logType != LogType.LogAll)
            {
                string type = "";
                switch (logType)
                {
                    case LogType.LogBuild:
                        type = "build";
                        break;
                    case LogType.LogQuery:
                        type = "query";
                        break;
                    case LogType.LogError:
                        type = "error";
                        break;
                    case LogType.LogAll:
                        type = "all";
                        break;
                }
                if (param.Length == 0)
                    param += string.Format("?type={0}", type);
                else
                    param += string.Format("&type={0}", type);
            }
            return ExecuteRequest(CallType.Write, "GET", string.Format("/1/logs{0}", param), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetLogsAsync(int, int, bool)"/>
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="onlyErrors">If set to true, the answer will only contain API calls with errors.</param>
        public JObject GetLogs(int offset = 0, int length = 10, bool onlyErrors = false)
        {
            return GetLogsAsync(offset, length, onlyErrors).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetLogsAsync(int, int, Algolia.Search.AlgoliaClient.LogType)"/>
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="logType">Specify the type of logs to include.</param>
        public JObject GetLogs(int offset, int length, LogType logType)
        {
            return GetLogsAsync(offset, length, logType).GetAwaiter().GetResult();
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
        /// List all existing user keys with their associated ACLs.
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        [Obsolete("ListUserKeysAsync is deprecated, please use ListApiKeysAsync instead.")]
        public Task<JObject> ListUserKeysAsync(CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(CallType.Read, "GET", "/1/keys", null, token);
        }

        /// <summary>
        /// List all existing api keys with their associated ACLs.
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        public Task<JObject> ListApiKeysAsync(CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(CallType.Read, "GET", "/1/keys", null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListApiKeysAsync"/>
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        [Obsolete("ListUserKeys is deprecated, please use ListApiKeys instead.")]
        public JObject ListUserKeys()
        {
            return ListApiKeysAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListApiKeysAsync"/>
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        public JObject ListApiKeys()
        {
            return ListApiKeysAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get ACL for an existing user key.
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        [Obsolete("GetUserKeyACLAsync is deprecated, please use GetApiKeyACLAsync instead.")]
        public Task<JObject> GetUserKeyAclAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(CallType.Read, "GET", "/1/keys/" + key, null, token);
        }

        /// <summary>
        /// Get ACL for an existing api key.
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        public Task<JObject> GetApiKeyAclAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(CallType.Read, "GET", "/1/keys/" + key, null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="GetApiKeyAclAsync"/>
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        [Obsolete("GetUserKeyACL is deprecated, please use GetApiKeyACL instead.")]
        public JObject GetUserKeyAcl(string key)
        {
            return GetApiKeyAclAsync(key).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="GetApiKeyAclAsync"/>
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        public JObject GetApiKeyAcl(string key)
        {
            return GetApiKeyAclAsync(key).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete an existing user key.
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        [Obsolete("DeleteUserKeyAsync is deprecated, please use DeleteApiKeyAsync instead.")]
        public Task<JObject> DeleteUserKeyAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(CallType.Write, "DELETE", "/1/keys/" + key, null, token);
        }

        /// <summary>
        /// Delete an existing user key.
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        public Task<JObject> DeleteApiKeyAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(CallType.Write, "DELETE", "/1/keys/" + key, null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteApiKeyAsync"/>
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        [Obsolete("DeleteUserKey is deprecated, please use DeleteApiKey instead.")]
        public JObject DeleteUserKey(string key)
        {
            return DeleteApiKeyAsync(key).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteApiKeyAsync"/>
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        public JObject DeleteApiKey(string key)
        {
            return DeleteApiKeyAsync(key).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create a new user key.
        /// </summary>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
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
        [Obsolete("AddUserKeyAsync is deprecated, please use AddApiKeyAsync instead.")]
        public Task<JObject> AddUserKeyAsync(Dictionary<string, object> parameters, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(CallType.Write, "POST", "/1/keys", parameters, token);
        }

        /// <summary>
        /// Create a new api key.
        /// </summary>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
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
        {
            return ExecuteRequest(CallType.Write, "POST", "/1/keys", parameters, token);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.AddApiKeyAsync"/>
        /// </summary>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
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
        [Obsolete("AddUserKey is deprecated, please use AddApiKey instead.")]
        public JObject AddUserKey(Dictionary<string, object> parameters)
        {
            return AddApiKeyAsync(parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.AddApiKeyAsync"/>
        /// </summary>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
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
        {
            return AddApiKeyAsync(parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create a new user key.
        /// </summary>
        /// <param name="acls">The list of ACL for this key. Defined by an array of strings that can contains the following values:
        ///   - search: allow searching (https and http)
        ///   - addObject: allow adding/updating an object in the index (https only)
        ///   - deleteObject : allow deleting an existing object (https only)
        ///   - deleteIndex : allow deleting an index (https only)
        ///   - settings : allow getting index settings (https only)
        ///   - editSettings : allow changing index settings (https only)</param>
        /// <param name="validity">The number of seconds after which the key will be automatically removed (0 means no time limit for this key).</param>
        /// <param name="maxQueriesPerIpPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        [Obsolete("AddUserKeyAsync is deprecated, please use AddApiKeyAsync instead.")]
        public Task<JObject> AddUserKeyAsync(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            if (indexes == null)
            {
                indexes = new string[0] { };
            }
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIpPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            content["indexes"] = indexes;
            return AddApiKeyAsync(content);
        }

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
        /// <param name="maxQueriesPerIpPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> AddApiKeyAsync(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            if (indexes == null)
            {
                indexes = new string[0] { };
            }
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIpPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            content["indexes"] = indexes;
            return AddApiKeyAsync(content);
        }

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
        /// <param name="maxQueriesPerIpPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        [Obsolete("AddUserKey is deprecated, please use AddApiKey instead.")]
        public JObject AddUserKey(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            return AddApiKeyAsync(acls, validity, maxQueriesPerIpPerHour, maxHitsPerQuery, indexes).GetAwaiter().GetResult();
        }

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
        /// <param name="maxQueriesPerIpPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public JObject AddApiKey(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            return AddApiKeyAsync(acls, validity, maxQueriesPerIpPerHour, maxHitsPerQuery, indexes).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Update a user key.
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
        /// can contains the following values:
        ///   - acl: array of string
        ///   - indices: array of string
        ///   - validity: int
        ///   - referers: array of string
        ///   - description: string
        ///   - maxHitsPerQuery: integer
        ///   - queryParameters: string
        ///   - maxQueriesPerIPPerHour: integer</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        [Obsolete("UpdateUserKeyAsync is deprecated, please use UpdateApiKeyAsync instead.")]
        public Task<JObject> UpdateUserKeyAsync(string key, Dictionary<string, object> parameters, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(CallType.Write, "PUT", "/1/keys/" + key, parameters, token);
        }

        /// <summary>
        /// Update an api key.
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
        /// can contains the following values:
        ///   - acl: array of string
        ///   - indices: array of string
        ///   - validity: int
        ///   - referers: array of string
        ///   - description: string
        ///   - maxHitsPerQuery: integer
        ///   - queryParameters: string
        ///   - maxQueriesPerIPPerHour: integer</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> UpdateApiKeyAsync(string key, Dictionary<string, object> parameters, CancellationToken token = default(CancellationToken))
        {
            return ExecuteRequest(CallType.Write, "PUT", "/1/keys/" + key, parameters, token);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.UpdateApiKeyAsync"/>
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
        /// can contains the following values:
        ///   - acl: array of string
        ///   - indices: array of string
        ///   - validity: int
        ///   - referers: array of string
        ///   - description: string
        ///   - maxHitsPerQuery: integer
        ///   - queryParameters: string
        ///   - maxQueriesPerIPPerHour: integer</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        [Obsolete("UpdateUserKey is deprecated, please use UpdateApiKey instead.")]
        public JObject UpdateUserKey(string key, Dictionary<string, object> parameters)
        {
            return UpdateApiKeyAsync(key, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.UpdateApiKeyAsync"/>
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
        /// can contains the following values:
        ///   - acl: array of string
        ///   - indices: array of string
        ///   - validity: int
        ///   - referers: array of string
        ///   - description: string
        ///   - maxHitsPerQuery: integer
        ///   - queryParameters: string
        ///   - maxQueriesPerIPPerHour: integer</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public JObject UpdateApiKey(string key, Dictionary<string, object> parameters)
        {
            return UpdateApiKeyAsync(key, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Update a user key.
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
        /// <param name="maxQueriesPerIpPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        [Obsolete("UpdateUserKeyAsync is deprecated, please use UpdateApiKeyAsync instead.")]
        public Task<JObject> UpdateUserKeyAsync(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            if (indexes == null)
            {
                indexes = new string[0]{};
            }
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIpPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            content["indexes"] = indexes;
            return UpdateApiKeyAsync(key, content);
        }

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
        /// <param name="maxQueriesPerIpPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> UpdateApiKeyAsync(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            if (indexes == null)
            {
                indexes = new string[0] { };
            }
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIpPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            content["indexes"] = indexes;
            return UpdateApiKeyAsync(key, content);
        }

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
        /// <param name="maxQueriesPerIpPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        [Obsolete("UpdateUserKey is deprecated, please use UpdateApiKey instead.")]
        public JObject UpdateUserKey(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            return UpdateApiKeyAsync(key, acls, validity, maxQueriesPerIpPerHour, maxHitsPerQuery, indexes).GetAwaiter().GetResult();
        }

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
        /// <param name="maxQueriesPerIpPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <param name="indexes">Restrict the new API key to specific index names.</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public JObject UpdateApiKey(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null)
        {
            return UpdateApiKeyAsync(key, acls, validity, maxQueriesPerIpPerHour, maxHitsPerQuery, indexes).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Send a batch targeting multiple indices
        /// </summary>
        /// <param name="requests">An array of requests to send.</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string) and a dictionary for the taskIDs.</returns>
        public Task<JObject> BatchAsync(IEnumerable<object> requests, CancellationToken token = default(CancellationToken))
        {
            Dictionary<string, object> batch = new Dictionary<string, object>();
            batch["requests"] = requests;
            return ExecuteRequest(AlgoliaClient.CallType.Write, "POST", "/1/indexes/*/batch", batch, token);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.BatchAsync"/>
        /// </summary>
        /// <param name="requests">An array of requests to send.</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string) and a dictionary for the taskIDs.</returns>
        public JObject Batch(IEnumerable<object> requests)
        {
            return BatchAsync(requests).GetAwaiter().GetResult();
        }



        /// <summary>
        /// Generates a secured and public API Key from a query parameters and an optional user token identifying the current user
        /// </summary>
        /// <param name="privateApiKey">Your private API Key</param>
        /// <param name="query">The query parameters applied to the query (used as security)</param>
        /// <param name="userToken">An optional token identifying the current user</param>
        /// <returns></returns>
        public string GenerateSecuredApiKey(string privateApiKey, Query query, string userToken = null)
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
        public string GenerateSecuredApiKey(string privateApiKey, string tagFilter, string userToken = null)
        {
            if (!tagFilter.Contains("="))
                return GenerateSecuredApiKey(privateApiKey, new Query().SetTagFilters(tagFilter), userToken);
            else
            {
                if (!string.IsNullOrEmpty(userToken))
                {
                    tagFilter = string.Format("{0}&userToken={1}", tagFilter, Uri.EscapeDataString(userToken));
                }

                var hash = Hmac(privateApiKey, tagFilter);
                byte[] content = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}{1}", hash, tagFilter));
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

        public enum CallType {
            Search,
            Write,
            Read
        };

        /// <summary>
        /// Used to execute the search request
        /// </summary>
        /// <param name="method">HTTP method</param>
        /// <param name="requestUrl">URL to request</param>
        /// <param name="content">The content</param>
        /// <returns></returns>
        public async Task<JObject> ExecuteRequest(CallType type, string method, string requestUrl, object content, CancellationToken token)
        {
            string[] hosts = null;
            HttpClient client = null;
            if (type == CallType.Search)
            {
                hosts = FilterOnActiveHosts(_readHosts, true);
                client = _searchHttpClient;
            }
            else
            {
                hosts = type == CallType.Read
                    ? FilterOnActiveHosts(_readHosts, true)
                    : FilterOnActiveHosts(_writeHosts, false);
                client = _buildHttpClient;
            }

            Dictionary<string, string> errors = new Dictionary<string, string>();
            foreach (string host in hosts)
            {
                try
                {
                    try
                    {
                        string url = string.Format("https://{0}{1}", host, requestUrl);
                        HttpResponseMessage responseMsg = null;
                        switch (method)
                        {
                            case "GET":
                                responseMsg = await client.GetAsync(url, token).ConfigureAwait(_continueOnCapturedContext);
                                break;
                            case "POST":
                                HttpContent postcontent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content));
                                responseMsg = await client.PostAsync(url, postcontent, token).ConfigureAwait(_continueOnCapturedContext);
                                break;
                            case "PUT":
                                HttpContent putContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content));
                                responseMsg = await client.PutAsync(url, putContent, token).ConfigureAwait(_continueOnCapturedContext);
                                break;
                            case "DELETE":
                                responseMsg = await client.DeleteAsync(url, token).ConfigureAwait(_continueOnCapturedContext);
                                break;
                        }
                        if (responseMsg.IsSuccessStatusCode)
                        {
                            string serializedJson = await responseMsg.Content.ReadAsStringAsync().ConfigureAwait(_continueOnCapturedContext);
                            JObject obj = JObject.Parse(serializedJson);
                            if(type == CallType.Search || type == CallType.Read)
                            {
                                _readHostsStatus[host] = SetHostStatus(true);
                            } else
                            {
                                _writeHostsStatus[host] = SetHostStatus(true);
                            }
                            return obj;
                        }
                        else
                        {
                            string serializedJson = await responseMsg.Content.ReadAsStringAsync().ConfigureAwait(_continueOnCapturedContext);
                            string message = "Internal Error";
                            string status = "0";
                            try
                            {
                                JObject obj = JObject.Parse(serializedJson);
                                message = obj["message"].ToString();
                                status = obj["status"].ToString();
                                if (obj["status"].ToObject<int>() / 100 == 4)
                                {
                                    throw new AlgoliaException(message);
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
                        if (type == CallType.Search || type == CallType.Read)
                        {
                            _readHostsStatus[host] = SetHostStatus(false);
                        }
                        else
                        {
                            _writeHostsStatus[host] = SetHostStatus(false);
                        }
                        throw;
                    }
                    catch (TaskCanceledException e)
                    {
                        if (token.IsCancellationRequested)
                        {
                            throw e;
                        }
                        if (type == CallType.Search || type == CallType.Read)
                        {
                            _readHostsStatus[host] = SetHostStatus(false);
                        }
                        else
                        {
                            _writeHostsStatus[host] = SetHostStatus(false);
                        }
                        errors.Add(host, "Timeout expired");
                    }
                    catch (Exception ex)
                    {
                        if (type == CallType.Search || type == CallType.Read)
                        {
                            _readHostsStatus[host] = SetHostStatus(false);
                        }
                        else
                        {
                            _writeHostsStatus[host] = SetHostStatus(false);
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
    }
}

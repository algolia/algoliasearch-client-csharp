﻿/*
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
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using System.Reflection;
using PCLCrypto;

namespace Algolia.Search
{

    /// <summary>
    /// Client for the Algolia Search cloud API.
    /// You should instantiate a Client object with your ApplicationID, ApiKey and Hosts to start using Algolia Search API
    /// </summary>
    public class AlgoliaClient
    {
        private IEnumerable<string> _hosts;
        private string _applicationId;
        private string _apiKey;
        private HttpClient _httpClient;
        private MockHttpMessageHandler _mock;
        private bool _continueOnCapturedContext;

        /// <summary>
        /// Algolia Search initialization
        /// </summary>
        /// <param name="applicationId">The application ID you have in your admin interface</param>
        /// <param name="apiKey">A valid API key for the service</param>
        /// <param name="hosts">The list of hosts that you have received for the service</param>
        /// <param name="mock">Mocking object for controlling HTTP message handler</param>
        public AlgoliaClient(string applicationId, string apiKey, IEnumerable<string> hosts = null, MockHttpMessageHandler mock = null)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
                throw new ArgumentOutOfRangeException("applicationId", "An application Id is required.");

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentOutOfRangeException("apiKey", "An API key is required.");

            if (hosts == null)
                hosts = new string[] {applicationId + "-1.algolia.net",
                                      applicationId + "-2.algolia.net",
                                      applicationId + "-3.algolia.net"};

            IEnumerable<string> allHosts = hosts as string[] ?? hosts.ToArray();
            if (!allHosts.Any())
                throw new ArgumentOutOfRangeException("hosts", "At least one host is required");

            _applicationId = applicationId;
            _apiKey = apiKey;

            _mock = mock;

            // randomize elements of hostsArray (act as a kind of load-balancer)
            _hosts = allHosts.OrderBy(s => Guid.NewGuid());

            HttpClient.DefaultRequestHeaders.Add("X-Algolia-Application-Id", applicationId);
            HttpClient.DefaultRequestHeaders.Add("X-Algolia-API-Key", apiKey);
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Algolia for Csharp " + AssemblyInfo.AssemblyVersion);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _continueOnCapturedContext = true;
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
        /// Add security tag header (see http://www.algolia.com/doc/guides/csharp#SecurityUser for more details)
        /// </summary>
        /// <param name="tag"></param>
        public void SetSecurityTags(string tag)
        {
            HttpClient.DefaultRequestHeaders.Add("X-Algolia-TagFilters", tag);
        }

        /// <summary>
        /// Add user-token header (see http://www.algolia.com/doc/guides/csharp#SecurityUser for more details)
        /// </summary>
        /// <param name="userToken"></param>
        public void SetUserToken(string userToken)
        {
            HttpClient.DefaultRequestHeaders.Add("X-Algolia-UserToken", userToken);
        }

        /// <summary>
        /// Set extra HTTP request headers
        /// </summary>
        /// <param name="key">The header key</param>
        /// <param name="value">The header value</param>
        public void SetExtraHeader(string key, string value)
        {
            HttpClient.DefaultRequestHeaders.Add(key, value);
        }


        /// <summary>
        /// This method allows querying multiple indexes with one API call
        /// </summary>
        /// <param name="queries">List of queries per index</param>
        /// <returns></returns>
        public Task<JObject> MultipleQueriesAsync(List<IndexQuery> queries)
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
            return ExecuteRequest("POST", "/1/indexes/*/queries", requests);

        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.MultipleQueriesAsync"/>
        /// </summary>
        /// <param name="queries">List of queries per index</param>
        /// <returns></returns>
        public JObject MultipleQueries(List<IndexQuery> queries)
        {
            return MultipleQueriesAsync(queries).GetAwaiter().GetResult();
        }

        /// <summary>
        /// List all existing indexes.
        /// </summary>
        /// <returns>An object in the form:
        ///    {"items": [ {"name": "contacts", "createdAt": "2013-01-18T15:33:13.556Z"},
        ///                {"name": "notes", "createdAt": "2013-01-18T15:33:13.556Z"} ] }
        /// </returns>
        public Task<JObject> ListIndexesAsync()
        {
            return ExecuteRequest("GET", "/1/indexes/");
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
        public Task<JObject> DeleteIndexAsync(string indexName)
        {
            return ExecuteRequest("DELETE", "/1/indexes/" + Uri.EscapeDataString(indexName));
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
        public Task<JObject> MoveIndexAsync(string srcIndexName, string dstIndexName)
        {
            Dictionary<string, object> operation = new Dictionary<string, object>();
            operation["operation"] = "move";
            operation["destination"] = dstIndexName;
            return ExecuteRequest("POST", string.Format("/1/indexes/{0}/operation", Uri.EscapeDataString(srcIndexName)), operation);
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
        public Task<JObject> CopyIndexAsync(string srcIndexName, string dstIndexName)
        {
            Dictionary<string, object> operation = new Dictionary<string, object>();
            operation["operation"] = "copy";
            operation["destination"] = dstIndexName;
            return ExecuteRequest("POST", string.Format("/1/indexes/{0}/operation", Uri.EscapeDataString(srcIndexName)), operation);
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
        public Task<JObject> GetLogsAsync(int offset = 0, int length = 10, bool onlyErrors = false)
        {
            return GetLogsAsync(offset, length, onlyErrors ? LogType.LOG_ERROR : LogType.LOG_ALL);
        }

        /// <summary>
        /// Return last logs entries.
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="logType">Specify the type of logs to include.</param>
        public Task<JObject> GetLogsAsync(int offset = 0, int length = 10, LogType logType = LogType.LOG_ALL)
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
                    param += string.Format("?onlyErrors={0}", type);
                else
                    param += string.Format("&onlyErrors={0}", type);
            }
            return ExecuteRequest("GET", String.Format("/1/logs{0}", param));
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
        public Task<JObject> ListUserKeysAsync()
        {
            return ExecuteRequest("GET", "/1/keys");
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListUserKeysAsync"/> 
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        public JObject ListUserKeys()
        {
            return ListUserKeysAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get ACL for an existing user key.
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        public Task<JObject> GetUserKeyACLAsync(string key)
        {
            return ExecuteRequest("GET", "/1/keys/" + key);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetUserKeyACLAsync"/> 
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        public JObject GetUserKeyACL(string key)
        {
            return GetUserKeyACLAsync(key).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete an existing user key.
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        public Task<JObject> DeleteUserKeyAsync(string key)
        {
            return ExecuteRequest("DELETE", "/1/keys/" + key);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteUserKeyAsync"/> 
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        public JObject DeleteUserKey(string key)
        {
            return DeleteUserKeyAsync(key).GetAwaiter().GetResult();
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
        /// <param name="maxQueriesPerIPPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> AddUserKeyAsync(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0)
        {
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIPPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            return ExecuteRequest("POST", "/1/keys", content);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.AddUserKeyAsync"/>
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
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public JObject AddUserKey(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0)
        {
            return AddUserKeyAsync(acls, validity, maxQueriesPerIPPerHour, maxHitsPerQuery).GetAwaiter().GetResult();
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
        /// <param name="maxQueriesPerIPPerHour">Specify the maximum number of API calls allowed from an IP address per hour.  Defaults to 0 (no rate limit).</param>
        /// <param name="maxHitsPerQuery">Specify the maximum number of hits this API key can retrieve in one call. Defaults to 0 (unlimited).</param>
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> UpdateUserKeyAsync(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0)
        {
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIPPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            return ExecuteRequest("PUT", "/1/keys/" + key, content);
        }

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.UpdateUserKeyAsync"/>
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
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public JObject UpdateUserKey(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0)
        {
            return UpdateUserKeyAsync(key, acls, validity, maxQueriesPerIPPerHour, maxHitsPerQuery).GetAwaiter().GetResult();
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
            string msg = tagFilter;
            if (!string.IsNullOrWhiteSpace(userToken))
            {
                msg += userToken;
            }
            return Hmac(privateApiKey, msg);
        }

        private string Hmac(string key, string msg)
        {
            var keyMaterial = StringToAscii(key);
            var data = StringToAscii(msg);

            var algorithm = WinRTCrypto.MacAlgorithmProvider.OpenAlgorithm(MacAlgorithm.HmacSha256);

            var hasher = algorithm.CreateHash(keyMaterial);
            hasher.Append(data);

            return hasher.GetValueAndReset().Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
        }

        private byte[] StringToAscii(string s)
        {
            byte[] retval = new byte[s.Length];
            for (int ix = 0; ix < s.Length; ++ix)
            {
                char ch = s[ix];
                if (ch <= 0x7f) retval[ix] = (byte)ch;
                else retval[ix] = (byte)'?';
            }
            return retval;
        }

        /// <summary>
        /// Main HTTP client
        /// </summary>
        protected HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    if (_mock == null)
                        _httpClient = new HttpClient();
                    else
                        _httpClient = new HttpClient(_mock);
                }
                return _httpClient;
            }
        }

        /// <summary>
        /// Used to execute the search request
        /// </summary>
        /// <param name="method">HTTP method</param>
        /// <param name="requestUrl">URL to request</param>
        /// <param name="content">The content</param>
        /// <returns></returns>
        public async Task<JObject> ExecuteRequest(string method, string requestUrl, object content = null)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            foreach (string host in _hosts)
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
                                responseMsg = await HttpClient.GetAsync(url).ConfigureAwait(_continueOnCapturedContext);
                                break;
                            case "POST":
                                HttpContent postcontent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content));
                                responseMsg = await HttpClient.PostAsync(url, postcontent).ConfigureAwait(_continueOnCapturedContext);
                                break;
                            case "PUT":
                                HttpContent putContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content));
                                responseMsg = await HttpClient.PutAsync(url, putContent).ConfigureAwait(_continueOnCapturedContext);
                                break;
                            case "DELETE":
                                responseMsg = await HttpClient.DeleteAsync(url).ConfigureAwait(_continueOnCapturedContext);
                                break;
                        }
                        if (responseMsg.IsSuccessStatusCode)
                        {
                            string serializedJSON = await responseMsg.Content.ReadAsStringAsync().ConfigureAwait(_continueOnCapturedContext);
                            JObject obj = JObject.Parse(serializedJSON);
                            return obj;
                        }
                        else if (responseMsg.StatusCode == HttpStatusCode.BadRequest || responseMsg.StatusCode == HttpStatusCode.Forbidden || responseMsg.StatusCode == HttpStatusCode.NotFound)
                        {
                            string serializedJSON = await responseMsg.Content.ReadAsStringAsync().ConfigureAwait(_continueOnCapturedContext);
                            JObject obj = JObject.Parse(serializedJSON);
                            string message = (string)obj["message"];
                            throw new AlgoliaException(message);
                        }
                        else
                        {
                            errors.Add(host, responseMsg.ReasonPhrase);
                        }
                    }
                    catch (Exception ex)
                    {
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

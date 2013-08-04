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
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Algolia.Search
{

    /// <summary>
    /// Client for the Algolia Search cloud API.
    /// You should instantiate a Client object with your ApplicationID, ApiKey and Hosts to start using Algolia Search API
    /// </summary>
    public class AlgoliaClient
    {
        private IEnumerable<string>  _hosts;
        private string               _applicationId;
        private string               _apiKey;
        private HttpClient           _httpClient;

        /// <summary>
        ///Algolia Search initialization
        /// </summary>
        /// <param name="applicationId">the application ID you have in your admin interface</param>
        /// <param name="apiKey">a valid API key for the service</param>
        public AlgoliaClient(string applicationId, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
                throw new ArgumentOutOfRangeException("applicationId", "An application Id is requred.");

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentOutOfRangeException("apiKey", "An API key is required.");

            IEnumerable<string> allHosts = new string[] {applicationId + "-1.algolia.io",
                                                         applicationId + "-2.algolia.io",
                                                         applicationId + "-3.algolia.io"};
            _applicationId = applicationId;
            _apiKey = apiKey;

            // randomize elements of hostsArray (act as a kind of load-balancer)
            _hosts = allHosts.OrderBy(s => Guid.NewGuid());

            HttpClient.DefaultRequestHeaders.Add("X-Algolia-Application-Id", applicationId);
            HttpClient.DefaultRequestHeaders.Add("X-Algolia-API-Key", apiKey);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        ///Algolia Search initialization
        /// </summary>
        /// <param name="applicationId">the application ID you have in your admin interface</param>
        /// <param name="apiKey">a valid API key for the service</param>
        /// <param name="hosts">the list of hosts that you have received for the service</param>
        public AlgoliaClient(string applicationId, string apiKey, IEnumerable<string> hosts)
        {
            if(string.IsNullOrWhiteSpace(applicationId))
                throw new ArgumentOutOfRangeException("applicationId","An application Id is requred.");

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentOutOfRangeException("apiKey", "An API key is required.");

            IEnumerable<string> allHosts = hosts as string[] ?? hosts.ToArray();
            if (hosts == null || !allHosts.Any())
                throw new ArgumentOutOfRangeException("hosts", "At least one host is requred");

            _applicationId = applicationId;
            _apiKey = apiKey;

            // randomize elements of hostsArray (act as a kind of load-balancer)
            _hosts = allHosts.OrderBy(s => Guid.NewGuid());

            HttpClient.DefaultRequestHeaders.Add("X-Algolia-Application-Id", applicationId);
            HttpClient.DefaultRequestHeaders.Add("X-Algolia-API-Key", apiKey);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// List all existing indexes.
        /// </summary>
        /// <returns>an object in the form:
        ///    {"items": [ {"name": "contacts", "createdAt": "2013-01-18T15:33:13.556Z"},
        ///                {"name": "notes", "createdAt": "2013-01-18T15:33:13.556Z"} ] }
        /// </returns>
        public Task<JObject> ListIndexes()
        {
            return ExecuteRequest("GET", "/1/indexes/");
        }

        /// <summary>
        /// Delete an index.
        /// </summary>
        /// <param name="indexName">the name of index to delete</param>
        /// <returns>an object containing a "deletedAt" attribute</returns>
        public Task<JObject> DeleteIndex(string indexName)
        {
            return ExecuteRequest("DELETE", "/1/indexes/" + Uri.EscapeDataString(indexName));
        }

        /// <summary>
        /// Get the index object initialized (no server call needed for initialization).
        /// </summary>
        /// <param name="indexName">the name of index</param>
        /// <returns>an instance of Index object that expose Index actions</returns>
        public Index InitIndex(string indexName)
        {
            return new Index(this, indexName);
        }

        /// <summary>
        /// List all existing user keys with their associated ACLs.
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        public Task<JObject> ListUserKeys()
        {
            return ExecuteRequest("GET", "/1/keys");
        }

        /// <summary>
        /// Get ACL of an existing user key.
        /// </summary>
        /// <returns>return an object with an "acls" array containing an array of string with rights.</returns>
        public Task<JObject> GetUserKeyACL(string key)
        {
            return ExecuteRequest("GET", "/1/keys/" + key);
        }

        /// <summary>
        /// Delete an existing user key.
        /// </summary>
        /// <returns>return an object with a "deleteAt" attribute.</returns>
        public Task<JObject> DeleteUserKey(string key)
        {
            return ExecuteRequest("DELETE", "/1/keys/" + key);
        }

        /// <summary>
        /// Create a new user key.
        /// </summary>
        /// <param name="acls">the list of ACL for this key. Defined by an array of strings that can contains the following values:
        ///   - search: allow to search (https and http)
        ///   - addObject: allows to add/update an object in the index (https only)
        ///   - deleteObject : allows to delete an existing object (https only)
        ///   - deleteIndex : allows to delete index content (https only)
        ///   - settings : allows to get index settings (https only)
        ///   - editSettings : allows to change index settings (https only)</param>
        /// <param name="validity">the number of seconds after which the key will be automatically removed (0 means no time limit for this key)</param>
        /// <returns>Return an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> AddUserKey(IEnumerable<string> acls, int validity = 0)
        {
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            return ExecuteRequest("POST", "/1/keys", content);
        }

        protected HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                }
                return _httpClient;
            }
        }

        public async Task<JObject> ExecuteRequest(string method, string requestUrl, object content = null)
        {
            foreach (string host in _hosts)
            {
                try
                {
                    string url = string.Format("https://{0}{1}", host, requestUrl);
                    HttpResponseMessage responseMsg = null;
                    switch (method)
                    {
                        case "GET":
                            responseMsg = await HttpClient.GetAsync(url);
                            break;
                        case "POST":
                            HttpContent postcontent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content));
                            responseMsg = await HttpClient.PostAsync(url, postcontent);
                            break;
                        case "PUT":
                            HttpContent putContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content));
                            responseMsg = await HttpClient.PutAsync(url, putContent);
                            break;
                        case "DELETE":
                            responseMsg = await HttpClient.DeleteAsync(url);
                            break;
                    }
                    if (responseMsg.IsSuccessStatusCode)
                    {
                        string serializedJSON = await responseMsg.Content.ReadAsStringAsync();
                        JObject obj = JObject.Parse(serializedJSON);
                        return obj;
                    }
                    else
                    {
                        string serializedJSON = await responseMsg.Content.ReadAsStringAsync();
                        
                        JObject obj = JObject.Parse(serializedJSON);
                        string message = (string)obj["message"];
                        throw new AlgoliaException(message);
                    }
                }
                catch (AlgoliaException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("AlgoliaClient exception: " + ex.ToString());
                }
            }
            throw new AlgoliaException("Hosts unreachable.");
        }
    }
}

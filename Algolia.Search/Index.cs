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
using Newtonsoft.Json.Linq;

namespace Algolia.Search
{
    /// <summary>
    ///  Contains all the functions related to one index
    /// You should use AlgoliaClient.initIndex(indexName) to instanciate this object.
    /// </summary>
    public class Index
    {
        private AlgoliaClient  _client;
        private string         _indexName;
        private string         _urlIndexName;

        /// <summary>
        /// Index initialization (You should not call this constructor yourself)
        /// </summary>
        public Index(AlgoliaClient client, string indexName)
        {
            _client = client;
            _indexName = indexName;
            _urlIndexName = Uri.EscapeDataString(indexName);
        }

        /// <summary>
        /// Add an object in this index.
        /// </summary>
        /// <param name="content">Represent your object that you want to add in the index.</param>
        /// <param name="objectId">Optional. an objectID you want to attribute to this object (if the attribute already exist the old object will be overwrite)</param>
        /// <returns>An object that contains an "objectID" attribute.</returns>
        public Task<JObject> AddObject(object content, string objectId = null)
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                return _client.ExecuteRequest("POST", string.Format("/1/indexes/{0}", _urlIndexName), content);
            }
            else
            {
                return _client.ExecuteRequest("PUT", string.Format("/1/indexes/{0}/{1}", _urlIndexName, Uri.EscapeDataString(objectId)), content);
            }
        }

        /// <summary>
        /// Add several objects.
        /// </summary>
        /// <param name="objects">contains an array of objects to add. If the object contains an objectID.</param>
        /// <returns>return an object containing an "objectIDs" attribute (array of string)</returns>
        public Task<JObject> AddObjects(IEnumerable<object> objects)
        {
            List<object> requests = new List<object>();
            foreach (object obj in objects) {
                Dictionary<string, object> request = new Dictionary<string, object>();
                request["action"] = "addObject";
                request["body"] = obj;
                requests.Add(request);
            }
            Dictionary<string, object> batch = new Dictionary<string, object>();
            batch["requests"] = requests;
            return _client.ExecuteRequest("POST", string.Format("/1/indexes/{0}/batch", _urlIndexName), batch);
        }

        /// <summary>
        /// Get an object from this index
        /// </summary>
        /// <param name="objectID">the unique identifier of the object to retrieve</param>
        /// <param name="attributesToRetrieve"> if set, contains the list of attributes to retrieve</param>
        /// <returns></returns>
        public Task<JObject> GetObject(string objectID, IEnumerable<string> attributesToRetrieve = null)
        {
            if (attributesToRetrieve == null)
            {
                return _client.ExecuteRequest("GET", string.Format("/1/indexes/{0}/{1}", _urlIndexName, Uri.EscapeDataString(objectID)));
            }
            else
            {
                string attributes = "";
                foreach (string attr in attributesToRetrieve)
                {
                    if (attributes.Length > 0)
                        attributes += ",";
                    attributes += Uri.EscapeDataString(attr);
                }
                return _client.ExecuteRequest("GET", string.Format("/1/indexes/{0}/{1}?attributes={2}", _urlIndexName, Uri.EscapeDataString(objectID), attributes));
            }
        }

        /// <summary>
        /// Update partially an object (only update attributes passed in argument).
        /// </summary>
        /// <param name="partialObject">contains the object attributes to override, the object must contains an objectID attribute</param>
        /// <returns>return an object containing an "updatedAt" attribute.</returns>
        public Task<JObject> PartialUpdateObject(JObject partialObject)
        {
            string objectID = (string)partialObject["objectID"];
            return _client.ExecuteRequest("POST", string.Format("/1/indexes/{0}/{1}/partial", _urlIndexName, Uri.EscapeDataString(objectID)), partialObject);
        }

        /// <summary>
        /// Override the content of object.
        /// </summary>
        /// <param name="obj">contains the object to save, the object must contains an objectID attribute.</param>
        /// <returns>return an object containing an "updatedAt" attribute.</returns>
        public Task<JObject> SaveObject(JObject obj)
        {
            string objectID = (string)obj["objectID"];
            return _client.ExecuteRequest("PUT", string.Format("/1/indexes/{0}/{1}", _urlIndexName, Uri.EscapeDataString(objectID)), obj);
        }

        /// <summary>
        /// Override the content of several objects.
        /// </summary>
        /// <param name="objects">contains an array of objects to update (each object must contains a objectID attribute).</param>
        /// <returns>return an object containing an "objectIDs" attribute (array of string)</returns>
        public Task<JObject> SaveObjects(IEnumerable<JObject> objects)
        {
            List<object> requests = new List<object>();
            foreach (JObject obj in objects)
            {
                Dictionary<string, object> request = new Dictionary<string, object>();
                request["action"] = "updateObject";
                request["objectID"] = obj["objectID"];
                request["body"] = obj;
                requests.Add(request);
            }
            Dictionary<string, object> batch = new Dictionary<string, object>();
            batch["requests"] = requests;
            return _client.ExecuteRequest("POST", string.Format("/1/indexes/{0}/batch", _urlIndexName), batch);
        }

        /// <summary>
        /// Delete an object from the index.
        /// </summary>
        /// <param name="objectID">the unique identifier of object to delete.</param>
        /// <returns>return an object containing a "deletedAt" attribute.</returns>
        public Task<JObject> DeleteObject(string objectID)
        {
            if (string.IsNullOrWhiteSpace(objectID))
                throw new ArgumentOutOfRangeException("objectID", "objectID is required.");
            return _client.ExecuteRequest("DELETE", string.Format("/1/indexes/{0}/{1}", _urlIndexName, Uri.EscapeDataString(objectID)));
        }


        /// <summary>
        /// Search inside the index.
        /// </summary>
        public Task<JObject> Search(Query q)
        {
            string paramsString = q.GetQueryString();
            if (paramsString.Length > 0)
                return _client.ExecuteRequest("GET", string.Format("/1/indexes/{0}?{1}", _urlIndexName, paramsString));
            else
                return _client.ExecuteRequest("GET", string.Format("/1/indexes/{0}", _urlIndexName));

        }

        /// <summary>
        /// Wait the publication of a task on the server. 
        /// All server task are asynchronous and you can check with this method that the task is published.
        /// </summary>
        /// <param name="taskID">the id of the task returned by server.</param>
        async public Task WaitTask(string taskID)
        {
            while (true)
            {
                JObject obj = await _client.ExecuteRequest("GET", string.Format("/1/indexes/{0}/task/{1}", _urlIndexName, taskID));
                string status = (string)obj["status"];
                if (status.Equals("published"))
                    return;
                await TaskEx.Delay(1000);
            }
        }

        /// <summary>
        /// Get settings of this index.
        /// </summary>
        /// <returns>an object containing settings.</returns>
        public Task<JObject> GetSettings()
        {
            return _client.ExecuteRequest("GET", string.Format("/1/indexes/{0}/settings", _urlIndexName));
        }

        /// <summary>
        /// Set settings for this index.
        /// </summary>
        /// <param name="settings">the settings object that can contains :
        ///  - minWordSizeForApprox1 (integer) the minimum number of characters to accept one typo (default = 3)
        ///  - minWordSizeForApprox2: (integer) the minimum number of characters to accept two typos (default = 7)
        ///  - hitsPerPage: (integer) the number of hits per page (default = 10)
        ///  - attributesToRetrieve: (array of strings) default list of attributes to retrieve for objects
        ///  - attributesToHighlight: (array of strings) default list of attributes to highlight.
        ///  - attributesToSnippet: (array of strings) default list of attributes to snippet alongside the number of words to return (syntax is 'attributeName:nbWords').
        ///    By default no snippet is computed.
        ///  - attributesToIndex: (array of strings) the list of fields you want to index. 
        ///    By default all textual attributes of your objects are indexed, but you should update it to get optimal 
        ///    results. This parameter has two important uses:
        ///       - Limit the attributes to index. 
        ///         For example if you store a binary image in base64, you want to store it in the index but you 
        ///         don't want to use the base64 string for search.
        ///       - Control part of the ranking (see the ranking parameter for full explanation). 
        ///         Matches in attributes at the beginning of the list will be considered more important than matches 
        ///         in attributes further down the list.
        ///  - ranking: (array of strings) controls the way results are sorted. 
        ///     We have four available criteria: 
        ///       - typo (sort according to number of typos), 
        ///       - geo: (sort according to decreassing distance when performing a geo-location based search),
        ///       - proximity: sort according to the proximity of query words in hits, 
        ///       - attribute: sort according to the order of attributes defined by **attributesToIndex**,
        ///       - exact: sort according to the number of words that are matched identical to query word (and not as a prefix),
        ///       - custom which is user defined
        ///     (the standard order is ["typo", "geo", "proximity", "attribute", "exact", "custom"])
        ///  - queryType: select how the query words are interpreted:
        ///       - prefixAll: all query words are interpreted as prefixes (default behavior).
        ///       - prefixLast: only the last word is interpreted as a prefix. This option is recommended if you have a lot of content to speedup the processing.
        ///       - prefixNone: no query word is interpreted as a prefix. This option is not recommended.
        ///  - customRanking: (array of strings) lets you specify part of the ranking. 
        ///    The syntax of this condition is an array of strings containing attributes prefixed 
        ///    by asc (ascending order) or desc (descending order) operator.</param>
        public Task<JObject> SetSettings(JObject settings)
        {
            return _client.ExecuteRequest("PUT", string.Format("/1/indexes/{0}/settings", _urlIndexName), settings);
        }

        /// <summary>
        /// List all existing user keys associated to this index with their associated ACLs.
        /// </summary>
        public Task<JObject> ListUserKeys()
        {
            return _client.ExecuteRequest("GET", string.Format("/1/indexes/{0}/keys", _urlIndexName));
        }

        /// <summary>
        /// Get ACL of a user key associated to this index.
        /// </summary>
        public Task<JObject> GetUserKeyACL(string key)
        {
            return _client.ExecuteRequest("GET", string.Format("/1/indexes/{0}/keys/{1}", _urlIndexName, key));
        }

        /// <summary>
        /// Delete an existing user key associated to this index.
        /// </summary>
        public Task<JObject> DeleteUserKey(string key)
        {
            return _client.ExecuteRequest("DELETE", string.Format("/1/indexes/{0}/keys/{1}", _urlIndexName, key));
        }

        /// <summary>
        /// Create a new user key associated to this index.
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
            return _client.ExecuteRequest("POST", string.Format("/1/indexes/{0}/keys", _urlIndexName), content);
        }
    }
}
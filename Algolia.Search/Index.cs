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
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Threading;

namespace Algolia.Search
{
    /// <summary>
    /// Contains all the functions related to one index.
    /// You should use AlgoliaClient.initIndex(indexName) to instantiate this object.
    /// </summary>
    public class Index
    {
        protected AlgoliaClient  Client;
        private string         _indexName;
        private string         _urlIndexName;

        /// <summary>
        /// Index initialization (You should not call this constructor yourself).
        /// </summary>
        public Index(AlgoliaClient client, string indexName)
        {
            Client = client;
            _indexName = indexName;
            _urlIndexName = Uri.EscapeDataString(indexName);
        }

        /// <summary>
        /// Add an object to this index.
        /// </summary>
        /// <param name="content">The object you want to add to the index.</param>
        /// <param name="objectId">Optional objectID you want to attribute to this object (if the attribute already exists the old object will be overwritten).</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>An object that contains an "objectID" attribute.</returns>
        public Task<JObject> AddObjectAsync(object content, string objectId = null, CancellationToken token = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "POST", string.Format("/1/indexes/{0}", _urlIndexName), content, token);
            }
            else
            {
                return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "PUT", string.Format("/1/indexes/{0}/{1}", _urlIndexName, Uri.EscapeDataString(objectId)), content, token);
            }
        }

        /// <summary>
        /// Synchronously call <see cref="Index.AddObjectAsync"/>.
        /// </summary>
        /// <param name="content">The object you want to add to the index.</param>
        /// <param name="objectId">Optional objectID you want to attribute to this object (if the attribute already exists the old object will be overwritten).</param>
        /// <returns>An object that contains an "objectID" attribute.</returns>
        public JObject AddObject(object content, string objectId = null)
        {
            return AddObjectAsync(content, objectId).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Add several objects to this index.
        /// </summary>
        /// <param name="objects">An array of objects to add. If the objects contains objectIDs, they will be used.</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string).</returns>
        public Task<JObject> AddObjectsAsync(IEnumerable<object> objects, CancellationToken token = default(CancellationToken))
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
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "POST", string.Format("/1/indexes/{0}/batch", _urlIndexName), batch, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.AddObjectsAsync"/>.
        /// </summary>
        /// <param name="objects">An array of objects to add. If the objects contains objectIDs, they will be used.</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string).</returns>
        public JObject AddObjects(IEnumerable<object> objects)
        {
            return AddObjectsAsync(objects).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get an object from this index.
        /// </summary>
        /// <param name="objectId">The unique identifier of the object to retrieve.</param>
        /// <param name="attributesToRetrieve">Optional list of attributes to retrieve.</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        public Task<JObject> GetObjectAsync(string objectId, IEnumerable<string> attributesToRetrieve = null, CancellationToken token = default(CancellationToken))
        {
            if (attributesToRetrieve == null)
            {
                return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "GET", string.Format("/1/indexes/{0}/{1}", _urlIndexName, Uri.EscapeDataString(objectId)), null, token);
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
                return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "GET", string.Format("/1/indexes/{0}/{1}?attributesToRetrieve={2}", _urlIndexName, Uri.EscapeDataString(objectId), attributes), null, token);
            }
        }

        /// <summary>
        /// Synchronously call <see cref="Index.GetObjectAsync"/>.
        /// </summary>
        /// <param name="objectId">The unique identifier of the object to retrieve.</param>
        /// <param name="attributesToRetrieve">Optional list of attributes to retrieve.</param>
        /// <returns></returns>
        public JObject GetObject(string objectId, IEnumerable<string> attributesToRetrieve = null)
        {
            return GetObjectAsync(objectId, attributesToRetrieve).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get several objects from this index.
        /// </summary>
        /// <param name="objectIDs">An array of unique identifiers of the objects to retrieve.</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        public Task<JObject> GetObjectsAsync(IEnumerable<string> objectIDs, CancellationToken token = default(CancellationToken))
        {
            JArray requests = new JArray();
            foreach (string id in objectIDs)
            {
                JObject request = new JObject();
                request.Add("indexName", this._indexName);
                request.Add("objectID", id);
                requests.Add(request);
            }
            JObject body = new JObject();
            body.Add("requests", requests);
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "POST", "/1/indexes/*/objects", body, token);
        }

        /// <summary>
        /// Get several objects from this index.
        /// </summary>
        /// <param name="objectIDs">An array of unique identifiers of the objects to retrieve.</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        public Task<JObject> GetObjectsAsync(IEnumerable<string> objectIDs, IEnumerable<string> attributesToRetrieve, CancellationToken token = default(CancellationToken))
        {
            JArray requests = new JArray();
            var attributes = "";
            foreach (string attr in attributesToRetrieve)
            {
                if (attributes.Length > 0) {
                    attributes += ",";
                    attributes += attr;
                }
            }

            foreach (string id in objectIDs)
            {
                JObject request = new JObject();
                request.Add("indexName", this._indexName);
                request.Add("objectID", id);
                request.Add("attributesToRetrieve", attributes);
                requests.Add(request);
            }

            JObject body = new JObject();
            body.Add("requests", requests);
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "POST", "/1/indexes/*/objects", body, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.GetObjectsAsync"/>.
        /// </summary>
        /// <param name="objectIDs">An array of unique identifiers of the objects to retrieve.</param>
        /// <returns></returns>
        public JObject GetObjects(IEnumerable<string> objectIDs)
        {
            return GetObjectsAsync(objectIDs).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="Index.GetObjectsWithAttributesAsync"/>.
        /// </summary>
        /// <param name="objectIDs">An array of unique identifiers of the objects to retrieve.</param>
        /// <param name="attributesToRetrieve">list of attributes to retrieve.</param>
        /// <returns></returns>
        public JObject GetObjects(IEnumerable<string> objectIDs, IEnumerable<string> attributesToRetrieve)
        {
            return GetObjectsAsync(objectIDs, attributesToRetrieve).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Partially update an object (only update attributes passed in argument).
        /// </summary>
        /// <param name="partialObject">The object attributes to override (must contains an objectID attribute).</param>
        /// <param name="createIfNotExists"></param>
        /// <param name="token"></param>
        /// <returns>An object containing an "updatedAt" attribute.</returns>
        public Task<JObject> PartialUpdateObjectAsync(JObject partialObject, bool createIfNotExists = true, CancellationToken token = default(CancellationToken))
        {
            string queryParam = "";
            if (partialObject["objectID"] == null)
            {
                throw new AlgoliaException("objectID is missing");
            }
            if (!createIfNotExists)
            {
                queryParam = "?createIfNotExists=false";
            }
            string objectId = (string)partialObject["objectID"];
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "POST", string.Format("/1/indexes/{0}/{1}/partial{2}", _urlIndexName, Uri.EscapeDataString(objectId), queryParam), partialObject, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.PartialUpdateObjectAsync"/>.
        /// </summary>
        /// <param name="partialObject">The object attributes to override (must contains an objectID attribute).</param>
        /// <returns>An object containing an "updatedAt" attribute.</returns>
        public JObject PartialUpdateObject(JObject partialObject, bool createIfNotExists = true)
        {
            return PartialUpdateObjectAsync(partialObject, createIfNotExists).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Partially update the content of several objects.
        /// </summary>
        /// <param name="objects">An array of objects to update (each object must contain an objectID attribute).</param>
        /// <param name="createIfNotExists"></param>
        /// <param name="token">Cancellation token</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string).</returns>
        public Task<JObject> PartialUpdateObjectsAsync(IEnumerable<JObject> objects, bool createIfNotExists = true, CancellationToken token = default(CancellationToken))
        {
            string action = "partialUpdateObject";
            if (!createIfNotExists)
            {
                action = "partialUpdateObjectNoCreate";
            }
            List<object> requests = new List<object>();
            foreach (JObject obj in objects)
            {
                Dictionary<string, object> request = new Dictionary<string, object>();
                request["action"] = action;
                if (obj["objectID"] == null)
                {
                    throw new AlgoliaException("objectID is missing");
                }
                request["objectID"] = obj["objectID"];
                request["body"] = obj;
                requests.Add(request);
            }
            Dictionary<string, object> batch = new Dictionary<string, object>();
            batch["requests"] = requests;
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "POST", string.Format("/1/indexes/{0}/batch", _urlIndexName), batch, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.PartialUpdateObjectsAsync"/>.
        /// </summary>
        /// <param name="objects">An array of objects to update (each object must contain an objectID attribute).</param>
        /// <param name="createIfNotExists"></param>
        /// <returns>An object containing an "objectIDs" attribute (array of string).</returns>
        public JObject PartialUpdateObjects(IEnumerable<JObject> objects, bool createIfNotExists = true)
        {
            return PartialUpdateObjectsAsync(objects, createIfNotExists).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Override the contents of an object.
        /// </summary>
        /// <param name="obj">The object to save (must contain an objectID attribute).</param>
        /// <param name="token"></param>
        /// <returns>An object containing an "updatedAt" attribute.</returns>
        public Task<JObject> SaveObjectAsync(JObject obj, CancellationToken token = default(CancellationToken))
        {
            if (obj["objectID"] == null)
            {
                throw new AlgoliaException("objectID is missing");
            }
            string objectId = (string)obj["objectID"];
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "PUT", string.Format("/1/indexes/{0}/{1}", _urlIndexName, Uri.EscapeDataString(objectId)), obj, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.SaveObjectAsync"/>.
        /// </summary>
        /// <param name="obj">The object to save (must contain an objectID attribute).</param>
        /// <returns>An object containing an "updatedAt" attribute.</returns>
        public JObject SaveObject(JObject obj)
        {
            return SaveObjectAsync(obj).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Override the contents of several objects.
        /// </summary>
        /// <param name="objects">An array of objects to update (each object must contain an objectID attribute).</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string).</returns>
        public Task<JObject> SaveObjectsAsync(IEnumerable<JObject> objects, CancellationToken token = default(CancellationToken))
        {
            List<object> requests = new List<object>();
            foreach (JObject obj in objects)
            {
                Dictionary<string, object> request = new Dictionary<string, object>();
                request["action"] = "updateObject";
                if (obj["objectID"] == null)
                {
                    throw new AlgoliaException("objectID is missing");
                }
                request["objectID"] = obj["objectID"];
                request["body"] = obj;
                requests.Add(request);
            }
            Dictionary<string, object> batch = new Dictionary<string, object>();
            batch["requests"] = requests;
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "POST", string.Format("/1/indexes/{0}/batch", _urlIndexName), batch, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.SaveObjectsAsync"/>.
        /// </summary>
        /// <param name="objects">An array of objects to update (each object must contain an objectID attribute).</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string).</returns>
        public JObject SaveObjects(IEnumerable<JObject> objects)
        {
            return SaveObjectsAsync(objects).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete an object from the index.
        /// </summary>
        /// <param name="objectId">The unique identifier of the object to delete.</param>
        /// <returns>An object containing a "deletedAt" attribute.</returns>
        public Task<JObject> DeleteObjectAsync(string objectId, CancellationToken token = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(objectId))
                throw new ArgumentOutOfRangeException("objectId", "objectID is required.");
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "DELETE", string.Format("/1/indexes/{0}/{1}", _urlIndexName, Uri.EscapeDataString(objectId)), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.DeleteObjectAsync"/>.
        /// </summary>
        /// <param name="objectId">The unique identifier of the object to delete.</param>
        /// <returns>An object containing a "deletedAt" attribute.</returns>
        public JObject DeleteObject(string objectId)
        {
            return DeleteObjectAsync(objectId).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete several objects.
        /// </summary>
        /// <param name="objects">An array of objectIDs to delete.</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> DeleteObjectsAsync(IEnumerable<string> objects, CancellationToken token = default(CancellationToken))
        {
            List<object> requests = new List<object>();
            foreach (string id in objects)
            {
                Dictionary<string, object> request = new Dictionary<string, object>();
                Dictionary<string, object> obj = new Dictionary<string, object>();
                obj["objectID"] = id;
                request["action"] = "deleteObject";
                request["body"] = obj;
                requests.Add(request);
            }
            Dictionary<string, object> batch = new Dictionary<string, object>();
            batch["requests"] = requests;
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "POST", string.Format("/1/indexes/{0}/batch", _urlIndexName), batch, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.DeleteObjectsAsync"/>.
        /// </summary>
        /// <param name="objects">An array of objectIDs to delete.</param>
        public JObject DeleteObjects(IEnumerable<string> objects)
        {
            return DeleteObjectsAsync(objects).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete all objects matching a query.
        /// </summary>
        /// <param name="query">The query.</param>
        public async Task DeleteByQueryAsync(Query query)
        {
            query.SetAttributesToRetrieve(new string[]{"objectID"});
            query.SetAttributesToHighlight(new string[]{});
            query.SetAttributesToSnippet(new string[] {});
            query.SetNbHitsPerPage(1000);
            query.EnableDistinct(false); // force distinct=false to improve performances

            JObject result = await this.BrowseFromAsync(query, null).ConfigureAwait(Client.GetContinueOnCapturedContext());
            while (((JArray)result["hits"]).Count != 0)
            {
                int i = 0;
                JArray hits = (JArray)result["hits"];
                string[] requests = new string[hits.Count];
                foreach (JObject hit in hits)
                {
                    requests[i++] =  hit["objectID"].ToObject<string>();
                }
                var task = await this.DeleteObjectsAsync(requests).ConfigureAwait(Client.GetContinueOnCapturedContext());
                await this.WaitTaskAsync(task["taskID"].ToObject<string>()).ConfigureAwait(Client.GetContinueOnCapturedContext());
                result = await this.SearchAsync(query).ConfigureAwait(Client.GetContinueOnCapturedContext());
            }
        }

        /// <summary>
        /// Synchronously call <see cref="Index.DeleteByQueryAsync"/>.
        /// </summary>
        /// <param name="query">The query.</param>
        public void DeleteByQuery(Query query)
        {
            DeleteByQueryAsync(query).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Search inside the index.
        /// </summary>
        /// <param name="q">The query.</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> SearchAsync(Query q, CancellationToken token = default(CancellationToken))
        {
            string paramsString = q.GetQueryString();
            if (paramsString.Length > 0)
            {
                Dictionary<string, object> body = new Dictionary<string, object>();
                body["params"] = paramsString;
                return Client.ExecuteRequest(AlgoliaClient.CallType.Search, "POST", string.Format("/1/indexes/{0}/query", _urlIndexName), body, token);
            }
            else
            {
                return Client.ExecuteRequest(AlgoliaClient.CallType.Search, "GET", string.Format("/1/indexes/{0}", _urlIndexName), null, token);
            }
        }

        /// <summary>
        /// Synchronously call <see cref="Index.SearchAsync"/>.
        /// </summary>
        /// <param name="q">The query.</param>
        public JObject Search(Query q)
        {
            return SearchAsync(q).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Check to see if the asynchronous server task is complete.
        /// </summary>
        /// <param name="taskId">The id of the task returned by server.</param>
        /// <param name="timeToWait">Time to wait in milliseconds (defaults to 100)</param>
        /// <param name="token">Cancellation token</param>
        public async Task WaitTaskAsync(string taskId, int timeToWait = 100, CancellationToken token = default(CancellationToken))
        {
            while (true)
            {
                JObject obj = await Client.ExecuteRequest(AlgoliaClient.CallType.Read, "GET", string.Format("/1/indexes/{0}/task/{1}", _urlIndexName, taskId), null, token).ConfigureAwait(Client.GetContinueOnCapturedContext());
                string status = (string)obj["status"];
                if (status.Equals("published"))
                    return;
                await Task.Delay(timeToWait, token).ConfigureAwait(Client.GetContinueOnCapturedContext());
                timeToWait *= 2;
                if (timeToWait > 10000)
                    timeToWait = 10000;
            }
        }

        /// <summary>
        /// Synchronously call <see cref="Index.WaitTaskAsync"/>.
        /// </summary>
        /// <param name="taskId">The id of the task returned by server.</param>
        /// <param name="timeToWait">Time to wait in milliseconds (defaults to 100)</param>
        public void WaitTask(string taskId, int timeToWait = 100)
        {
            WaitTaskAsync(taskId, timeToWait).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get the index settings.
        /// </summary>
        /// <returns>An object containing the settings.</returns>
        public Task<JObject> GetSettingsAsync(CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "GET", string.Format("/1/indexes/{0}/settings?getVersion=2", _urlIndexName), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.GetSettingsAsync"/>.
        /// </summary>
        /// <returns>An object containing the settings.</returns>
        public JObject GetSettings()
        {
            return GetSettingsAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        ///  Browse all index contents.
        /// </summary>
        /// <param name="page">The page number to browse.</param>
        /// <param name="hitsPerPage">The number of hits per page.</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> BrowseAsync(int page = 0, int hitsPerPage = 1000, CancellationToken token = default(CancellationToken))
        {
            string param = "";
            if (page != 0)
                param += string.Format("?page={0}", page);

            if (hitsPerPage != 1000)
            {
                if (param.Length == 0)
                    param += string.Format("?hitsPerPage={0}", hitsPerPage);
                else
                    param += string.Format("&hitsPerPage={0}", hitsPerPage);
            }
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "GET", string.Format("/1/indexes/{0}/browse{1}", _urlIndexName, param), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.BrowseAsync"/>.
        /// </summary>
        /// <param name="page">The page number to browse.</param>
        /// <param name="hitsPerPage">The number of hits per page.</param>
        public JObject Browse(int page = 0, int hitsPerPage = 1000)
        {
            return BrowseAsync(page, hitsPerPage).GetAwaiter().GetResult();
        }

        /// <summary>
        ///  Browse all index contents.
        /// </summary>
        /// <param name="q">The query parameters for the browse.</param>
        /// <param name="cursor">The cursor to start the browse can be empty.</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> BrowseFromAsync(Query q, string cursor, CancellationToken token = default(CancellationToken))
        {
            string cursorParam = "";
            if (!string.IsNullOrEmpty(cursor))
            {
                cursorParam = string.Format("&cursor={0}",  Uri.EscapeDataString(cursor));
            }
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "GET", string.Format("/1/indexes/{0}/browse?{1}{2}", _urlIndexName, q.GetQueryString(), cursorParam), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.BrowseFromAsync"/>.
        /// </summary>
        /// <param name="q">The query parameters for the browse.</param>
        /// <param name="cursor">The cursor to start the browse can be empty.</param>
        public JObject BrowseFrom(Query q, string cursor)
        {
            return BrowseFromAsync(q, cursor).GetAwaiter().GetResult();
        }

        public class IndexIterator : IEnumerable<JObject> {

            Index _index;
            Query _query;
            string _cursor;

            public IndexIterator(Index ind, Query q, string cursor)
            {
                this._index = ind;
                this._query = q;
                this._cursor = cursor;
            }

            public IEnumerator<JObject> GetEnumerator()
            {
                return new IndexEnumerator(_index, _query, _cursor);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new IndexEnumerator(_index, _query, _cursor);
            }
        }

        public class IndexEnumerator : IEnumerator<JObject>
        {
            Index _index;
            JObject _answer;
            int _pos;
            string _cursor;
            Query _query;
            JObject _hit;

            public IndexEnumerator(Index ind, Query q, string cursor)
            {
                this._index = ind;
                this._query = q;
                this._cursor = cursor;
                Reset();
            }

            private void LoadNextPage() {
                _pos = 0;
                _answer = _index.BrowseFromAsync(_query, GetCursor()).GetAwaiter().GetResult();
            }

            public string GetCursor()
            {
                return _answer["cursor"].ToObject<string>();
            }

            public JObject Current => _hit;

            object IEnumerator.Current => _hit;

            public bool MoveNext()
            {
                while (true) {
                    if (_pos < ((JArray)_answer["hits"]).Count())
                    {
                        _hit = ((JArray)_answer["hits"])[_pos++].ToObject<JObject>();
                        return true;
                    }
                    if (_answer["cursor"] != null && _answer["cursor"].ToObject<string>().Length > 0)
                    {
                        LoadNextPage();
                        continue;
                    }
                    return false;
                }
            }

            public void Reset()
            {
                _pos = 0;
                _answer = new JObject {{"cursor", _cursor}};
                LoadNextPage();
            }

            public void Dispose()
            {
                // Nothing to do
            }
        }



        /// <summary>
        ///  Browse all index contents.
        /// </summary>
        /// <param name="q">The query parameters for the browse.</param>
        public IndexIterator BrowseAll(Query q)
        {
            return new IndexIterator(this, q, "");
        }

        /// <summary>
        /// Delete the index contents without removing settings and index specific API keys.
        /// </summary>
        public Task<JObject> ClearIndexAsync(CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "POST", string.Format("/1/indexes/{0}/clear", _urlIndexName), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.ClearIndexAsync"/>.
        /// </summary>
        public JObject ClearIndex()
        {
            return ClearIndexAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Set the settings for this index.
        /// </summary>
        /// <param name="settings">The settings object can contain:
        ///  - minWordSizefor1Typo: (integer) the minimum number of characters to accept one typo (default = 3).
        ///  - minWordSizefor2Typos: (integer) the minimum number of characters to accept two typos (default = 7).
        ///  - hitsPerPage: (integer) the number of hits per page (default = 10).
        ///  - attributesToRetrieve: (array of strings) default list of attributes to retrieve in objects.
        ///    If set to null, all attributes are retrieved.
        ///  - attributesToHighlight: (array of strings) default list of attributes to highlight.
        ///    If set to null, all indexed attributes are highlighted.
        ///  - attributesToSnippet**: (array of strings) default list of attributes to snippet alongside the number of words to return (syntax is attributeName:nbWords).
        ///    By default no snippet is computed. If set to null, no snippet is computed.
        ///  - searchableAttributes(formerly attributesToIndex): (array of strings) the list of fields you want to index.
        ///    If set to null, all textual and numerical attributes of your objects are indexed, but you should update it to get optimal results.
        ///    This parameter has two important uses:
        ///      - Limit the attributes to index: For example if you store a binary image in base64, you want to store it and be able to
        ///        retrieve it but you don't want to search in the base64 string.
        ///      - Control part of the ranking*: (see the ranking parameter for full explanation) Matches in attributes at the beginning of
        ///        the list will be considered more important than matches in attributes further down the list.
        ///        In one attribute, matching text at the beginning of the attribute will be considered more important than text after, you can disable
        ///        this behavior if you add your attribute inside `unordered(AttributeName)`, for example searchableAttributes: ["title", "unordered(text)"].
        ///  - attributesForFaceting: (array of strings) The list of fields you want to use for faceting.
        ///    All strings in the attribute selected for faceting are extracted and added as a facet. If set to null, no attribute is used for faceting.
        ///  - ranking: (array of strings) controls the way results are sorted.
        ///    We have six available criteria:
        ///     - typo: sort according to number of typos,
        ///     - geo: sort according to decreassing distance when performing a geo-location based search,
        ///     - proximity: sort according to the proximity of query words in hits,
        ///     - attribute: sort according to the order of attributes defined by searchableAttributes,
        ///     - exact: sort according to the number of words that are matched identical to query word (and not as a prefix),
        ///     - custom: sort according to a user defined formula set in **customRanking** attribute.
        ///    The standard order is ["typo", "geo", "proximity", "attribute", "exact", "custom"]
        ///  - customRanking: (array of strings) lets you specify part of the ranking.
        ///    The syntax of this condition is an array of strings containing attributes prefixed by asc (ascending order) or desc (descending order) operator.
        ///    For example `"customRanking" => ["desc(population)", "asc(name)"]`
        ///  - queryType: Select how the query words are interpreted, it can be one of the following value:
        ///    - prefixAll: all query words are interpreted as prefixes,
        ///    - prefixLast: only the last word is interpreted as a prefix (default behavior),
        ///    - prefixNone: no query word is interpreted as a prefix. This option is not recommended.
        ///  - highlightPreTag: (string) Specify the string that is inserted before the highlighted parts in the query result (default to "<em>").
        ///  - highlightPostTag: (string) Specify the string that is inserted after the highlighted parts in the query result (default to "</em>").
        ///  - optionalWords: (array of strings) Specify a list of words that should be considered as optional when found in the query.
        /// </param>
        /// <param name="forwardToReplicas">Forward the operation to the replica indices</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> SetSettingsAsync(JObject settings, bool forwardToReplicas = false, CancellationToken token = default(CancellationToken))
        {
            string changeSettingsPath = forwardToReplicas
                ? string.Format("/1/indexes/{0}/settings?forwardToReplicas={1}", _urlIndexName, forwardToReplicas.ToString().ToLower())
                : string.Format("/1/indexes/{0}/settings", _urlIndexName);
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "PUT", changeSettingsPath, settings, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.SetSettingsAsync"/>.
        /// </summary>
        public JObject SetSettings(JObject settings, bool forwardToReplicas = false)
        {
            return SetSettingsAsync(settings, forwardToReplicas).GetAwaiter().GetResult();
        }

        /// <summary>
        /// List all user keys associated with this index along with their associated ACLs.
        /// </summary>
        [Obsolete("ListUserKeysAsync is deprecated, please use ListApiKeysAsync instead.")]
        public Task<JObject> ListUserKeysAsync(CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "GET", string.Format("/1/indexes/{0}/keys", _urlIndexName), null, token);
        }

        /// <summary>
        /// List all api keys associated with this index along with their associated ACLs.
        /// </summary>
        public Task<JObject> ListApiKeysAsync(CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "GET", string.Format("/1/indexes/{0}/keys", _urlIndexName), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.ListApiKeysAsync"/>.
        /// </summary>
        [Obsolete("ListUserKeys is deprecated, please use ListApiKeys instead.")]
        public JObject ListUserKeys()
        {
            return ListApiKeysAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="Index.ListApiKeysAsync"/>.
        /// </summary>
        public JObject ListApiKeys()
        {
            return ListApiKeysAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get ACL of a user key associated with this index.
        /// </summary>
        [Obsolete("GetUserKeyACLAsync is deprecated, please use GetUserApiACLAsync instead.")]
        public Task<JObject> GetUserKeyAclAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "GET", string.Format("/1/indexes/{0}/keys/{1}", _urlIndexName, key), null, token);
        }

        /// <summary>
        /// Get ACL of an api key associated with this index.
        /// </summary>
        public Task<JObject> GetApiKeyAclAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "GET", string.Format("/1/indexes/{0}/keys/{1}", _urlIndexName, key), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="GetUserKeyAclAsync"/>.
        /// </summary>
        [Obsolete("GetUserKeyACL is deprecated, please use GetApiKeyACL instead.")]
        public JObject GetUserKeyAcl(string key)
        {
            return GetApiKeyAclAsync(key).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="GetApiKeyAclAsync"/>.
        /// </summary>
        public JObject GetApiKeyAcl(string key)
        {
            return GetApiKeyAclAsync(key).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete an existing user key associated with this index.
        /// </summary>
        [Obsolete("DeleteUserKeyAsync is deprecated, please use DeleteApiKeyAsync instead.")]
        public Task<JObject> DeleteUserKeyAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "DELETE", string.Format("/1/indexes/{0}/keys/{1}", _urlIndexName, key), null, token);
        }

        /// <summary>
        /// Delete an existing api key associated with this index.
        /// </summary>
        public Task<JObject> DeleteApiKeyAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "DELETE", string.Format("/1/indexes/{0}/keys/{1}", _urlIndexName, key), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.DeleteApiKeyAsync"/>.
        /// </summary>
        [Obsolete("DeleteUserKey is deprecated, please use DeleteApiKey instead.")]
        public JObject DeleteUserKey(string key)
        {
            return DeleteApiKeyAsync(key).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="Index.DeleteApiKeyAsync"/>.
        /// </summary>
        public JObject DeleteApiKey(string key)
        {
            return DeleteApiKeyAsync(key).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create a new user key associated with this index.
        /// </summary>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
        /// can contains the following values:
        ///   - acl: array of string
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
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "POST", string.Format("/1/indexes/{0}/keys", _urlIndexName), parameters, token);
        }

        /// <summary>
        /// Create a new api key associated with this index.
        /// </summary>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
        /// can contains the following values:
        ///   - acl: array of string
        ///   - validity: int
        ///   - referers: array of string
        ///   - description: string
        ///   - maxHitsPerQuery: integer
        ///   - queryParameters: string
        ///   - maxQueriesPerIPPerHour: integer
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> AddApiKeyAsync(Dictionary<string, object> parameters, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "POST", string.Format("/1/indexes/{0}/keys", _urlIndexName), parameters, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.AddApiKeyAsync"/>.
        /// </summary>
        [Obsolete("AddUserKey is deprecated, please use AddApiKey instead.")]
        public JObject AddUserKey(Dictionary<string, object> parameters)
        {
            return AddApiKeyAsync(parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="Index.AddApiKeyAsync"/>.
        /// </summary>
        public JObject AddApiKey(Dictionary<string, object> parameters)
        {
            return AddApiKeyAsync(parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create a new user key associated with this index.
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
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        [Obsolete("AddUserKeyAsync is deprecated, please use AddApiKeyAsync instead.")]
        public Task<JObject> AddUserKeyAsync(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0)
        {
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIpPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            return AddApiKeyAsync(content);
        }

        /// <summary>
        /// Create a new api key associated with this index.
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
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> AddApiKeyAsync(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0)
        {
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIpPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            return AddApiKeyAsync(content);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.AddApiKeyAsync"/>.
        /// </summary>
        [Obsolete("AddUserKey is deprecated, please use AddApiKey instead.")]
        public JObject AddUserKey(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0)
        {
            return AddApiKeyAsync(acls, validity, maxQueriesPerIpPerHour, maxHitsPerQuery).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="Index.AddApiKeyAsync"/>.
        /// </summary>
        public JObject AddApiKey(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0)
        {
            return AddApiKeyAsync(acls, validity, maxQueriesPerIpPerHour, maxHitsPerQuery).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Update a user key associated to this index.
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
        /// can contains the following values:
        ///   - acl: array of string
        ///   - validity: int
        ///   - referers: array of string
        ///   - description: string
        ///   - maxHitsPerQuery: integer
        ///   - queryParameters: string
        ///   - maxQueriesPerIPPerHour: integer
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        [Obsolete("UpdateUserKeyAsync is deprecated, please use UpdateApiKeyAsync instead.")]
        public Task<JObject> UpdateUserKeyAsync(string key, Dictionary<string, object> parameters, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "PUT", string.Format("/1/indexes/{0}/keys/{1}", _urlIndexName, key), parameters, token);
        }


        /// <summary>
        /// Update an api key associated to this index.
        /// </summary>
        /// <param name="key">The user key</param>
        /// <param name="parameters">the list of parameters for this key. Defined by a Dictionary that
        /// can contains the following values:
        ///   - acl: array of string
        ///   - validity: int
        ///   - referers: array of string
        ///   - description: string
        ///   - maxHitsPerQuery: integer
        ///   - queryParameters: string
        ///   - maxQueriesPerIPPerHour: integer
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> UpdateApiKeyAsync(string key, Dictionary<string, object> parameters, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "PUT", string.Format("/1/indexes/{0}/keys/{1}", _urlIndexName, key), parameters, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.UpdateApiKeyAsync"/>.
        /// </summary>
        [Obsolete("UpdateUserKey is deprecated, please use UpdateApiKey instead.")]
        public JObject UpdateUserKey(string key, Dictionary<string, object> parameters)
        {
            return UpdateApiKeyAsync(key, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="Index.UpdateApiKeyAsync"/>.
        /// </summary>
        public JObject UpdateApiKey(string key, Dictionary<string, object> parameters)
        {
            return UpdateApiKeyAsync(key, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Update a user key associated to this index.
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
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        [Obsolete("UpdateUserKeyAsync is deprecated, please use UpdateApiKeyAsync instead.")]
        public Task<JObject> UpdateUserKeyAsync(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0)
        {
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIpPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            return UpdateApiKeyAsync(key, content);
        }

        /// <summary>
        /// Update an api key associated to this index.
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
        /// <returns>Returns an object with a "key" string attribute containing the new key.</returns>
        public Task<JObject> UpdateApiKeyAsync(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0)
        {
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["acl"] = acls;
            content["validity"] = validity;
            content["maxQueriesPerIPPerHour"] = maxQueriesPerIpPerHour;
            content["maxHitsPerQuery"] = maxHitsPerQuery;
            return UpdateApiKeyAsync(key, content);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.UpdateApiKeyAsync"/>.
        /// </summary>
        [Obsolete("UpdateUserKey is deprecated, please use UpdateApiKey instead.")]
        public JObject UpdateUserKey(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0)
        {
            return UpdateApiKeyAsync(key, acls, validity, maxQueriesPerIpPerHour, maxHitsPerQuery).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="Index.UpdateApiKeyAsync"/>.
        /// </summary>
        public JObject UpdateApiKey(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIpPerHour = 0, int maxHitsPerQuery = 0)
        {
            return UpdateApiKeyAsync(key, acls, validity, maxQueriesPerIpPerHour, maxHitsPerQuery).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Perform a search with disjunctive facets generating as many queries as number of disjunctive facets
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="disjunctiveFacets">The array of disjunctive facets.</param>
        /// <param name="refinements">The current refinements. Example: { "my_facet1" => ["my_value1", "my_value2"], "my_disjunctive_facet1" => ["my_value1", "my_value2"] }.</param>
        public async Task<JObject> SearchDisjunctiveFacetingAsync(Query query, IEnumerable<string> disjunctiveFacets, Dictionary<string, IEnumerable<string>> refinements = null)
        {
            if (refinements == null)
                refinements = new Dictionary<string, IEnumerable<string>>();

            Dictionary<string, IEnumerable<string>> disjunctiveRefinements = new Dictionary<string,IEnumerable<string>>();
            var facets = disjunctiveFacets as string[] ?? disjunctiveFacets.ToArray();
            foreach (string key in refinements.Keys)
            {
                if (facets.Contains(key))
                {
                    disjunctiveRefinements.Add(key, refinements[key]);
                }
            }

            // build queries
            List<IndexQuery> queries = new List<IndexQuery>();
            // hits + regular facets query
            List<string> filters = new List<string>();
            foreach (string key in refinements.Keys)
            {
                string or = "(";
                bool first = true;
                foreach (string value in refinements[key])
                {
                    if (disjunctiveRefinements.ContainsKey(key))
                    {
                        // disjunctive refinements are ORed
                        if (!first)
                            or += ',';
                        first = false;
                        or += string.Format("{0}:{1}", key, value);
                    }
                    else
                    {
                        filters.Add(string.Format("{0}:{1}", key, value));
                    }
                }
                // Add or
                if (disjunctiveRefinements.ContainsKey(key))
                {
                    filters.Add(or + ')');
                }
            }


            queries.Add(new IndexQuery(_indexName, query.Clone().SetFacetFilters(filters)));
            // one query per disjunctive facet (use all refinements but the current one + histPerPage=1 + single facet)
            foreach (string disjunctiveFacet in facets)
            {
                filters = new List<string>();
                foreach (string key in refinements.Keys)
                {
                    if (disjunctiveFacet.Equals(key))
                        continue;

                    string or = "(";
                    bool first = true;
                    foreach (string value in refinements[key])
                    {
                        if (disjunctiveRefinements.ContainsKey(key))
                        {
                            // disjunctive refinements are ORed
                            if (!first)
                                or += ',';

                            first = false;
                            or += string.Format("{0}:{1}", key, value);
                        }
                        else
                        {
                            filters.Add(string.Format("{0}:{1}", key, value));
                        }
                    }
                    // Add or
                    if (disjunctiveRefinements.ContainsKey(key))
                    {
                        filters.Add(or + ')');
                    }
                }
                queries.Add(new IndexQuery(_indexName, query.Clone().SetPage(0).SetNbHitsPerPage(0).EnableAnalytics(false).SetAttributesToRetrieve(new List<string>()).SetAttributesToHighlight(new List<string>()).SetAttributesToSnippet(new List<string>()).SetFacets(new string[]{disjunctiveFacet}).SetFacetFilters(filters)));
            }

            JObject answers = await Client.MultipleQueriesAsync(queries).ConfigureAwait(Client.GetContinueOnCapturedContext());

            // aggregate answers
            // first answer stores the hits + regular facets
            JObject aggregatedAnswer = answers["results"].ToObject<JArray>()[0].ToObject<JObject>();
            JObject disjunctiveFacetsJson = new JObject();
            bool first2 = true;
            foreach (JToken answer in answers["results"].ToObject<JArray>())
            {
                if (first2)
                {
                    first2 = false;
                    continue;
                }
                JObject a = answer.ToObject<JObject>();
                foreach (KeyValuePair<string, JToken> elt in a["facets"].ToObject<JObject>())
                {
                    // Add the facet to the disjunctive facet hash
                    disjunctiveFacetsJson.Add(elt.Key, elt.Value);
                    // concatenate missing refinements
                    if (!disjunctiveRefinements.ContainsKey(elt.Key))
                        continue;
                    foreach (string refine in disjunctiveRefinements[elt.Key])
                    {
                        if (disjunctiveFacetsJson[elt.Key].ToObject<JObject>()[refine] == null)
                        {
                            disjunctiveFacetsJson[elt.Key].ToObject<JObject>().Add(refine, 0);
                        }
                    }

                }
            }
            aggregatedAnswer.Add("disjunctiveFacets", disjunctiveFacetsJson.ToObject<JToken>());
            return aggregatedAnswer;
        }

        /// <summary>
        /// Synchronously call <see cref="Index.UpdateUserKeyAsync"/>.
        /// </summary>
        public JObject SearchDisjunctiveFaceting(Query query, IEnumerable<string> disjunctiveFacets, Dictionary<string, IEnumerable<string>> refinements = null)
        {
            return SearchDisjunctiveFacetingAsync(query, disjunctiveFacets, refinements).GetAwaiter().GetResult();
        }

        /// <summary>
        /// The type of synonyms
        /// </summary>
        public enum SynonymType
        {
            Synonym,
            SynonymOneway,
            Placeholder,
            Altcorrection1,
            Altcorrection2
        }

        private string SynonymsTypeToString(SynonymType type) {
            switch (type)
            {
                case SynonymType.Synonym:
                    return "synonym";
                case SynonymType.SynonymOneway:
                    return "oneWaySynonym";
                case SynonymType.Placeholder:
                    return "placeholder";
                case SynonymType.Altcorrection1:
                    return "altCorrection1";
                case SynonymType.Altcorrection2:
                    return "altCorrection2";
            }
            return null;
        }

        /// <summary>
        /// Search/Browse all synonyms
        /// </summary>
        /// <param name="query">The query string</param>
        /// <param name="types">Specify the types</param>
        /// <param name="page">The page to fetch</param>
        /// <param name="hitsPerPage">number of synonyms to fetch</param>
        public Task<JObject> SearchSynonymsAsync(string query, IEnumerable<SynonymType> types = null, int? page = null, int? hitsPerPage = null, CancellationToken token = default(CancellationToken))
        {
            string[] typesStr = null;
            if (types != null)
            {
                var synonymTypes = types as SynonymType[] ?? types.ToArray();
                typesStr = new string[synonymTypes.Count()];
                for (int i = 0; i < synonymTypes.Count(); ++i)
                {
                    typesStr[i] = SynonymsTypeToString(synonymTypes.ElementAt(i));
                }
            }
            return SearchSynonymsAsync(query, typesStr, page, hitsPerPage, token);
        }
        /// <summary>
        /// Synchronously call <see cref="Index.SearchSynonymsAsync"/>.
        /// </summary>
        /// <param name="query">The query string</param>
        /// <param name="types">Specify the types</param>
        /// <param name="page">The page to fetch</param>
        /// <param name="hitsPerPage">number of synonyms to fetch</param>
        public JObject SearchSynonyms(string query, IEnumerable<SynonymType> types = null, int? page = null, int? hitsPerPage = null)
        {

            return SearchSynonymsAsync(query, types, page, hitsPerPage).GetAwaiter().GetResult();
        }
        /// <summary>
        /// Search/Browse all synonyms
        /// </summary>
        /// <param name="query">The query string</param>
        /// <param name="types">Specify the types</param>
        /// <param name="page">The page to fetch</param>
        /// <param name="hitsPerPage">number of synonyms to fetch</param>
        public Task<JObject> SearchSynonymsAsync(string query, IEnumerable<string> types = null, int? page = null, int? hitsPerPage = null, CancellationToken token = default(CancellationToken))
        {
            Dictionary<string, object> body = new Dictionary<string, object>();
            body["query"] = query;
            if (types != null)
            {
                string typeStr = string.Join(",", types);
                if (typeStr != null)
                    body["type"] = typeStr;
            }
            if (page.HasValue)
                body["page"] = page.Value;
            if (hitsPerPage.HasValue)
                body["hitsPerPage"] = hitsPerPage.Value;
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "POST", string.Format("/1/indexes/{0}/synonyms/search", _urlIndexName), body, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.SearchSynonymsAsync"/>.
        /// </summary>
        /// <param name="query">The query string</param>
        /// <param name="types">Specify the types</param>
        /// <param name="page">The page to fetch</param>
        /// <param name="hitsPerPage">number of synonyms to fetch</param>
        public JObject SearchSynonyms(string query, IEnumerable<string> types = null, int? page = null, int? hitsPerPage = null)
        {

            return SearchSynonymsAsync(query, types, page, hitsPerPage).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get one synonym
        /// </summary>
        /// <param name="objectId">The objectID of the synonym</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> GetSynonymAsync(string objectId, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "GET", string.Format("/1/indexes/{0}/synonyms/{1}", _urlIndexName, Uri.EscapeDataString(objectId)), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.GetSynonymAsync"/>.
        /// </summary>
        /// <param name="objectId">The objectID of the synonym</param>
        public JObject GetSynonym(string objectId)
        {
            return GetSynonymAsync(objectId).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete one synonym
        /// </summary>
        /// <param name="objectId">The objectID of the synonym</param>
        /// <param name="forwardToReplicas">Forward the operation to the replica indices</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> DeleteSynonymAsync(string objectId, bool forwardToReplicas = false, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "DELETE", string.Format("/1/indexes/{0}/synonyms/{1}?forwardToReplicas={2}", _urlIndexName, Uri.EscapeDataString(objectId), forwardToReplicas ? "true" : "false"), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.DeleteSynonymAsync"/>.
        /// </summary>
        /// <param name="objectId">The objectID of the synonym</param>
        /// <param name="forwardToReplicas">Forward the operation to the replica indices</param>
        public JObject DeleteSynonym(string objectId, bool forwardToReplicas = false)
        {
            return DeleteSynonymAsync(objectId, forwardToReplicas).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete all synonym set
        /// </summary>
        /// <param name="forwardToReplicas">Forward the operation to the replica indices</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> ClearSynonymsAsync(bool forwardToReplicas = false, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "POST", string.Format("/1/indexes/{0}/synonyms/clear?forwardToReplicas={1}", _urlIndexName, forwardToReplicas ? "true" : "false"), null, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.BrowseFromAsync"/>.
        /// </summary>
        /// <param name="forwardToReplicas">Forward the operation to the replica indices</param>
        public JObject ClearSynonyms(bool forwardToReplicas = false)
        {
            return ClearSynonymsAsync(forwardToReplicas).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Add or Replace a list of synonyms
        /// </summary>
        /// <param name="forwardToReplicas">Forward the operation to the replica indices</param>
        /// <param name="replaceExistingSynonyms">Replace the existing synonyms with this batch</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> BatchSynonymsAsync(IEnumerable<object> objects, bool forwardToReplicas = false, bool replaceExistingSynonyms = false, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "POST", string.Format("/1/indexes/{0}/synonyms/batch?replaceExistingSynonyms={1}&forwardToReplicas={2}", _urlIndexName, replaceExistingSynonyms ? "true" : "false", forwardToReplicas ? "true" : "false"), objects, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.BatchSynonymsAsync"/>.
        /// </summary>
        /// <param name="forwardToReplicas">Forward the operation to the replica indices</param>
        /// <param name="replaceExistingSynonyms">Replace the existing synonyms with this batch</param>
        public JObject BatchSynonyms(IEnumerable<object> objects, bool forwardToReplicas = false, bool replaceExistingSynonyms = false)
        {
            return BatchSynonymsAsync(objects, forwardToReplicas, replaceExistingSynonyms).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Update one synonym
        /// </summary>
        /// <param name="objectId">The objectID of the synonym</param>
        /// <param name="content">The new content of this synonym</param>
        /// <param name="forwardToReplicas">Forward the operation to the replica indices</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> SaveSynonymAsync(string objectId, object content, bool forwardToReplicas = false, CancellationToken token = default(CancellationToken))
        {
            return Client.ExecuteRequest(AlgoliaClient.CallType.Write, "PUT", string.Format("/1/indexes/{0}/synonyms/{1}?forwardToReplicas={2}", _urlIndexName,  Uri.EscapeDataString(objectId), forwardToReplicas ? "true" : "false"), content, token);
        }

        /// <summary>
        /// Synchronously call <see cref="Index.SaveSynonymAsync"/>.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="forwardToReplicas">Forward the operation to the replica indices</param>
        /// <param name="replaceExistingSynonyms"></param>
        public JObject SaveSynonym(string objectId, object content, bool forwardToReplicas = false, bool replaceExistingSynonyms = false)
        {
            return SaveSynonymAsync(objectId, content, forwardToReplicas).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously call <see cref="Index.SearchForFacetValuestAsync"/>.
        /// </summary>
        /// <param name="facetName">Name of the facet</param>
        /// <param name="facetQuery">Current query</param>
        /// <param name="queryParams">Optional query parameter</param>
        public JObject SearchForFacetValues(string facetName, string facetQuery, Query queryParams = null)
        {
            return SearchForFacetValuesAsync(facetName, facetQuery, queryParams).GetAwaiter().GetResult();
        }

        /// <summary>
        ///= kept for backward compatibility - Synchronously call <see cref="Index.SearchFacetAsync"/>.
        /// </summary>
        /// <param name="facetName">Name of the facet</param>
        /// <param name="facetQuery">Current query</param>
        /// <param name="queryParams">Optional query parameter</param>
        public JObject SearchFacet(string facetName, string facetQuery, Query queryParams = null)
        {
            return SearchFacetAsync(facetName, facetQuery, queryParams).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Search for facets async
        /// </summary>
        /// <param name="facetName">Name of the facet</param>
        /// <param name="facetQuery">Current Query</param>
        /// <param name="queryParams">Optional query parameter</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> SearchFacetAsync(string facetName, string facetQuery, Query queryParams = null, CancellationToken token = default(CancellationToken))
        {
            if(queryParams == null)
            {
                queryParams = new Query();
            }
            queryParams.AddCustomParameter("facetQuery", facetQuery);
            string paramsString = queryParams.GetQueryString();
            Dictionary<string, object> body = new Dictionary<string, object> {["params"] = paramsString};
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "POST", string.Format("/1/indexes/{0}/facets/{1}/query", _urlIndexName, facetName), body, token);
        }

        /// <summary>
        /// Search for facets async = kept for backward compatibility
        /// </summary>
        /// <param name="facetName">Name of the facet</param>
        /// <param name="facetQuery">Current Query</param>
        /// <param name="queryParams">Optional query parameter</param>
        /// <param name="token">Cancellation token</param>
        public Task<JObject> SearchForFacetValuesAsync(string facetName, string facetQuery, Query queryParams = null, CancellationToken token = default(CancellationToken))
        {
            if (queryParams == null)
            {
                queryParams = new Query();
            }
            queryParams.AddCustomParameter("facetQuery", facetQuery);
            string paramsString = queryParams.GetQueryString();
            Dictionary<string, object> body = new Dictionary<string, object> {["params"] = paramsString};
            return Client.ExecuteRequest(AlgoliaClient.CallType.Read, "POST", string.Format("/1/indexes/{0}/facets/{1}/query", _urlIndexName, facetName), body, token);
        }
    }
}
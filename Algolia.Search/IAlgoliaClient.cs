using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Models;
using Newtonsoft.Json.Linq;

namespace Algolia.Search
{
    public interface IAlgoliaClient
    {
        HostStatus setHostStatus(bool up);
        IList<string> filterOnActiveHosts(IList<string> hosts, bool isQuery);

        /// <summary>
        /// Configure the await in the library. Useful to avoid a deadlock with ASP.NET projects.
        /// </summary>
        /// <param name="continueOnCapturedContext">Set to false to turn it off and avoid deadlocks</param>
        void ConfigureAwait(bool continueOnCapturedContext);

        /// <summary>
        /// Get the context
        /// </summary>
        /// <returns></returns>
        bool getContinueOnCapturedContext();

        /// <summary>
        /// Set the read timeout for the search and for the build operation
        /// This method should be called before any api call.
        /// </summary>
        void setTimeout(double searchTimeout, double writeTimeout);

        /// <summary>
        /// Add security tag header (see http://www.algolia.com/doc/guides/csharp#SecurityUser for more details)
        /// </summary>
        /// <param name="tag"></param>
        void SetSecurityTags(string tag);

        /// <summary>
        /// Add user-token header (see http://www.algolia.com/doc/guides/csharp#SecurityUser for more details)
        /// </summary>
        /// <param name="userToken"></param>
        void SetUserToken(string userToken);

        /// <summary>
        /// Set extra HTTP request headers
        /// </summary>
        /// <param name="key">The header key</param>
        /// <param name="value">The header value</param>
        void SetExtraHeader(string key, string value);

        /// <summary>
        /// This method allows querying multiple indexes with one API call
        /// </summary>
        /// <param name="queries">List of queries per index</param>
        /// <param name="requestOptions"></param>
        /// <param name="strategy">Strategy applied on the sequence of queries</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<JObject> MultipleQueriesAsync(List<IndexQuery> queries, RequestOptions requestOptions, string strategy = "none", CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.MultipleQueriesAsync"/>
        /// </summary>
        /// <param name="queries">List of queries per index</param>
        /// <param name="requestOptions"></param>
        /// <param name="strategy"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        JObject MultipleQueries(List<IndexQuery> queries, RequestOptions requestOptions, string strategy = "none", CancellationToken token = default(CancellationToken));

        /// <summary>
        /// List all existing indexes.
        /// </summary>
        /// <returns>An object in the form:
        ///    {"items": [ {"name": "contacts", "createdAt": "2013-01-18T15:33:13.556Z"},
        ///                {"name": "notes", "createdAt": "2013-01-18T15:33:13.556Z"} ] }
        /// </returns>
        Task<JObject> ListIndexesAsync(RequestOptions requestOptions, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListIndexesAsync"/>
        /// </summary>
        /// <returns>An object in the form:
        ///    {"items": [ {"name": "contacts", "createdAt": "2013-01-18T15:33:13.556Z"},
        ///                {"name": "notes", "createdAt": "2013-01-18T15:33:13.556Z"} ] }
        /// </returns>
        JObject ListIndexes(RequestOptions requestOptions);

        /// <summary>
        /// Delete an index.
        /// </summary>
        /// <param name="indexName">The name of index to delete</param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns>An object containing a "deletedAt" attribute</returns>
        Task<JObject> DeleteIndexAsync(string indexName, RequestOptions requestOptions, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteIndexAsync"/>
        /// </summary>
        /// <returns>An object containing a "deletedAt" attribute</returns>
        JObject DeleteIndex(string indexName, RequestOptions requestOptions);

        /// <summary>
        /// Move an existing index.
        /// </summary>
        /// <param name="srcIndexName">The name of index to move.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        Task<JObject> MoveIndexAsync(string srcIndexName, string dstIndexName, RequestOptions requestOptions, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.MoveIndexAsync"/>
        /// </summary>
        /// <param name="srcIndexName">The name of index to move.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        /// <param name="requestOptions"></param>
        JObject MoveIndex(string srcIndexName, string dstIndexName, RequestOptions requestOptions);

        /// <summary>
        /// Copy an existing index.
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <param name="scopes">the scope of the copy, as a list.</param>
        Task<JObject> CopyIndexAsync(string srcIndexName, string dstIndexName, RequestOptions requestOptions, CancellationToken token = default(CancellationToken), List<CopyScope> scopes = null);

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.CopyIndexAsync"/>
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        /// <param name="requestOptions"></param>
        /// <param name="scopes">the scope of the copy, as a list.</param>
        JObject CopyIndex(string srcIndexName, string dstIndexName, RequestOptions requestOptions, List<CopyScope> scopes = null);

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.CopyIndexAsync"/>
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        JObject CopyIndex(string srcIndexName, string dstIndexName, RequestOptions requestOptions);

        /// <summary>
        /// Return last logs entries.
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="onlyErrors">If set to true, the answer will only contain API calls with errors.</param>
        Task<JObject> GetLogsAsync(RequestOptions requestOptions, int offset = 0, int length = 10, bool onlyErrors = false);

        /// <summary>
        /// Return last logs entries.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="logType">Specify the type of logs to include.</param>
        /// <param name="token"></param>
        Task<JObject> GetLogsAsync(RequestOptions requestOptions, int offset = 0, int length = 10, AlgoliaClient.LogType logType = AlgoliaClient.LogType.LOG_ALL, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetLogsAsync(int, int, bool)"/>
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="onlyErrors">If set to true, the answer will only contain API calls with errors.</param>
        JObject GetLogs(RequestOptions requestOptions, int offset = 0, int length = 10, bool onlyErrors = false);

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetLogsAsync(int, int, Algolia.Search.AlgoliaClient.LogType)"/>
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="logType">Specify the type of logs to include.</param>
        /// <param name="requestOptions"></param>
        JObject GetLogs(int offset, int length, AlgoliaClient.LogType logType, RequestOptions requestOptions);

        /// <summary>
        /// Get the index object initialized (no server call needed for initialization).
        /// </summary>
        /// <param name="indexName">The name of the index</param>
        /// <returns>An instance of the Index object that exposes Index actions</returns>
        Index InitIndex(string indexName);

        /// <summary>
        /// Get the analytics object initialized (no server call needed for initialization).
        /// </summary>
        /// <returns>An instance of the analytics object that exposes Index actions</returns>
        Analytics InitAnalytics();

        /// <summary>
        /// List all existing api keys with their associated ACLs.
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        Task<JObject> ListApiKeysAsync(RequestOptions requestOptions, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListApiKeysAsync"/>
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        JObject ListApiKeys(RequestOptions requestOptions);

        /// <summary>
        /// Get ACL for an existing user key.
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        Task<JObject> GetApiKeyAsync(string key, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetApiKeyAsync"/>
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        JObject GetApiKey(string key, RequestOptions requestOptions = null);

        /// <summary>
        /// Delete an existing user key.
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        Task<JObject> DeleteApiKeyAsync(string key, RequestOptions requestOptions, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteApiKeyAsync"/>
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        JObject DeleteApiKey(string key, RequestOptions requestOptions);

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
        Task<JObject> AddApiKeyAsync(Dictionary<string, object> parameters, RequestOptions requestOptions, CancellationToken token = default(CancellationToken));

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
        JObject AddApiKey(Dictionary<string, object> parameters, RequestOptions requestOptions);

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
        Task<JObject> AddApiKeyAsync(IEnumerable<string> acls, RequestOptions requestOptions, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null);

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
        JObject AddApiKey(IEnumerable<string> acls, RequestOptions requestOptions, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null);

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
        Task<JObject> UpdateApiKeyAsync(string key, Dictionary<string, object> parameters, RequestOptions requestOptions, CancellationToken token = default(CancellationToken));

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
        JObject UpdateApiKey(string key, Dictionary<string, object> parameters, RequestOptions requestOptions);

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
        Task<JObject> UpdateApiKeyAsync(string key, IEnumerable<string> acls, RequestOptions requestOptions, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null);

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
        JObject UpdateApiKey(string key, IEnumerable<string> acls, RequestOptions requestOptions, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null);

        /// <summary>
        /// Send a batch targeting multiple indices
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <param name="actions">An array of action to send.</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string) and a dictionary for the taskIDs.</returns>
        Task<JObject> BatchAsync(IEnumerable<object> requests, RequestOptions requestOptions, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.BatchAsync"/>
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="requestOptions"></param>
        /// <param name="actions">An array of action to send.</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string) and a dictionary for the taskIDs.</returns>
        JObject Batch(IEnumerable<object> requests, RequestOptions requestOptions);

        /// <summary>
        /// Generates a secured and public API Key from a query parameters and an optional user token identifying the current user
        /// </summary>
        /// <param name="privateApiKey">Your private API Key</param>
        /// <param name="query">The query parameters applied to the query (used as security)</param>
        /// <param name="userToken">An optional token identifying the current user</param>
        /// <returns></returns>
        string GenerateSecuredApiKey(String privateApiKey, Query query, String userToken = null);

        /// <summary>
        /// Generates a secured and public API Key from a list of tagFilters and an optional user token identifying the current user
        /// </summary>
        /// <param name="privateApiKey">Your private API Key</param>
        /// <param name="tagFilter">The list of tags applied to the query (used as security)</param>
        /// <param name="userToken">An optional token identifying the current user</param>
        /// <returns></returns>
        string GenerateSecuredApiKey(String privateApiKey, String tagFilter, String userToken = null);

        string HmacSha256(string key, string data);

        /// <summary>
        /// Used to execute the search request
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method">HTTP method</param>
        /// <param name="requestUrl">URL to request</param>
        /// <param name="content">The content</param>
        /// <param name="token"></param>
        /// <param name="requestOptions">The additional request options</param>
        /// <returns></returns>
        Task<JObject> ExecuteRequest(AlgoliaClient.callType type, string method, string requestUrl, object content, CancellationToken token, RequestOptions requestOptions);

        /// <summary>
        /// This method allows querying multiple indexes with one API call
        /// </summary>
        /// <param name="queries">List of queries per index</param>
        /// <param name="strategy">Strategy applied on the sequence of queries</param>
        /// <returns></returns>
        Task<JObject> MultipleQueriesAsync(List<IndexQuery> queries, string strategy = "none", CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.MultipleQueriesAsync"/>
        /// </summary>
        /// <param name="queries">List of queries per index</param>
        /// <returns></returns>
        JObject MultipleQueries(List<IndexQuery> queries, string strategy = "none", CancellationToken token = default(CancellationToken));

        /// </returns>
        Task<JObject> ListIndexesAsync(CancellationToken token = default(CancellationToken));

        /// </returns>
        JObject ListIndexes();

        /// <summary>
        /// Delete an index.
        /// </summary>
        /// <param name="indexName">The name of index to delete</param>
        /// <returns>An object containing a "deletedAt" attribute</returns>
        Task<JObject> DeleteIndexAsync(string indexName, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteIndexAsync"/>
        /// </summary>
        /// <returns>An object containing a "deletedAt" attribute</returns>
        JObject DeleteIndex(string indexName);

        /// <summary>
        /// Move an existing index.
        /// </summary>
        /// <param name="srcIndexName">The name of index to move.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        Task<JObject> MoveIndexAsync(string srcIndexName, string dstIndexName, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.MoveIndexAsync"/>
        /// </summary>
        /// <param name="srcIndexName">The name of index to move.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        JObject MoveIndex(string srcIndexName, string dstIndexName);

        /// <summary>
        /// Copy an existing index.
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        Task<JObject> CopyIndexAsync(string srcIndexName, string dstIndexName, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.CopyIndexAsync"/>
        /// </summary>
        /// <param name="srcIndexName">The name of index to copy.</param>
        /// <param name="dstIndexName">The new index name that will contain a copy of srcIndexName (destination will be overriten if it already exists).</param>
        JObject CopyIndex(string srcIndexName, string dstIndexName);

        /// <summary>
        /// Return last logs entries.
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="onlyErrors">If set to true, the answer will only contain API calls with errors.</param>
        Task<JObject> GetLogsAsync(int offset = 0, int length = 10, bool onlyErrors = false);

        /// <summary>
        /// Return last logs entries.
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="logType">Specify the type of logs to include.</param>
        Task<JObject> GetLogsAsync(int offset = 0, int length = 10, AlgoliaClient.LogType logType = AlgoliaClient.LogType.LOG_ALL, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetLogsAsync(int, int, bool)"/>
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="onlyErrors">If set to true, the answer will only contain API calls with errors.</param>
        JObject GetLogs(int offset = 0, int length = 10, bool onlyErrors = false);

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetLogsAsync(int, int, Algolia.Search.AlgoliaClient.LogType)"/>
        /// </summary>
        /// <param name="offset">Specify the first entry to retrieve (0-based, 0 is the most recent log entry).</param>
        /// <param name="length">Specify the maximum number of entries to retrieve starting at offset. Maximum allowed value: 1000.</param>
        /// <param name="logType">Specify the type of logs to include.</param>
        JObject GetLogs(int offset, int length, AlgoliaClient.LogType logType);

        /// <summary>
        /// List all existing api keys with their associated ACLs.
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        Task<JObject> ListApiKeysAsync(CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListApiKeysAsync"/>
        /// </summary>
        /// <returns>An object containing the list of keys.</returns>
        JObject ListApiKeys();

        /// <summary>
        /// Get ACL for an existing api key.
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        Task<JObject> GetApiKeyACLAsync(string key, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetApiKeyACLAsync"/>
        /// </summary>
        /// <returns>Returns an object with an "acls" array containing an array of strings with rights.</returns>
        JObject GetApiKeyACL(string key);

        /// <summary>
        /// Delete an existing user key.
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        Task<JObject> DeleteApiKeyAsync(string key, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.DeleteApiKeyAsync"/>
        /// </summary>
        /// <returns>Returns an object with a "deleteAt" attribute.</returns>
        JObject DeleteApiKey(string key);

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
        Task<JObject> AddApiKeyAsync(Dictionary<string, object> parameters, CancellationToken token = default(CancellationToken));

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
        JObject AddApiKey(Dictionary<string, object> parameters);

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
        Task<JObject> AddApiKeyAsync(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null);

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
        JObject AddApiKey(IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null);

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
        Task<JObject> UpdateApiKeyAsync(string key, Dictionary<string, object> parameters, CancellationToken token = default(CancellationToken));

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
        JObject UpdateApiKey(string key, Dictionary<string, object> parameters);

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
        Task<JObject> UpdateApiKeyAsync(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null);

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
        JObject UpdateApiKey(string key, IEnumerable<string> acls, int validity = 0, int maxQueriesPerIPPerHour = 0, int maxHitsPerQuery = 0, IEnumerable<string> indexes = null);

        /// <summary>
        /// Send a batch targeting multiple indices
        /// </summary>
        /// <param name="actions">An array of action to send.</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string) and a dictionary for the taskIDs.</returns>
        Task<JObject> BatchAsync(IEnumerable<object> requests, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.BatchAsync"/>
        /// </summary>
        /// <param name="actions">An array of action to send.</param>
        /// <returns>An object containing an "objectIDs" attribute (array of string) and a dictionary for the taskIDs.</returns>
        JObject Batch(IEnumerable<object> requests);

        /// <summary>
        /// Add a userID to the mapping
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="clusterName"></param>
        /// <param name="requestOptions"></param>
        /// <returns>an objecct containing a "updateAt" attribute {'updatedAt': 'XXXX'}</returns>
        Task<JObject> AssignUserIDAsync(string userID, string clusterName, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.AssignUserIDAsync"/>
        /// </summary>
        JObject AssignUserID(string userID, string clusterName, RequestOptions requestOptions = null);

        /// <summary>
        /// Remove a userID from the mapping
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="clusterName"></param>
        /// <param name="requestOptions"></param>
        /// <returns>an objecct containing a "deletedAt" attribute {'deletedAt': 'XXXX'}</returns>
        Task<JObject> RemoveUserIDAsync(string userID, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.RemoveUserIDAsync"/>
        /// </summary>
        JObject RemoveUserID(string userID, RequestOptions requestOptions = null);

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
        Task<JObject> ListClustersAsync(RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListClustersAsync"/>
        /// </summary>
        JObject ListClusters(RequestOptions requestOptions = null);

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
        Task<JObject> GetUserIDAsync(string userID, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetUserIDAsync"/>
        /// </summary>
        JObject GetUserID(string userID, RequestOptions requestOptions = null);

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
        Task<JObject> ListUserIDsAsync(int page =0, int hitsPerPage = 20, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.ListUserIDsAsync"/>
        /// </summary>
        JObject ListUserIDs(int page = 0, int hitsPerPage = 20, RequestOptions requestOptions = null);

        ///"page": 0,
        ///"hitsPerPage": 20
        ///}
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<JObject> GetTopUserIDAsync(RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.GetTopUserIDAsync"/>
        /// </summary>
        JObject GetTopUserID(RequestOptions requestOptions = null);

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
        Task<JObject> SearchUserIdsAsync(string query, string clusterName = null, int? page = null, int? hitsPerPage = null, RequestOptions requestOptions = null, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Synchronously call <see cref="AlgoliaClient.SearchUserIdsAsync"/>
        /// </summary>
        JObject SearchUserIds(string query, string clusterName = null, int? page = null, int? hitsPerPage = null, RequestOptions requestOptions = null);
    }
}
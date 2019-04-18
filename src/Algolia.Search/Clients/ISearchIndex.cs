/*
* Copyright (c) 2018 Algolia
* http://www.algolia.com/
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

using Algolia.Search.Http;
using Algolia.Search.Iterators;
using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;
using Algolia.Search.Models.Synonyms;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Search index interface
    /// </summary>
    public interface ISearchIndex
    {
        /// <summary>
        /// Index configuration
        /// </summary>
        AlgoliaConfig Config { get; }

        /// <summary>
        /// Update one or more attributes of an existing object.
        /// This method enables you to update only a part of an object by singling out one or more attributes of an existing object and performing the following actions:
        /// </summary>
        /// <param name="data">Data to send</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="createIfNotExists">When true, a partial update on a nonexistent object will create the object (generating the objectID and using the attributes as defined in the object). WHen false, a partial update on a nonexistent object will be ignored (but no error will be sent back).</param>
        /// <returns></returns>
        UpdateObjectResponse PartialUpdateObject<T>(T data, RequestOptions requestOptions = null,
            bool createIfNotExists = false)
            where T : class;

        /// <summary>
        /// Update one or more attributes of an existing object.
        /// This method enables you to update only a part of an object by singling out one or more attributes of an existing object and performing the following actions:
        /// </summary>
        /// <param name="data">Data to send</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="createIfNotExists">When true, a partial update on a nonexistent object will create the object (generating the objectID and using the attributes as defined in the object). WHen false, a partial update on a nonexistent object will be ignored (but no error will be sent back).</param>
        /// <returns></returns>
        Task<UpdateObjectResponse> PartialUpdateObjectAsync<T>(T data,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool createIfNotExists = false) where T : class;

        /// <summary>
        /// Update one or more attributes of an existing object.
        /// This method enables you to update only a part of an object by singling out one or more attributes of an existing object and performing the following actions:
        /// </summary>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <param name="data">Data to send</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="createIfNotExists">When true, a partial update on a nonexistent object will create the object (generating the objectID and using the attributes as defined in the object). WHen false, a partial update on a nonexistent object will be ignored (but no error will be sent back).</param>
        /// <returns></returns>
        BatchIndexingResponse PartialUpdateObjects<T>(IEnumerable<T> data, RequestOptions requestOptions = null,
            bool createIfNotExists = false)
            where T : class;

        /// <summary>
        /// Update one or more attributes of an existing object.
        /// This method enables you to update only a part of an object by singling out one or more attributes of an existing object and performing the following actions:
        /// </summary>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <param name="data">Data to send</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="createIfNotExists">When true, a partial update on a nonexistent object will create the object (generating the objectID and using the attributes as defined in the object). WHen false, a partial update on a nonexistent object will be ignored (but no error will be sent back).</param>
        /// <returns></returns>
        Task<BatchIndexingResponse> PartialUpdateObjectsAsync<T>(IEnumerable<T> data,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool createIfNotExists = false) where T : class;

        /// <summary>
        /// This method allows you to create records on your index by sending one or more objects
        /// Each object contains a set of attributes and values, which represents a full record on an index.
        /// </summary>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <param name="data">Data to send</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="autoGenerateObjectId">Add objectID if not present</param>
        /// <returns></returns>>
        BatchIndexingResponse SaveObject<T>(T data, RequestOptions requestOptions = null,
            bool autoGenerateObjectId = false) where T : class;

        /// <summary>
        /// This method allows you to create records on your index by sending one or more objects
        /// Each object contains a set of attributes and values, which represents a full record on an index.
        /// </summary>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <param name="data">Data to send</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="autoGenerateObjectId">Add objectID if not present</param>
        /// <returns></returns>
        Task<BatchIndexingResponse> SaveObjectAsync<T>(T data, RequestOptions requestOptions = null,
            CancellationToken ct = default, bool autoGenerateObjectId = false) where T : class;

        /// <summary>
        /// This method allows you to create records on your index by sending one or more objects
        /// Each object contains a set of attributes and values, which represents a full record on an index.
        /// </summary>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <param name="data">Data to send</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="autoGenerateObjectId">Add objectIDs if not present</param>
        /// <returns></returns>
        BatchIndexingResponse SaveObjects<T>(IEnumerable<T> data, RequestOptions requestOptions = null,
            bool autoGenerateObjectId = false)
            where T : class;

        /// <summary>
        /// This method allows you to create records on your index by sending one or more objects
        /// Each object contains a set of attributes and values, which represents a full record on an index.
        /// </summary>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <param name="data">Data to send</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="autoGenerateObjectId">Add objectIDs if not present</param>
        /// <returns></returns>
        Task<BatchIndexingResponse> SaveObjectsAsync<T>(IEnumerable<T> data,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool autoGenerateObjectId = false) where T : class;

        /// <summary>
        /// Push a new set of objects and remove all previous ones. Settings, synonyms and query rules are untouched.
        /// Replace all records in an index without any downtime.
        /// </summary>
        /// <param name="data">Data to send</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="safe">Run all api calls synchronously</param>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        MultiResponse ReplaceAllObjects<T>(IEnumerable<T> data, RequestOptions requestOptions = null, bool safe = false)
            where T : class;

        /// <summary>
        /// Push a new set of objects and remove all previous ones. Settings, synonyms and query rules are untouched.
        /// Replace all records in an index without any downtime.
        /// </summary>
        /// <param name="data">Data to send</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="safe">Run all api calls synchronously</param>
        /// <typeparam name="T">Type of the data to send/retrieve</typeparam>
        /// <returns></returns>
        Task<MultiResponse> ReplaceAllObjectsAsync<T>(IEnumerable<T> data,
            RequestOptions requestOptions = null, CancellationToken ct = default, bool safe = false)
            where T : class;

        /// <summary>
        /// Batch the given request
        /// </summary>
        /// <typeparam name="T">Type of the object to send</typeparam>
        /// <param name="operations">Operations to send to the api</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        BatchResponse Batch<T>(IEnumerable<BatchOperation<T>> operations, RequestOptions requestOptions = null)
            where T : class;

        /// <summary>
        /// Perform several indexing operations in one API call.
        /// </summary>
        /// <typeparam name="T">Type of the object to send</typeparam>
        /// <param name="operations">Operations to send to the api</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<BatchResponse> BatchAsync<T>(IEnumerable<BatchOperation<T>> operations,
            RequestOptions requestOptions = null, CancellationToken ct = default) where T : class;

        /// <summary>
        /// Perform several indexing operations in one API call.
        /// </summary>
        /// <typeparam name="T">Type of the object to send</typeparam>
        /// <param name="request">Request to send to the api</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        BatchResponse Batch<T>(BatchRequest<T> request, RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// Perform several indexing operations in one API call.
        /// </summary>
        /// <typeparam name="T">Type of the object to send</typeparam>
        /// <param name="request">Batch request</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<BatchResponse> BatchAsync<T>(BatchRequest<T> request, RequestOptions requestOptions = null,
            CancellationToken ct = default) where T : class;

        /// <summary>
        /// Remove objects from an index using their object ids.
        /// </summary>
        /// <param name="objectId">Algolia's objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        DeleteResponse DeleteObject(string objectId, RequestOptions requestOptions = null);

        /// <summary>
        /// Delete the index and all its settings, including links to its replicas.
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        DeleteResponse Delete(RequestOptions requestOptions = null);

        /// <summary>
        /// Delete the index and all its settings, including links to its replicas.
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteAsync(RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Remove objects from an index using their object ids.
        /// </summary>
        /// <param name="objectId">Algolia's objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteObjectAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Remove objects from an index using their object ids.
        /// </summary>
        /// <param name="objectIds">List of objectId</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        BatchIndexingResponse
            DeleteObjects(IEnumerable<string> objectIds, RequestOptions requestOptions = null);

        /// <summary>
        /// Remove objects from an index using their object ids.
        /// </summary>
        /// <param name="objectIds">List of objectId</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<BatchIndexingResponse> DeleteObjectsAsync(IEnumerable<string> objectIds,
            RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Remove all objects matching a filter (including geo filters).
        /// This method enables you to delete one or more objects based on filters (numeric, facet, tag or geo queries).
        /// It does not accept empty filters or a query.
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        DeleteResponse DeleteBy(Query query, RequestOptions requestOptions = null);

        /// <summary>
        /// Remove all objects matching a filter (including geo filters).
        /// This method enables you to delete one or more objects based on filters (numeric, facet, tag or geo queries).
        /// It does not accept empty filters or a query.
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteByAsync(Query query, RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Clear the records of an index without affecting its settings.
        /// This method enables you to delete an index’s contents (records) without removing any settings, rules and synonyms.
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        DeleteResponse ClearObjects(RequestOptions requestOptions = null);

        /// <summary>
        /// Clear the records of an index without affecting its settings.
        /// This method enables you to delete an index’s contents (records) without removing any settings, rules and synonyms.
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<DeleteResponse> ClearObjectsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        ///  Method used for querying an index.
        /// The search query only allows for the retrieval of up to 1000 hits.
        /// If you need to retrieve more than 1000 hits (e.g. for SEO), you can either leverage the Browse index method or increase the paginationLimitedTo parameter.
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        SearchResponse<T> Search<T>(Query query, RequestOptions requestOptions = null) where T : class;

        /// <summary>
        ///  Method used for querying an index.
        /// The search query only allows for the retrieval of up to 1000 hits.
        /// If you need to retrieve more than 1000 hits (e.g. for SEO), you can either leverage the Browse index method or increase the paginationLimitedTo parameter.
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<SearchResponse<T>> SearchAsync<T>(Query query, RequestOptions requestOptions = null,
            CancellationToken ct = default) where T : class;

        /// <summary>
        /// Search for a set of values within a given facet attribute. Can be combined with a query.
        /// This method enables you to search through the values of a facet attribute, selecting only a subset of those values that meet a given criteria.
        /// </summary>
        /// <param name="query">Search for facet query</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        SearchForFacetResponse SearchForFacetValue(SearchForFacetRequest query,
            RequestOptions requestOptions = null);

        /// <summary>
        /// Search for a set of values within a given facet attribute. Can be combined with a query.
        /// This method enables you to search through the values of a facet attribute, selecting only a subset of those values that meet a given criteria.
        /// </summary>
        /// <param name="query">Search for facet query</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<SearchForFacetResponse> SearchForFacetValueAsync(SearchForFacetRequest query,
            RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Get one or more objects using their object ids.
        /// </summary>
        /// <param name="objectId">Algolia's objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="attributesToRetrieve">List of attributes to retrieve. By default, all retrievable attributes are returned.</param>
        /// <returns></returns>
        T GetObject<T>(string objectId, RequestOptions requestOptions = null,
            IEnumerable<string> attributesToRetrieve = null) where T : class;

        /// <summary>
        /// Get one or more objects using their object ids.
        /// </summary>
        /// <param name="objectId">Algolia's objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="attributesToRetrieve">List of attributes to retrieve. By default, all retrievable attributes are returned.</param>
        /// <returns></returns>
        Task<T> GetObjectAsync<T>(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default, IEnumerable<string> attributesToRetrieve = null)
            where T : class;

        /// <summary>
        /// Retrieve one or more objects, potentially from the index, in a single API call.
        /// </summary>
        /// <param name="objectIDs"> ID of the object within that index</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="attributesToRetrieve">List of attributes to retrieve. By default, all retrievable attributes are returned.</param>
        /// <returns></returns>
        IEnumerable<T> GetObjects<T>(IEnumerable<string> objectIDs, RequestOptions requestOptions = null,
            IEnumerable<string> attributesToRetrieve = null) where T : class;

        /// <summary>
        /// Retrieve one or more objects, potentially from the index, in a single API call.
        /// </summary>
        /// <param name="objectIDs">ID of the object within that index</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="attributesToRetrieve">List of attributes to retrieve. By default, all retrievable attributes are returned.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetObjectsAsync<T>(IEnumerable<string> objectIDs, RequestOptions requestOptions = null,
            CancellationToken ct = default, IEnumerable<string> attributesToRetrieve = null)
            where T : class;

        /// <summary>
        /// This method allows you to retrieve all index content
        /// It can retrieve up to 1,000 records per call and supports full text search and filters.
        /// You can use the same query parameters as for a search query.
        /// </summary>
        /// <param name="query">Browse index query</param>
        /// <returns></returns>
        IndexIterator<T> Browse<T>(BrowseIndexQuery query) where T : class;

        /// <summary>
        /// This method allows you to retrieve all index content
        /// It can retrieve up to 1,000 records per call and supports full text search and filters.
        /// You can use the same query parameters as for a search query.
        /// </summary>
        /// <param name="query">Browse index query</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        BrowseIndexResponse<T> BrowseFrom<T>(BrowseIndexQuery query, RequestOptions requestOptions = null)
            where T : class;

        /// <summary>
        /// This method allows you to retrieve all index content
        /// It can retrieve up to 1,000 records per call and supports full text search and filters.
        /// You can use the same query parameters as for a search query.
        /// </summary>
        /// <param name="query">Browse index query</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<BrowseIndexResponse<T>> BrowseFromAsync<T>(BrowseIndexQuery query,
            RequestOptions requestOptions = null, CancellationToken ct = default) where T : class;

        /// <summary>
        /// Get the specified by its objectID
        /// </summary>
        /// <param name="objectId">Algolia's objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        Rule GetRule(string objectId, RequestOptions requestOptions = null);

        /// <summary>
        /// Get the specified rule by its objectID
        /// </summary>
        /// <param name="objectId">Algolia's objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<Rule> GetRuleAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Retrieve an index’s full list of rules using an iterator.
        /// The list contains the rule name, plus the complete details of its conditions and consequences.
        /// The list includes all rules, whether created on the dashboard or pushed by the API.
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        RulesIterator BrowseRules(RequestOptions requestOptions = null);

        /// <summary>
        /// Search for rules matching various criteria.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        SearchResponse<Rule> SearchRule(RuleQuery query = null, RequestOptions requestOptions = null);

        /// <summary>
        /// Search for rules matching various criteria.
        /// </summary>
        /// <param name="query">Rule query</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<SearchResponse<Rule>> SearchRuleAsync(RuleQuery query = null,
            RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Create or update a single rule.
        /// </summary>
        /// <param name="rule">Query rule</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        SaveRuleResponse SaveRule(Rule rule, RequestOptions requestOptions = null, bool forwardToReplicas = false);

        /// <summary>
        /// Create or update a single rule.
        /// </summary>
        /// <param name="rule">Query rule</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        Task<SaveRuleResponse> SaveRuleAsync(Rule rule, RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false);

        /// <summary>
        /// Create or update a specified set of rules, or all rules.
        /// </summary>
        /// <param name="rules">List of rules</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="forwardToReplicas">Forward to the replicas the request</param>
        /// <param name="clearExistingRules">Clear all existing rules</param>
        /// <returns></returns>
        BatchResponse SaveRules(IEnumerable<Rule> rules, RequestOptions requestOptions = null,
            bool forwardToReplicas = false, bool clearExistingRules = false);

        /// <summary>
        /// Create or update a specified set of rules, or all rules.
        /// </summary>
        /// <param name="rules">List of rules</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="forwardToReplicas">Forward to the replicas the request</param>
        /// <param name="clearExistingRules">Clear all existing rules</param>
        /// <returns></returns>
        Task<BatchResponse> SaveRulesAsync(IEnumerable<Rule> rules, RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false,
            bool clearExistingRules = false);

        /// <summary>
        /// Push a new set of rules and erase all previous ones.
        /// This method, like replaceAllObjects, guarantees zero downtime.
        /// All existing rules are deleted and replaced with the new ones, in a single, atomic operation
        /// </summary>
        /// <param name="rules">List of rules</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="forwardToReplicas">Forward to the replicas the request</param>
        /// <returns></returns>
        BatchResponse ReplaceAllRules(IEnumerable<Rule> rules, RequestOptions requestOptions = null,
            bool forwardToReplicas = false);

        /// <summary>
        /// Push a new set of rules and erase all previous ones.
        /// This method, like replaceAllObjects, guarantees zero downtime.
        /// All existing rules are deleted and replaced with the new ones, in a single, atomic operation
        /// </summary>
        /// <param name="rules">List of rules</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="forwardToReplicas">Forward to the replicas the request</param>
        /// <returns></returns>
        Task<BatchResponse> ReplaceAllRulesAsync(IEnumerable<Rule> rules, RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false);

        /// <summary>
        /// Delete the rule for the given ruleId
        /// </summary>
        /// <param name="objectId">Algolia's objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        DeleteResponse DeleteRule(string objectId, RequestOptions requestOptions = null,
            bool forwardToReplicas = false);

        /// <summary>
        /// Delete the rule for the given ruleId
        /// </summary>
        /// <param name="objectId">Algolia's objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteRuleAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false);

        /// <summary>
        /// Delete all rules in an index.
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        DeleteResponse ClearRules(RequestOptions requestOptions = null, bool forwardToReplicas = false);

        /// <summary>
        /// Delete all rules in an index.
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        Task<DeleteResponse> ClearRulesAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false);

        /// <summary>
        /// Get settings for the given index
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        IndexSettings GetSettings(RequestOptions requestOptions = null);

        /// <summary>
        /// Get settings for the given index
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<IndexSettings> GetSettingsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Create or change an index’s settings.
        /// Only specified settings are overridden; unspecified settings are left unchanged.
        /// Specifying null for a setting resets it to its default value.
        /// </summary>
        /// <param name="settings">Algolia's index settings</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="forwardToReplicas">Forward to the replicas the request</param>
        /// <returns></returns>
        SetSettingsResponse SetSettings(IndexSettings settings, RequestOptions requestOptions = null,
            bool forwardToReplicas = false);

        /// <summary>
        /// Create or change an index’s settings.
        /// Only specified settings are overridden; unspecified settings are left unchanged.
        /// Specifying null for a setting resets it to its default value.
        /// </summary>
        /// <param name="settings">Algolia's index settings</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="forwardToReplicas">Forward to the replicas the request</param>
        /// <returns></returns>
        Task<SetSettingsResponse> SetSettingsAsync(IndexSettings settings,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool forwardToReplicas = false);

        /// <summary>
        /// Get all synonyms that match a query.
        /// </summary>
        /// <param name="query">Synonym query</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        SearchResponse<Synonym> SearchSynonyms(SynonymQuery query, RequestOptions requestOptions = null);

        /// <summary>
        /// Get all synonyms that match a query.
        /// </summary>
        /// <param name="query">Synonym query</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<SearchResponse<Synonym>> SearchSynonymsAsync(SynonymQuery query,
            RequestOptions requestOptions = null, CancellationToken ct = default);

        /// <summary>
        /// Get a single synonym using its object id.
        /// </summary>
        /// <param name="synonymObjectId">The synonym objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        Synonym GetSynonym(string synonymObjectId, RequestOptions requestOptions = null);

        /// <summary>
        /// Get a single synonym using its object id.
        /// </summary>
        /// <param name="synonymObjectId">The synonym objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<Synonym> GetSynonymAsync(string synonymObjectId, RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Retrieve an index’s complete list of synonyms.
        /// The list includes all synonyms - whether created on the dashboard or pushed by the API.
        /// The method returns an iterator.
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        SynonymsIterator BrowseSynonyms(RequestOptions requestOptions = null);

        /// <summary>
        /// Create or update multiple synonyms.
        /// </summary>
        /// <param name="synonyms">List of synonyms</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="forwardToReplicas">Forward to the replicas the request</param>
        /// <param name="replaceExistingSynonyms"></param>
        /// <returns></returns>
        SaveSynonymResponse SaveSynonyms(IEnumerable<Synonym> synonyms, RequestOptions requestOptions = null,
            bool forwardToReplicas = false,
            bool replaceExistingSynonyms = false);

        /// <summary>
        /// Create or update multiple synonyms.
        /// </summary>
        /// <param name="synonyms">List of synonyms</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="forwardToReplicas">Forward to the replicas the request</param>
        /// <param name="replaceExistingSynonyms">Replace all existing synonyms</param>
        /// <returns></returns>
        Task<SaveSynonymResponse> SaveSynonymsAsync(IEnumerable<Synonym> synonyms,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool forwardToReplicas = false, bool replaceExistingSynonyms = false);

        /// <summary>
        /// Push a new set of synonyms and erase all previous ones.
        /// This method, like replaceAllObjects, guarantees zero downtime.
        /// All existing synonyms are deleted and replaced with the new ones, in a single, atomic operation
        /// </summary>
        /// <param name="synonyms">List of synonyms</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="forwardToReplicas">Forward to the replicas the request</param>
        /// <returns></returns>
        SaveSynonymResponse ReplaceAllSynonyms(IEnumerable<Synonym> synonyms, RequestOptions requestOptions = null,
            bool forwardToReplicas = false);

        /// <summary>
        /// Push a new set of synonyms and erase all previous ones.
        /// This method, like replaceAllObjects, guarantees zero downtime.
        /// All existing synonyms are deleted and replaced with the new ones, in a single, atomic operation
        /// </summary>
        /// <param name="synonyms">List of synonyms</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="forwardToReplicas">Forward to the replicas the request</param>
        /// <returns></returns>
        Task<SaveSynonymResponse> ReplaceAllSynonymsAsync(IEnumerable<Synonym> synonyms,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool forwardToReplicas = false);

        /// <summary>
        /// Create or update a single synonym on an index.
        /// </summary>
        /// <param name="synonym">Algolia's synonym</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        SaveSynonymResponse SaveSynonym(Synonym synonym, RequestOptions requestOptions = null,
            bool forwardToReplicas = false);

        /// <summary>
        /// Create or update a single synonym on an index.
        /// </summary>
        /// <param name="synonym">Algolia's synonym</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        Task<SaveSynonymResponse> SaveSynonymAsync(Synonym synonym, RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false);

        /// <summary>
        /// Remove a single synonym from an index using its object id.
        /// </summary>
        /// <param name="synonymObjectId">The synonym objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        DeleteResponse DeleteSynonym(string synonymObjectId, RequestOptions requestOptions = null,
            bool forwardToReplicas = false);

        /// <summary>
        /// Remove a single synonym from an index using its object id.
        /// </summary>
        /// <param name="synonymObjectId">The synonym objectID</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteSynonymAsync(string synonymObjectId,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool forwardToReplicas = false);

        /// <summary>
        /// Remove all synonyms from an index.
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        ClearSynonymsResponse ClearSynonyms(RequestOptions requestOptions = null, bool forwardToReplicas = false);

        /// <summary>
        /// Remove all synonyms from an index.
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="forwardToReplicas">Forward the request to the replicas</param>
        /// <returns></returns>
        Task<ClearSynonymsResponse> ClearSynonymsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false);

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        MoveIndexResponse MoveFrom(string sourceIndex, RequestOptions requestOptions = null);

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<MoveIndexResponse> MoveFromAsync(string sourceIndex, RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Wait for a task to complete before executing the next line of code, to synchronize index updates.
        /// All write operations in Algolia are asynchronous by design.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        void WaitTask(long taskId, int timeToWait = 100, RequestOptions requestOptions = null);

        /// <summary>
        /// Wait for a task to complete before executing the next line of code, to synchronize index updates.
        /// All write operations in Algolia are asynchronous by design.
        /// </summary>
        /// <param name="taskId">The task ID to wait</param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task WaitTaskAsync(long taskId, int timeToWait = 100, RequestOptions requestOptions = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get the status of the given task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <returns></returns>
        TaskStatusResponse GetTask(long taskId, RequestOptions requestOptions = null);

        /// <summary>
        /// Get the status of the given task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        Task<TaskStatusResponse> GetTaskAsync(long taskId, RequestOptions requestOptions = null,
            CancellationToken ct = default);
    }
}

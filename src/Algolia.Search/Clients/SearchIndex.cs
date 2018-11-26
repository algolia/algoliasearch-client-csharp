/*
* Copyright (c) 2018 Algolia
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

using Algolia.Search.Http;
using Algolia.Search.Iterators;
using Algolia.Search.Models.Batch;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Query;
using Algolia.Search.Models.Requests;
using Algolia.Search.Models.Responses;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Settings;
using Algolia.Search.Models.Synonyms;
using Algolia.Search.Transport;
using Algolia.Search.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    public class SearchIndex : ISearchIndex
    {
        /// <summary>
        /// Index configuration
        /// </summary>
        public AlgoliaConfig Config { get; }

        /// <summary>
        /// The Requester wrapper
        /// </summary>
        private readonly IRequesterWrapper _requesterWrapper;

        /// <summary>
        /// Url encoded index name
        /// </summary>
        private readonly string _urlEncodedIndexName;

        /// <summary>
        /// Original index name
        /// </summary>
        private readonly string _indexName;

        /// <summary>
        /// Instantiate an Index for a given client
        /// </summary>
        /// <param name="requesterWrapper"></param>
        /// <param name="indexName"></param>
        public SearchIndex(IRequesterWrapper requesterWrapper, AlgoliaConfig config, string indexName)
        {
            _requesterWrapper = requesterWrapper ?? throw new ArgumentNullException(nameof(requesterWrapper));
            _indexName = !string.IsNullOrWhiteSpace(indexName)
                    ? indexName
                    : throw new ArgumentNullException(nameof(indexName));
            _urlEncodedIndexName = WebUtility.UrlEncode(indexName);
            Config = config;
        }

        /// <summary>
        /// Add an object to the given index
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public AddObjectResponse AddObject<T>(T data, RequestOptions requestOptions = null) where T : class =>
                    AsyncHelper.RunSync(() => AddObjectAysnc(data, requestOptions));

        /// <summary>
        /// Add an object to the given index
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<AddObjectResponse> AddObjectAysnc<T>(T data, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (ReferenceEquals(data, null))
            {
                throw new ArgumentNullException(nameof(data));
            }

            AddObjectResponse response = await _requesterWrapper.ExecuteRequestAsync<AddObjectResponse, T>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}", CallType.Write, data, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Update one or more attributes of an existing object.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public UpdateObjectResponse PartialUpdateObject<T>(T data, RequestOptions requestOptions = null) where T : class =>
                    AsyncHelper.RunSync(() => PartialUpdateObjectAsync(data, requestOptions));

        /// <summary>
        /// Update one or more attributes of an existing object.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<UpdateObjectResponse> PartialUpdateObjectAsync<T>(T data, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (ReferenceEquals(data, null))
            {
                throw new ArgumentNullException(nameof(data));
            }

            PropertyInfo pi = typeof(T).GetTypeInfo().GetDeclaredProperty("ObjectID");

            if (pi == null)
            {
                throw new Exception("The class mut have an ObjectID property");
            }

            if (pi.GetType() != typeof(string))
            {
                throw new NotSupportedException("ObjectID property must be a string");
            }

            string objectId = (string)pi.GetValue(data);

            UpdateObjectResponse response = await _requesterWrapper.ExecuteRequestAsync<UpdateObjectResponse, T>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/{objectId}/partial", CallType.Write, data, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Add objects to the given index
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public BatchResponse AddObjects<T>(IEnumerable<T> datas, RequestOptions requestOptions = null) where T : class =>
                    AsyncHelper.RunSync(() => AddObjectsAysnc(datas, requestOptions));

        /// <summary>
        /// Add objects to the given index
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<BatchResponse> AddObjectsAysnc<T>(IEnumerable<T> datas, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (datas == null)
            {
                throw new ArgumentNullException(nameof(datas));
            }

            var batch = new BatchRequest<T>(BatchActionType.AddObject, datas);

            BatchResponse response = await _requesterWrapper.ExecuteRequestAsync<BatchResponse, BatchRequest<T>>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/batch", CallType.Write, batch, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Replace all objects
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="safe"></param>
        /// <param name="requestOptions"></param>
        /// <typeparam name="T"></typeparam>
        public MultiResponse ReplaceAllObjects<T>(IEnumerable<T> datas, bool safe = false, RequestOptions requestOptions = null) where T : class =>
                    AsyncHelper.RunSync(() => ReplaceAllObjectsAsync(datas, safe, requestOptions));

        /// <summary>
        /// Replace all objects
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="safe"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<MultiResponse> ReplaceAllObjectsAsync<T>(IEnumerable<T> datas, bool safe = false, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken)) where T : class
        {
            Random rnd = new Random();
            string tmpIndexName = $"{this._indexName}_tmp_{rnd.Next(100)}";
            SearchIndex tmpIndex = new SearchIndex(_requesterWrapper, Config, tmpIndexName);

            List<string> scopes = new List<string> { CopyScope.Rules, CopyScope.Settings, CopyScope.Synonyms };
            MultiResponse response = new MultiResponse { Responses = new List<IAlgoliaWaitableResponse>() };

            // Copy index ressources
            CopyToResponse copyResponse = await CopyToAsync(tmpIndexName, scopes, requestOptions, ct).ConfigureAwait(false);
            response.Responses.Add(copyResponse);

            if (safe)
            {
                copyResponse.Wait();
            }

            BatchResponse saveObjectsResponse = await tmpIndex.AddObjectsAysnc<T>(datas, requestOptions, ct).ConfigureAwait(false);
            response.Responses.Add(copyResponse);

            if (safe)
            {
                saveObjectsResponse.Wait();
            }

            // Move temporary index to source index
            MoveIndexResponse moveResponse = await MoveFromAsync(tmpIndexName, requestOptions, ct).ConfigureAwait(false);
            response.Responses.Add(copyResponse);

            if (safe)
            {
                moveResponse.Wait();
            }

            return response;
        }

        /// <summary>
        /// Batch the given request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public BatchResponse Batch<T>(IEnumerable<BatchOperation<T>> operations, RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => BatchAsync(operations, requestOptions));

        /// <summary>
        /// Batch the given request
        /// </summary>
        /// <typeparam name="BatchResponse"></typeparam>
        public async Task<BatchResponse> BatchAsync<T>(IEnumerable<BatchOperation<T>> operations, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (operations == null)
            {
                throw new ArgumentNullException(nameof(operations));
            }

            var batch = new BatchRequest<T>(operations);

            BatchResponse response = await _requesterWrapper.ExecuteRequestAsync<BatchResponse, BatchRequest<T>>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/batch", CallType.Write, batch, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Remove objects from an index using its object id.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public DeleteResponse DeleteObject(string objectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteObjectAsync(objectId, requestOptions));

        /// <summary>
        /// Remove objects from an index using its object id.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> DeleteObjectAsync(string objectId, RequestOptions requestOptions = null,
                 CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Get,
                $"/1/indexes/{_urlEncodedIndexName}/{objectId}", CallType.Read, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Delete all the objects for their objectIds
        /// </summary>
        /// <param name="objectIds"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public BatchResponse DeleteObjects(IEnumerable<string> objectIds, RequestOptions requestOptions = null) =>
                    AsyncHelper.RunSync(() => DeleteObjectsAsync(objectIds, requestOptions));

        /// <summary>
        /// Delete all the objects for their objectIds
        /// </summary>
        /// <param name="objectIds"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<BatchResponse> DeleteObjectsAsync(IEnumerable<string> objectIds, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            if (objectIds == null || !objectIds.Any())
            {
                throw new ArgumentNullException(nameof(objectIds));
            }

            var batch = new BatchRequest<string>(BatchActionType.DeleteObject, objectIds);

            BatchResponse response = await _requesterWrapper.ExecuteRequestAsync<BatchResponse, BatchRequest<string>>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/batch", CallType.Write, batch, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Remove all objects matching a filter (including geo filters).
        /// This method enables you to delete one or more objects based on filters (numeric, facet, tag or geo queries).
        /// It does not accept empty filters or a query.        
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public DeleteResponse DeleteBy(SearchQuery query, RequestOptions requestOptions = null) =>
                    AsyncHelper.RunSync(() => DeleteByAsync(query, requestOptions));

        /// <summary>
        /// Remove all objects matching a filter (including geo filters).
        /// This method enables you to delete one or more objects based on filters (numeric, facet, tag or geo queries).
        /// It does not accept empty filters or a query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> DeleteByAsync(SearchQuery query, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse, SearchQuery>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/deleteByQuery", CallType.Write, query, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Clear the records of an index without affecting its settings.
        /// This method enables you to delete an index’s contents (records) without removing any settings, rules and synonyms.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public DeleteResponse ClearObjects(RequestOptions requestOptions = null) =>
                    AsyncHelper.RunSync(() => ClearObjectsAsync(requestOptions));

        /// <summary>
        /// Clear the records of an index without affecting its settings.
        /// This method enables you to delete an index’s contents (records) without removing any settings, rules and synonyms.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> ClearObjectsAsync(RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/clear", CallType.Write, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Search in the index for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SearchResponse<T> Search<T>(SearchQuery query, RequestOptions requestOptions = null) where T : class =>
                    AsyncHelper.RunSync(() => SearchAsync<T>(query, requestOptions));

        /// <summary>
        ///  Search in the index for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SearchResponse<T>> SearchAsync<T>(SearchQuery query, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _requesterWrapper.ExecuteRequestAsync<SearchResponse<T>, SearchQuery>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/query", CallType.Read, query, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Search for a set of values within a given facet attribute. Can be combined with a query.
        /// This method enables you to search through the values of a facet attribute, selecting only a subset of those values that meet a given criteria.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SearchForFacetResponse SearchForFacetValue(SearchForFacetRequest query, RequestOptions requestOptions = null) =>
                    AsyncHelper.RunSync(() => SearchForFacetValueAsync(query, requestOptions));

        /// <summary>
        /// Search for a set of values within a given facet attribute. Can be combined with a query.
        /// This method enables you to search through the values of a facet attribute, selecting only a subset of those values that meet a given criteria.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SearchForFacetResponse> SearchForFacetValueAsync(SearchForFacetRequest query, RequestOptions requestOptions = null,
                CancellationToken ct = default(CancellationToken))
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (string.IsNullOrWhiteSpace(query.FacetName))
            {
                throw new ArgumentNullException(nameof(query.FacetName));
            }

            return await _requesterWrapper.ExecuteRequestAsync<SearchForFacetResponse, SearchForFacetRequest>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/facets/{query.FacetName}/query", CallType.Read, query, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get object for the specified ID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public T GetObject<T>(string objectId, RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => GetObjectAsync<T>(objectId, requestOptions));

        /// <summary>
        /// Get the specified object by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<T> GetObjectAsync<T>(string objectId, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            return await _requesterWrapper.ExecuteRequestAsync<T>(HttpMethod.Get,
                $"/1/indexes/{_urlEncodedIndexName}/{objectId}", CallType.Read, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// This method allows you to retrieve all index content  
        /// It can retrieve up to 1,000 records per call and supports full text search and filters. 
        /// You can use the same query parameters as for a search query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IndexIterator<T> Browse<T>(BrowseIndexQuery query) where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return new IndexIterator<T>(this, query);
        }

        /// <summary>
        /// This method allows you to retrieve all index content  
        /// It can retrieve up to 1,000 records per call and supports full text search and filters. 
        /// You can use the same query parameters as for a search query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public BrowseIndexResponse<T> BrowseFrom<T>(BrowseIndexQuery query, RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => BrowseFromAsync<T>(query, requestOptions));

        /// <summary>
        /// This method allows you to retrieve all index content  
        /// It can retrieve up to 1,000 records per call and supports full text search and filters. 
        /// You can use the same query parameters as for a search query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<BrowseIndexResponse<T>> BrowseFromAsync<T>(BrowseIndexQuery query, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _requesterWrapper.ExecuteRequestAsync<BrowseIndexResponse<T>, BrowseIndexQuery>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/browse", CallType.Read, query, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the specified by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public Rule GetRule(string objectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetRuleAsync(objectId, requestOptions));

        /// <summary>
        /// Get the specified rule by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Rule> GetRuleAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            return await _requesterWrapper.ExecuteRequestAsync<Rule>(HttpMethod.Get,
                $"/1/indexes/{_urlEncodedIndexName}/rules/{objectId}", CallType.Read, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Search rules sync
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SearchResponse<Rule> SearchRule(RuleQuery query = null, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SearchRuleAsync(query, requestOptions));

        /// <summary>
        /// Search query 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SearchResponse<Rule>> SearchRuleAsync(RuleQuery query = null, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<SearchResponse<Rule>, RuleQuery>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/rules/search", CallType.Read, query, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        ///  Save rule 
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SaveRuleResponse SaveRule(Rule rule, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SaveRuleAsync(rule, requestOptions));

        /// <summary>
        /// Save the given rule
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SaveRuleResponse> SaveRuleAsync(Rule rule, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            SaveRuleResponse response = await _requesterWrapper.ExecuteRequestAsync<SaveRuleResponse, Rule>(HttpMethod.Put,
                $"/1/indexes/{_urlEncodedIndexName}/rules/{rule.ObjectID}", CallType.Write, rule, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Save several rules
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="clearExistingRules"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public BatchResponse SaveRules(IEnumerable<Rule> rules, bool forwardToReplicas = false, bool clearExistingRules = false, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SaveRulesAsync(rules, forwardToReplicas, clearExistingRules, requestOptions));

        /// <summary>
        /// Save several rules
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="clearExistingRules"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<BatchResponse> SaveRulesAsync(IEnumerable<Rule> rules, bool forwardToReplicas = false, bool clearExistingRules = false, RequestOptions requestOptions = null,
             CancellationToken ct = default(CancellationToken))
        {
            if (rules == null || !rules.Any())
            {
                throw new ArgumentNullException(nameof(rules));
            }

            var dic = new Dictionary<string, string>
            {
                { nameof(forwardToReplicas), forwardToReplicas.ToString().ToLower()},
                { nameof(clearExistingRules), clearExistingRules.ToString().ToLower() }
            };

            RequestOptions requestOptionsToSend = RequestOptionsHelper.Create(requestOptions, dic);

            BatchResponse response = await _requesterWrapper.ExecuteRequestAsync<BatchResponse, IEnumerable<Rule>>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/rules/batch", CallType.Write, rules, requestOptionsToSend, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Add a list of rules and clear the existing one
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public BatchResponse ReplaceAllRules(IEnumerable<Rule> rules, bool forwardToReplicas = false, RequestOptions requestOptions = null)
        {
            return SaveRules(rules, forwardToReplicas, true, requestOptions);
        }

        /// <summary>
        /// Add a list of rules and clear the existing one
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public async Task<BatchResponse> ReplaceAllRulesAsync(IEnumerable<Rule> rules, bool forwardToReplicas = false, RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            return await SaveRulesAsync(rules, forwardToReplicas, true, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete the rule for the given ruleId
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public DeleteResponse DeleteRule(string objectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteRuleAsync(objectId, requestOptions));

        /// <summary>
        /// Delete the rule for the given ruleId
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> DeleteRuleAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                $"/1/indexes/{_urlEncodedIndexName}/rules/{objectId}", CallType.Write, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Delete all rules in an index.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public DeleteResponse ClearRules(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ClearRulesAsync(requestOptions));

        /// <summary>
        /// Delete all rules in an index.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> ClearRulesAsync(RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/rules/clear", CallType.Write, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Make a copy of the rules of an index
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public CopyToResponse CopyRulesTo(string destinationIndex, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopyRulesToAsync(destinationIndex, requestOptions));

        /// <summary>
        /// Make a copy of the rules of an index
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CopyToResponse> CopyRulesToAsync(string destinationIndex, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            var scopes = new List<string> { CopyScope.Rules };
            return await CopyToAsync(destinationIndex, scopes).ConfigureAwait(false);
        }

        /// <summary>
        /// Get settings for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public IndexSettings GetSettings(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetSettingsAsync(requestOptions));

        /// <summary>
        /// Get settings for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<IndexSettings> GetSettingsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<IndexSettings>(HttpMethod.Get,
                $"/1/indexes/{_urlEncodedIndexName}/settings", CallType.Read, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Set settings
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SetSettingsResponse SetSettings(IndexSettings settings, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SetSettingsAsync(settings, requestOptions));

        /// <summary>
        /// Set settings for the given index
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SetSettingsResponse> SetSettingsAsync(IndexSettings settings, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            SetSettingsResponse response = await _requesterWrapper.ExecuteRequestAsync<SetSettingsResponse, IndexSettings>(HttpMethod.Put,
                $"/1/indexes/{_urlEncodedIndexName}/settings", CallType.Write, settings, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Make a copy of the settings of an index
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public CopyToResponse CopySettingsTo(string destinationIndex, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopySettingsToAsync(destinationIndex, requestOptions));

        /// <summary>
        /// Make a copy of the settings of an index
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CopyToResponse> CopySettingsToAsync(string destinationIndex, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            var scopes = new List<string> { CopyScope.Settings };
            return await CopyToAsync(destinationIndex, scopes).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all synonyms that match a query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SearchResponse<Synonym> SearchSynonyms(SynonymQuery query, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SearchSynonymsAsync(query, requestOptions));

        /// <summary>
        /// Get all synonyms that match a query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SearchResponse<Synonym>> SearchSynonymsAsync(SynonymQuery query, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _requesterWrapper.ExecuteRequestAsync<SearchResponse<Synonym>, SynonymQuery>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/synonyms/search", CallType.Read, query, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a single synonym using its object id.
        /// </summary>
        /// <param name="synonymObjectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public Synonym GetSynonym(string synonymObjectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetSynonymAsync(synonymObjectId, requestOptions));

        /// <summary>
        /// Get a single synonym using its object id.
        /// </summary>
        /// <param name="synonymObjectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Synonym> GetSynonymAsync(string synonymObjectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (synonymObjectId == null)
            {
                throw new ArgumentNullException(nameof(synonymObjectId));
            }

            return await _requesterWrapper.ExecuteRequestAsync<Synonym>(HttpMethod.Get,
                $"/1/indexes/{_urlEncodedIndexName}/synonyms/{synonymObjectId}", CallType.Read, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Create or update multiple synonyms.
        /// </summary>
        /// <param name="synonyms"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="replaceExistingSynonyms"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SaveSynonymResponse SaveSynonyms(IEnumerable<Synonym> synonyms, bool forwardToReplicas = false, bool replaceExistingSynonyms = false, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SaveSynonymsAsync(synonyms, forwardToReplicas, replaceExistingSynonyms, requestOptions));

        /// <summary>
        /// Create or update multiple synonyms.
        /// </summary>
        /// <param name="synonyms"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="replaceExistingSynonyms"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SaveSynonymResponse> SaveSynonymsAsync(IEnumerable<Synonym> synonyms, bool forwardToReplicas = false, bool replaceExistingSynonyms = false, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (synonyms == null || !synonyms.Any())
            {
                throw new ArgumentNullException(nameof(synonyms));
            }

            var dic = new Dictionary<string, string>
            {
                { nameof(forwardToReplicas), forwardToReplicas.ToString().ToLower() },
                { nameof(replaceExistingSynonyms), replaceExistingSynonyms.ToString().ToLower() }
            };

            RequestOptions requestOptionsToSend = RequestOptionsHelper.Create(requestOptions, dic);

            SaveSynonymResponse response = await _requesterWrapper.ExecuteRequestAsync<SaveSynonymResponse, IEnumerable<Synonym>>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/synonyms/batch", CallType.Write, synonyms, requestOptionsToSend, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Add a new list of synonyms and clear the exsiting one
        /// </summary>
        /// <param name="synonyms"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SaveSynonymResponse ReplaceAllSynonyms(IEnumerable<Synonym> synonyms, bool forwardToReplicas = false, RequestOptions requestOptions = null)
        {
            return SaveSynonyms(synonyms, forwardToReplicas, true, requestOptions);
        }

        /// <summary>
        /// Add a new list of synonyms and clear the exsiting one
        /// </summary>
        /// <param name="synonyms"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SaveSynonymResponse> ReplaceAllSynonymsAsync(IEnumerable<Synonym> synonyms, bool forwardToReplicas = false, RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            return await SaveSynonymsAsync(synonyms, forwardToReplicas, true, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Create or update a single synonym on an index.
        /// </summary>
        /// <param name="synonym"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SaveSynonymResponse SaveSynonym(Synonym synonym, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SaveSynonymAsync(synonym, requestOptions));

        /// <summary>
        /// Create or update a single synonym on an index.
        /// </summary>
        /// <param name="synonym"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SaveSynonymResponse> SaveSynonymAsync(Synonym synonym, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (synonym == null)
            {
                throw new ArgumentNullException(nameof(synonym));
            }

            if (string.IsNullOrWhiteSpace(synonym.ObjectID))
            {
                throw new ArgumentNullException(nameof(synonym.ObjectID));
            }

            SaveSynonymResponse response = await _requesterWrapper.ExecuteRequestAsync<SaveSynonymResponse, Synonym>(HttpMethod.Put,
                $"/1/indexes/{_urlEncodedIndexName}/synonyms/{synonym.ObjectID}", CallType.Write, synonym, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Remove a single synonym from an index using its object id.
        /// </summary>
        /// <param name="synonymObjectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public DeleteResponse DeleteSynonym(string synonymObjectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteSynonymAsync(synonymObjectId, requestOptions));

        /// <summary>
        /// Remove a single synonym from an index using its object id.
        /// </summary>
        /// <param name="synonymObjectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> DeleteSynonymAsync(string synonymObjectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (synonymObjectId == null)
            {
                throw new ArgumentNullException(nameof(synonymObjectId));
            }

            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                $"/1/indexes/{_urlEncodedIndexName}/synonyms/{synonymObjectId}", CallType.Write, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Remove all synonyms from an index.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public ClearSynonymsResponse ClearSynonyms(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ClearSynonymsAsync(requestOptions));

        /// <summary>
        /// Remove all synonyms from an index.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ClearSynonymsResponse> ClearSynonymsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            ClearSynonymsResponse response = await _requesterWrapper.ExecuteRequestAsync<ClearSynonymsResponse>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/synonyms/clear", CallType.Write, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Make a copy of the synonyms of an index
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public CopyToResponse CopySynonymsTo(string destinationIndex, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopySynonymsToAsync(destinationIndex, requestOptions));

        /// <summary>
        /// Make a copy of the synonyms of an index
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CopyToResponse> CopySynonymsToAsync(string destinationIndex, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            var scopes = new List<string> { CopyScope.Synonyms };
            return await CopyToAsync(destinationIndex, scopes).ConfigureAwait(false);
        }

        /// <summary>
        /// Make a copy of an index, including its objects, settings, synonyms, and query rules.
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="scope"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public CopyToResponse CopyTo(string destinationIndex, IEnumerable<string> scope = null, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopyToAsync(destinationIndex, scope, requestOptions));

        /// <summary>
        /// Make a copy of an index, including its objects, settings, synonyms, and query rules.
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="scope"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CopyToResponse> CopyToAsync(string destinationIndex, IEnumerable<string> scope = null, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(destinationIndex))
            {
                throw new ArgumentNullException(destinationIndex);
            }

            var data = new CopyToRequest
            {
                Operation = MoveType.Copy,
                IndexNameDest = destinationIndex,
                Scope = scope
            };

            CopyToResponse response = await _requesterWrapper.ExecuteRequestAsync<CopyToResponse, CopyToRequest>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/operation", CallType.Write, data, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public MoveIndexResponse MoveFrom(string destinationIndex, RequestOptions requestOptions = null) =>
                    AsyncHelper.RunSync(() => MoveFromAsync(destinationIndex, requestOptions));

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="sourceIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<MoveIndexResponse> MoveFromAsync(string sourceIndex, RequestOptions requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(sourceIndex))
            {
                throw new ArgumentNullException(sourceIndex);
            }

            MoveIndexRequest request = new MoveIndexRequest
            {
                Operation = MoveType.Move,
                Destination = _indexName
            };

            MoveIndexResponse response = await _requesterWrapper.ExecuteRequestAsync<MoveIndexResponse, MoveIndexRequest>(HttpMethod.Post,
                $"/1/indexes/{sourceIndex}/operation", CallType.Write, request, requestOptions, ct).ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// This function waits for the Algolia's API task to finish
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions"></param>
        public void WaitTask(long taskId, int timeToWait = 100, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => WaitTaskAsync(taskId, timeToWait, requestOptions));

        /// <summary>
        /// This function waits for the Algolia's API task to finish
        /// </summary>
        /// <param name="taskId">The task ID to wait</param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task WaitTaskAsync(long taskId, int timeToWait = 100, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            while (true)
            {
                TaskStatusResponse response = await GetTaskAsync(taskId, requestOptions, ct).ConfigureAwait(false);

                if (response.Status.Equals("published"))
                {
                    return;
                }

                await Task.Delay(timeToWait, ct).ConfigureAwait(false);
                timeToWait *= 2;

                if (timeToWait > 10000)
                {
                    timeToWait = 10000;
                }
            }
        }

        /// <summary>
        /// Get the status of the given task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public TaskStatusResponse GetTask(long taskId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetTaskAsync(taskId, requestOptions));

        /// <summary>
        /// Get the status of the given task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<TaskStatusResponse> GetTaskAsync(long taskId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<TaskStatusResponse>(HttpMethod.Get,
                $"/1/indexes/{_urlEncodedIndexName}/task/{taskId}", CallType.Read, requestOptions, ct).ConfigureAwait(false);
        }
    }
}

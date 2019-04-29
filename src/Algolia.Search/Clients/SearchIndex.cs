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
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Settings;
using Algolia.Search.Models.Synonyms;
using Algolia.Search.Transport;
using Algolia.Search.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    /// <summary>
    /// Search Index implementation of <see cref="ISearchIndex"/>
    /// </summary>
    public class SearchIndex : ISearchIndex
    {
        /// <summary>
        /// Index configuration
        /// </summary>
        public AlgoliaConfig Config { get; }

        private readonly HttpTransport _transport;
        private readonly string _urlEncodedIndexName;
        private readonly string _indexName;

        /// <inheritdoc />
        internal SearchIndex(HttpTransport transport, AlgoliaConfig config, string indexName)
        {
            _transport = transport ?? throw new ArgumentNullException(nameof(transport));
            _indexName = !string.IsNullOrWhiteSpace(indexName)
                ? indexName
                : throw new ArgumentNullException(nameof(indexName));
            _urlEncodedIndexName = WebUtility.UrlEncode(indexName);
            Config = config;
        }

        /// <inheritdoc />
        public UpdateObjectResponse PartialUpdateObject<T>(T data, RequestOptions requestOptions = null,
            bool createIfNotExists = false)
            where T : class =>
            AsyncHelper.RunSync(() =>
                PartialUpdateObjectAsync(data, requestOptions, createIfNotExists: createIfNotExists));

        /// <inheritdoc />
        public async Task<UpdateObjectResponse> PartialUpdateObjectAsync<T>(T data,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool createIfNotExists = false) where T : class
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data is IEnumerable)
            {
                throw new ArgumentException($"{nameof(data)} should not be an IEnumerable/List/Collection");
            }

            // Get && check if the generic type has an objectID
            string objectId = AlgoliaHelper.GetObjectID(data);

            var dic = new Dictionary<string, string>
            {
                {nameof(createIfNotExists), createIfNotExists.ToString().ToLower()}
            };

            requestOptions = requestOptions.AddQueryParams(dic);

            UpdateObjectResponse response = await _transport.ExecuteRequestAsync<UpdateObjectResponse, T>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/{WebUtility.UrlEncode(objectId)}/partial", CallType.Write, data,
                    requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public BatchIndexingResponse PartialUpdateObjects<T>(IEnumerable<T> data, RequestOptions requestOptions = null,
            bool createIfNotExists = false)
            where T : class =>
            AsyncHelper.RunSync(() =>
                PartialUpdateObjectsAsync(data, requestOptions, createIfNotExists: createIfNotExists));

        /// <inheritdoc />
        public async Task<BatchIndexingResponse> PartialUpdateObjectsAsync<T>(IEnumerable<T> data,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool createIfNotExists = false) where T : class
        {
            string action = createIfNotExists
                ? BatchActionType.PartialUpdateObject
                : BatchActionType.PartialUpdateObjectNoCreate;

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return await SplitIntoBatchesAsync(data, action, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public BatchIndexingResponse SaveObject<T>(T data, RequestOptions requestOptions = null,
            bool autoGenerateObjectId = false) where T : class =>
            AsyncHelper.RunSync(() =>
                SaveObjectAsync(data, requestOptions, autoGenerateObjectId: autoGenerateObjectId));

        /// <inheritdoc />
        public async Task<BatchIndexingResponse> SaveObjectAsync<T>(T data, RequestOptions requestOptions = null,
            CancellationToken ct = default, bool autoGenerateObjectId = false) where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data is IEnumerable)
            {
                throw new ArgumentException($"{nameof(data)} should not be an IEnumerable/List/Collection");
            }

            return await SaveObjectsAsync(new List<T> { data }, requestOptions, ct, autoGenerateObjectId);
        }

        /// <inheritdoc />
        public BatchIndexingResponse SaveObjects<T>(IEnumerable<T> data, RequestOptions requestOptions = null,
            bool autoGenerateObjectId = false)
            where T : class =>
            AsyncHelper.RunSync(
                () => SaveObjectsAsync(data, requestOptions, autoGenerateObjectId: autoGenerateObjectId));

        /// <inheritdoc />
        public async Task<BatchIndexingResponse> SaveObjectsAsync<T>(IEnumerable<T> data,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool autoGenerateObjectId = false) where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (autoGenerateObjectId)
            {
                return await SplitIntoBatchesAsync(data, BatchActionType.AddObject, requestOptions, ct)
                    .ConfigureAwait(false);
            }

            AlgoliaHelper.EnsureObjectID<T>();

            return await SplitIntoBatchesAsync(data, BatchActionType.UpdateObject, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public MultiResponse ReplaceAllObjects<T>(IEnumerable<T> data, RequestOptions requestOptions = null,
            bool safe = false) where T : class =>
            AsyncHelper.RunSync(() => ReplaceAllObjectsAsync(data, requestOptions, safe: safe));

        /// <inheritdoc />
        public async Task<MultiResponse> ReplaceAllObjectsAsync<T>(IEnumerable<T> data,
            RequestOptions requestOptions = null,
            CancellationToken ct = default, bool safe = false) where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Random rnd = new Random();
            string tmpIndexName = $"{_indexName}_tmp_{rnd.Next(100)}";
            SearchIndex tmpIndex = new SearchIndex(_transport, Config, tmpIndexName);

            List<string> scopes = new List<string> { CopyScope.Rules, CopyScope.Settings, CopyScope.Synonyms };
            MultiResponse response = new MultiResponse { Responses = new List<IAlgoliaWaitableResponse>() };

            // Copy index ressources
            CopyToResponse copyResponse =
                await CopyToAsync(tmpIndexName, scope: scopes, requestOptions: requestOptions, ct: ct)
                    .ConfigureAwait(false);
            response.Responses.Add(copyResponse);

            if (safe)
            {
                copyResponse.Wait();
            }

            BatchIndexingResponse saveObjectsResponse =
                await tmpIndex.SaveObjectsAsync(data, requestOptions, ct).ConfigureAwait(false);
            response.Responses.Add(copyResponse);

            if (safe)
            {
                saveObjectsResponse.Wait();
            }

            // Move temporary index to source index
            MoveIndexResponse moveResponse =
                await MoveFromAsync(tmpIndexName, requestOptions, ct).ConfigureAwait(false);
            response.Responses.Add(copyResponse);

            if (safe)
            {
                moveResponse.Wait();
            }

            return response;
        }

        /// <inheritdoc />
        public BatchResponse Batch<T>(IEnumerable<BatchOperation<T>> operations, RequestOptions requestOptions = null)
            where T : class =>
            AsyncHelper.RunSync(() => BatchAsync(operations, requestOptions));

        /// <inheritdoc />
        public async Task<BatchResponse> BatchAsync<T>(IEnumerable<BatchOperation<T>> operations,
            RequestOptions requestOptions = null, CancellationToken ct = default) where T : class
        {
            if (operations == null)
            {
                throw new ArgumentNullException(nameof(operations));
            }

            var batch = new BatchRequest<T>(operations);

            return await BatchAsync(batch, requestOptions, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public BatchResponse Batch<T>(BatchRequest<T> request, RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => BatchAsync(request, requestOptions));

        /// <summary>
        /// Split records into smaller chunks before sending them to the API
        /// </summary>
        internal async Task<BatchIndexingResponse> SplitIntoBatchesAsync<T>(IEnumerable<T> data, string actionType,
            RequestOptions requestOptions = null, CancellationToken ct = default) where T : class
        {
            BatchIndexingResponse ret = new BatchIndexingResponse { Responses = new List<BatchResponse>() };
            List<T> records = new List<T>();

            foreach (var item in data)
            {
                if (records.Count == Config.BatchSize)
                {
                    var request = new BatchRequest<T>(actionType, records);
                    BatchResponse batch = await BatchAsync(request, requestOptions, ct).ConfigureAwait(false);
                    ret.Responses.Add(batch);
                    records.Clear();
                }

                records.Add(item);
            }

            if (records.Count > 0)
            {
                var request = new BatchRequest<T>(actionType, records);
                BatchResponse batch = await BatchAsync(request, requestOptions, ct).ConfigureAwait(false);
                ret.Responses.Add(batch);
            }

            return ret;
        }

        /// <inheritdoc />
        public async Task<BatchResponse> BatchAsync<T>(BatchRequest<T> request, RequestOptions requestOptions = null,
            CancellationToken ct = default) where T : class
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            BatchResponse response = await _transport.ExecuteRequestAsync<BatchResponse, BatchRequest<T>>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/batch", CallType.Write, request,
                    requestOptions,
                    ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public DeleteResponse DeleteObject(string objectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteObjectAsync(objectId, requestOptions));

        /// <inheritdoc />
        public async Task<DeleteResponse> DeleteObjectAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            DeleteResponse response = await _transport.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                    $"/1/indexes/{_urlEncodedIndexName}/{WebUtility.UrlEncode(objectId)}", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public BatchIndexingResponse
            DeleteObjects(IEnumerable<string> objectIds, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteObjectsAsync(objectIds, requestOptions));

        /// <inheritdoc />
        public async Task<BatchIndexingResponse> DeleteObjectsAsync(IEnumerable<string> objectIds,
            RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            if (objectIds == null || !objectIds.Any())
            {
                throw new ArgumentNullException(nameof(objectIds));
            }

            var request = objectIds.Select(x => new Dictionary<string, string> { { "objectID", x } });

            return await SplitIntoBatchesAsync(request, BatchActionType.DeleteObject, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public DeleteResponse Delete(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteAsync(requestOptions));

        /// <inheritdoc />
        public async Task<DeleteResponse> DeleteAsync(RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            return await _transport.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                    $"/1/indexes/{_urlEncodedIndexName}", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public DeleteResponse DeleteBy(Query query, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteByAsync(query, requestOptions));

        /// <inheritdoc />
        public async Task<DeleteResponse> DeleteByAsync(Query query, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            DeleteResponse response = await _transport.ExecuteRequestAsync<DeleteResponse, Query>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/deleteByQuery", CallType.Write, query,
                    requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public DeleteResponse ClearObjects(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ClearObjectsAsync(requestOptions));

        /// <inheritdoc />
        public async Task<DeleteResponse> ClearObjectsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            DeleteResponse response = await _transport.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/clear", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public SearchResponse<T> Search<T>(Query query, RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => SearchAsync<T>(query, requestOptions));

        /// <inheritdoc />
        public async Task<SearchResponse<T>> SearchAsync<T>(Query query, RequestOptions requestOptions = null,
            CancellationToken ct = default) where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _transport.ExecuteRequestAsync<SearchResponse<T>, Query>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/query", CallType.Read, query, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SearchForFacetResponse SearchForFacetValue(SearchForFacetRequest query,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SearchForFacetValueAsync(query, requestOptions));

        /// <inheritdoc />
        public async Task<SearchForFacetResponse> SearchForFacetValueAsync(SearchForFacetRequest query,
            RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (string.IsNullOrWhiteSpace(query.FacetName))
            {
                throw new ArgumentNullException(nameof(query.FacetName));
            }

            return await _transport.ExecuteRequestAsync<SearchForFacetResponse, SearchForFacetRequest>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/facets/{query.FacetName}/query", CallType.Read,
                    query, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public T GetObject<T>(string objectId, RequestOptions requestOptions = null,
            IEnumerable<string> attributesToRetrieve = null) where T : class =>
            AsyncHelper.RunSync(() =>
                GetObjectAsync<T>(objectId, requestOptions, attributesToRetrieve: attributesToRetrieve));

        /// <inheritdoc />
        public async Task<T> GetObjectAsync<T>(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default, IEnumerable<string> attributesToRetrieve = null)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            if (attributesToRetrieve != null && attributesToRetrieve.Any())
            {
                var dic = new Dictionary<string, string>
                {
                    {nameof(attributesToRetrieve), WebUtility.UrlEncode(string.Join(",", attributesToRetrieve))}
                };

                requestOptions = requestOptions.AddQueryParams(dic);
            }

            return await _transport.ExecuteRequestAsync<T>(HttpMethod.Get,
                    $"/1/indexes/{_urlEncodedIndexName}/{WebUtility.UrlEncode(objectId)}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public IEnumerable<T> GetObjects<T>(IEnumerable<string> objectIDs,
            RequestOptions requestOptions = null, IEnumerable<string> attributesToRetrieve = null) where T : class =>
            AsyncHelper.RunSync(() =>
                GetObjectsAsync<T>(objectIDs, requestOptions, attributesToRetrieve: attributesToRetrieve));

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetObjectsAsync<T>(
            IEnumerable<string> objectIDs, RequestOptions requestOptions = null,
            CancellationToken ct = default, IEnumerable<string> attributesToRetrieve = null)
            where T : class
        {
            if (objectIDs == null)
            {
                throw new ArgumentNullException(nameof(objectIDs));
            }

            List<MultipleGetObject> queries = new List<MultipleGetObject>();

            foreach (var objectId in objectIDs)
            {
                queries.Add(new MultipleGetObject
                {
                    IndexName = this._indexName,
                    ObjectID = objectId,
                    AttributesToRetrieve = attributesToRetrieve
                });
            }

            var request = new MultipleGetObjectsRequest { Requests = queries };

            var response = await _transport
                .ExecuteRequestAsync<MultipleGetObjectsResponse<T>, MultipleGetObjectsRequest>(HttpMethod.Post,
                    "/1/indexes/*/objects", CallType.Read, request, requestOptions, ct)
                .ConfigureAwait(false);

            return response.Results;
        }

        /// <inheritdoc />
        public IndexIterator<T> Browse<T>(BrowseIndexQuery query) where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return new IndexIterator<T>(this, query);
        }

        /// <inheritdoc />
        public BrowseIndexResponse<T> BrowseFrom<T>(BrowseIndexQuery query, RequestOptions requestOptions = null)
            where T : class =>
            AsyncHelper.RunSync(() => BrowseFromAsync<T>(query, requestOptions));

        /// <inheritdoc />
        public async Task<BrowseIndexResponse<T>> BrowseFromAsync<T>(BrowseIndexQuery query,
            RequestOptions requestOptions = null, CancellationToken ct = default) where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _transport.ExecuteRequestAsync<BrowseIndexResponse<T>, BrowseIndexQuery>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/browse", CallType.Read, query, requestOptions,
                    ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Rule GetRule(string objectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetRuleAsync(objectId, requestOptions));

        /// <inheritdoc />
        public async Task<Rule> GetRuleAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            return await _transport.ExecuteRequestAsync<Rule>(HttpMethod.Get,
                    $"/1/indexes/{_urlEncodedIndexName}/rules/{WebUtility.UrlEncode(objectId)}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public RulesIterator BrowseRules(RequestOptions requestOptions = null)
        {
            return new RulesIterator(this, requestOptions: requestOptions);
        }

        /// <inheritdoc />
        public SearchResponse<Rule> SearchRule(RuleQuery query = null, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SearchRuleAsync(query, requestOptions));

        /// <inheritdoc />
        public async Task<SearchResponse<Rule>> SearchRuleAsync(RuleQuery query = null,
            RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            return await _transport.ExecuteRequestAsync<SearchResponse<Rule>, RuleQuery>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/rules/search", CallType.Read, query, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SaveRuleResponse SaveRule(Rule rule, RequestOptions requestOptions = null,
            bool forwardToReplicas = false) =>
            AsyncHelper.RunSync(() => SaveRuleAsync(rule, requestOptions, forwardToReplicas: forwardToReplicas));

        /// <inheritdoc />
        public async Task<SaveRuleResponse> SaveRuleAsync(Rule rule, RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            var dic = new Dictionary<string, string>
            {
                {nameof(forwardToReplicas), forwardToReplicas.ToString().ToLower()}
            };

            requestOptions = requestOptions.AddQueryParams(dic);

            SaveRuleResponse response = await _transport.ExecuteRequestAsync<SaveRuleResponse, Rule>(
                    HttpMethod.Put, $"/1/indexes/{_urlEncodedIndexName}/rules/{WebUtility.UrlEncode(rule.ObjectID)}", CallType.Write, rule,
                    requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public BatchResponse SaveRules(IEnumerable<Rule> rules, RequestOptions requestOptions = null,
            bool forwardToReplicas = false,
            bool clearExistingRules = false) =>
            AsyncHelper.RunSync(() => SaveRulesAsync(rules, forwardToReplicas: forwardToReplicas,
                clearExistingRules: clearExistingRules, requestOptions: requestOptions));

        /// <inheritdoc />
        public async Task<BatchResponse> SaveRulesAsync(IEnumerable<Rule> rules, RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false,
            bool clearExistingRules = false)
        {
            if (rules == null)
            {
                throw new ArgumentNullException(nameof(rules));
            }

            var dic = new Dictionary<string, string>
            {
                {nameof(forwardToReplicas), forwardToReplicas.ToString().ToLower()},
                {nameof(clearExistingRules), clearExistingRules.ToString().ToLower()}
            };

            requestOptions = requestOptions.AddQueryParams(dic);

            BatchResponse response = await _transport.ExecuteRequestAsync<BatchResponse, IEnumerable<Rule>>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/rules/batch", CallType.Write, rules,
                    requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public BatchResponse ReplaceAllRules(IEnumerable<Rule> rules, RequestOptions requestOptions = null,
            bool forwardToReplicas = false)
        {
            return SaveRules(rules, forwardToReplicas: forwardToReplicas, clearExistingRules: true,
                requestOptions: requestOptions);
        }

        /// <inheritdoc />
        public async Task<BatchResponse> ReplaceAllRulesAsync(IEnumerable<Rule> rules,
            RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false)
        {
            return await SaveRulesAsync(rules, forwardToReplicas: forwardToReplicas, clearExistingRules: true,
                requestOptions: requestOptions, ct: ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public DeleteResponse DeleteRule(string objectId, RequestOptions requestOptions = null,
            bool forwardToReplicas = false) =>
            AsyncHelper.RunSync(() => DeleteRuleAsync(objectId, requestOptions, forwardToReplicas: forwardToReplicas));

        /// <inheritdoc />
        public async Task<DeleteResponse> DeleteRuleAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false)
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            var dic = new Dictionary<string, string>
            {
                {nameof(forwardToReplicas), forwardToReplicas.ToString().ToLower()}
            };

            requestOptions = requestOptions.AddQueryParams(dic);

            DeleteResponse response = await _transport.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                    $"/1/indexes/{_urlEncodedIndexName}/rules/{WebUtility.UrlEncode(objectId)}", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public DeleteResponse ClearRules(RequestOptions requestOptions = null, bool forwardToReplicas = false) =>
            AsyncHelper.RunSync(() => ClearRulesAsync(requestOptions, forwardToReplicas: forwardToReplicas));

        /// <inheritdoc />
        public async Task<DeleteResponse> ClearRulesAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false)
        {
            var dic = new Dictionary<string, string>
            {
                {nameof(forwardToReplicas), forwardToReplicas.ToString().ToLower()}
            };

            requestOptions = requestOptions.AddQueryParams(dic);

            DeleteResponse response = await _transport.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/rules/clear", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public IndexSettings GetSettings(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetSettingsAsync(requestOptions));

        /// <inheritdoc />
        public async Task<IndexSettings> GetSettingsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            var dic = new Dictionary<string, string>
            {
                {"getVersion","2"}
            };

            requestOptions = requestOptions.AddQueryParams(dic);

            return await _transport.ExecuteRequestAsync<IndexSettings>(HttpMethod.Get,
                    $"/1/indexes/{_urlEncodedIndexName}/settings", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SetSettingsResponse SetSettings(IndexSettings settings, RequestOptions requestOptions = null,
            bool forwardToReplicas = false) =>
            AsyncHelper.RunSync(() => SetSettingsAsync(settings, requestOptions, forwardToReplicas: forwardToReplicas));

        /// <inheritdoc />
        public async Task<SetSettingsResponse> SetSettingsAsync(IndexSettings settings,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool forwardToReplicas = false)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var dic = new Dictionary<string, string>
            {
                {nameof(forwardToReplicas), forwardToReplicas.ToString().ToLower()}
            };

            requestOptions = requestOptions.AddQueryParams(dic);

            SetSettingsResponse response = await _transport
                .ExecuteRequestAsync<SetSettingsResponse, IndexSettings>(HttpMethod.Put,
                    $"/1/indexes/{_urlEncodedIndexName}/settings", CallType.Write, settings, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public SearchResponse<Synonym> SearchSynonyms(SynonymQuery query, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SearchSynonymsAsync(query, requestOptions));

        /// <inheritdoc />
        public async Task<SearchResponse<Synonym>> SearchSynonymsAsync(SynonymQuery query,
            RequestOptions requestOptions = null, CancellationToken ct = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _transport.ExecuteRequestAsync<SearchResponse<Synonym>, SynonymQuery>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/synonyms/search", CallType.Read, query, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Synonym GetSynonym(string synonymObjectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetSynonymAsync(synonymObjectId, requestOptions));

        /// <inheritdoc />
        public async Task<Synonym> GetSynonymAsync(string synonymObjectId, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            if (synonymObjectId == null)
            {
                throw new ArgumentNullException(nameof(synonymObjectId));
            }

            return await _transport.ExecuteRequestAsync<Synonym>(HttpMethod.Get,
                    $"/1/indexes/{_urlEncodedIndexName}/synonyms/{WebUtility.UrlEncode(synonymObjectId)}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SynonymsIterator BrowseSynonyms(RequestOptions requestOptions = null)
        {
            return new SynonymsIterator(this, requestOptions: requestOptions);
        }

        /// <inheritdoc />
        public SaveSynonymResponse SaveSynonyms(IEnumerable<Synonym> synonyms, RequestOptions requestOptions = null,
            bool forwardToReplicas = false,
            bool replaceExistingSynonyms = false) =>
            AsyncHelper.RunSync(() =>
                SaveSynonymsAsync(synonyms, requestOptions, forwardToReplicas: forwardToReplicas,
                    replaceExistingSynonyms: replaceExistingSynonyms));

        /// <inheritdoc />
        public async Task<SaveSynonymResponse> SaveSynonymsAsync(IEnumerable<Synonym> synonyms,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool forwardToReplicas = false, bool replaceExistingSynonyms = false)
        {
            if (synonyms == null)
            {
                throw new ArgumentNullException(nameof(synonyms));
            }

            var dic = new Dictionary<string, string>
            {
                {nameof(forwardToReplicas), forwardToReplicas.ToString().ToLower()},
                {nameof(replaceExistingSynonyms), replaceExistingSynonyms.ToString().ToLower()}
            };

            requestOptions = requestOptions.AddQueryParams(dic);

            SaveSynonymResponse response = await _transport
                .ExecuteRequestAsync<SaveSynonymResponse, IEnumerable<Synonym>>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/synonyms/batch", CallType.Write, synonyms, requestOptions,
                    ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public SaveSynonymResponse ReplaceAllSynonyms(IEnumerable<Synonym> synonyms,
            RequestOptions requestOptions = null,
            bool forwardToReplicas = false)
        {
            return SaveSynonyms(synonyms, forwardToReplicas: forwardToReplicas, replaceExistingSynonyms: true,
                requestOptions: requestOptions);
        }

        /// <inheritdoc />
        public async Task<SaveSynonymResponse> ReplaceAllSynonymsAsync(IEnumerable<Synonym> synonyms,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool forwardToReplicas = false)
        {
            return await SaveSynonymsAsync(synonyms, forwardToReplicas: forwardToReplicas,
                replaceExistingSynonyms: true, requestOptions: requestOptions, ct: ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SaveSynonymResponse SaveSynonym(Synonym synonym, RequestOptions requestOptions = null,
            bool forwardToReplicas = false) =>
            AsyncHelper.RunSync(() => SaveSynonymAsync(synonym, requestOptions, forwardToReplicas: forwardToReplicas));

        /// <inheritdoc />
        public async Task<SaveSynonymResponse> SaveSynonymAsync(Synonym synonym, RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false)
        {
            if (synonym == null)
            {
                throw new ArgumentNullException(nameof(synonym));
            }

            if (string.IsNullOrWhiteSpace(synonym.ObjectID))
            {
                throw new ArgumentNullException(nameof(synonym.ObjectID));
            }

            var dic = new Dictionary<string, string>
            {
                {nameof(forwardToReplicas), forwardToReplicas.ToString().ToLower()}
            };

            requestOptions = requestOptions.AddQueryParams(dic);

            SaveSynonymResponse response = await _transport.ExecuteRequestAsync<SaveSynonymResponse, Synonym>(
                    HttpMethod.Put, $"/1/indexes/{_urlEncodedIndexName}/synonyms/{WebUtility.UrlEncode(synonym.ObjectID)}", CallType.Write,
                    synonym, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public DeleteResponse DeleteSynonym(string synonymObjectId, RequestOptions requestOptions = null,
            bool forwardToReplicas = false) =>
            AsyncHelper.RunSync(() =>
                DeleteSynonymAsync(synonymObjectId, requestOptions, forwardToReplicas: forwardToReplicas));

        /// <inheritdoc />
        public async Task<DeleteResponse> DeleteSynonymAsync(string synonymObjectId,
            RequestOptions requestOptions = null, CancellationToken ct = default,
            bool forwardToReplicas = false)
        {
            if (synonymObjectId == null)
            {
                throw new ArgumentNullException(nameof(synonymObjectId));
            }

            var dic = new Dictionary<string, string>
            {
                {nameof(forwardToReplicas), forwardToReplicas.ToString().ToLower()}
            };

            requestOptions = requestOptions.AddQueryParams(dic);

            DeleteResponse response = await _transport.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                    $"/1/indexes/{_urlEncodedIndexName}/synonyms/{WebUtility.UrlEncode(synonymObjectId)}", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public ClearSynonymsResponse
            ClearSynonyms(RequestOptions requestOptions = null, bool forwardToReplicas = false) =>
            AsyncHelper.RunSync(() => ClearSynonymsAsync(requestOptions, forwardToReplicas: forwardToReplicas));

        /// <inheritdoc />
        public async Task<ClearSynonymsResponse> ClearSynonymsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default, bool forwardToReplicas = false)
        {
            var dic = new Dictionary<string, string>
            {
                {nameof(forwardToReplicas), forwardToReplicas.ToString().ToLower()}
            };

            requestOptions = requestOptions.AddQueryParams(dic);

            ClearSynonymsResponse response = await _transport.ExecuteRequestAsync<ClearSynonymsResponse>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/synonyms/clear", CallType.Write,
                    requestOptions,
                    ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <summary>
        /// Make a copy of an index, including its objects, settings, synonyms, and query rules.
        /// </summary>
        /// <param name="destinationIndex">The destination index</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="scope">The scope copy</param>
        /// <returns></returns>
        internal CopyToResponse CopyTo(string destinationIndex, RequestOptions requestOptions = null,
            IEnumerable<string> scope = null) =>
            AsyncHelper.RunSync(() => CopyToAsync(destinationIndex, requestOptions: requestOptions, scope: scope));

        /// <summary>
        /// Make a copy of an index, including its objects, settings, synonyms, and query rules.
        /// </summary>
        /// <param name="destinationIndex">The destination index</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <param name="scope">The scope copy</param>
        /// <returns></returns>
        internal async Task<CopyToResponse> CopyToAsync(string destinationIndex, RequestOptions requestOptions = null,
            CancellationToken ct = default, IEnumerable<string> scope = null)
        {
            if (string.IsNullOrWhiteSpace(destinationIndex))
            {
                throw new ArgumentNullException(destinationIndex);
            }

            var data = new CopyToRequest { Operation = MoveType.Copy, IndexNameDest = destinationIndex, Scope = scope };

            CopyToResponse response = await _transport.ExecuteRequestAsync<CopyToResponse, CopyToRequest>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/operation", CallType.Write, data,
                    requestOptions,
                    ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public MoveIndexResponse MoveFrom(string sourceIndex, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => MoveFromAsync(sourceIndex, requestOptions));

        /// <inheritdoc />
        public async Task<MoveIndexResponse> MoveFromAsync(string sourceIndex, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(sourceIndex))
            {
                throw new ArgumentNullException(sourceIndex);
            }

            MoveIndexRequest request = new MoveIndexRequest { Operation = MoveType.Move, Destination = _indexName };

            MoveIndexResponse response = await _transport
                .ExecuteRequestAsync<MoveIndexResponse, MoveIndexRequest>(HttpMethod.Post,
                    $"/1/indexes/{sourceIndex}/operation", CallType.Write, request, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitTask = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public void WaitTask(long taskId, int timeToWait = 100, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => WaitTaskAsync(taskId, timeToWait, requestOptions));

        /// <inheritdoc />
        public async Task WaitTaskAsync(long taskId, int timeToWait = 100, RequestOptions requestOptions = null,
            CancellationToken ct = default)
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

                if (timeToWait > Defaults.MaxTimeToWait)
                {
                    timeToWait = Defaults.MaxTimeToWait;
                }
            }
        }

        /// <inheritdoc />
        public TaskStatusResponse GetTask(long taskId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetTaskAsync(taskId, requestOptions));

        /// <inheritdoc />
        public async Task<TaskStatusResponse> GetTaskAsync(long taskId, RequestOptions requestOptions = null,
            CancellationToken ct = default)
        {
            return await _transport.ExecuteRequestAsync<TaskStatusResponse>(HttpMethod.Get,
                    $"/1/indexes/{_urlEncodedIndexName}/task/{taskId}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }
    }
}

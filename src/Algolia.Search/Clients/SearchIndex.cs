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

        /// <inheritdoc />
        public SearchIndex(IRequesterWrapper requesterWrapper, AlgoliaConfig config, string indexName)
        {
            _requesterWrapper = requesterWrapper ?? throw new ArgumentNullException(nameof(requesterWrapper));
            _indexName = !string.IsNullOrWhiteSpace(indexName)
                ? indexName
                : throw new ArgumentNullException(nameof(indexName));
            _urlEncodedIndexName = WebUtility.UrlEncode(indexName);
            Config = config;
        }

        /// <inheritdoc />
        public AddObjectResponse AddObject<T>(T data, RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => AddObjectAysnc(data, requestOptions));

        /// <inheritdoc />
        public async Task<AddObjectResponse> AddObjectAysnc<T>(T data, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (ReferenceEquals(data, null))
            {
                throw new ArgumentNullException(nameof(data));
            }

            AddObjectResponse response = await _requesterWrapper.ExecuteRequestAsync<AddObjectResponse, T>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}", CallType.Write, data, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public UpdateObjectResponse PartialUpdateObject<T>(T data, RequestOptions requestOptions = null)
            where T : class =>
            AsyncHelper.RunSync(() => PartialUpdateObjectAsync(data, requestOptions));

        /// <inheritdoc />
        public async Task<UpdateObjectResponse> PartialUpdateObjectAsync<T>(T data,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (ReferenceEquals(data, null))
            {
                throw new ArgumentNullException(nameof(data));
            }

            // Get && check if the generic type has an objectID
            string objectId = AlgoliaHelper.GetObjectID(data);

            UpdateObjectResponse response = await _requesterWrapper.ExecuteRequestAsync<UpdateObjectResponse, T>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/{objectId}/partial", CallType.Write, data,
                    requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public BatchIndexingResponse PartialUpdateObjects<T>(IEnumerable<T> datas, RequestOptions requestOptions = null)
            where T : class =>
            AsyncHelper.RunSync(() => PartialUpdateObjectsAsync(datas, requestOptions));

        /// <inheritdoc />
        public async Task<BatchIndexingResponse> PartialUpdateObjectsAsync<T>(IEnumerable<T> datas,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (datas == null)
            {
                throw new ArgumentNullException(nameof(datas));
            }

            return await SplitIntoBatchesAsync(datas, BatchActionType.PartialUpdateObjectNoCreate, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public BatchIndexingResponse AddObjects<T>(IEnumerable<T> datas, RequestOptions requestOptions = null)
            where T : class =>
            AsyncHelper.RunSync(() => AddObjectsAysnc(datas, requestOptions));

        /// <inheritdoc />
        public async Task<BatchIndexingResponse> AddObjectsAysnc<T>(IEnumerable<T> datas,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (datas == null)
            {
                throw new ArgumentNullException(nameof(datas));
            }

            return await SplitIntoBatchesAsync(datas, BatchActionType.AddObject, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public BatchIndexingResponse SaveObject<T>(T data, RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => SaveObjectAsync(data, requestOptions));

        /// <inheritdoc />
        public async Task<BatchIndexingResponse> SaveObjectAsync<T>(T data, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class
        {
            return await SaveObjectsAsync(new List<T> {data}, requestOptions, ct);
        }

        /// <inheritdoc />
        public BatchIndexingResponse SaveObjects<T>(IEnumerable<T> datas, RequestOptions requestOptions = null)
            where T : class =>
            AsyncHelper.RunSync(() => SaveObjectsAsync(datas, requestOptions));

        /// <inheritdoc />
        public async Task<BatchIndexingResponse> SaveObjectsAsync<T>(IEnumerable<T> datas,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken),
            bool autoGenerateObjectId = false) where T : class
        {
            if (datas == null)
            {
                throw new ArgumentNullException(nameof(datas));
            }

            if (autoGenerateObjectId)
            {
                return await AddObjectsAysnc(datas, requestOptions, ct).ConfigureAwait(false);
            }

            AlgoliaHelper.EnsureObjectID(datas);

            return await SplitIntoBatchesAsync(datas, BatchActionType.UpdateObject, requestOptions, ct);
        }

        /// <inheritdoc />
        public MultiResponse ReplaceAllObjects<T>(IEnumerable<T> datas, bool safe = false,
            RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => ReplaceAllObjectsAsync(datas, safe, requestOptions));

        /// <inheritdoc />
        public async Task<MultiResponse> ReplaceAllObjectsAsync<T>(IEnumerable<T> datas, bool safe = false,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken)) where T : class
        {
            Random rnd = new Random();
            string tmpIndexName = $"{_indexName}_tmp_{rnd.Next(100)}";
            SearchIndex tmpIndex = new SearchIndex(_requesterWrapper, Config, tmpIndexName);

            List<string> scopes = new List<string> {CopyScope.Rules, CopyScope.Settings, CopyScope.Synonyms};
            MultiResponse response = new MultiResponse {Responses = new List<IAlgoliaWaitableResponse>()};

            // Copy index ressources
            CopyToResponse copyResponse =
                await CopyToAsync(tmpIndexName, scopes, requestOptions, ct).ConfigureAwait(false);
            response.Responses.Add(copyResponse);

            if (safe)
            {
                copyResponse.Wait();
            }

            BatchIndexingResponse saveObjectsResponse =
                await tmpIndex.AddObjectsAysnc(datas, requestOptions, ct).ConfigureAwait(false);
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
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken)) where T : class
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

        /// <inheritdoc />
        internal async Task<BatchIndexingResponse> SplitIntoBatchesAsync<T>(IEnumerable<T> datas, string actionType,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken)) where T : class
        {
            BatchIndexingResponse ret = new BatchIndexingResponse {Responses = new List<BatchResponse>()};
            List<T> records = new List<T>();

            foreach (var data in datas)
            {
                if (records.Count == Config.BatchSize)
                {
                    var request = new BatchRequest<T>(BatchActionType.AddObject, records);
                    BatchResponse batch = await BatchAsync(request, requestOptions, ct).ConfigureAwait(false);
                    ret.Responses.Add(batch);
                    records.Clear();
                }

                records.Add(data);
            }

            if (records.Count > 0)
            {
                var request = new BatchRequest<T>(BatchActionType.AddObject, records);
                BatchResponse batch = await BatchAsync(request, requestOptions, ct).ConfigureAwait(false);
                ret.Responses.Add(batch);
            }

            return ret;
        }

        /// <inheritdoc />
        public async Task<BatchResponse> BatchAsync<T>(BatchRequest<T> request, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            BatchResponse response = await _requesterWrapper.ExecuteRequestAsync<BatchResponse, BatchRequest<T>>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/batch", CallType.Write, request,
                    requestOptions,
                    ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public DeleteResponse DeleteObject(string objectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteObjectAsync(objectId, requestOptions));

        /// <inheritdoc />
        public async Task<DeleteResponse> DeleteObjectAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Get,
                    $"/1/indexes/{_urlEncodedIndexName}/{objectId}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public BatchIndexingResponse
            DeleteObjects(IEnumerable<string> objectIds, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteObjectsAsync(objectIds, requestOptions));

        /// <inheritdoc />
        public async Task<BatchIndexingResponse> DeleteObjectsAsync(IEnumerable<string> objectIds,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (objectIds == null || !objectIds.Any())
            {
                throw new ArgumentNullException(nameof(objectIds));
            }

            return await SplitIntoBatchesAsync(objectIds, BatchActionType.DeleteObject, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public DeleteResponse DeleteBy(SearchQuery query, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteByAsync(query, requestOptions));

        /// <inheritdoc />
        public async Task<DeleteResponse> DeleteByAsync(SearchQuery query, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse, SearchQuery>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/deleteByQuery", CallType.Write, query,
                    requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public DeleteResponse ClearObjects(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ClearObjectsAsync(requestOptions));

        /// <inheritdoc />
        public async Task<DeleteResponse> ClearObjectsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/clear", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public SearchResponse<T> Search<T>(SearchQuery query, RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => SearchAsync<T>(query, requestOptions));

        /// <inheritdoc />
        public async Task<SearchResponse<T>> SearchAsync<T>(SearchQuery query, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _requesterWrapper.ExecuteRequestAsync<SearchResponse<T>, SearchQuery>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/query", CallType.Read, query, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SearchForFacetResponse SearchForFacetValue(SearchForFacetRequest query,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SearchForFacetValueAsync(query, requestOptions));

        /// <inheritdoc />
        public async Task<SearchForFacetResponse> SearchForFacetValueAsync(SearchForFacetRequest query,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (string.IsNullOrWhiteSpace(query.FacetName))
            {
                throw new ArgumentNullException(nameof(query.FacetName));
            }

            return await _requesterWrapper.ExecuteRequestAsync<SearchForFacetResponse, SearchForFacetRequest>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/facets/{query.FacetName}/query", CallType.Read,
                    query, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public T GetObject<T>(string objectId, RequestOptions requestOptions = null) where T : class =>
            AsyncHelper.RunSync(() => GetObjectAsync<T>(objectId, requestOptions));

        /// <inheritdoc />
        public async Task<T> GetObjectAsync<T>(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            return await _requesterWrapper.ExecuteRequestAsync<T>(HttpMethod.Get,
                    $"/1/indexes/{_urlEncodedIndexName}/{objectId}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
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
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken)) where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _requesterWrapper.ExecuteRequestAsync<BrowseIndexResponse<T>, BrowseIndexQuery>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/browse", CallType.Read, query, requestOptions,
                    ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Rule GetRule(string objectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetRuleAsync(objectId, requestOptions));

        /// <inheritdoc />
        public async Task<Rule> GetRuleAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            return await _requesterWrapper.ExecuteRequestAsync<Rule>(HttpMethod.Get,
                    $"/1/indexes/{_urlEncodedIndexName}/rules/{objectId}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SearchResponse<Rule> SearchRule(RuleQuery query = null, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SearchRuleAsync(query, requestOptions));

        /// <inheritdoc />
        public async Task<SearchResponse<Rule>> SearchRuleAsync(RuleQuery query = null,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<SearchResponse<Rule>, RuleQuery>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/rules/search", CallType.Read, query, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SaveRuleResponse SaveRule(Rule rule, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SaveRuleAsync(rule, requestOptions));

        /// <inheritdoc />
        public async Task<SaveRuleResponse> SaveRuleAsync(Rule rule, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            SaveRuleResponse response = await _requesterWrapper.ExecuteRequestAsync<SaveRuleResponse, Rule>(
                    HttpMethod.Put, $"/1/indexes/{_urlEncodedIndexName}/rules/{rule.ObjectID}", CallType.Write, rule,
                    requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public BatchResponse SaveRules(IEnumerable<Rule> rules, bool forwardToReplicas = false,
            bool clearExistingRules = false, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SaveRulesAsync(rules, forwardToReplicas, clearExistingRules, requestOptions));

        /// <inheritdoc />
        public async Task<BatchResponse> SaveRulesAsync(IEnumerable<Rule> rules, bool forwardToReplicas = false,
            bool clearExistingRules = false, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
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

            BatchResponse response = await _requesterWrapper.ExecuteRequestAsync<BatchResponse, IEnumerable<Rule>>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/rules/batch", CallType.Write, rules,
                    requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public BatchResponse ReplaceAllRules(IEnumerable<Rule> rules, bool forwardToReplicas = false,
            RequestOptions requestOptions = null)
        {
            return SaveRules(rules, forwardToReplicas, true, requestOptions);
        }

        /// <inheritdoc />
        public async Task<BatchResponse> ReplaceAllRulesAsync(IEnumerable<Rule> rules, bool forwardToReplicas = false,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            return await SaveRulesAsync(rules, forwardToReplicas, true, requestOptions, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public DeleteResponse DeleteRule(string objectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteRuleAsync(objectId, requestOptions));

        /// <inheritdoc />
        public async Task<DeleteResponse> DeleteRuleAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                    $"/1/indexes/{_urlEncodedIndexName}/rules/{objectId}", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public DeleteResponse ClearRules(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ClearRulesAsync(requestOptions));

        /// <inheritdoc />
        public async Task<DeleteResponse> ClearRulesAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/rules/clear", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public IndexSettings GetSettings(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetSettingsAsync(requestOptions));

        /// <inheritdoc />
        public async Task<IndexSettings> GetSettingsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<IndexSettings>(HttpMethod.Get,
                    $"/1/indexes/{_urlEncodedIndexName}/settings", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SetSettingsResponse SetSettings(IndexSettings settings, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SetSettingsAsync(settings, requestOptions));

        /// <inheritdoc />
        public async Task<SetSettingsResponse> SetSettingsAsync(IndexSettings settings,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            SetSettingsResponse response = await _requesterWrapper
                .ExecuteRequestAsync<SetSettingsResponse, IndexSettings>(HttpMethod.Put,
                    $"/1/indexes/{_urlEncodedIndexName}/settings", CallType.Write, settings, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public SearchResponse<Synonym> SearchSynonyms(SynonymQuery query, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SearchSynonymsAsync(query, requestOptions));

        /// <inheritdoc />
        public async Task<SearchResponse<Synonym>> SearchSynonymsAsync(SynonymQuery query,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _requesterWrapper.ExecuteRequestAsync<SearchResponse<Synonym>, SynonymQuery>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/synonyms/search", CallType.Read, query, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Synonym GetSynonym(string synonymObjectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetSynonymAsync(synonymObjectId, requestOptions));

        /// <inheritdoc />
        public async Task<Synonym> GetSynonymAsync(string synonymObjectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (synonymObjectId == null)
            {
                throw new ArgumentNullException(nameof(synonymObjectId));
            }

            return await _requesterWrapper.ExecuteRequestAsync<Synonym>(HttpMethod.Get,
                    $"/1/indexes/{_urlEncodedIndexName}/synonyms/{synonymObjectId}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SaveSynonymResponse SaveSynonyms(IEnumerable<Synonym> synonyms, bool forwardToReplicas = false,
            bool replaceExistingSynonyms = false, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() =>
                SaveSynonymsAsync(synonyms, forwardToReplicas, replaceExistingSynonyms, requestOptions));

        /// <inheritdoc />
        public async Task<SaveSynonymResponse> SaveSynonymsAsync(IEnumerable<Synonym> synonyms,
            bool forwardToReplicas = false, bool replaceExistingSynonyms = false, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
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

            SaveSynonymResponse response = await _requesterWrapper
                .ExecuteRequestAsync<SaveSynonymResponse, IEnumerable<Synonym>>(HttpMethod.Post,
                    $"/1/indexes/{_urlEncodedIndexName}/synonyms/batch", CallType.Write, synonyms, requestOptions,
                    ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public SaveSynonymResponse ReplaceAllSynonyms(IEnumerable<Synonym> synonyms, bool forwardToReplicas = false,
            RequestOptions requestOptions = null)
        {
            return SaveSynonyms(synonyms, forwardToReplicas, true, requestOptions);
        }

        /// <inheritdoc />
        public async Task<SaveSynonymResponse> ReplaceAllSynonymsAsync(IEnumerable<Synonym> synonyms,
            bool forwardToReplicas = false, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await SaveSynonymsAsync(synonyms, forwardToReplicas, true, requestOptions, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public SaveSynonymResponse SaveSynonym(Synonym synonym, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => SaveSynonymAsync(synonym, requestOptions));

        /// <inheritdoc />
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

            SaveSynonymResponse response = await _requesterWrapper.ExecuteRequestAsync<SaveSynonymResponse, Synonym>(
                    HttpMethod.Put, $"/1/indexes/{_urlEncodedIndexName}/synonyms/{synonym.ObjectID}", CallType.Write,
                    synonym, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public DeleteResponse DeleteSynonym(string synonymObjectId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteSynonymAsync(synonymObjectId, requestOptions));

        /// <inheritdoc />
        public async Task<DeleteResponse> DeleteSynonymAsync(string synonymObjectId,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (synonymObjectId == null)
            {
                throw new ArgumentNullException(nameof(synonymObjectId));
            }

            DeleteResponse response = await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete,
                    $"/1/indexes/{_urlEncodedIndexName}/synonyms/{synonymObjectId}", CallType.Write, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public ClearSynonymsResponse ClearSynonyms(RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => ClearSynonymsAsync(requestOptions));

        /// <inheritdoc />
        public async Task<ClearSynonymsResponse> ClearSynonymsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            ClearSynonymsResponse response = await _requesterWrapper.ExecuteRequestAsync<ClearSynonymsResponse>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/synonyms/clear", CallType.Write,
                    requestOptions,
                    ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public CopyToResponse CopyTo(string destinationIndex, IEnumerable<string> scope = null,
            RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => CopyToAsync(destinationIndex, scope, requestOptions));

        /// <inheritdoc />
        public async Task<CopyToResponse> CopyToAsync(string destinationIndex, IEnumerable<string> scope = null,
            RequestOptions requestOptions = null, CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(destinationIndex))
            {
                throw new ArgumentNullException(destinationIndex);
            }

            var data = new CopyToRequest {Operation = MoveType.Copy, IndexNameDest = destinationIndex, Scope = scope};

            CopyToResponse response = await _requesterWrapper.ExecuteRequestAsync<CopyToResponse, CopyToRequest>(
                    HttpMethod.Post, $"/1/indexes/{_urlEncodedIndexName}/operation", CallType.Write, data,
                    requestOptions,
                    ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public MoveIndexResponse MoveFrom(string sourceIndex, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => MoveFromAsync(sourceIndex, requestOptions));

        /// <inheritdoc />
        public async Task<MoveIndexResponse> MoveFromAsync(string sourceIndex, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(sourceIndex))
            {
                throw new ArgumentNullException(sourceIndex);
            }

            MoveIndexRequest request = new MoveIndexRequest {Operation = MoveType.Move, Destination = _indexName};

            MoveIndexResponse response = await _requesterWrapper
                .ExecuteRequestAsync<MoveIndexResponse, MoveIndexRequest>(HttpMethod.Post,
                    $"/1/indexes/{sourceIndex}/operation", CallType.Write, request, requestOptions, ct)
                .ConfigureAwait(false);

            response.WaitDelegate = t => WaitTask(t);
            return response;
        }

        /// <inheritdoc />
        public void WaitTask(long taskId, int timeToWait = 100, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => WaitTaskAsync(taskId, timeToWait, requestOptions));

        /// <inheritdoc />
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

        /// <inheritdoc />
        public TaskStatusResponse GetTask(long taskId, RequestOptions requestOptions = null) =>
            AsyncHelper.RunSync(() => GetTaskAsync(taskId, requestOptions));

        /// <inheritdoc />
        public async Task<TaskStatusResponse> GetTaskAsync(long taskId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<TaskStatusResponse>(HttpMethod.Get,
                    $"/1/indexes/{_urlEncodedIndexName}/task/{taskId}", CallType.Read, requestOptions, ct)
                .ConfigureAwait(false);
        }
    }
}
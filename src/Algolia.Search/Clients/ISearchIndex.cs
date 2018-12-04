﻿/*
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
using Algolia.Search.Models.Query;
using Algolia.Search.Models.Requests;
using Algolia.Search.Models.Responses;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Settings;
using Algolia.Search.Models.Synonyms;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    public interface ISearchIndex
    {
        AlgoliaConfig Config { get; }

        /// <summary>
        /// Add an object to the given index
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        AddObjectResponse AddObject<T>(T data, RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// Add an object to the given index
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<AddObjectResponse> AddObjectAysnc<T>(T data, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Update one or more attributes of an existing object.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        UpdateObjectResponse PartialUpdateObject<T>(T data, RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// Update one or more attributes of an existing object.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<UpdateObjectResponse> PartialUpdateObjectAsync<T>(T data, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Add objects to the given index
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        BatchIndexingResponse AddObjects<T>(IEnumerable<T> datas, RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// Add objects to the given index
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<BatchIndexingResponse> AddObjectsAysnc<T>(IEnumerable<T> datas, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Remove objects from an index using its object id.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        DeleteResponse DeleteObject(string objectId, RequestOptions requestOptions = null);

        /// <summary>
        /// Remove objects from an index using its object id.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteObjectAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Delete all the objects for their objectIds
        /// </summary>
        /// <param name="objectIds"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        BatchIndexingResponse DeleteObjects(IEnumerable<string> objectIds, RequestOptions requestOptions = null);

        /// <summary>
        /// Delete all the objects for their objectIds
        /// </summary>
        /// <param name="objectIds"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<BatchIndexingResponse> DeleteObjectsAsync(IEnumerable<string> objectIds,
            RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Search in the index for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SearchResponse<T> Search<T>(SearchQuery query, RequestOptions requestOptions = null) where T : class;

        /// <summary>
        ///  Search in the index for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SearchResponse<T>> SearchAsync<T>(SearchQuery query, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Get object for the specified ID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        T GetObject<T>(string objectId, RequestOptions requestOptions = null) where T : class;

        /// <summary>
        /// Get the specified object by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<T> GetObjectAsync<T>(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// This method allows you to retrieve all index content  
        /// It can retrieve up to 1,000 records per call and supports full text search and filters. 
        /// You can use the same query parameters as for a search query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IndexIterator<T> Browse<T>(BrowseIndexQuery query) where T : class;

        /// <summary>
        /// This method allows you to retrieve all index content  
        /// It can retrieve up to 1,000 records per call and supports full text search and filters. 
        /// You can use the same query parameters as for a search query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        BrowseIndexResponse<T> BrowseFrom<T>(BrowseIndexQuery query, RequestOptions requestOptions = null)
            where T : class;

        /// <summary>
        /// This method allows you to retrieve all index content  
        /// It can retrieve up to 1,000 records per call and supports full text search and filters. 
        /// You can use the same query parameters as for a search query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<BrowseIndexResponse<T>> BrowseFromAsync<T>(BrowseIndexQuery query, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Get the specified by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        Rule GetRule(string objectId, RequestOptions requestOptions = null);

        /// <summary>
        /// Get the specified rule by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Rule> GetRuleAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Search rules sync
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SearchResponse<Rule> SearchRule(RuleQuery query = null, RequestOptions requestOptions = null);

        /// <summary>
        /// Search query 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SearchResponse<Rule>> SearchRuleAsync(RuleQuery query = null, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        ///  Save rule 
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SaveRuleResponse SaveRule(Rule rule, RequestOptions requestOptions = null);

        /// <summary>
        /// Save the given rule
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SaveRuleResponse> SaveRuleAsync(Rule rule, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));


        /// <summary>
        /// Batch rules
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="clearExistingRules"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        BatchResponse SaveRules(IEnumerable<Rule> rules, bool forwardToReplicas = false,
            bool clearExistingRules = false, RequestOptions requestOptions = null);

        /// <summary>
        /// Batch rules
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="clearExistingRules"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<BatchResponse> SaveRulesAsync(IEnumerable<Rule> rules, bool forwardToReplicas = false,
            bool clearExistingRules = false, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Delete the rule for the given ruleId
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        DeleteResponse DeleteRule(string objectId, RequestOptions requestOptions = null);

        /// <summary>
        /// Delete the rule for the given ruleId
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteRuleAsync(string objectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get settings for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        IndexSettings GetSettings(RequestOptions requestOptions = null);

        /// <summary>
        /// Get settings for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IndexSettings> GetSettingsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Set settings
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SetSettingsResponse SetSettings(IndexSettings settings, RequestOptions requestOptions = null);

        /// <summary>
        /// Set settings for the given index
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SetSettingsResponse> SetSettingsAsync(IndexSettings settings, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get all synonyms that match a query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SearchResponse<Synonym> SearchSynonyms(SynonymQuery query, RequestOptions requestOptions = null);

        /// <summary>
        /// Get all synonyms that match a query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SearchResponse<Synonym>> SearchSynonymsAsync(SynonymQuery query, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get a single synonym using its object id.
        /// </summary>
        /// <param name="synonymObjectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        Synonym GetSynonym(string synonymObjectId, RequestOptions requestOptions = null);

        /// <summary>
        /// Get a single synonym using its object id.
        /// </summary>
        /// <param name="synonymObjectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Synonym> GetSynonymAsync(string synonymObjectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Create or update multiple synonyms.
        /// </summary>
        /// <param name="synonyms"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="replaceExistingSynonyms"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SaveSynonymResponse SaveSynonyms(IEnumerable<Synonym> synonyms, bool forwardToReplicas = false,
            bool replaceExistingSynonyms = false, RequestOptions requestOptions = null);

        /// <summary>
        /// Create or update multiple synonyms.
        /// </summary>
        /// <param name="synonyms"></param>
        /// <param name="forwardToReplicas"></param>
        /// <param name="replaceExistingSynonyms"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SaveSynonymResponse> SaveSynonymsAsync(IEnumerable<Synonym> synonyms, bool forwardToReplicas = false,
            bool replaceExistingSynonyms = false, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Create or update a single synonym on an index.
        /// </summary>
        /// <param name="synonym"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SaveSynonymResponse SaveSynonym(Synonym synonym, RequestOptions requestOptions = null);

        /// <summary>
        /// Create or update a single synonym on an index.
        /// </summary>
        /// <param name="synonym"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SaveSynonymResponse> SaveSynonymAsync(Synonym synonym, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Remove a single synonym from an index using its object id.
        /// </summary>
        /// <param name="synonymObjectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        DeleteResponse DeleteSynonym(string synonymObjectId, RequestOptions requestOptions = null);

        /// <summary>
        /// Remove a single synonym from an index using its object id.
        /// </summary>
        /// <param name="synonymObjectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteSynonymAsync(string synonymObjectId, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Remove all synonyms from an index.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        ClearSynonymsResponse ClearSynonyms(RequestOptions requestOptions = null);

        /// <summary>
        /// Remove all synonyms from an index.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ClearSynonymsResponse> ClearSynonymsAsync(RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        MoveIndexResponse MoveFrom(string destinationIndex, RequestOptions requestOptions = null);

        /// <summary>
        /// Rename an index. Normally used to reindex your data atomically, without any down time.
        /// </summary>
        /// <param name="destinationIndex"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<MoveIndexResponse> MoveFromAsync(string destinationIndex, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// This function waits for the Algolia's API task to finish
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions"></param>
        void WaitTask(long taskID, int timeToWait = 100, RequestOptions requestOptions = null);

        /// <summary>
        /// This function waits for the Algolia's API task to finish
        /// </summary>
        /// <param name="taskID">The task ID to wait</param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task WaitTaskAsync(long taskID, int timeToWait = 100, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get the status of the given task
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        TaskStatusResponse GetTask(long taskID, RequestOptions requestOptions = null);

        /// <summary>
        /// Get the status of the given task
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<TaskStatusResponse> GetTaskAsync(long taskID, RequestOptions requestOptions = null,
            CancellationToken ct = default(CancellationToken));
    }
}
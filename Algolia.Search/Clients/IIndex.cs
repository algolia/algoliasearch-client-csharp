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
using Algolia.Search.Models.Responses;
using Algolia.Search.Models.Rules;
using Algolia.Search.Models.Query;
using Algolia.Search.Models.Settings;
using Algolia.Search.Models.Requests;
using Algolia.Search.Iterators;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    public interface IIndex
    {

        /// <summary>
        /// Add objects to the given index
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        BatchResponse AddObjects<T>(IEnumerable<T> datas, RequestOption requestOptions = null) where T : class;

        /// <summary>
        /// Add objects to the given index
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<BatchResponse> AddObjectsAysnc<T>(IEnumerable<T> datas, RequestOption requestOptions = null,
                    CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Add an object to the given index
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        AddObjectResponse AddObject<T>(T data, RequestOption requestOptions = null) where T : class;

        /// <summary>
        /// Add an object to the given index
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<AddObjectResponse> AddObjectAysnc<T>(T data, RequestOption requestOptions = null,
                    CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Search in the index for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SearchResponse<T> Search<T>(SearchQuery query, RequestOption requestOptions = null) where T : class;

        /// <summary>
        ///  Search in the index for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SearchResponse<T>> SearchAsync<T>(SearchQuery query, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Get object for the specified ID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        T GetObject<T>(string objectId, RequestOption requestOptions = null) where T : class;

        /// <summary>
        /// Get the specified object by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<T> GetObjectAsync<T>(string objectId, RequestOption requestOptions = null,
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
        /// <returns></returns>
        BrowseIndexResponse<T> BrowseFrom<T>(BrowseIndexQuery query, RequestOption requestOptions = null) where T : class;

        /// <summary>
        /// This method allows you to retrieve all index content  
        /// It can retrieve up to 1,000 records per call and supports full text search and filters. 
        /// You can use the same query parameters as for a search query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<BrowseIndexResponse<T>> BrowseFromAsync<T>(BrowseIndexQuery query, RequestOption requestOptions = null,
                    CancellationToken ct = default(CancellationToken)) where T : class;

        /// <summary>
        /// Get the specified by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        Rule GetRule(string objectId, RequestOption requestOptions = null);

        /// <summary>
        /// Get the specified rule by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Rule> GetRuleAsync(string objectId, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Search rules sync
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SearchResponse<Rule> SearchRule(RuleQuery query = null, RequestOption requestOptions = null);

        /// <summary>
        /// Search query 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SearchResponse<Rule>> SearchRuleAsync(RuleQuery query = null, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        ///  Save rule 
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SaveRuleResponse SaveRule(Rule rule, RequestOption requestOptions = null);

        /// <summary>
        /// Save the given rule
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SaveRuleResponse> SaveRuleAsync(Rule rule, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Delete the rule for the given ruleId
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        DeleteResponse DeleteRule(string objectId, RequestOption requestOptions = null);

        /// <summary>
        /// Delete the rule for the given ruleId
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteRuleAsync(string objectId, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get settings for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        IndexSettings GetSettings(RequestOption requestOptions = null);

        /// <summary>
        /// Get settings for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IndexSettings> GetSettingsAsync(RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Set settings
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        SetSettingsResponse SetSettings(IndexSettings settings, RequestOption requestOptions = null);

        /// <summary>
        /// Set settings for the given index
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SetSettingsResponse> SetSettingsAsync(IndexSettings settings, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// This function waits for the Algolia's API task to finish
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions"></param>
        void WaitForCompletion(long taskID, int timeToWait = 100, RequestOption requestOptions = null);

        /// <summary>
        /// This function waits for the Algolia's API task to finish
        /// </summary>
        /// <param name="taskID">The task ID to wait</param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task WaitForCompletionAsync(long taskID, int timeToWait = 100, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Get the status of the given task
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        TaskStatusResponse GetTask(long taskID, RequestOption requestOptions = null);

        /// <summary>
        /// Get the status of the given task
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<TaskStatusResponse> GetTaskAsync(long taskID, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));
    }
}

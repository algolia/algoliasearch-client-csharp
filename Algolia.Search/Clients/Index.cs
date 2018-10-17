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
using Algolia.Search.Models.Query;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Responses;
using Algolia.Search.Models.RuleQuery;
using Algolia.Search.Models.Settings;
using Algolia.Search.Transport;
using Algolia.Search.Utils;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    public class Index<T> : IIndex<T> where T : class
    {
        /// <summary>
        /// The Requester wrapper
        /// </summary>
        private readonly IRequesterWrapper _requesterWrapper;

        /// <summary>
        /// Url encoded index name
        /// </summary>
        private readonly string _urlEncodedIndexName;

        /// <summary>
        /// Instantiate an Index for a given client
        /// </summary>
        /// <param name="requesterWrapper"></param>
        /// <param name="indexName"></param>
        public Index(IRequesterWrapper requesterWrapper, string indexName)
        {
            _requesterWrapper = requesterWrapper ?? throw new ArgumentNullException(nameof(requesterWrapper));
            _urlEncodedIndexName = !string.IsNullOrEmpty(indexName)
                ? WebUtility.UrlEncode(indexName)
                : throw new ArgumentNullException(nameof(indexName));
        }

        /// <summary>
        /// Search in the index for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SearchResponse<T> Search(SearchQuery query, RequestOption requestOptions = null) =>
                    AsyncHelper.RunSync(() => SearchAsync(query, requestOptions));

        /// <summary>
        ///  Search in the index for the given query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SearchResponse<T>> SearchAsync(SearchQuery query, RequestOption requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<SearchResponse<T>, SearchQuery>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/query", CallType.Read, query, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get object for the specified ID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public T GetObject(string objectId, RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => GetObjectAsync(objectId, requestOptions));

        /// <summary>
        /// Get the specified object by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<T> GetObjectAsync(string objectId, RequestOption requestOptions = null,
                    CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            return await _requesterWrapper.ExecuteRequestAsync<T>(HttpMethod.Get,
                $"/1/indexes/{_urlEncodedIndexName}/{objectId}", CallType.Read, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the specified by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public Rule GetRule(string objectId, RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => GetRuleAsync(objectId, requestOptions));

        /// <summary>
        /// Get the specified rule by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Rule> GetRuleAsync(string objectId, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(objectId))
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
        public SearchRuleResponse SearchRule(Rule query = null, RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => SearchRuleAsync(query, requestOptions));

        /// <summary>
        /// Search query 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SearchRuleResponse> SearchRuleAsync(Rule query = null, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<SearchRuleResponse, Rule>(HttpMethod.Post,
                $"/1/indexes/{_urlEncodedIndexName}/rules/search", CallType.Read, query, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        ///  Save rule 
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SaveRuleResponse SaveRule(Rule rule, RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => SaveRuleAsync(rule, requestOptions));

        /// <summary>
        /// Save the given rule
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SaveRuleResponse> SaveRuleAsync(Rule rule, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<SaveRuleResponse, Rule>(HttpMethod.Put,
                $"/1/indexes/{_urlEncodedIndexName}/rules/{rule.ObjectID}", CallType.Write, rule, requestOptions, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete the rule for the given ruleId
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public DeleteResponse DeleteRule(string objectId, RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => DeleteRuleAsync(objectId, requestOptions));

        /// <summary>
        /// Delete the rule for the given ruleId
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<DeleteResponse> DeleteRuleAsync(string objectId, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            return await _requesterWrapper.ExecuteRequestAsync<DeleteResponse>(HttpMethod.Delete, $"/1/indexes/{_urlEncodedIndexName}/rules/{objectId}", CallType.Write,
                requestOptions: requestOptions, ct: ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get logs for the given index
        /// </summary>
        /// <returns></returns>
        public LogResponse GetLogResponse(RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => GetLogsAsync(requestOptions));

        /// <summary>
        /// Get logs for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<LogResponse> GetLogsAsync(RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<LogResponse>(HttpMethod.Get, "/1/logs", CallType.Read,
                requestOptions: requestOptions, ct: ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get settings for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public IndexSettings GetSettings(RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => GetSettingsAsync(requestOptions));

        /// <summary>
        /// Get settings for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<IndexSettings> GetSettingsAsync(RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<IndexSettings>(HttpMethod.Get, $"/1/indexes/{_urlEncodedIndexName}/settings", CallType.Read,
                requestOptions: requestOptions, ct: ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Set settings
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public SetSettingsResponse SetSettings(IndexSettings settings, RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => SetSettingsAsync(settings, requestOptions));

        /// <summary>
        /// Set settings for the given index
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SetSettingsResponse> SetSettingsAsync(IndexSettings settings, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<SetSettingsResponse, IndexSettings>(HttpMethod.Put, $"/1/indexes/{_urlEncodedIndexName}/settings", CallType.Write,
               settings, requestOptions: requestOptions, ct: ct).ConfigureAwait(false);
        }

        /// <summary>
        /// This function waits for the Algolia's API task to finish
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions"></param>
        public void WaitForCompletion(int taskID, int timeToWait = 100, RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => WaitForCompletionAsync(taskID, timeToWait, requestOptions));

        /// <summary>
        /// This function waits for the Algolia's API task to finish
        /// </summary>
        /// <param name="taskID">The task ID to wait</param>
        /// <param name="timeToWait"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task WaitForCompletionAsync(int taskID, int timeToWait = 100, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            while (true)
            {
                TaskStatusResponse response = await GetTaskAsync(taskID, requestOptions, ct).ConfigureAwait(false);

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
        /// <param name="taskID"></param>
        /// <param name="requestOptions"></param>
        /// <returns></returns>
        public TaskStatusResponse GetTask(int taskID, RequestOption requestOptions = null) =>
            AsyncHelper.RunSync(() => GetTaskAsync(taskID, requestOptions));

        /// <summary>
        /// Get the status of the given task
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<TaskStatusResponse> GetTaskAsync(int taskID, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<TaskStatusResponse>(HttpMethod.Get, $"/1/indexes/{_urlEncodedIndexName}/task/{taskID}", CallType.Read,
                requestOptions: requestOptions, ct: ct).ConfigureAwait(false);
        }
    }
}

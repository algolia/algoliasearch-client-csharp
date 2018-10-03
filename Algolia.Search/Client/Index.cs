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

using Algolia.Search.Models.RuleQuery;
using Algolia.Search.RetryStrategy;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Algolia.Search.Http;
using Algolia.Search.Models.Responses;

namespace Algolia.Search.Client
{
    public class Index : IIndex
    {
        private readonly IRequesterWrapper _requesterWrapper;
        private readonly string _indexName;
        private readonly string _urlIndexName;

        /// <summary>
        /// Instantiate an Index for a given client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="indexName"></param>
        public Index(IRequesterWrapper requesterWrapper, string indexName)
        {
            _requesterWrapper = requesterWrapper ?? throw new ArgumentNullException(nameof(requesterWrapper));
            _indexName = string.IsNullOrEmpty(indexName) ? throw new ArgumentNullException(nameof(indexName)) : indexName;
            _urlIndexName = WebUtility.UrlEncode(_indexName);
        }

        /// <summary>
        /// Get the specified by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public Rule GetRule(string objectId)
        {
            return AsyncHelper.RunSync(() => GetRuleAsync(objectId));
        }

        /// <summary>
        /// Get the specified rule by its objectID
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public async Task<Rule> GetRuleAsync(string objectId)
        {
            if (string.IsNullOrEmpty(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            return await _requesterWrapper.ExecuteRequestAsync<Rule>(HttpMethod.Get, $"/1/indexes/{_urlIndexName}/rules/", objectId);
        }

        /// <summary>
        /// Search query 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SearchRuleResponse> SearchRuleAsync(Rule query = null,
            CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<SearchRuleResponse, Rule>(HttpMethod.Post, $"/1/indexes/{_urlIndexName}/rules/search", query, ct);
        }

        /// <summary>
        /// Get logs for the given index
        /// </summary>
        /// <returns></returns>
        public LogResponse GetLogResponse()
        {
            return AsyncHelper.RunSync(() => GetLogsAsync());
        }

        /// <summary>
        /// Get logs for the given index
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<LogResponse> GetLogsAsync(CancellationToken ct = default(CancellationToken))
        {
            return await _requesterWrapper.ExecuteRequestAsync<LogResponse>(HttpMethod.Get, $"/1/logs", ct: ct);
        }
    }

}

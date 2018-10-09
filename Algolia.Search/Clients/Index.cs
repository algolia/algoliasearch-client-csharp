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
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Responses;
using Algolia.Search.Models.RuleQuery;
using Algolia.Search.Transport;
using Algolia.Search.Utils;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    public class Index : IIndex
    {
        /// <summary>
        /// The Requester wrapper
        /// </summary>
        private readonly IRequesterWrapper _requesterWrapper;

        /// <summary>
        /// Url encoded index name
        /// </summary>
        private readonly string _urlIndexName;

        /// <summary>
        /// Instantiate an Index for a given client
        /// </summary>
        /// <param name="requesterWrapper"></param>
        /// <param name="indexName"></param>
        public Index(IRequesterWrapper requesterWrapper, string indexName)
        {
            _requesterWrapper = requesterWrapper ?? throw new ArgumentNullException(nameof(requesterWrapper));
            _urlIndexName = !string.IsNullOrEmpty(indexName)
                ? WebUtility.UrlEncode(indexName)
                : throw new ArgumentNullException(nameof(indexName));
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
                $"/1/indexes/{_urlIndexName}/rules/{objectId}", CallType.Read, requestOptions, ct);
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
                $"/1/indexes/{_urlIndexName}/rules/search", CallType.Read, query, requestOptions, ct);
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
                requestOptions: requestOptions, ct: ct);
        }
    }
}

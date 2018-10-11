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
using Algolia.Search.Models.RuleQuery;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Clients
{
    public interface IIndex
    {
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
        SearchRuleResponse SearchRule(Rule query = null, RequestOption requestOptions = null);

        /// <summary>
        /// Search query 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<SearchRuleResponse> SearchRuleAsync(Rule query = null, RequestOption requestOptions = null,
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
        /// Get logs for the given index
        /// </summary>
        /// <returns></returns>
        LogResponse GetLogResponse(RequestOption requestOptions = null);

        /// <summary>
        /// Get logs for the given index
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<LogResponse> GetLogsAsync(RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken));
    }
}

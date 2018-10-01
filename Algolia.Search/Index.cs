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

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Algolia.Search.Client;
using Algolia.Search.Models.Responses;
using Algolia.Search.Models.RuleQuery;

namespace Algolia.Search
{
    public class Index : IIndex
    {
        private readonly AlgoliaClient _client;
        private readonly string _indexName;
        private readonly string _urlIndexName;

        public Index(AlgoliaClient client, string indexName)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _indexName = string.IsNullOrEmpty(indexName) ? throw new ArgumentNullException(nameof(indexName)) : indexName;
            _urlIndexName = WebUtility.UrlEncode(indexName);
        }

        public SearchRuleResponse GetRule(string objectId)
        {
            throw new NotImplementedException();
        }

        public async Task<Rule> GetRuleAsync(string objectId)
        {
            if (string.IsNullOrEmpty(objectId))
            {
                throw new ArgumentNullException(nameof(objectId));
            }

            return await _client.ExecuteRequestAsync<Rule>(HttpMethod.Get, $"/1/indexes/{_urlIndexName}/rules/{objectId}");
        }
    }
}

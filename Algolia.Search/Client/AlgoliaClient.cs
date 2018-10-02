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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Client
{
    public class AlgoliaClient : IAlgoliaClient
    {
        private readonly string _applicationId;
        private readonly string _apiKey;
        private IEnumerable<string> _hosts;
        private static IHttpRequester _httpClient;

        /// <summary>
        /// Instance a new Algolia client
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="apiKey"></param>
        /// <param name="hosts"></param>
        /// <param name="customHttpClient"></param>
        public AlgoliaClient(string applicationId, string apiKey, ICollection<string> hosts = null,
            IHttpRequester customHttpClient = null)
        {
            _applicationId = string.IsNullOrEmpty(applicationId)
                ? throw new ArgumentNullException(nameof(applicationId), "Application ID is required")
                : applicationId;
            _apiKey = string.IsNullOrEmpty(apiKey)
                ? throw new ArgumentNullException(nameof(apiKey), "An API key is required")
                : applicationId;
            _httpClient = customHttpClient ?? new AlgoliaHttpRequester(applicationId, apiKey);

            if (hosts != null)
            {
                if (hosts.Any(string.IsNullOrEmpty))
                    throw new ArgumentNullException(nameof(hosts), "Hosts must not contain empty string");
                _hosts = hosts;
            }
            else
            {
                _hosts = new List<string>(3)
                {
                    $"{applicationId}-1.algolianet.com",
                    $"{applicationId}-2.algolianet.com",
                    $"{applicationId}-3.algolianet.com"
                }.Shuffle();
            }
        }

        /// <summary>
        /// Initialize an index for the given client
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public Index InitIndex(string indexName)
        {
            return string.IsNullOrEmpty(indexName)
                ? throw new ArgumentNullException(nameof(indexName), "Index name is required") : new Index(this, indexName);
        }

        /// <summary>
        /// More friendly execute request for GET/DELETE Method
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="queryParameters">GET or DELETE query parameters</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<TResult> ExecuteRequestAsync<TResult>(HttpMethod method, string uri, string queryParameters,
            CancellationToken ct = default(CancellationToken))
            where TResult : class
        {
            return await ExecuteRequestAsync<TResult, string>(method, uri, queryParameters, ct);
        }

        /// <summary>
        /// Call api with retry strategy
        /// </summary>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <typeparam name="TData">Data type</typeparam>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="data">Your data</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<TResult> ExecuteRequestAsync<TResult, TData>(HttpMethod method, string uri, TData data = default(TData),
            CancellationToken ct = default(CancellationToken))
            where TResult : class
            where TData : class
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            // TODO : Retry strategy
            var uriToCall = method == HttpMethod.Get || method == HttpMethod.Delete 
                ? new Uri(new Uri($"https://{_hosts.ElementAt(0)}"), $"{uri}{data}") 
                : new Uri(new Uri($"https://{_hosts.ElementAt(0)}"), uri);

            return await _httpClient.SendRequestAsync<TResult, TData>(method, uriToCall, data, ct);
        }
    }
}

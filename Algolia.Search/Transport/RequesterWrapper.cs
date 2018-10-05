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

using Algolia.Search.Clients;
using Algolia.Search.Http;
using Algolia.Search.Models.Enums;
using Algolia.Search.Models.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Transport
{
    internal class RequesterWrapper : IRequesterWrapper
    {
        private readonly IHttpRequester _httpClient;
        private readonly AlgoliaConfig _algoliaConfig;

        /// <summary>
        /// default constructor, intantiate with default configuration and default http client
        /// </summary>
        public RequesterWrapper()
        {
            _algoliaConfig = new AlgoliaConfig();
            _httpClient = new AlgoliaHttpRequester();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public RequesterWrapper(AlgoliaConfig config)
        {
            _algoliaConfig = config;
            _httpClient = new AlgoliaHttpRequester();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="httpClient"></param>
        public RequesterWrapper(AlgoliaConfig config, IHttpRequester httpClient)
        {
            _algoliaConfig = config;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Execute the request (more likely request with no body like GET or Delete)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="callType"></param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<TResult> ExecuteRequestAsync<TResult>(HttpMethod method, string uri, CallType callType, RequestOption requestOptions = null,
            CancellationToken ct = default(CancellationToken))
            where TResult : class => await ExecuteRequestAsync<TResult, string>(method, uri, callType, requestOptions: requestOptions, ct: ct);

        /// <summary>
        /// Call api with retry strategy
        /// </summary>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <typeparam name="TData">Data type</typeparam>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="callType"></param>
        /// <param name="data">Your data</param>
        /// <param name="requestOptions"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<TResult> ExecuteRequestAsync<TResult, TData>(HttpMethod method, string uri, CallType callType,
            TData data = default(TData), RequestOption requestOptions = null,
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

            // WIP : Retry strategy
            var hosts = new List<string>(3)
                {
                    $"{_algoliaConfig.AppId}-1.algolianet.com",
                    $"{_algoliaConfig.AppId}-2.algolianet.com",
                    $"{_algoliaConfig.AppId}-3.algolianet.com"
                };

            string jsonString = JsonConvert.SerializeObject(data, JsonConfig.AlgoliaJsonSerializerSettings);

            var request = new Request
            {
                Method = method,
                Body = jsonString,
                Headers = GenerateAlgoliaHeaders()
            };

            foreach (var host in hosts)
            {
                try
                {
                    request.Uri = BuildUri(method, host, uri);

                    string response = await _httpClient
                        .SendRequestAsync(request, _algoliaConfig.ConnectTimeOut, _algoliaConfig.ReadTimeOut, ct)
                        .ConfigureAwait(false);

                    return JsonConvert.DeserializeObject<TResult>(response, JsonConfig.AlgoliaJsonSerializerSettings);
                }
                catch (HttpRequestException httpEx)
                {
                    // call retry
                    throw;
                }
                catch (TimeoutException taskEx)
                {
                    // Time out are handled with task canceled exception
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            throw new AlgoliaException("Unreachable hosts");
        }

        /// <summary>
        /// Generate common headers from the config 
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GenerateAlgoliaHeaders()
        {
            return new Dictionary<string, string>
            {
                {"X-Algolia-Application-Id", _algoliaConfig.AppId},
                {"X-Algolia-API-Key", _algoliaConfig.ApiKey},
                {"User-Agent", "Algolia for Csharp 5.0.0"},
                {"Accept", "application/json"}
            };
        }

        /// <summary>
        /// Build uri depending on the method
        /// </summary>
        /// <param name="method"></param>
        /// <param name="host"></param>
        /// <param name="baseUri"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private Uri BuildUri(HttpMethod method, string host, string baseUri)
        {
            var builder = new UriBuilder { Scheme = "https", Host = host, Path = baseUri };

            return builder.Uri;
        }
    }
}
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

using Algolia.Search.Models.Request;
using Algolia.Search.Utils;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Http
{
    /// <summary>
    /// WIP : Algolia's implementation of the generic HttpRequester
    /// </summary>
    public class AlgoliaHttpRequester : IHttpRequester
    {
        /// <summary>
        /// https://docs.microsoft.com/en-gb/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
        /// </summary>
        private readonly HttpClient _httpClient = new HttpClient(
            new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });

        /// <summary>
        /// Don't use it directly
        /// Send request to the REST API 
        /// </summary>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <typeparam name="TData">Parameter type</typeparam>
        /// <param name="request"></param>
        /// <param name="connectTimeOut"></param>
        /// <param name="totalTimeout"></param>
        /// <param name="uri"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<TResult> SendRequestAsync<TResult, TData>(Request<TData> request, int connectTimeOut,
            int totalTimeout, CancellationToken ct = default(CancellationToken))
            where TResult : class
            where TData : class
        {
            if (request.Method == null)
            {
                throw new ArgumentNullException(nameof(request.Method), "No HTTP method found");
            }

            if (request.Uri == null)
            {
                throw new ArgumentNullException(nameof(request), "No URI found");
            }

            // Handle query parameters
            if ((request.Method == HttpMethod.Get || request.Method == HttpMethod.Delete) && request.Body != null)
            {
                request.Uri = new Uri(request.Uri, request.Body.ToString());
            }

            string jsonString = JsonConvert.SerializeObject(request.Body, JsonConfig.AlgoliaJsonSerializerSettings);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = request.Method,
                RequestUri = request.Uri,
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
            };

            httpRequestMessage.Headers.Fill(request.Headers);

            using (httpRequestMessage)
            using (HttpResponseMessage response =
                await _httpClient.SendAsync(httpRequestMessage, ct).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(((int)response.StatusCode).ToString());
                }

                string responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(responseString, JsonConfig.AlgoliaJsonSerializerSettings);
            }
        }
    }
}

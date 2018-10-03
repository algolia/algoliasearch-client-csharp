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

using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        /// Must be static
        /// https://docs.microsoft.com/en-gb/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
        /// </summary>
        private static HttpClient _httpClient;

        /// <summary>
        /// Algolia's implementation of the generic HttpRequester
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="apiKey"></param>
        public AlgoliaHttpRequester(string applicationId, string apiKey)
        {
            if (string.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentNullException(nameof(applicationId), "Application ID is required");

            }

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey), "An API key is required");
            }

            _httpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
            _httpClient.DefaultRequestHeaders.Add("X-Algolia-Application-Id", applicationId);
            _httpClient.DefaultRequestHeaders.Add("X-Algolia-API-Key", apiKey);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Algolia for Csharp 5.0.0");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        /// <summary>
        /// Don't use it directly
        /// Send request to the REST API 
        /// </summary>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <typeparam name="TData">Parameter type</typeparam>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<TResult> SendRequestAsync<TResult, TData>(HttpMethod method, Uri uri, TData data = default(TData), CancellationToken ct = default(CancellationToken))
            where TResult : class
            where TData : class
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method), "No HTTP method found");

            if (uri == null)
                throw new ArgumentNullException(nameof(uri), "No URI found");

            string jsonString = JsonConvert.SerializeObject(data, JsonConfig.AlgoliaJsonSerializerSettings);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = method,
                RequestUri = uri,
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage, ct).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }

            string responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(responseString, JsonConfig.AlgoliaJsonSerializerSettings);
        }
    }
}

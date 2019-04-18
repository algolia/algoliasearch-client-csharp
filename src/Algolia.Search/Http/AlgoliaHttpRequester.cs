/*
* Copyright (c) 2018 Algolia
* http://www.algolia.com/
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

using Algolia.Search.Models.Common;
using Algolia.Search.Serializer;
using Algolia.Search.Utils;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Http
{
    /// <summary>
    /// Algolia's HTTP requester
    /// You can inject your own by the SearchClient or Analytics Client
    /// </summary>
    internal class AlgoliaHttpRequester : IHttpRequester
    {
        /// <summary>
        /// https://docs.microsoft.com/en-gb/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
        /// </summary>
        private readonly HttpClient _httpClient = new HttpClient(
            new TimeoutHandler
            {
                InnerHandler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip }
            });

        /// <summary>
        /// Don't use it directly
        /// Send request to the REST API
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="totalTimeout">Timeout in seconds</param>
        /// <param name="ct">Optional cancellation token</param>
        /// <returns></returns>
        public async Task<AlgoliaHttpResponse> SendRequestAsync(Request request, int totalTimeout,
            CancellationToken ct = default)
        {
            if (request.Method == null)
            {
                throw new ArgumentNullException(nameof(request.Method), "No HTTP method found");
            }

            if (request.Uri == null)
            {
                throw new ArgumentNullException(nameof(request), "No URI found");
            }

#if DEBUG
            await LogHelper.LogToFile(request.Body);
#endif

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = request.Method,
                RequestUri = request.Uri,
                Content = request.Body != null ? new StreamContent(request.Body) : null
            };

            httpRequestMessage.Headers.Fill(request.Headers);
            httpRequestMessage.SetTimeout(TimeSpan.FromSeconds(totalTimeout));

            try
            {
                using (httpRequestMessage)
                using (HttpResponseMessage response =
                    await _httpClient.SendAsync(httpRequestMessage, ct).ConfigureAwait(false))
                {
                    var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        MemoryStream outputStream = new MemoryStream();
                        await stream.CopyToAsync(outputStream).ConfigureAwait(false);
                        outputStream.Seek(0, SeekOrigin.Begin);

#if DEBUG
                        await LogHelper.LogToFile(outputStream);
#endif

                        return new AlgoliaHttpResponse
                        {
                            Body = outputStream,
                            HttpStatusCode = (int)response.StatusCode
                        };
                    }

                    var content = await SerializerHelper.StreamToStringAsync(stream).ConfigureAwait(false);

                    return new AlgoliaHttpResponse
                    {
                        Error = content,
                        HttpStatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (TimeoutException timeOutException)
            {
                return new AlgoliaHttpResponse { IsTimedOut = true, Error = timeOutException.ToString() };
            }
        }
    }
}

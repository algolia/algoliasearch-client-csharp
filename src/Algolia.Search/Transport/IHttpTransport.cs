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

using Algolia.Search.Http;
using Algolia.Search.Models.Enums;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Algolia.Search.Transport
{
    /// <summary>
    /// Algolia's requester wrapper interface
    /// </summary>
    public interface IHttpTransport
    {
        /// <summary>
        /// Call api with retry strategy
        /// </summary>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <typeparam name="TData">Data type</typeparam>
        /// <param name="method">The HttpMethod <see cref="HttpMethod"/></param>
        /// <param name="uri">The endpoint URI</param>
        /// <param name="callType">The method Algolia's call type <see cref="CallType"/> </param>
        /// <param name="data">Your data</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        Task<TResult> ExecuteRequestAsync<TResult, TData>(HttpMethod method, string uri, CallType callType,
            TData data = default,
            RequestOptions requestOptions = null, CancellationToken ct = default)
            where TResult : class
            where TData : class;

        /// <summary>
        /// Execute the request (more likely request with no body like GET or Delete)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="method">The HttpMethod <see cref="HttpMethod"/></param>
        /// <param name="uri">The endpoint URI</param>
        /// <param name="callType">The method Algolia's call type <see cref="CallType"/> </param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="ct">Optional cancellation token</param>
        Task<TResult> ExecuteRequestAsync<TResult>(HttpMethod method, string uri, CallType callType,
            RequestOptions requestOptions = null,
            CancellationToken ct = default)
            where TResult : class;
    }
}

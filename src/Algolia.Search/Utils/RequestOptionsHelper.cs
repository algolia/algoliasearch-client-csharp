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
using System.Collections.Generic;

namespace Algolia.Search.Utils
{
    /// <summary>
    /// Helper for Algolia's request options
    /// </summary>
    public static class RequestOptionsHelper
    {
        /// <summary>
        /// Create a request option with or without existing queryParams or Request options
        /// </summary>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        /// <param name="queryParams">Query params</param>
        /// <returns></returns>
        public static RequestOptions AddQueryParams(this RequestOptions requestOptions,
            Dictionary<string, string> queryParams)
        {
            if (requestOptions == null)
            {
                return new RequestOptions { QueryParameters = queryParams };
            }

            if (requestOptions.QueryParameters == null)
            {
                requestOptions.QueryParameters = queryParams;
                return requestOptions;
            }

            requestOptions.QueryParameters.MergeWith(queryParams);
            return requestOptions;
        }

        /// <summary>
        /// Create a request option with or without existing headers or Request options
        /// </summary>
        /// <param name="requestOptions">request options</param>
        /// <param name="headers">Custom headers</param>
        /// <returns></returns>
        public static RequestOptions AddHeaders(this RequestOptions requestOptions, Dictionary<string, string> headers)
        {
            if (requestOptions == null)
            {
                return new RequestOptions { Headers = headers };
            }

            if (requestOptions.Headers == null)
            {
                requestOptions.Headers = headers;
                return requestOptions;
            }

            requestOptions.Headers.MergeWith(headers);
            return requestOptions;
        }
    }
}
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
using System.Collections.Generic;
using System.Net.Http.Headers;
using Algolia.Search.Utils;

namespace Algolia.Search.Http
{
    internal static class HttpRequestHeadersExtensions
    {
        /// <summary>
        /// Extension method to easily fill the HttpRequesterHeaders object
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        internal static HttpRequestHeaders Fill(this HttpRequestHeaders headers, Dictionary<string, string> dictionary)
        {
            foreach (var header in dictionary)
            {
                headers.Add(header.Key, header.Value);
            }

            return headers;
        }

        /// <summary>
        /// Extension method to easily fill HttpContentHeaders with the Request object
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="request"></param>
        internal static HttpContentHeaders Fill(this HttpContentHeaders headers, Request request)
        {
            if (request.Body != null)
            {
                headers.Add(Defaults.ContentType, Defaults.ApplicationJson);

                if (request.CanCompress)
                {
                    headers.ContentEncoding.Add(Defaults.GzipEncoding);
                }
            }

            return headers;
        }
    }
}

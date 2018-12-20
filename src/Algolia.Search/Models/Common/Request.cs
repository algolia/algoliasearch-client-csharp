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

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Algolia.Search.Models.Common
{
    /// <summary>
    /// Request to send to the API
    /// </summary>
    public class Request
    {
        /// <summary>
        /// The HTTP verb GET,POST etc.
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Uri of the request
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Headers a dictionary
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Body of the request
        /// </summary>
        public Stream Body { get; set; }
    }
}
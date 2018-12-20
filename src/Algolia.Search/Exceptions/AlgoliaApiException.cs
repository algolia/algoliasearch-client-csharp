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

namespace Algolia.Search.Exceptions
{
    /// <summary>
    /// Exception sent by Algolia's API
    /// </summary>
    public class AlgoliaApiException : Exception
    {
        /// <summary>
        /// Http error code
        /// </summary>
        public int HttpErrorCode { get; set; }

        /// <summary>
        /// Algolia's Execption
        /// </summary>
        public AlgoliaApiException()
        {
        }

        /// <summary>
        /// Create a new Algolia's api exception.
        /// </summary>
        /// <param name="message">The exception details.</param>
        public AlgoliaApiException(string message) : base(message)
        {
        }

        /// <summary>
        /// Algolia API Exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public AlgoliaApiException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Ctor with error code and message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="httpErrorCode"></param>
        public AlgoliaApiException(string message, int httpErrorCode)
            : base(message)
        {
            HttpErrorCode = httpErrorCode;
        }
    }
}
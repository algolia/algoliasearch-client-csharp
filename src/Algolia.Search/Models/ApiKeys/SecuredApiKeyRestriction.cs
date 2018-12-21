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

using Algolia.Search.Models.Search;
using System.Collections.Generic;

namespace Algolia.Search.Models.ApiKeys
{
    /// <summary>
    /// Secured Api Key restrictions
    /// </summary>
    public class SecuredApiKeyRestriction
    {
        /// <summary>
        /// Search query parameters
        /// </summary>
        public Query Query { get; set; }

        /// <summary>
        /// A Unix timestamp used to define the expiration date of the API key.
        /// </summary>
        public long? ValidUntil { get; set; }

        /// <summary>
        /// List of index names that can be queried.
        /// </summary>
        public List<string> RestrictIndices { get; set; }

        /// <summary>
        /// IPv4 network allowed to use the generated key. This is used for more protection against API key leaking and reuse.
        /// </summary>
        public List<string> RestrictSources { get; set; }

        /// <summary>
        /// Specify a user identifier. This is often used with rate limits.
        /// </summary>
        public string UserToken { get; set; }
    }
}
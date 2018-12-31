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

namespace Algolia.Search.Models.Rules
{
    /// <summary>
    /// Rule query matching various criteria.
    /// </summary>
    public class RuleQuery
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="query">Full text query.</param>
        public RuleQuery(string query = "")
        {
            Query = query;
        }

        /// <summary>
        /// Full text query.
        /// </summary>
        public string Query { get; set; } = string.Empty;

        /// <summary>
        /// When specified, restricts matches to rules with a specific anchoring type. When omitted, all anchoring types may match.
        /// </summary>
        public string Anchoring { get; set; }

        /// <summary>
        /// Restricts matches to contextual rules with a specific context (exact match).
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Requested page (zero-based).
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Maximum number of hits in a page. Minimum is 1, maximum is 1000.
        /// </summary>
        public int? HitsPerPage { get; set; }

        /// <summary>
        /// When specified, restricts matches to rules with a specific enabled status.
        /// When absent (default), all rules are retrieved, regardless of their enabled status.
        /// </summary>
        public bool? Enabled { get; set; }
    }
}
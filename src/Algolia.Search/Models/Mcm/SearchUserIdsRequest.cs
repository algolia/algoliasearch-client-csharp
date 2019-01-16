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

namespace Algolia.Search.Models.Mcm
{
    /// <summary>
    /// Search user Ids Request
    /// </summary>
    public class SearchUserIdsRequest
    {
        /// <summary>
        /// Query to search. The search is a prefix search with typoTolerance. Use empty query to retrieve all users.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// If specified only clusters assigned to this cluster can be returned.
        /// </summary>
        public string Cluster { get; set; }

        /// <summary>
        /// Page to fetch.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Number of users to return by page.
        /// </summary>
        public int HitsPerPage { get; set; }
    }
}
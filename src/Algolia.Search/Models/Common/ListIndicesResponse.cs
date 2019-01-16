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

namespace Algolia.Search.Models.Common
{
    /// <summary>
    /// https://www.algolia.com/doc/api-reference/api-methods/list-indices/
    /// </summary>
    public class ListIndicesResponse
    {
        /// <summary>
        /// List of index response
        /// </summary>
        public List<IndicesResponse> Items { get; set; }
    }

    /// <summary>
    /// Index response
    /// </summary>
    public class IndicesResponse
    {
        /// <summary>
        /// Index name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Index creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date of last update.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Number of records contained in the index
        /// </summary>
        public int Entries { get; set; }

        /// <summary>
        /// Number of bytes of the index in minified format.
        /// </summary>
        public int DataSize { get; set; }

        /// <summary>
        /// Number of bytes of the index binary file.
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// Last build time in seconds.
        /// </summary>
        public int LastBuildTimes { get; set; }

        /// <summary>
        /// Number of pending indexing operations.
        /// </summary>
        public int NumberOfPendingTasks { get; set; }

        /// <summary>
        /// A boolean which says whether the index has pending tasks.
        /// </summary>
        public bool PendingTask { get; set; }
    }
}
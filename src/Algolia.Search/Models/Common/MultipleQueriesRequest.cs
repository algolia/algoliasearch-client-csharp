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
using Algolia.Search.Serializer;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Algolia.Search.Models.Common
{
    /// <summary>
    /// Class used to perfom multiple requests
    /// </summary>
    public class MultipleQueriesRequest
    {
        /// <summary>
        /// List of requests
        /// </summary>
        public IEnumerable<MultipleQueries> Requests { get; set; }

        /// <summary>
        /// Request strategy <see cref="Enums.StrategyType"/>
        /// </summary>
        public string Strategy { get; set; }
    }

    /// <summary>
    /// Multiple queries
    /// </summary>
    public class MultipleQueries
    {
        /// <summary>
        /// The name of the index to perform the operation
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// Search query parameters
        /// </summary>
        [JsonConverter(typeof(QueryConverter))]
        public Query Params { get; set; }
    }
}

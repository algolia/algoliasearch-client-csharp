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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algolia.Search.Models.Batch
{
    /// <summary>
    /// Batch request for algoli'as API
    /// </summary>
    public class BatchRequest<T> where T : class
    {
        /// <summary>
        /// Create a new batch request with operations
        /// </summary>
        /// <param name="operations"></param>
        public BatchRequest(IEnumerable<BatchOperation<T>> operations)
        {
            if (operations == null)
            {
                throw new ArgumentNullException(nameof(operations));
            }

            Operations = operations.ToList();
        }

        /// <summary>
        /// Create a new batch request with action type and data
        /// </summary>
        /// <param name="actionType">Batch Action Type</param>
        /// <param name="data">Data to send</param>
        public BatchRequest(string actionType, IEnumerable<T> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Operations = new List<BatchOperation<T>>();

            foreach (var item in data)
            {
                Operations.Add(new BatchOperation<T>
                {
                    Action = actionType,
                    Body = item
                });
            }
        }

        /// <summary>
        /// List of operations of the batch request
        /// </summary>
        [JsonProperty(PropertyName = "requests")]
        public ICollection<BatchOperation<T>> Operations { get; set; }
    }
}
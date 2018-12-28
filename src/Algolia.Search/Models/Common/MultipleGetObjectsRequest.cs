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

using System.Collections.Generic;

namespace Algolia.Search.Models.Common
{
    /// <summary>
    /// Used to create multiple get
    /// </summary>
    public class MultipleGetObjectsRequest
    {
        /// <summary>
        /// List of requests
        /// </summary>
        public IEnumerable<MultipleGetObject> Requests { get; set; }
    }

    /// <summary>
    /// Multiple request
    /// </summary>
    public class MultipleGetObject
    {
        /// <summary>
        /// Name of the index containing the object
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// ID of the object within that index
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// List of attributes to retrieve. By default, all retrievable attributes are returned.
        /// </summary>
        public IEnumerable<string> AttributesToRetrieve { get; set; }
    }
}
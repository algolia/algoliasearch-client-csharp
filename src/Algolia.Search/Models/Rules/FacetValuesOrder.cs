/*
* Copyright (c) 2021 Algolia
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
using Algolia.Search.Models.Enums;

namespace Algolia.Search.Models.Rules
{
    /// <summary>
    /// Facet values ordering rule container.
    /// </summary>
    public class FacetValuesOrder
    {
        /// <summary>
        /// An ordered list of facet values to present.
        /// </summary>
        public List<string> Order { get; set; }

        /// <summary>
        /// A constant defining what to do with remaining facet values not included in Order:
        ///   - "alpha": sort the remaining values in alphanumeric order
        ///   - "count": sort the remaining values by count in descending order
        ///   - "hidden": hide the remaining values
        /// </summary>
        public string SortRemainingBy { get; set; }
    }
}

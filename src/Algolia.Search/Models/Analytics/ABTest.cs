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

namespace Algolia.Search.Models.Analytics
{
    /// <summary>
    /// https://www.algolia.com/doc/rest-api/ab-test/
    /// </summary>
    public class ABTest
    {
        /// <summary>
        /// AB Test name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// List of variants for the ab test
        /// </summary>
        public IEnumerable<Variant> Variants { get; set; }

        /// <summary>
        /// End date for the AB Test
        /// </summary>
        public DateTime? EndAt { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Ab test ID
        /// </summary>
        public long? AbTestId { get; set; }

        /// <summary>
        /// ABTest significance based on click data. Should be > 0.95 to be considered significant (no matter which variant is winning).
        /// </summary>
        public int? ClickSignificance { get; set; }

        /// <summary>
        /// ABTest significance based on conversion data. Should be > 0.95 to be considered significant (no matter which variant is winning)
        /// </summary>
        public int? ConversionSignificance { get; set; }
    }
}
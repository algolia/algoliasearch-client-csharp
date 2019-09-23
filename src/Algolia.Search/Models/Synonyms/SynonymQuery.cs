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

namespace Algolia.Search.Models.Synonyms
{
    /// <summary>
    /// Query to search synonyms on an index
    /// </summary>
    public class SynonymQuery
    {
        /// <summary>
        /// Constructor with query string
        /// </summary>
        /// <param name="query">the text to search</param>
        public SynonymQuery(string query = "")
        {
            Query = query;
        }

        /// <summary>
        /// The text to search
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// There are 4 synonym types. The parameter can be one of the following values <see cref="Enums.SynonymType"/>
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Page to retrieve
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Number of hits per page
        /// </summary>
        public int? HitsPerPage { get; set; }
    }
}

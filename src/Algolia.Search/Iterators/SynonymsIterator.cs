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

using System.Collections;
using System.Collections.Generic;
using Algolia.Search.Clients;
using Algolia.Search.Http;
using Algolia.Search.Models.Search;
using Algolia.Search.Models.Synonyms;

namespace Algolia.Search.Iterators
{
    /// <summary>
    /// Algolia's synonym iterator
    /// </summary>
    public class SynonymsIterator : IEnumerable<Synonym>
    {
        private readonly ISearchIndex _index;
        private readonly SynonymQuery _query = new SynonymQuery();
        private readonly RequestOptions _requestOptions;
        private int _hits;

        /// <summary>
        /// Create a new instance of a synonym iterator
        /// </summary>
        /// <param name="index">The index to fetch the synonym from</param>
        /// <param name="hitsPerpage">Hits per page for each call default = 1000</param>
        /// <param name="requestOptions">Add extra http header or query parameters to Algolia</param>
        public SynonymsIterator(ISearchIndex index, int hitsPerpage = 1000, RequestOptions requestOptions = null)
        {
            _index = index;
            _query.HitsPerPage = hitsPerpage;
            _query.Page = 0;
            _requestOptions = requestOptions;
        }

        /// <inheritdoc />
        /// <summary>
        /// Iterator for synonym (perfom an api call)
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Synonym> GetEnumerator()
        {
            do
            {
                SearchResponse<Synonym> result = _index.SearchSynonyms(_query, _requestOptions);
                _hits = result.Hits.Count;
                _query.Page++;

                if (_hits == 0)
                {
                    _query.Page = 0;
                    yield break;
                }

                foreach (var hit in result.Hits)
                {
                    yield return hit;
                }
            } while (_hits > 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

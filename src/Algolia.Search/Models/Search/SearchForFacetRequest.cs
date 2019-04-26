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

using Algolia.Search.Serializer;
using Newtonsoft.Json;

namespace Algolia.Search.Models.Search
{
    /// <summary>
    /// Search for facets
    /// </summary>
    public class SearchForFacetRequest
    {
        /// <summary>
        /// Attribute name.
        /// Note that for this to work, attribute must be declared in the attributesForFaceting index setting with the searchable() modifier.
        /// </summary>
        [JsonIgnore]
        public string FacetName { get; set; }

        /// <summary>
        /// The search query used to search the facet attribute.
        /// Follows the same rules for an index query: a single character, a partial word, a word, or a phrase
        /// </summary>
        public string FacetQuery { get; set; }

        /// <summary>
        /// Search parameters to be used to search the underlying index
        /// </summary>
        [JsonProperty(PropertyName = "params")]
        [JsonConverter(typeof(QueryConverter))]
        public Query SearchParameters { get; set; }
    }
}

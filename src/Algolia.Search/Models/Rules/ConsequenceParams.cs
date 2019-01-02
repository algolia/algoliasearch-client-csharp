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

namespace Algolia.Search.Models.Rules
{
    /// <summary>
    /// Consequence params
    /// </summary>
    public class ConsequenceParams : Query
    {
        /// <summary>
        /// When providing a string, it replaces the entire query string.
        /// When providing an object, it describes incremental edits to be made to the query string (but you can’t do both).
        /// </summary>
        public ConsequenceQuery Query { get; set; }

        /// <summary>
        /// Names of facets to which automatic filtering must be applied; they must match the facet name of a facet value placeholder in the query pattern.
        /// </summary>
        [JsonConverter(typeof(AutomaticFacetFiltersConverter))]
        public List<AutomaticFacetFilter> AutomaticFacetFilters { get; set; }

        /// <summary>
        /// Same syntax as automaticFacetFilters, but the engine treats the filters as optional.
        /// Behaves like optionalFilters.
        /// </summary>
        public List<AutomaticFacetFilter> AutomaticOptionalFacetFilters { get; set; }
    }
}
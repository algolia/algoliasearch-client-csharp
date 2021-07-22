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
using Algolia.Search.Models.Search;
using Algolia.Search.Serializer;
using Algolia.Search.Models.Rules;
using Newtonsoft.Json;

namespace Algolia.Search.Models.Rules
{
    /// <summary>
    /// Consequence params
    /// </summary>
    public class ConsequenceParams : Query
    {
        /// <summary>
        /// When providing an object, it describes incremental edits to be made to the query string.
        /// </summary>
        /// <remarks>Setting a ConsequenceQuery will override SearchQuery if set. Both can't be set at the same time.</remarks>
        public ConsequenceQuery Query { get; set; }

        /// <summary>
        /// Providing a SearchQuery in ConsequenceParams replaces the entire query string for the given pattern.
        /// </summary>
        /// <remarks>Setting a SearchQuery will override ConsequenceQuery if set. Both can't be set at the same time.</remarks>
        [JsonIgnore]
        public new string SearchQuery
        {
            get
            {
                return Query?.SearchQuery;
            }

            set
            {
                Query = new ConsequenceQuery { SearchQuery = value };
            }
        }

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

        /// <summary>
        /// Content defining how the search interface should be rendered. 
        /// This is set via the settings for a default value and can be overridden via rules.
        /// </summary>
        public RenderingContent RenderingContent { get; set; }
    }
}

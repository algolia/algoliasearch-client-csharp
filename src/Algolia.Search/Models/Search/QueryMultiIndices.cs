using Algolia.Search.Models.Common;

namespace Algolia.Search.Models.Search
{
    /// <summary>
    /// For more information regarding the parameters
    /// https://www.algolia.com/doc/api-reference/search-api-parameters/
    /// </summary>
    public class QueryMultiIndices : Query, IMultipleQueries
    {
        /// <summary>
        /// Create a new query with an empty search query, for a dedicated indice,
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="searchQuery"></param>
        public QueryMultiIndices(string indexName, string searchQuery = null)
        {
            IndexName = indexName;
            SearchQuery = searchQuery;
        }

        /// <summary>
        /// The name of the index to perform the operation
        /// </summary>
        public string IndexName { get; set; }
    }
}

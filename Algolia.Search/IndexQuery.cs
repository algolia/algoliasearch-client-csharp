﻿namespace Algolia.Search
{
    /// <summary>
    /// Used for building index queries.
    /// </summary>
    public class IndexQuery
    {
        /// <summary>
        /// Get or set the index name.
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// Get or set the query.
        /// </summary>
        public Query Query { get; set; }

        /// <summary>
        /// Create a new index query.
        /// </summary>
        /// <param name="index">The name of the index.</param>
        /// <param name="query">The query.</param>
        public IndexQuery(string index, Query query)
        {
            this.Index = index;
            this.Query = query;
        }

        /// <summary>
        /// Create a new index query.
        /// </summary>
        public IndexQuery()
        {
        }
    }
}

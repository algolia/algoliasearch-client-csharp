using System.Collections.Generic;

namespace Algolia.Search.Models.Mcm
{
    /// <summary>
    /// List of cluster response
    /// </summary>
    public class ListClustersResponse
    {
        /// <summary>
        /// List of clusters
        /// </summary>
        public IEnumerable<ClustersResponse> Clusters { get; set; }
    }

    /// <summary>
    /// Cluster response
    /// </summary>
    public class ClustersResponse
    {
        /// <summary>
        /// The cluster name
        /// </summary>
        public string ClusterName { get; set; }

        /// <summary>
        /// Number of records in the cluster.
        /// </summary>
        public int NbRecords { get; set; }

        /// <summary>
        /// Number of users assign to the cluster.
        /// </summary>
        public int NbUserIDs { get; set; }

        /// <summary>
        /// Data size taken by all the users assigned to the cluster.
        /// </summary>
        public int DataSize { get; set; }
    }
}
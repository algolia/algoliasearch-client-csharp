using System.Collections.Generic;

namespace Algolia.Search.Models.Responses
{
    public class ListClustersResponse
    {
        public IEnumerable<ClustersResponse> Clusters { get; set; }
    }

    public class ClustersResponse
    {
        public string ClusterName { get; set; }
        public int NbRecords { get; set; }
        public int NbUserIDs { get; set; }
        public int DataSize { get; set; }
    }
}
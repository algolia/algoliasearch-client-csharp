using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algolia.Search
{
    public class IndexQuery
    {
        public String Index {get; set;}
        public Query Query{get; set;}

        public IndexQuery(String index, Query query)
        {
            this.Index = index;
            this.Query = query;
        }

        public IndexQuery()
        {
        }
    }
}

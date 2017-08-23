using System;
using Newtonsoft.Json;

namespace Algolia.Search.Models
{
    public class RuleQuery
	{
		public RuleQuery() { }

        public RuleQuery(string query, string anchoring, string context)
        {
            Query = query;
            Anchoring = anchoring;
            Context = context;
        }

        public RuleQuery(string query, string anchoring, string context, int page, int hitsPerPage)
        {
            Query = query;
            Anchoring = anchoring;
            Context = context;
            Page = page;
            HitsPerPage = hitsPerPage;
        }

	    public string Query { get; set; } = "";
	    public string Anchoring { get; set; } = "";
	    public string Context { get; set; } = "";

	    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }

	    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? HitsPerPage { get; set; }
    }

}

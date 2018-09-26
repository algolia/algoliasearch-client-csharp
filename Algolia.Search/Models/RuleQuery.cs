using Newtonsoft.Json;

namespace Algolia.Search.Models
{
	public class RuleQuery
	{
		public RuleQuery()
		{
		}

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

		[JsonProperty(PropertyName = "query")]
		public string Query { get; set; } = "";

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "anchoring")]
		public string Anchoring { get; set; }

		[JsonProperty(PropertyName = "context")]
		public string Context { get; set; } = "";

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "page")]
		public int? Page { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "hitsPerPage")]
		public int? HitsPerPage { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "enabled")]
        public bool? Enabled { get; set; }
    }
}

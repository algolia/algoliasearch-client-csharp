using Newtonsoft.Json;

namespace Algolia.Search.Models
{
    public class FacetScoring
    {
        [JsonProperty(PropertyName = "score", NullValueHandling = NullValueHandling.Ignore)]
        public long Score { get; set; }
    }
}
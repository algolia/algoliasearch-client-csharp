using Newtonsoft.Json;

namespace Algolia.Search.Models
{
    public class EventScoring
    {
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "score", NullValueHandling = NullValueHandling.Ignore)]
        public long Score { get; set; }
    }
}
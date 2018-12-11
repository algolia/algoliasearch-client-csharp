using System.Collections.Generic;
using Newtonsoft.Json;

namespace Algolia.Search.Models
{
    public class PersonalizationStrategyResponse
    {
        [JsonProperty(PropertyName = "eventsScoring", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, EventScoring> EventsScoring { get; set; }

        [JsonProperty(PropertyName = "facetsScoring", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, FacetScoring> FacetsScoring { get; set; }

        [JsonProperty(PropertyName = "taskId", NullValueHandling = NullValueHandling.Ignore)]
        public long TaskId { get; set; }
    }
}
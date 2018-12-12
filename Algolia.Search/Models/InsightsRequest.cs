using System.Collections.Generic;
using Newtonsoft.Json;

namespace Algolia.Search.Models
{
    public class InsightsRequest
    {
        [JsonProperty(PropertyName = "events", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<InsightsEvent> Events { get; set; }
    }

    public class InsightsEvent
    {
        [JsonProperty(PropertyName = "eventType", NullValueHandling = NullValueHandling.Ignore)]
        public string EventType { get; set; }

        [JsonProperty(PropertyName = "eventName", NullValueHandling = NullValueHandling.Ignore)]
        public string EventName { get; set; }

        [JsonProperty(PropertyName = "index", NullValueHandling = NullValueHandling.Ignore)]
        public string Index { get; set; }

        [JsonProperty(PropertyName = "userToken", NullValueHandling = NullValueHandling.Ignore)]
        public string UserToken { get; set; }

        [JsonProperty(PropertyName = "timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public long? Timestamp { get; set; }

        [JsonProperty(PropertyName = "queryID", NullValueHandling = NullValueHandling.Ignore)]
        public string QueryID { get; set; }

        [JsonProperty(PropertyName = "objectIDs", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> ObjectIDs { get; set; }

        [JsonProperty(PropertyName = "filters", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> Filters { get; set; }

        [JsonProperty(PropertyName = "positions", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<uint> Positions { get; set; }
    }
}
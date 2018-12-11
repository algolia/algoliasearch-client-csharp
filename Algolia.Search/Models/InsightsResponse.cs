using Newtonsoft.Json;

namespace Algolia.Search.Models
{
    public class InsightsResponse
    {
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
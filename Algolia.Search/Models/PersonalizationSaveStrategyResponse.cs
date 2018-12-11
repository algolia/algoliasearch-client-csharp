using System;
using Newtonsoft.Json;

namespace Algolia.Search.Models
{
    public class PersonalizationSaveStrategyResponse
    {
        [JsonProperty(PropertyName = "updatedAt", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime UpdatedAt { get; set; }
    }
}
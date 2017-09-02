using Newtonsoft.Json;

namespace MyWeb.Models
{
    public class ClaimInfo
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

}

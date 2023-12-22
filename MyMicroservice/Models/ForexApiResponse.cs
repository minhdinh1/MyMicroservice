using Newtonsoft.Json;

namespace MyMicroservice.Models
{
    public class ForexApiResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("terms")]
        public string Terms { get; set; }
        [JsonProperty("privacy")]
        public string Privacy { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("quotes")]
        public Dictionary<string, double> Quotes { get; set; }
    }
}

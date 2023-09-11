using Newtonsoft.Json;

namespace HackerEarthApi.JsonModels
{
    public class RequestBody
    {
        [JsonProperty("lang")]
        public required string Language { get; set; }
        [JsonProperty("source")]
        public required string Source { get; set; }
        [JsonProperty("input")]
        public string? Input { get; set; }
        [JsonProperty("memory_limit")]
        public int? MemoryLimit { get; set; }
        [JsonProperty("time_limit")]
        public int? TimeLimit { get; set; }
    }
}
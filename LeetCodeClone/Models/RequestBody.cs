using Newtonsoft.Json;

namespace LeetCodeClone
{
    class RequestBody
    {
        [JsonProperty("lang")]
        public required string Lang { get; set; }
        [JsonProperty("source")]
        public required string Source { get; set; }
        [JsonProperty("input")]
        public string? Input { get; set; }
        [JsonProperty("memory_limit")]
        public int? MemoryLimit { get; set; }
        [JsonProperty("time_limit")]
        public int? TimeLimit { get; set; }
        [JsonProperty("context")]
        public string? Context { get; set; }
        [JsonProperty("callback")]
        public string? CallbackURL { get; set; }
    }
}
using Newtonsoft.Json;
using PropertyChanged;

namespace HackerEarthApi.JsonModels
{
    [AddINotifyPropertyChangedInterface]
    internal class RunStatus
    {
        [JsonProperty("output")]
        public string OutputString { get; set; }
        [JsonProperty("time_used")]
        public float TimeUsed { get; set; }
        [JsonProperty("memory_used")]
        public int MemoryUsed { get; set; }
    }
}
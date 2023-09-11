using Newtonsoft.Json;
using PropertyChanged;

namespace HackerEarthApi.JsonModels
{
    [AddINotifyPropertyChangedInterface]
    internal class Result
    {
        [JsonProperty("run_status")]
        public RunStatus RunStatus { get; set; }
    }
}
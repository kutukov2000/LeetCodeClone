using Newtonsoft.Json;
using PropertyChanged;

namespace HackerEarthApi.JsonModels
{
    [AddINotifyPropertyChangedInterface]
    public class HackerEarthApiOutput
    {
        [JsonProperty("he_id")]
        private string _id { get; set; }
        [JsonProperty("request_status")]
        private RequestStatus _requestStatus { get; set; }
        [JsonProperty("result")]
        private Result _result { get; set; }

        public string Id { get => _id; }
        public int MemoryUsed { get => _result.RunStatus.MemoryUsed; }
        public float TimeUsed { get => _result.RunStatus.TimeUsed; }
        public string ExecutionStatus { get => _requestStatus.Status; }
        public string OutputString { get => _result.RunStatus.OutputString; }
    }
}
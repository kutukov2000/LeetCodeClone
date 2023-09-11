using Newtonsoft.Json;
using PropertyChanged;

namespace HackerEarthApi.JsonModels
{
    [AddINotifyPropertyChangedInterface]
    internal class RequestStatus
    {
        [JsonProperty("code")]
        public string Status { get; set; }

    }
}
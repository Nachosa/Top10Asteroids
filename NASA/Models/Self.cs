using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NASA.Models
{
    public class Self
    {
        [JsonProperty("self")]
        public string SelfLink { get; set; }
    }
}

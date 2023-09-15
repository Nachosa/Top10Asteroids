using Newtonsoft.Json;

namespace NASA.Models
{
    public class Week
    {
        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("element_count")]
        public int ElementCount { get; set; }

        [JsonProperty("near_earth_objects")]
        public SortedDictionary<string, List<Asteroid>> Asteroids { get; set; }
    }
}

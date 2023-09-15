using Newtonsoft.Json;

namespace NASA.Models
{
    public class EstimatedDiameter
    {
        [JsonProperty("kilometers")]
        public DiameterInKilometers Kilometers { get; set; }

        [JsonProperty("meters")]
        public DiameterInMeters Meters { get; set; }

        [JsonProperty("miles")]
        public DiameterInMiles Miles { get; set; }

        [JsonProperty("feet")]
        public DiameterInFeet Feet { get; set; }

    }
}

using Newtonsoft.Json;

namespace NASA.Models
{
    public class CloseApproachData : IComparable
    {
        [JsonProperty("close_approach_date")]
        public string CloseApproachDate { get; set; }

        [JsonProperty("close_approach_date_full")]
        public string CloseApproachDateFull { get; set; }

        [JsonProperty("epoch_date_close_approach")]
        public long EpochDateCloseApproach { get; set; }

        [JsonProperty("relative_velocity")]
        public RelativeVelocity RelativeVelocity { get; set; }

        [JsonProperty("miss_distance")]
        public MissDistance MissDistance { get; set; }

        [JsonProperty("orbiting_body")]
        public string OrbitingBody { get; set; }

        /// <summary>
        /// CloseApproachData objects are compared by their MissDistance in Kilometers from the Earth.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            CloseApproachData otherCloseApproachData = obj as CloseApproachData;

            if (otherCloseApproachData != null)
            {
                var missDist = this.MissDistance.Kilometers;
                var otherMissDist = otherCloseApproachData.MissDistance.Kilometers;
                return missDist.CompareTo(otherMissDist);

            }
            else
            {
                throw new ArgumentException("Object is not a CloseApproachData");
            }

        }
    }
}

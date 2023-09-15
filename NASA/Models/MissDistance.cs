using Newtonsoft.Json;

namespace NASA.Models
{
    public class MissDistance : IComparable
    {
        [JsonProperty("astronomical")]
        public double Astronomical { get; set; }

        [JsonProperty("lunar")]
        public double Lunar { get; set; }

        [JsonProperty("kilometers")]
        public double Kilometers { get; set; }

        [JsonProperty("miles")]
        public double Miles { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            MissDistance otherMissDistance = obj as MissDistance;

            if (otherMissDistance != null)
            {
                var missDist = this.Kilometers;
                var otherMissDist = otherMissDistance.Kilometers;
                return missDist.CompareTo(otherMissDist);

            }
            else
            {
                throw new ArgumentException("Object is not a MissDistane");
            }

        }
    }
}

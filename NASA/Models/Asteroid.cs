using Newtonsoft.Json;

namespace NASA.Models
{
    public class Asteroid : IComparable
    {
        [JsonProperty("links")]
        public Self Self { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("neo_reference_id")]
        public string NeoReferenceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nasa_jpl_url")]
        public string NasaJplUrl { get; set; }

        [JsonProperty("absolute_magnitude_h")]
        public double AbsoluteMagnitudeH { get; set; }

        [JsonProperty("estimated_diameter")]
        public EstimatedDiameter EstimatedDiameter { get; set; }

        [JsonProperty("is_potentially_hazardous_asteroid")]
        public bool IsPotentiallyHazardousAsteroid { get; set; }

        [JsonProperty("close_approach_data")]
        public List<CloseApproachData> CloseApproachData { get; set; }

        [JsonProperty("is_sentry_object")]
        public bool IsSentryObject { get; set; }

        /// <summary>
        /// Asteroid objects are compared by their MissDistance in Kilometers from the Earth.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Asteroid otherNearEarthObject = obj as Asteroid;

            if (otherNearEarthObject != null)
            {
                var closeApproach = this.CloseApproachData
                                    .OrderBy(closeApproach => closeApproach.MissDistance.Kilometers)
                                    .FirstOrDefault();
                var otherCloseApproach = otherNearEarthObject.CloseApproachData
                                    .OrderBy(closeApproach => closeApproach.MissDistance.Kilometers)
                                    .FirstOrDefault();

                return closeApproach.MissDistance.Kilometers.CompareTo(otherCloseApproach.MissDistance.Kilometers);
            }
            else
            {
                throw new ArgumentException("Object is not a CloseApproachData");
            }

        }
    }
}

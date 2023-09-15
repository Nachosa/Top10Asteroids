namespace NASA.Models.Dtos
{
    public class GetAsteroidDto
    {
        public GetAsteroidDto(Asteroid asteroid)
        {
            var closestApproach = asteroid.CloseApproachData.FirstOrDefault();

            SelfLink = asteroid.Self.SelfLink;
            Id = asteroid.Id;
            Name = asteroid.Name;
            CloseApproachDateFull = closestApproach.CloseApproachDateFull;
            IsPotentiallyHazardousAsteroid = asteroid.IsPotentiallyHazardousAsteroid;
            MissDistanceInKilometers = closestApproach.MissDistance.Kilometers;
            MissDistanceInMiles = closestApproach.MissDistance.Miles;
            KilometersPerHour = closestApproach.RelativeVelocity.KilometersPerHour;
            MilesPerHour = closestApproach.RelativeVelocity.MilesPerHour;

            AvgDiameterInMeters = (asteroid.EstimatedDiameter.Meters.EstimatedDiameterMax +
                                  asteroid.EstimatedDiameter.Meters.EstimatedDiameterMin) / 2;

            AvgDiameterInFeet = (asteroid.EstimatedDiameter.Feet.EstimatedDiameterMax +
                                  asteroid.EstimatedDiameter.Feet.EstimatedDiameterMin) / 2;
        }

        public string SelfLink { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string CloseApproachDateFull { get; set; }

        public bool IsPotentiallyHazardousAsteroid { get; set; }

        public double MissDistanceInKilometers { get; set; }

        public double MissDistanceInMiles { get; set; }

        public double KilometersPerHour { get; set; }

        public double MilesPerHour { get; set; }

        public double AvgDiameterInMeters { get; set; }

        public double AvgDiameterInFeet { get; set; }
    }
}

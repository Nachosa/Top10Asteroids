using NASA.Helper.Contracts;
using NASA.Models;

namespace NASA.Helper
{
    public class AsteroidsOrganizer : IAsteroidsOrganizer
    {
        public List<Asteroid> Top10CloseApproachAsteroids(SortedDictionary<string, List<Asteroid>> weekWithAsteroids)
        {
            List<Asteroid> Asteroids = this.TakeAllAsteroids(weekWithAsteroids);

            List<Asteroid> Top10 = Asteroids
                                 .OrderBy(a => a.CloseApproachData
                                 .OrderBy(closeApproach => closeApproach.MissDistance.Kilometers)
                                    .FirstOrDefault())
                                 .Take(10)
                                 .ToList();
            return Top10;

        }
        public List<Asteroid> TakeAllAsteroids(SortedDictionary<string, List<Asteroid>> weekWithAsteroids)
        {
            List<Asteroid> Asteroids = new List<Asteroid>();
            foreach (var day in weekWithAsteroids)
            {
                Asteroids.AddRange(day.Value);
            }
            return Asteroids;
        }
    }
}

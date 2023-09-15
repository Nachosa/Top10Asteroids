using NASA.Models;

namespace NASA.Helper.Contracts
{
    public interface IAsteroidsOrganizer
    {
        /// <summary>
        /// Extract a collection wtih top 10 asteroids ordered by their approach to the earth in kilometers.
        /// </summary>
        /// <param name="weekWithAsteroids">SortedDictionay with Key= day of the week and Value= collection of asteroids.</param>
        List<Asteroid> Top10CloseApproachAsteroids(SortedDictionary<string, List<Asteroid>> weekWithAsteroids);

        /// <summary>
        /// Extract a collection wtih asteroids for a certain period of days.
        /// </summary>
        /// <param name="weekWithAsteroids">SortedDictionay with Key= day of the week and Value= collection of asteroids.</param>
        List<Asteroid> TakeAllAsteroids(SortedDictionary<string, List<Asteroid>> weekWithAsteroids);

    }
}

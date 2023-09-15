using NASA.Models.Dtos;

namespace NASA.Models.ViewModels
{
    public class HomePageViewModel
    {

        public HomePageViewModel(List<GetAsteroidDto> _asteroids, int allAsteroidsCount)
        {
            asteroids = _asteroids;
            AllAsteroidsCount = allAsteroidsCount;
        }

        public List<GetAsteroidDto> asteroids { get; set; } = new List<GetAsteroidDto>();

        public int AllAsteroidsCount { get; set; }
    }
}

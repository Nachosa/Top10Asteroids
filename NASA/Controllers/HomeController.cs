using Microsoft.AspNetCore.Mvc;
using NASA.Helper.Contracts;
using NASA.Models.Dtos;
using NASA.Models.ViewModels;
using NASA.Services;
using NASA.Services.Contracts;

namespace NASA.Controllers
{
    public class HomeController : Controller
    {
        private readonly INasaApis _nasa;
        private readonly IAsteroidsOrganizer _asteroidsOrganizer;


        public HomeController(INasaApis nasa, IAsteroidsOrganizer asteroidsOrganizer)
        {
            _nasa = nasa;
            _asteroidsOrganizer = asteroidsOrganizer;
        }

        public IActionResult Index()
        {

            var weekWithAsteroids = _nasa.NeoWsFeedAsync(DateTime.Now).Result;
            var Top10CloseApproachAsteroids = _asteroidsOrganizer.Top10CloseApproachAsteroids(weekWithAsteroids.Asteroids);


            var Top10CloseApproachGetAsteroidsDto = Top10CloseApproachAsteroids
                                                    .Select(a => new GetAsteroidDto(a))
                                                    .ToList();



            var HomeViewModel = new HomePageViewModel(Top10CloseApproachGetAsteroidsDto,weekWithAsteroids.ElementCount);
            return View(HomeViewModel);
        }

        [HttpGet]
        public IActionResult AstronomyPictureOfTheDay(DateTime date)
        {

            var apod = _nasa.AstronomyPictureOfTheDayAsync(date).Result;
            if (apod is null)
            {
                 apod = _nasa.AstronomyPictureOfTheDayAsync(DateTime.Now).Result;
                ViewBag.NotFoundMessage = $"Picture for day {date.ToString(NasaApis.dateFormat)} was not found!";
            }
            return View(apod);
        }

        public IActionResult Error()
        {
            var errorView = new ErrorViewModel();
            errorView.ErrorMessage = "Ooops something happened, please try again later.";
            this.HttpContext.Response.StatusCode = 500;
            return View(errorView);
        }

    }
}
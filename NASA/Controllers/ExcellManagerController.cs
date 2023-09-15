using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using NASA.Helper.Contracts;
using NASA.Models;
using NASA.Services;
using NASA.Services.Contracts;
using OfficeOpenXml;

namespace NASA.Controllers
{
    public class ExcellManagerController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly IAsteroidsOrganizer _asteroidsOrganizer;
        private readonly INasaApis _nasa;

        public ExcellManagerController(IMemoryCache cache,IAsteroidsOrganizer asteroidsOrganizer,INasaApis nasa)
        {
            _cache = cache;
            _asteroidsOrganizer = asteroidsOrganizer;
            _nasa = nasa;
        }

        /// <summary>
        /// Generates .xlsx table for all asteroid approaches in next 7 days.
        /// </summary>
        [HttpGet]
        public IActionResult DownloadExcel()
        {
            List<Asteroid> asteroids;
            if (_cache.TryGetValue(NasaApis.weekKey, out Week week))
            {
                asteroids = _asteroidsOrganizer.TakeAllAsteroids(week.Asteroids);
            }
            else
            {
                var weekWithAsteroids = _nasa.NeoWsFeedAsync(DateTime.Now).Result;

                _cache.Set(NasaApis.weekKey, weekWithAsteroids, TimeSpan.FromMinutes(30));

                asteroids = _asteroidsOrganizer.TakeAllAsteroids(weekWithAsteroids.Asteroids);

            }


            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Absolute Magnitude";
                worksheet.Cells[1, 4].Value = "Potential Earth Destroyer?";
                worksheet.Cells[1, 5].Value = "Approach Date";
                worksheet.Cells[1, 6].Value = "Distance in kilometers";
                worksheet.Cells[1, 7].Value = "Distance in miles";
                worksheet.Cells[1, 8].Value = "Astronomical distance";
                worksheet.Cells[1, 9].Value = "Speed in Kph";
                worksheet.Cells[1, 10].Value = "Speed in Mph";
                worksheet.Cells[1, 11].Value = "Speed in k per second";
                worksheet.Cells[1, 12].Value = "Min Diameter in Miles";
                worksheet.Cells[1, 13].Value = "Max Diameter in Miles";
                worksheet.Cells[1, 14].Value = "Min Diameter in Kilometers";
                worksheet.Cells[1, 15].Value = "Max Diameter in Kilometers";
                worksheet.Cells[1, 16].Value = "Min Diameter in Feet";
                worksheet.Cells[1, 17].Value = "Max Diameter in Feet";

                for (int i = 0; i < asteroids.Count; i++)
                {
                    var asteroid = asteroids[i];
                    var closestApproach = asteroid.CloseApproachData.FirstOrDefault();

                    worksheet.Cells[i + 2, 1].Value = asteroid.Id;
                    worksheet.Cells[i + 2, 2].Value = asteroid.Name;
                    worksheet.Cells[i + 2, 3].Value = asteroid.AbsoluteMagnitudeH;
                    worksheet.Cells[i + 2, 4].Value = asteroid.IsPotentiallyHazardousAsteroid ? "Yes":"No" ;

                    worksheet.Cells[i + 2, 5].Value = closestApproach.CloseApproachDateFull;
                    worksheet.Cells[i + 2, 6].Value = closestApproach.MissDistance.Kilometers.ToString("0.000");
                    worksheet.Cells[i + 2, 7].Value = closestApproach.MissDistance.Miles.ToString("0.000");
                    worksheet.Cells[i + 2, 8].Value = closestApproach.MissDistance.Astronomical.ToString("0.000");

                    worksheet.Cells[i + 2, 9].Value = closestApproach.RelativeVelocity.KilometersPerHour.ToString("0.");
                    worksheet.Cells[i + 2, 10].Value = closestApproach.RelativeVelocity.MilesPerHour.ToString("0.");
                    worksheet.Cells[i + 2, 11].Value = closestApproach.RelativeVelocity.KilometersPerSecond.ToString("0.");

                    worksheet.Cells[i + 2, 12].Value = asteroid.EstimatedDiameter.Miles.EstimatedDiameterMin.ToString("0.00");
                    worksheet.Cells[i + 2, 13].Value = asteroid.EstimatedDiameter.Miles.EstimatedDiameterMax.ToString("0.00");
                    worksheet.Cells[i + 2, 14].Value = asteroid.EstimatedDiameter.Meters.EstimatedDiameterMin.ToString("0.00");
                    worksheet.Cells[i + 2, 15].Value = asteroid.EstimatedDiameter.Meters.EstimatedDiameterMax.ToString("0.00");
                    worksheet.Cells[i + 2, 16].Value = asteroid.EstimatedDiameter.Feet.EstimatedDiameterMin.ToString("0.00");
                    worksheet.Cells[i + 2, 17].Value = asteroid.EstimatedDiameter.Feet.EstimatedDiameterMax.ToString("0.00");
                }
                worksheet.Cells[worksheet.Dimension.Address].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                
                var stream = new MemoryStream(package.GetAsByteArray());

                //Използваме ContentDispositionHeaderValue за да attach-нем .xlsx файла и да ориентираме браузъра, че 
                //е файл за сваляне вместо да го диплейва в браузъра
                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Asteroids.xlsx"
                };
                Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
                Response.Headers[HeaderNames.ContentType] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Asteroids.xlsx");
            }
        }
    }
}


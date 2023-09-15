using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NASA.Models;
using NASA.Models.Config;
using NASA.Services.Contracts;
using Newtonsoft.Json;

namespace NASA.Services
{
    public class NasaApis : INasaApis
    {
        public const string dateFormat = "yyyy-MM-dd";
        public const string weekKey = "WeekKey";

        private readonly IMemoryCache _cache;
        private readonly IOptions<ApiKeys> _keys;

        public NasaApis(IMemoryCache cache,IOptions<ApiKeys> keys)
        {
            _cache = cache;
            _keys = keys;
        }


        public async Task<APOD> AstronomyPictureOfTheDayAsync(DateTime date)
        {
            string formattedDate = date.ToString(dateFormat);
            try
            {
                if (_cache.TryGetValue($"{formattedDate}", out APOD apod))
                {
                    return apod;
                }
                else
                {

                    using (var client = new HttpClient())
                    {
                        
                        client.BaseAddress = new Uri("https://api.nasa.gov/");
                        var response = await client.GetAsync($"https://api.nasa.gov/planetary/apod?date={formattedDate}&api_key={_keys.Value.NasaApiKey}");
                        var stringResult = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            APOD aPicOfTheDay = JsonConvert.DeserializeObject<APOD>(stringResult);

                            _cache.Set($"{aPicOfTheDay.Date}", aPicOfTheDay, TimeSpan.FromMinutes(30));

                            return aPicOfTheDay;

                        }
                        else if ((int)response.StatusCode == StatusCodes.Status404NotFound)
                        {
                            return null;
                        }
                        else
                        {
                            throw new HttpRequestException($"Request in AstronomyPictureOfTheDay failed with status code {(int)response.StatusCode}");
                        }
                    }

                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }


        public async Task<Week> NeoWsFeedAsync(DateTime startDate)
        {
            try
            {
                if (_cache.TryGetValue(weekKey, out Week week))
                {
                    return week;
                }
                else
                {

                    using (var client = new HttpClient())
                    {
                        var formattedStartDate = startDate.ToString(dateFormat);

                        client.BaseAddress = new Uri("https://api.nasa.gov/");
                        var response = await client.GetAsync($"https://api.nasa.gov/neo/rest/v1/feed?start_date={formattedStartDate}&api_key={_keys.Value.NasaApiKey}");
                        var stringResult = await response.Content.ReadAsStringAsync();


                        if (response.IsSuccessStatusCode)
                        {
                            Week nextSevenDaysWithAsteroids = JsonConvert.DeserializeObject<Week>(stringResult);

                            _cache.Set(weekKey, nextSevenDaysWithAsteroids, TimeSpan.FromMinutes(30));

                            return nextSevenDaysWithAsteroids;

                        }
                        else
                        {
                            throw new HttpRequestException($"Request in NeoWsFeed failed with status code {(int)response.StatusCode}");
                        }

                    }
                }

            }
            catch (HttpRequestException)
            {
                throw; ;
            }
        }

    }

}

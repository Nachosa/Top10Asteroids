﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NASA.Helper.Exceptions;
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
        public const string DEMO_KEY = "DEMO_KEY";

        private static HttpClient client = new HttpClient();

        private readonly IMemoryCache _cache;
        private readonly IOptions<ApiKeys> _keys;

        public NasaApis(IMemoryCache cache, IOptions<ApiKeys> keys)
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

                    //client.BaseAddress = new Uri("https://api.nasa.gov/");
                    var response = await client.GetAsync($"https://api.nasa.gov/planetary/apod?date={formattedDate}&api_key={_keys.Value.NasaApiKey ?? DEMO_KEY}");
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
                        throw new NotSuccessfulAPICallException($"AstronomyPictureOfTheDayAsync failed with code {(int)response.StatusCode}!");
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
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
                    var formattedStartDate = startDate.ToString(dateFormat);

                    client.BaseAddress = new Uri("https://api.nasa.gov/");
                    var response = await client.GetAsync($"https://api.nasa.gov/neo/rest/v1/feed?start_date={formattedStartDate}&api_key={_keys.Value.NasaApiKey ?? DEMO_KEY}");
                    var stringResult = await response.Content.ReadAsStringAsync();


                    if (response.IsSuccessStatusCode)
                    {
                        Week nextSevenDaysWithAsteroids = JsonConvert.DeserializeObject<Week>(stringResult);

                        _cache.Set(weekKey, nextSevenDaysWithAsteroids, TimeSpan.FromMinutes(30));

                        return nextSevenDaysWithAsteroids;

                    }
                    else
                    {
                        throw new NotSuccessfulAPICallException($"NeoWsFeedAsync failed with code {(int)response.StatusCode}!");
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

    }

}

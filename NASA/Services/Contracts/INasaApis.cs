using NASA.Models;

namespace NASA.Services.Contracts
{
    public interface INasaApis
    {

        /// <summary>
        ///  Astronomy Picture of the Day - With APOD a user can:
        ///  View different image or photograph for every day, along with a brief explanation written by a professional astronomer.
        /// </summary>
        /// <param name="date">The date of APOD user wants to see.</param>
        /// <returns>Object APOD with Author,Date,Explanation,Link to the picture,Media type and API version.</returns>
        Task<APOD> AstronomyPictureOfTheDayAsync(DateTime date);

        /// <summary>
        /// Near Earth Object Web Service Feed - With NeoWs Feed a user can:
        /// Retrieve a list of Asteroids based on their closest approach date to Earth.
        /// </summary>
        /// <param name="startDate">The start date of a certain period.</param>
        /// <returns>Object Week with a Dictionary which contains the next 7 days with a collections of close approach Asteroids,
        /// links for the previous and next week and all Asteroids count.</returns>
        Task<Week> NeoWsFeedAsync(DateTime startDate);

    }
}

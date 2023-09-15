using System.Net;

namespace NASA.Helper
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _log;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    // custom application error

                    default:
                        _log.LogError(error.ToString());
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var homeErrorUrl = context.Request.PathBase + "/Home/Error";
                        context.Response.Redirect(homeErrorUrl);
                        return;
                }
            }

        }
    }
}



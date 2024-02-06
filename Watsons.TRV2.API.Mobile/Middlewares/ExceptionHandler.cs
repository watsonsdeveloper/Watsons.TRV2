namespace Watsons.TRV2.API.Mobile.Middlewares
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadHttpRequestException ex)
            {
                // Handle BadHttpRequestException here
                _logger.LogError(ex, "Bad HTTP request");

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                //context.Response.ContentType = "text/plain";


                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions here
                _logger.LogError(ex, "An unexpected error occurred");

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "text/plain";

                await context.Response.WriteAsync("Internal Server Error");
            }
        }
    }

}

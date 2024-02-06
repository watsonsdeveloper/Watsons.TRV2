using System;
using System.Text.Json;
using System.Threading.Tasks;
using Watsons.Common;

namespace Watsons.TRV2.API.Mobile.Middlewares
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            //if (context.Response.StatusCode == StatusCodes.Status200OK)
            //{
                var response = context.Items["Response"] as ServiceResult<object>;
                if (response != null)
                {
                    if (response.IsSuccess)
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response.Data));
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response.ErrorMessage));
                    }
                }
            //}
        }
    }

}

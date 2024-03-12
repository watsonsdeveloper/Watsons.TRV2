using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Watsons.Common;
using Watsons.TRV2.DTO.Portal.User;
using Watsons.TRV2.DTO.Portal.User.MfaLoginDto;
using Watsons.TRV2.Services.Portal;

namespace Watsons.TRV2.API.Portal
{
    public static class Routers
    {
        public static void ConfigureEndpoints(WebApplication app)
        {
            app.MapGet("/", () =>
            {
                return "WELCOME TO TRV2 PORTAL API";
            });

            #region portalApi

            app.MapPost("/testJWt", () =>
            {
                return Results.Ok("JWT is valid!");
            }).RequireAuthorization();

            var portalApi = app.MapGroup("/portalApi");
            portalApi.MapPost("/mfaLogin", async ([FromBody] DTO.Portal.User.MfaLoginDto.Request request, UserService service) =>
            {
                ServiceResult<DTO.Portal.User.MfaLoginDto.Response> response;
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(request);
                bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errorMessage = validationResults[0].ErrorMessage ?? string.Empty;
                    response = ServiceResult<DTO.Portal.User.MfaLoginDto.Response>.Failure(errorMessage);
                    return Results.BadRequest(response);
                }

                response = await service.MfaLogin(request);
                return Results.Ok(response);
            });

            portalApi.MapPost("/verifyMfaLoginOtp", async ([FromBody] VerifyLoginOtpRequest request, UserService service) =>
            {
                ServiceResult<string> response;
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(request);
                bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errorMessage = validationResults[0].ErrorMessage ?? string.Empty;
                    response = ServiceResult<string>.Failure(errorMessage);
                    return Results.BadRequest(response);
                }

                response = await service.VerifyMfaLoginOtp(request);
                return Results.Ok(response);
            });

            #endregion
        }
    }
}

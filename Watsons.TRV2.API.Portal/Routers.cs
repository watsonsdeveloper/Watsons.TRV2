using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Watsons.Common;
using Watsons.Common.EmailHelpers;
using Watsons.TRV2.DTO.Portal;
using Watsons.TRV2.DTO.Portal.OrderDto;
using Watsons.TRV2.DTO.Portal.User;
using Watsons.TRV2.Services.Portal;
using Watsons.TRV2.Services.RTS;

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

            app.MapPost("/testJwt", () =>
            {
                return Results.Ok("JWT is valid!");
            }).RequireAuthorization(PolicyRoles.ADMIN);

            app.MapPost("/testSendEmail", async ([FromBody] Common.EmailHelpers.DTO.SendEmailBySpParams request, UserService service) =>
            {
                if(await service.SendEmail())
                {
                    return Results.Ok("Email sent successful.");
                }
                
                return Results.Ok("Failed");
            });

            app.MapPost("/decodeJwt", async (UserService service) =>
            {
                var response = await service.DecodeJwtToken();
                return Results.Ok(response);
            }).RequireAuthorization(PolicyRoles.ADMIN);

            app.MapGet("/rts", async (int storeId, string plu, RtsService service) =>
            {
                var request = new Services.RTS.DTO.GetMultipleProductSingleStore.Request()
                {
                    StoreID = storeId,
                    PluList = new List<string>() { plu }
                };
                var response = await service.GetMultipleProductSingleStore(request);
                return response;
            });

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
                    response = ServiceResult<DTO.Portal.User.MfaLoginDto.Response>.Fail(errorMessage);
                    return Results.BadRequest(response);
                }

                response = await service.MfaLogin(request);
                return Results.Ok(response);
            });

            portalApi.MapPost("/mfaLogout", async (UserService service) =>
            {
                ServiceResult<bool> response;

                response = await service.MfaLogout();
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
                    response = ServiceResult<string>.Fail(errorMessage);
                    return Results.BadRequest(response);
                }

                response = await service.VerifyMfaLoginOtp(request);
                return Results.Ok(response);
            });

            portalApi.MapPost("/fetchUserProfile", async ([FromBody] FetchUserProfileRequest request, UserService service) =>
            {
                ServiceResult<FetchUserProfileResponse> response;
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(request);
                bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errorMessage = validationResults[0].ErrorMessage ?? string.Empty;
                    response = ServiceResult<FetchUserProfileResponse>.Fail(errorMessage);
                    return Results.BadRequest(response);
                }

                response = await service.FetchUserProfile(request);
                return Results.Ok(response);
            }).RequireAuthorization();

            portalApi.MapPost("/fetchOrderList", async ([FromBody] FetchOrderListRequest request, OrderService service) =>
            {
                ServiceResult<FetchOrderListResponse> response;
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(request);
                bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errorMessage = validationResults[0].ErrorMessage ?? string.Empty;
                    response = ServiceResult<FetchOrderListResponse>.Fail(errorMessage);
                    return Results.BadRequest(response);
                }

                response = await service.FetchOrderList(request);
                return Results.Ok(response);
           
            }).RequireAuthorization(PolicyClaims.ORDER_OWN_R, PolicyClaims.ORDER_SUPPLIER_R);

            portalApi.MapPost("/fetchOrderOwn", async ([FromBody] FetchOrderOwnRequest request, OrderService service) =>
            {
                ServiceResult<FetchOrderOwnResponse> response;
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(request);
                bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errorMessage = validationResults[0].ErrorMessage ?? string.Empty;
                    response = ServiceResult<FetchOrderOwnResponse>.Fail(errorMessage);
                    return Results.BadRequest(response);
                }

                response = await service.FetchOrderOwn(request);
                return Results.Ok(response);

            }).RequireAuthorization(PolicyClaims.ORDER_OWN_R);

            //portalApi.MapPost("/fetchOrderSupplier", async ([FromBody] VerifyLoginOtpRequest request, UserService service) =>
            //{
            //    ServiceResult<string> response;
            //    var validationResults = new List<ValidationResult>();
            //    var validationContext = new ValidationContext(request);
            //    bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

            //    if (!isValid)
            //    {
            //        var errorMessage = validationResults[0].ErrorMessage ?? string.Empty;
            //        response = ServiceResult<string>.Failure(errorMessage);
            //        return Results.BadRequest(response);
            //    }

            //    response = await service.VerifyMfaLoginOtp(request);
            //    return Results.Ok(response);
            //}).RequireAuthorization(PolicyClaims.ORDER_SUPPLIER_R);

            portalApi.MapPost("/updateOrderOwn", async ([FromBody] UpdateOrderOwnRequest request, OrderService service) =>
            {
                ServiceResult<UpdateOrderOwnResponse> response;
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(request);
                bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errorMessage = validationResults[0].ErrorMessage ?? string.Empty;
                    response = ServiceResult<UpdateOrderOwnResponse>.Fail(errorMessage);
                    return Results.BadRequest(response);
                }

                response = await service.UpdateOrdersOwn(request);
                return Results.Ok(response);

            }).RequireAuthorization(PolicyClaims.ORDER_OWN_W);

            portalApi.MapPost("/fetchReportOwn", async ([FromBody] ReportOwnRequest request, ReportService service) =>
            {
                ServiceResult<ReportOwnResponse> response;
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(request);
                bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errorMessage = validationResults[0].ErrorMessage ?? string.Empty;
                    response = ServiceResult<ReportOwnResponse>.Fail(errorMessage);
                    return Results.BadRequest(response);
                }

                response = await service.FetchReportOwn(request);
                return Results.Ok(response);

            }).RequireAuthorization(PolicyClaims.REPORT_OWN_R);

            portalApi.MapPost("/fetchReportSupplier", async ([FromBody] ReportSupplierRequest request, ReportService service) =>
            {
                ServiceResult<ReportSupplierResponse> response;
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(request);
                bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errorMessage = validationResults[0].ErrorMessage ?? string.Empty;
                    response = ServiceResult<ReportSupplierResponse>.Fail(errorMessage);
                    return Results.BadRequest(response);
                }

                response = await service.FetchReportSupplier(request);
                return Results.Ok(response);

            }).RequireAuthorization(PolicyClaims.REPORT_SUPPLIER_R);

            portalApi.MapPost("/fetchReportSupplierFulfillment", async ([FromBody] ReportSupplierFulfillmentRequest request, ReportService service) =>
            {
                ServiceResult<ReportSupplierFulfillmentResponse> response;
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(request);
                bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errorMessage = validationResults[0].ErrorMessage ?? string.Empty;
                    response = ServiceResult<ReportSupplierFulfillmentResponse>.Fail(errorMessage);
                    return Results.BadRequest(response);
                }

                response = await service.FetchReportSupplierFulfillment(request);
                return Results.Ok(response);

            }).RequireAuthorization(PolicyClaims.REPORT_FULFILLMENT_R);

            #endregion

            #region jobApi

            var jobApi = app.MapGroup("/job");
            jobApi.MapGet("/emailNotifyStoreOrderPending", async (JobService service) =>
            {
                await service.EmailNotifyStoreOrderPending();
                return Results.Ok("EmailNotifyStoreOrderPending completed.");
            });

            #endregion
        }
    }
}

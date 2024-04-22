
using Microsoft.AspNetCore.Mvc;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.DTO.Mobile;
using Watsons.TRV2.DTO.Mobile.TrCart;
using Watsons.TRV2.DTO.Mobile.TrOrder;
using Watsons.TRV2.DTO.Mobile.UploadImage;
using Watsons.TRV2.Services.Mobile;

namespace Watsons.TRV2.API.Mobile
{
    public static class Routers
    {
        public static void ConfigureEndpoints(WebApplication app)
        {
            app.MapGet("/", () =>
            {
                
                return "WELCOME TO TRV2 MOBILE API";
            });

            #region mobileApi

            var mobileApi = app.MapGroup("/mobileApi");
            mobileApi.MapGet("/searchProduct/{pluOrBarcode}", async (string pluOrBarcode, ProductService service) =>
            {
                var response = await service.SearchByPluOrBarcode(pluOrBarcode);
                return Results.Ok(response);
            });

            mobileApi.MapPost("/trCart/addToTrCart", async (AddToTrCartRequest request, TrCartService service) =>
            {
                var response = await service.AddToTrCart(request);
                return Results.Ok(response);
            });

            mobileApi.MapDelete("/trCart/removeTrCart", async ([FromBody] RemoveTrCartRequest request, TrCartService service) =>
            {
                var response = await service.RemoveTrCart(request);
                return Results.Ok(response);
            });


            mobileApi.MapGet("/trCart/getTrCart", async (long trCartId, int storeId, TrCartService service) =>
            {
                var response = await service.GetTrCart(new GetTrCartRequest()
                {
                    TrCartId = trCartId,
                    StoreId = storeId
                });
                return Results.Ok(response);
            });

            mobileApi.MapPost("/trCart/getTrCart", async ([FromBody]GetTrCartRequest request, TrCartService service) =>
            {
                var response = await service.GetTrCart(request);
                return Results.Ok(response);
            });

            mobileApi.MapGet("/trCart/getTrCartList", async (int storeId, Brand brand, TrCartService service) =>
            {
                var response = await service.GetTrCartList(new GetTrCartListRequest
                {
                    StoreId = storeId,
                    Brand = brand
                });
                return Results.Ok(response);
            });

            mobileApi.MapPost("/trCart/getTrCartList", async ([FromBody] GetTrCartListRequest request, TrCartService service) =>
            {
                var response = await service.GetTrCartList(request);
                return Results.Ok(response);
            });

            mobileApi.MapPost("/trCart/updateTrCartRequirement", async ([FromBody] UpdateTrCartRequirementRequest request, TrCartService service) =>
            {
                var response = await service.UpdateTrCartRequirement(request);
                return Results.Ok(response);
            });

            //mobileApi.MapGet("/trCart/updateTrCart", async ([FromBody] GetTrCartRequest, TrCartService service) =>
            //{
            //    //var response = await service.GetTrCart(storeId);
            //    return Results.Ok();
            //});

            mobileApi.MapPost("/trOrder/getTrOrderBatchList", async ([FromBody] GetTrOrderBatchListRequest request, TrOrderService service) =>
            {
                return Results.Ok(await service.GetTrOrderBatchList(request));
            });

            mobileApi.MapPost("/trOrder/getTrOrderList", async ([FromBody] GetTrOrderListRequest request, TrOrderService service) =>
            {
                return Results.Ok(await service.GetTrOrderList(request));
            });

            mobileApi.MapGet("/trOrder/getTrOrder", async (long trOrderId, int storeId, TrOrderService service) =>
            {
                var request = new GetTrOrderRequest(trOrderId, storeId);
                return Results.Ok(await service.GetTrOrder(request));
            });

            mobileApi.MapPost("/trOrder/getTrOrder", async ([FromBody] GetTrOrderRequest request, [FromServices] TrOrderService service) =>
            {
                return Results.Ok(await service.GetTrOrder(request));
            });

            mobileApi.MapPost("/trOrder/addToTrOrder", async ([FromBody] AddToTrOrderRequest request, [FromServices] TrOrderService service) =>
            {
                return Results.Ok(await service.AddToTrOrder(request));
            });

            mobileApi.MapGet("/uploadImage/getUploadedImageUrls", async (int storeId, long? trCartid, long? trOrderId, [FromServices] UploadImageService service) =>
            {
                var request = new GetUploadedImageUrlsRequest()
                {
                    StoreId = storeId,
                    TrCartId = trCartid,
                    TrOrderId = trOrderId,
                };
                return Results.Ok(await service.GetUploadedImageUrls(request));
            });

            mobileApi.MapPost("/uploadImage/getUploadedImageUrls", async ([FromBody] GetUploadedImageUrlsRequest request, [FromServices] UploadImageService service) =>
            {
                return Results.Ok(await service.GetUploadedImageUrls(request));
            });

            mobileApi.MapPost("/uploadImage", async ([FromBody] UploadImageRequest request, UploadImageService service) =>
            {
                return Results.Ok(await service.UploadImage(request));
            });

            mobileApi.MapDelete("/uploadImage/deleteUploadedImages", async ([FromBody] DeleteUploadedImagesRequest request, [FromServices] UploadImageService service) =>
            {
                return Results.Ok(await service.DeleteUploadedImagesByImageIds(request));
            });

            #endregion
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.DTO.Mobile;
using Watsons.TRV2.DTO.Mobile.TrOrder;
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

            var mobileApi = app.MapGroup("/mobileApi");
            mobileApi.MapGet("/searchProduct/{pluOrBarcode}", async (string pluOrBarcode, ProductService service) =>
            {
                var response = await service.SearchByPluOrBarcode(pluOrBarcode);
                return Results.Ok(response);
            });

            mobileApi.MapPost("/requestTr", async (RequestTrRequest request, TrService service) =>
            {
                var response = await service.RequestTr(request);
                return Results.Ok(response);
            });

            mobileApi.MapGet("/trList", async (int storeId, TrStatus? status, String? pluOrBarcode, TrService service) =>
            {
                var response = await service.TrList(storeId, status, pluOrBarcode);
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

            mobileApi.MapPost("/trOrder/getTrOrder", async ([FromBody] GetTrOrderRequest request, TrOrderService service) =>
            {
                return Results.Ok(await service.GetTrOrder(request));
            });

            mobileApi.MapPost("/trOrder/addToTrOrder", async ([FromBody] AddToTrOrderRequest request, TrOrderService service) =>
            {
                return Results.Ok(await service.AddToTrOrder(request));
            });

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DTO.Portal.OrderDto;

namespace Watsons.TRV2.Services.Portal
{
    public interface IOrderService
    {
        Task<ServiceResult<FetchOrderListResponse>> FetchOrderList(FetchOrderListRequest request);
        //Task<DTO.Portal.OrderDto.FetchOrderSupplierResponse> FetchOrderSupplier(DTO.Portal.OrderDto.FetchOrderSupplierRequest request);
        //Task<DTO.Portal.OrderDto.FetchOrderItemResponse> FetchOrderItem(DTO.Portal.OrderDto.FetchOrderItemRequest request);
        Task<ServiceResult<FetchOrderOwnResponse>> FetchOrderOwn(FetchOrderOwnRequest request);
        Task<ServiceResult<UpdateOrderOwnResponse>> UpdateOrdersOwn(UpdateOrderOwnRequest request);


    }
}

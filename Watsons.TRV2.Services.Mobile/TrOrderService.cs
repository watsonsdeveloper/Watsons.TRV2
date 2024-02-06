using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.Common.ImageHelpers;
using Watsons.TRV2.DA.Repositories;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.DTO.Mobile;
using Watsons.TRV2.DTO.Mobile.TrOrder;
using Watsons.TRV2.Services.RTS;

namespace Watsons.TRV2.Services.Mobile
{
    public interface ITrOrderService
    {
        Task<ServiceResult<List<TrOrderBatchDto>>> GetTrOrderBatchList(GetTrOrderBatchListRequest request);
        Task<ServiceResult<List<TrOrderDto>>> GetTrOrderList(GetTrOrderListRequest request);
        Task<ServiceResult<TrOrderDto>> GetTrOrder(GetTrOrderRequest request);
        Task<ServiceResult<List<TrCartDto>>> AddToTrOrder(AddToTrOrderRequest request);
    }
    public class TrOrderService : ITrOrderService
    {
        private readonly IMapper _mapper;
        private readonly IRtsService _rtsService;
        private readonly ITrOrderRepository _trOrderRepository;
        private readonly ITrOrderBatchRepository _trOrderBatchRepository;
        private readonly ITrCartRepository _trCartRepository;
        private readonly IItemMasterRepository _itemMasterRepository;
        private readonly IStoreMasterRepository _storeMasterRepository;

        private readonly RtsSettings _rtsSettings;
        public TrOrderService(IMapper mapper, IRtsService rtsService,
            ITrOrderRepository trOrderRepository, ITrOrderBatchRepository trOrderBatchRepository,
            ITrCartRepository trCartRepository,
            IItemMasterRepository itemMasterRepository, IStoreMasterRepository storeMasterRepository,
            IOptions<RtsSettings> rtsSettings)
        {
            _mapper = mapper;
            _rtsService = rtsService;
            _trOrderRepository = trOrderRepository;
            _trOrderBatchRepository = trOrderBatchRepository;
            _trCartRepository = trCartRepository;
            _itemMasterRepository = itemMasterRepository;
            _storeMasterRepository = storeMasterRepository;

            _rtsSettings = rtsSettings.Value;
        }

        public async Task<ServiceResult<List<TrOrderBatchDto>>> GetTrOrderBatchList(GetTrOrderBatchListRequest request)
        {
            byte? status = request.Status != TrOrderBatchStatus.All ? (byte)request.Status : null;
            var trOrderBatchList = await _trOrderBatchRepository.List(new List<int>() { request.StoreId }, (byte)request.Brand, status);

            var trOrderBatchListDto = _mapper.Map<List<TrOrderBatchDto>>(trOrderBatchList);
            return ServiceResult<List<TrOrderBatchDto>>.Success(trOrderBatchListDto);
        }

        public async Task<ServiceResult<List<TrOrderDto>>> GetTrOrderList(GetTrOrderListRequest request)
        {
            var trOrderBatch = await _trOrderBatchRepository.Select(request.TrOrderBatchId, request.StoreId);
            if (trOrderBatch == null)
                return ServiceResult<List<TrOrderDto>>.Failure("TrOrderBatch not found");

            byte? status = request.Status != TrOrderStatus.All ? (byte)request.Status : null;
            var trOrderList = await _trOrderRepository.List(request.TrOrderBatchId, status, request.PluOrBarcode);

            var trOrderListDto = _mapper.Map<List<TrOrderDto>>(trOrderList);
            return ServiceResult<List<TrOrderDto>>.Success(trOrderListDto);
        }

        public async Task<ServiceResult<TrOrderDto>> GetTrOrder(GetTrOrderRequest request)
        {
            var trOrder = await _trOrderRepository.Select(request.TrOrderId);
            if (trOrder == null)
                return ServiceResult<TrOrderDto>.Failure("TrOrder not found");

            var trOrderBatch = await _trOrderBatchRepository.Select(trOrder.TrOrderBatchId, request.StoreId);
            if (trOrderBatch == null)
                return ServiceResult<TrOrderDto>.Failure("TrOrder not found");

            // TODO : uploaded image
            //var uploadedImages = await 

            var trOrderDto = _mapper.Map<TrOrderDto>(trOrder);

            var item = await _itemMasterRepository.SearchByPlu(trOrder.Plu);
            trOrderDto.ProductName = item?.RetekItemDesc;

            return ServiceResult<TrOrderDto>.Success(trOrderDto);
        }

        public async Task<ServiceResult<List<TrCartDto>>> AddToTrOrder(AddToTrOrderRequest request)
        {
            var trCartList = await _trCartRepository.List(request.StoreId, (byte)request.Brand);
            if (trCartList == null || trCartList.Count() <= 0)
            {
                return ServiceResult<List<TrCartDto>>.Failure("No item in cart.");
            }

            var trCartIdList = request.TrCartDtoList.Select(c => c.TrCartId).ToList();

            var trOrderPendingList = await _trOrderRepository.List(request.StoreId, (byte)TrOrderBatchStatus.Pending);

            var monthlyRequestedPlus = new List<string>();
            var monthlyRequestedOrder = await _trOrderRepository.GetStoreMonthlyTrOrders(request.StoreId, (byte)request.Brand);
            if (monthlyRequestedOrder?.TrOrderList != null && monthlyRequestedOrder?.TrOrderList.Count >= 0)
            {
                monthlyRequestedPlus = monthlyRequestedOrder!.TrOrderList.Select(o => o.Plu).ToList();
            }

            var requestPluList = request.TrCartDtoList.Select(c => c.Plu).ToList();
            var rtsDto = await _rtsService.GetMultipleProductSingleStore(new RTS.DTO.GetMultipleProductSingleStore.Request
            {
                StoreID = request.StoreId,
                PluList = requestPluList
            });

            var hasError = false;
            foreach (var cart in request.TrCartDtoList)
            {
                var rtsProduct = rtsDto?.FirstOrDefault(rts => rts.Plu == cart.Plu);

                if (!trCartIdList.Contains(cart.TrCartId))
                {
                    // check if the item is in cart
                    cart.ErrorMessage = "Item is not in cart";
                    hasError = true;
                }
                else if (trOrderPendingList.Any(o => o.Plu == cart.Plu))
                {
                    // check if the item is in pending order
                    cart.ErrorMessage = "Item in pending order";
                    hasError = true;
                }
                else if (monthlyRequestedPlus.Contains(cart.Plu) && cart.Justification.IsNullOrEmpty())
                {
                    // check if the item is in monthly requested order
                    cart.ErrorMessage = "Required justify";
                    cart.RequireJustify = true;
                    hasError = true;
                }
                else if (rtsProduct != null && rtsProduct.AvailableStock <= _rtsSettings.MinStockRequired)
                {
                    // check if the item is in stock
                    cart.ErrorMessage = "Not enough stock";
                    cart.IsAvailableStock = false;
                    hasError = true;
                }
            }

            if (hasError)
            {
                return ServiceResult<List<TrCartDto>>.FailureData(request.TrCartDtoList, "Error in cart");
            }

            var trCart = trCartList.FirstOrDefault();
            var trOrderBatchId = Guid.NewGuid().ToString();
            TrOrderBatch trOrderBatch = new TrOrderBatch
            {
                TrOrderBatchId = trOrderBatchId,
                StoreId = request.StoreId,
                BrandId = (byte)request.Brand,
                Status = (byte)TrOrderBatchStatus.Pending,
                CreatedBy = request.CreatedBy
            };
            await _trOrderBatchRepository.Insert(trOrderBatch);

            var trOrderList = new List<TrOrder>();
            foreach (var cart in trCartList)
            {
                var trOrder = new TrOrder
                {
                    TrOrderBatchId = trOrderBatchId,
                    ProductName = cart.ProductName,
                    BrandName = cart.BrandName,
                    Plu = cart.Plu,
                    Barcode = cart.Barcode,
                    SupplierName = cart.SupplierName,
                    SupplierCode = cart.SupplierCode,
                    CreatedBy = cart.CreatedBy,
                    RequireJustify = cart.RequireJustify,
                    Reason = cart.Reason,
                    Justification = cart.Justification,
                    Status = (byte)TrOrderStatus.Pending
                };
                trOrderList.Add(trOrder);
                // TODO: transfer image from cart to order
            }

            await _trOrderRepository.InsertRangeTrOrder(trOrderList);

            // remove from cart
            await _trCartRepository.DeleteRange(trCartIdList);

            // TODO : adjustment stock

            return ServiceResult<List<TrCartDto>>.Success(request.TrCartDtoList);
        }
    }
}
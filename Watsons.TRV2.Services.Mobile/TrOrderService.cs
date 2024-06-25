using AutoMapper;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.Common.ImageHelpers;
using Watsons.TRV2.DA.MyMaster.Repositories;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models.Order;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.DTO.Mobile;
using Watsons.TRV2.DTO.Mobile.TrOrder;
using Watsons.TRV2.DTO.Mobile.UploadImage;
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
        private readonly IUploadImageService _uploadImageService;
        private readonly ITrOrderRepository _trOrderRepository;
        private readonly ITrOrderBatchRepository _trOrderBatchRepository;
        private readonly ITrCartRepository _trCartRepository;
        private readonly IItemMasterRepository _itemMasterRepository;
        private readonly IStoreMasterRepository _storeMasterRepository;
        private readonly IStoreSalesBandRepository _storeSalesBandRepository;

        private readonly ImageSettings _imageSettings;
        private readonly RtsSettings _rtsSettings;
        public TrOrderService(IMapper mapper, IRtsService rtsService, IUploadImageService uploadImageService,
            ITrOrderRepository trOrderRepository, ITrOrderBatchRepository trOrderBatchRepository,
            ITrCartRepository trCartRepository,
            IItemMasterRepository itemMasterRepository, IStoreMasterRepository storeMasterRepository,
            IStoreSalesBandRepository storeSalesBandRepository,
            IOptions<ImageSettings> imageSettings,
            IOptions<RtsSettings> rtsSettings)
        {
            _mapper = mapper;
            _rtsService = rtsService;
            _uploadImageService = uploadImageService;
            _trOrderRepository = trOrderRepository;
            _trOrderBatchRepository = trOrderBatchRepository;
            _trCartRepository = trCartRepository;
            _itemMasterRepository = itemMasterRepository;
            _storeMasterRepository = storeMasterRepository;
            _storeSalesBandRepository = storeSalesBandRepository;

            _imageSettings = imageSettings.Value;
            _rtsSettings = rtsSettings.Value;
        }

        public async Task<ServiceResult<List<TrOrderBatchDto>>> GetTrOrderBatchList(GetTrOrderBatchListRequest request)
        {
            var parameters = new OrderBatchList()
            {
                BrandId = request.Brand != null ? (byte)request.Brand : null,
                TrOrderBatchId = request.TrOrderBatchId,
                StoreIds = new List<int>() { request.StoreId },
                TrOrderBatchStatus = request.TrOrderBatchStatus != null ? (byte)request.TrOrderBatchStatus : null,
                PluOrBarcode = request.PluOrBarcode,
                Page = request.Page,
                PageSize = request.PageSize,
            };
            var trOrderBatchList = await _trOrderBatchRepository.List(parameters);

            var trOrderBatchListDto = _mapper.Map<List<TrOrderBatchDto>>(trOrderBatchList);
            return ServiceResult<List<TrOrderBatchDto>>.Success(trOrderBatchListDto);
        }

        public async Task<ServiceResult<List<TrOrderDto>>> GetTrOrderList(GetTrOrderListRequest request)
        {
            var trOrderBatch = await _trOrderBatchRepository.Select(request.TrOrderBatchId, request.StoreId);
            if (trOrderBatch == null)
                return ServiceResult<List<TrOrderDto>>.Fail("TrOrderBatch not found");

            byte? status = request.Status != TrOrderStatus.All ? (byte)request.Status : null;
            ListSearchParams parameters = new ListSearchParams()
            {
                TrOrderBatchId = request.TrOrderBatchId,
                TrOrderStatus = status,
                PluOrBarcode = request.PluOrBarcode,
                StoreIds = new List<int>() { request.StoreId },
                Brand = (byte)request.Brand,
            };
            var trOrderList = await _trOrderRepository.ListSearch(parameters);

            var trOrderListDto = _mapper.Map<List<TrOrderDto>>(trOrderList);
            return ServiceResult<List<TrOrderDto>>.Success(trOrderListDto);
        }

        public async Task<ServiceResult<TrOrderDto>> GetTrOrder(GetTrOrderRequest request)
        {
            var trOrder = await _trOrderRepository.Select(request.TrOrderId);
            if (trOrder == null)
                return ServiceResult<TrOrderDto>.Fail("TrOrder not found");

            var trOrderBatch = await _trOrderBatchRepository.Select(trOrder.TrOrderBatchId, request.StoreId);
            if (trOrderBatch == null)
                return ServiceResult<TrOrderDto>.Fail("TrOrder not found");

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
                return ServiceResult<List<TrCartDto>>.Fail("No item in cart.");
            }
            var trDtoCartList = _mapper.Map<List<TrCartDto>>(trCartList);
            var trCartIdList = trDtoCartList.Select(c => c.TrCartId).ToList();

            var trOrderPendingList = await _trOrderRepository.List(request.StoreId, (byte)TrOrderBatchStatus.Pending);
            Dictionary<string, TrOrder> trOrderPendingPluList = trOrderPendingList?.ToDictionary(o => o.Plu, o => o);

            var pluList = trDtoCartList.Select(c => c.Plu).ToList();
            Dictionary<string, int>? rtsDictionary = null;
            try
            {
                rtsDictionary = await _rtsService.GetMultipleProductSingleStore(new RTS.DTO.GetMultipleProductSingleStore.Request
                {
                    StoreID = request.StoreId,
                    PluList = pluList
                });
                if (rtsDictionary == null || !rtsDictionary.Any())
                {
                    return ServiceResult<List<TrCartDto>>.Fail("Store Sales Band Not Found.");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<List<TrCartDto>>.Fail("Error while getting RTS stock");
            }

            var storeSalesBand = await _storeSalesBandRepository.GetStoreSalesBandDetails(request.StoreId);
            if (storeSalesBand == null)
            {
                return ServiceResult<List<TrCartDto>>.Fail("Store Sales Band Not Found.");
            }
            var storePluCapped = storeSalesBand?.PluCapped ?? 0;
            var monthlyStoreOrder = await _trOrderRepository.GetProductQuantityOfMonthlyStoreOrder(request.StoreId, (byte)request.Brand);

            var totalUploadedImagesList = await _uploadImageService.ListByStore(new ListByStoreRequest
            {
                StoreId = request.StoreId,
                Brand = request.Brand
            });
            var totalUploadedImages = totalUploadedImagesList.Data.Where(i => i.TrCartId != null).GroupBy(i => i.TrCartId).Select(i => i.Count()).ToList();
            var totalUploadedImagesDictionary = totalUploadedImagesList.Data.Where(i => i.TrCartId != null).GroupBy(i => i.TrCartId).ToDictionary(i => i.Key, i => i.Count());

            var requiredJustifyPluList = new Dictionary<string, bool>();
            var hasError = false;
            foreach (var cart in trDtoCartList)
            {
                if (!trCartIdList.Contains(cart.TrCartId))
                {
                    // check if the item is in cart
                    cart.ErrorMessage = "Item is not in cart";
                    hasError = true;
                    continue;
                }

                // check if the item is tester product
                //if (!item.ItemStatus.Contains("1") && !item.ItemStatus.Contains("2"))
                //{
                //    return ServiceResult<TrCartDto>.Failure("Product is not tester product.");
                //}

                var itemStoreOrdered = monthlyStoreOrder.TryGetValue(cart.Plu, out var itemQuantityOrdered) ? itemQuantityOrdered : 0;
                if (itemStoreOrdered > 0 && itemStoreOrdered > storePluCapped && cart.Justification.IsNullOrEmpty())
                {
                    // check if the item is in monthly requested order
                    cart.ErrorMessage = "Remark required.";
                    cart.RequireJustify = true;
                    hasError = true;
                    requiredJustifyPluList.Add(cart.Plu, true);
                }
                else
                {
                    requiredJustifyPluList.Add(cart.Plu, false);
                    cart.RequireJustify = false;
                }

                if (request.Brand == Brand.Own && rtsDictionary.ContainsKey(cart.Plu) && rtsDictionary[cart.Plu] <= _rtsSettings.MinStockRequired)
                {
                    cart.ErrorMessage = "SOH is less than 4 units. Please request later after restock.";
                    cart.IsAvailableStock = false;
                    hasError = true;
                    continue;
                }
                else
                {
                    cart.IsAvailableStock = true;
                }

                if (cart.Reason == null)
                {
                    cart.ErrorMessage = "Reason required";
                    hasError = true;
                    continue;
                }
                else if (cart.Reason != TrReason.NewListing && cart.Reason != TrReason.Missing)
                {
                    //check min image upload if reason not new listing or missing
                    if (totalUploadedImagesDictionary[cart.TrCartId] < _imageSettings.MinImageUpload)
                    {
                        cart.ErrorMessage = $"Minimum {_imageSettings.MinImageUpload} {(_imageSettings.MinImageUpload == 1 ? "image" : "images")} to be uploaded.";
                        hasError = true;
                    }
                }

                if (trOrderPendingPluList != null && trOrderPendingPluList.ContainsKey(cart.Plu))
                {
                    // check if the item is in pending order
                    cart.ErrorMessage = "Item in order upon approval";
                    hasError = true;
                    continue;
                }
            }

            if (hasError)
            {
                return ServiceResult<List<TrCartDto>>.FailureData(trDtoCartList, "Error in cart");
            }

            var trCart = trDtoCartList.FirstOrDefault();
            TrOrderBatch trOrderBatch = new TrOrderBatch
            {
                StoreId = request.StoreId,
                Brand = (byte)request.Brand,
                TrOrderBatchStatus = (byte)TrOrderBatchStatus.Pending,
                CreatedBy = request.CreatedBy
            };
            await _trOrderBatchRepository.Insert(trOrderBatch);

            var trOrderList = new List<TrOrder>();
            foreach (var cart in trDtoCartList)
            {
                var trOrder = new TrOrder
                {
                    ProductName = cart.ProductName,
                    BrandName = cart.BrandName,
                    Plu = cart.Plu,
                    Barcode = cart.Barcode,
                    SupplierName = cart.SupplierName,
                    SupplierCode = cart.SupplierCode,
                    CreatedBy = cart.CreatedBy,
                    CreatedAt = cart.CreatedAt,
                    IsRequireJustify = requiredJustifyPluList[cart.Plu],
                    Reason = cart.Reason != null ? (byte)cart.Reason : null,
                    Justification = cart.Justification,
                    //SalesBandPluCappedSnapshot = storePluCapped,
                    TrOrderStatus = (byte)TrOrderStatus.Pending,
                    TrOrderBatchId = trOrderBatch.TrOrderBatchId,
                    TrCartId = cart.TrCartId
                };
                await _trOrderRepository.InsertTrOrder(trOrder);
                trOrderList.Add(trOrder);

                var transferImages = new TransferImageFromCartToOrderRequest()
                {
                    StoreId = request.StoreId,
                    TrCartId = cart.TrCartId,
                    TrOrderId = trOrder.TrOrderId,
                };
                await _uploadImageService.TransferImageFromCartToOrder(transferImages);
            }

            //await _trOrderRepository.InsertRangeTrOrder(trOrderList);

            await _trCartRepository.DeleteRange(trCartIdList);

            return ServiceResult<List<TrCartDto>>.Success(trDtoCartList);
        }
    }
}
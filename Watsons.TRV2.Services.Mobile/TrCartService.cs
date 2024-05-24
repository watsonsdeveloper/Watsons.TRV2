using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common.ImageHelpers;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.DTO.Mobile;
using Brand = Watsons.TRV2.DTO.Common.Brand;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.Services.RTS;
using Watsons.TRV2.DTO.Mobile.UploadImage;
using Watsons.TRV2.DTO.Mobile.TrCart;
using Watsons.TRV2.DTO.Mobile.UploadedImage;
using Watsons.TRV2.DA.MyMaster.Repositories;
using Watsons.TRV2.DA.TR.Models.SalesBand;

namespace Watsons.TRV2.Services.Mobile
{
    public interface ITrCartService
    {
        Task<ServiceResult<TrCartDto>> GetTrCart(GetTrCartRequest request);
        Task<ServiceResult<List<TrCartDto>>> GetTrCartList(GetTrCartListRequest request);
        Task<ServiceResult<TrCartDto>> AddToTrCart(AddToTrCartRequest request);
        Task<ServiceResult<TrCartDto>> RemoveTrCart(RemoveTrCartRequest request);
        Task<ServiceResult<TrCartDto>> UpdateTrCartRequirement(UpdateTrCartRequirementRequest request);

    }
    public class TrCartService : ITrCartService
    {
        private readonly IMapper _mapper;
        private readonly IRtsService _rtsService;
        private readonly IUploadImageService _uploadImageService;
        private readonly ITrOrderRepository _trOrderRepository;
        private readonly ITrCartRepository _trCartRepository;
        private readonly ITrImageRepository _trImageRepository;
        private readonly IItemMasterRepository _itemMasterRepository;
        private readonly IStoreMasterRepository _storeMasterRepository;
        private readonly IStoreSalesBandRepository _storeSalesBandRepository;
        private readonly IImageHelper _imageHelper;

        private readonly ImageSettings _imageSettings;
        private readonly RtsSettings _rtsSettings;
        public TrCartService(IMapper mapper, IRtsService rtsService,
            IUploadImageService uploadImageService,
            ITrOrderRepository trOrderRepository, ITrCartRepository trCartRepository,
            ITrImageRepository trImageRepository, IItemMasterRepository itemMasterRepository,
            IStoreMasterRepository storeMasterRepository, IStoreSalesBandRepository storeSalesBandRepository,
            IImageHelper imageHelper,
            IOptions<ImageSettings> imageSettings, IOptions<RtsSettings> rtsSettings)
        {
            _mapper = mapper;
            _rtsService = rtsService;
            _uploadImageService = uploadImageService;
            _trOrderRepository = trOrderRepository;
            _trCartRepository = trCartRepository;
            _trImageRepository = trImageRepository;
            _itemMasterRepository = itemMasterRepository;
            _storeMasterRepository = storeMasterRepository;
            _storeSalesBandRepository = storeSalesBandRepository;
            _imageHelper = imageHelper;

            _imageSettings = imageSettings.Value;
            _rtsSettings = rtsSettings.Value;
        }

        /// <summary>
        /// Update trCartDto
        /// 1. IsAvailableStock from RTS
        /// 2. RequireJustify from StoreSalesBand
        /// </summary>
        /// <param name="trCartDto"></param>
        /// <param name="rtsDict"></param>
        /// <param name="monthlyStoreOrder"></param>
        /// <param name="storeSalesBandDto"></param>
        /// <returns></returns>
        private async Task<TrCartDto> MapTrCartDto(TrCartDto trCartDto, Dictionary<string, int>? rtsDict,
            Dictionary<string, int>? monthlyStoreOrder, StoreSalesBandDto? storeSalesBandDto, List<TrImage>? uploadedImages)
        {
            #region check if product available stock in RTS
            if (trCartDto.Brand == Brand.Supplier)
            {
                trCartDto.IsAvailableStock = true;
            }
            else
            {
                if (rtsDict == null)
                {
                    rtsDict = await _rtsService.GetMultipleProductSingleStore(
                        new RTS.DTO.GetMultipleProductSingleStore.Request()
                        {
                            StoreID = trCartDto.StoreId,
                            PluList = [trCartDto.Plu],
                        });
                }
                var rts = rtsDict?[trCartDto.Plu];
                if (rts != null && rts > _rtsSettings.MinStockRequired)
                {
                    trCartDto.IsAvailableStock = true;
                }
                else
                {
                    trCartDto.IsAvailableStock = false;
                }
            }
            #endregion

            #region justification check if sales band threshold within a month
            trCartDto.RequireJustify = true;
            if (storeSalesBandDto == null)
            {
                var storeSalesBand = await _storeSalesBandRepository.GetStoreSalesBandDetails(trCartDto.StoreId);
                if (storeSalesBand == null)
                {
                    return trCartDto;
                }
                storeSalesBandDto = _mapper.Map<StoreSalesBandDto>(storeSalesBand);
            }

            // check if product has requested within a month
            if (monthlyStoreOrder == null)
            {
                monthlyStoreOrder = await _trOrderRepository.GetProductQuantityOfMonthlyStoreOrder(trCartDto.StoreId, (byte)trCartDto.Brand);
            }

            if (monthlyStoreOrder.ContainsKey(trCartDto.Plu) && monthlyStoreOrder[trCartDto.Plu] > storeSalesBandDto.PluCapped)
            {
                trCartDto.RequireJustify = true;
            }
            else
            {
                trCartDto.RequireJustify = false;
            }
            #endregion

            if (uploadedImages == null)
            {
                uploadedImages = await _trImageRepository.ListByTrCartId(trCartDto.TrCartId) as List<TrImage>;
            }

            if (uploadedImages != null && uploadedImages.Count > 0)
            {
                // remove those not belong to this trCartId
                uploadedImages = uploadedImages.Where(i => i.TrCartId == trCartDto.TrCartId).ToList();

                trCartDto.UploadedImages = _mapper.Map<List<UploadedImageDto>>(uploadedImages);
            }

            return trCartDto;
        }

        public async Task<ServiceResult<TrCartDto>> GetTrCart(GetTrCartRequest request)
        {
            var trCart = await _trCartRepository.Select(request.TrCartId, request.StoreId);
            if (trCart == null)
            {
                return ServiceResult<TrCartDto>.Fail("Cart Not Found.");
            }

            var trCartDto = _mapper.Map<TrCartDto>(trCart);

            trCartDto = await MapTrCartDto(trCartDto, null, null, null, null);

            return ServiceResult<TrCartDto>.Success(trCartDto);
        }

        public async Task<ServiceResult<List<TrCartDto>>> GetTrCartList(GetTrCartListRequest request)
        {
            var store = await _storeMasterRepository.SelectStore(request.StoreId);
            if (store == null)
            {
                return ServiceResult<List<TrCartDto>>.Fail("Store Not Found.");
            }

            var trCartList = await _trCartRepository.List(store.StoreId, (byte)request.Brand);
            if (trCartList == null && !trCartList!.Any())
            {
                return ServiceResult<List<TrCartDto>>.Fail("Product Not Found.");
            }
            var trCartIds = trCartList.Select(c => c.TrCartId).ToList();

            Dictionary<string, int>? rtsDictionary = null;
            if (request.Brand == Brand.Own)
            {
                // check if product available stock in RTS
                rtsDictionary = await _rtsService.GetMultipleProductSingleStore(new RTS.DTO.GetMultipleProductSingleStore.Request()
                {
                    StoreID = store.StoreId,
                    PluList = trCartList!.Select(c => c.Plu).ToList(),
                });
            }

            var storeSalesBand = await _storeSalesBandRepository.GetStoreSalesBandDetails(store.StoreId);
            if (storeSalesBand == null)
            {
                return ServiceResult<List<TrCartDto>>.Fail("Store Sales Band Not Found.");
            }
            var storeSalesBandDto = _mapper.Map<StoreSalesBandDto>(storeSalesBand);

            var uploadedImages = await _trImageRepository.ListByTrCartIds(trCartIds) as List<TrImage>;

            // check if product has been requested within a month
            var monthlyStoreOrder = await _trOrderRepository.GetProductQuantityOfMonthlyStoreOrder(request.StoreId, (byte)request.Brand);

            var trCartListDto = _mapper.Map<List<TrCartDto>>(trCartList);

            for (int i = 0; i < trCartListDto.Count; i++)
            {
                trCartListDto[i] = await MapTrCartDto(trCartListDto[i], rtsDictionary, monthlyStoreOrder, storeSalesBandDto, uploadedImages);
            }

            return ServiceResult<List<TrCartDto>>.Success(trCartListDto);
        }

        public async Task<ServiceResult<TrCartDto>> AddToTrCart(AddToTrCartRequest request)
        {
            var store = await _storeMasterRepository.SelectStore(request.StoreId);
            if (store == null)
            {
                return ServiceResult<TrCartDto>.Fail("Store Not Found.");
            }

            var item = await _itemMasterRepository.SearchByPluOrBarcode(request.PluOrBarcode);
            if (item == null)
            {
                return ServiceResult<TrCartDto>.Fail("Product Not Found.");
            }

            var isTesterProducts = new List<int>() { 1, 2, 3 };
            if (!isTesterProducts.Contains(item.Trid ?? 0))
            {
                return ServiceResult<TrCartDto>.Fail("This PLU is not available for tester request.");
            }

            if (item.ItemStatus == null)
            {
                return ServiceResult<TrCartDto>.Fail("This PLU is not available for tester request.");
            }
            else if (request.Brand == Brand.Own && !item.ItemStatus.Contains("1") && !item.ItemStatus.Contains("2") && !item.ItemStatus.Contains("3"))
            {
                return ServiceResult<TrCartDto>.Fail("Item Status - Deleted.");
            }
            else if (request.Brand == Brand.Supplier && !item.ItemStatus.Contains("1") && !item.ItemStatus.Contains("2"))
            {
                return ServiceResult<TrCartDto>.Fail("Item Status – Deleted or De-listed.");
            }

            var ownBrandLabel = new List<int>() { 1, 2, 4, 5, 6 };
            var supplierBrandLabel = new List<int>() { 3, 9 };
            var ownBrandName = new List<string>() { "Baobab", "C.CODE" };
            if (request.Brand == Brand.Own)
            {
                if (item.Brand != null && ownBrandName.Contains(item.Brand))
                {
                }
                else if (item.LabelType != null && ownBrandLabel.Contains(item.LabelType ?? 0))
                {
                }
                else
                {
                    return ServiceResult<TrCartDto>.Fail("This PLU is not listed under Own Label.");
                }
            }
            else if (request.Brand == Brand.Supplier)
            {
                if (item.Brand != null && ownBrandName.Contains(item.Brand))
                {
                    return ServiceResult<TrCartDto>.Fail("This PLU is not listed under Other Brands.");
                }
                else if (item.LabelType != null && !supplierBrandLabel.Contains(item.LabelType ?? 0))
                {
                    return ServiceResult<TrCartDto>.Fail("This PLU is not listed under Other Brands.");
                }
            }

            // supplier accept dept is "3 – Skincare" or "4 –Cosmetic"
            if (request.Brand == Brand.Supplier)
            {
                if (item.Dept == null || (!item.Dept.StartsWith("3") && !item.Dept.StartsWith("4")))
                {
                    return ServiceResult<TrCartDto>.Fail("This PLU is not available for tester request.");
                }
            }

            // Check Cart Pending
            if (await _trCartRepository.HasInCart(store.StoreId, item.Item))
            {
                return ServiceResult<TrCartDto>.Fail("Product is listed in cart.");
            }

            // Check Order Pending
            if (await _trOrderRepository.HasOrderPending(store.StoreId, item.Item))
            {
                return ServiceResult<TrCartDto>.Fail("You have submitted a request for this PLU.");
            }

            if (request.Brand == Brand.Supplier && await _trOrderRepository.HasOrderProcessed(store.StoreId, item.Item))
            {
                return ServiceResult<TrCartDto>.Fail("Product is proccessed by supplier.");
            }

            if (request.Brand == Brand.Own)
            {
                // check sales band
                var storeSalesBand = await _storeSalesBandRepository.GetTypeValue(request.StoreId);
                if (storeSalesBand == null || storeSalesBand.Count == 0)
                {
                    return ServiceResult<TrCartDto>.Fail("Store sales band not found.");
                }

                var pluUnitLimit = storeSalesBand[SalesBandConstants.PLU_UNIT_LIMIT_OWN].Value;
                if (pluUnitLimit == null)
                {
                    return ServiceResult<TrCartDto>.Fail("Store sales band not found.");
                }

                Dictionary<string, int>? rtsDictionary;
                try
                {
                    var rtsRequest = new RTS.DTO.GetMultipleProductSingleStore.Request()
                    {
                        StoreID = store.StoreId,
                        PluList = new List<string>() { item.Item }
                    };
                    rtsDictionary = await _rtsService.GetMultipleProductSingleStore(rtsRequest);
                }
                catch (Exception ex)
                {
                    return ServiceResult<TrCartDto>.Fail("Error while getting multiple product single store");
                }

                if (rtsDictionary == null || !rtsDictionary.Any())
                {
                    return ServiceResult<TrCartDto>.Fail("Product not found in RTS.");
                }
                else if (rtsDictionary[item.Item] <= _rtsSettings.MinStockRequired)
                {
                    return ServiceResult<TrCartDto>.Fail($"SOH is less than {_rtsSettings.MinStockRequired + 1} units. Please request later after restock.");
                }
            }

            if (request.Brand == Brand.Supplier)
            {
                // check sales band
                var storeSalesBand = await _storeSalesBandRepository.GetTypeValue(request.StoreId);
                if (storeSalesBand == null || storeSalesBand.Count == 0)
                {
                    return ServiceResult<TrCartDto>.Fail("Store sales band not found.");
                }
                //string salesBand = Enum.GetName(typeof(DTO.Common.SalesBand), DTO.Common.SalesBand.PLU_UNIT_LIMIT_SUPPLIER);
                //var pluUnitLimit = storeSalesBand[SalesBandConstants.PLU_UNIT_LIMIT_SUPPLIER].Value;
                //if (pluUnitLimit == null)
                //{
                //    return ServiceResult<TrCartDto>.Fail("Store sales band not found.");
                //}
                decimal pluUnitLimit = -1;
                if(item.Dept.StartsWith("3"))
                {
                    if (!storeSalesBand.ContainsKey(SalesBandConstants.PLU_UNIT_LIMIT_SKINCARE))
                    {
                        return ServiceResult<TrCartDto>.Fail("Store sales band not found.");
                    }
                    pluUnitLimit = storeSalesBand[SalesBandConstants.PLU_UNIT_LIMIT_SKINCARE].Value;
                   
                }
                else if (item.Dept.StartsWith("4"))
                {
                    if (!storeSalesBand.ContainsKey(SalesBandConstants.PLU_UNIT_LIMIT_COSMETIC))
                    {
                        return ServiceResult<TrCartDto>.Fail("Store sales band not found.");
                    }
                    pluUnitLimit = storeSalesBand[SalesBandConstants.PLU_UNIT_LIMIT_COSMETIC].Value;
                }
                else
                {
                    return ServiceResult<TrCartDto>.Fail("Plu limit is not set");
                }

                var monthlyStoreOrder = await _trOrderRepository.GetProductQuantityOfMonthlyStoreOrder(request.StoreId, (byte)request.Brand);
                monthlyStoreOrder.TryGetValue(item.Item, out var monthlyOrderedUnit);
                if (monthlyOrderedUnit >= pluUnitLimit)
                {
                    return ServiceResult<TrCartDto>.Fail("You have exceeded the limit set.");
                }
            }

            var trCart = new TrCart()
            {
                ProductName = item.EcomItemDesc ?? item.RetekItemDesc,
                BrandName = item.Brand,
                Plu = item.Item,
                Barcode = item.Barcode,
                StoreId = store.StoreId,
                Brand = (byte)request.Brand,
                CreatedBy = request.CreatedBy,
                SupplierCode = item.SupplierCode,
                SupplierName = item.SupplierName,
            };
            await _trCartRepository.Insert(trCart);

            var trCartDto = _mapper.Map<TrCartDto>(trCart);
            trCartDto = await MapTrCartDto(trCartDto, null, null, null, null);

            return ServiceResult<TrCartDto>.Success(trCartDto);

        }

        public async Task<ServiceResult<TrCartDto>> RemoveTrCart(RemoveTrCartRequest request)
        {
            var store = await _storeMasterRepository.SelectStore(request.StoreId);
            if (store == null)
            {
                return ServiceResult<TrCartDto>.Fail("Store Not Found.");
            }

            var trCart = await _trCartRepository.Select(request.TrCartId, store.StoreId);
            if (trCart == null)
            {
                return ServiceResult<TrCartDto>.Fail("Cart Not Found.");
            }

            var uploadedImages = await _trImageRepository.ListByTrCartId(request.TrCartId);
            if (uploadedImages.Any())
            {
                var isImagesDeleted = await _uploadImageService.DeleteAllUploadedImagesByTrCartId(new DeleteAllUploadedImagesRequest()
                {
                    TrCartId = trCart.TrCartId,
                    StoreId = store.StoreId
                });

                if (!isImagesDeleted.IsSuccess)
                    return ServiceResult<TrCartDto>.Fail("Failed to remove product images from cart.");
            }

            var isDeleted = await _trCartRepository.Delete(trCart);
            if (!isDeleted)
            {
                return ServiceResult<TrCartDto>.Fail("Failed to remove product from cart.");
            }

            var trCartDto = _mapper.Map<TrCartDto>(trCart);
            return ServiceResult<TrCartDto>.Success(trCartDto);
        }

        public async Task<ServiceResult<TrCartDto>> UpdateTrCartRequirement(UpdateTrCartRequirementRequest request)
        {
            try
            {
                var trCart = await _trCartRepository.Select(request.TrCartId, request.StoreId);
                if (trCart == null)
                {
                    return ServiceResult<TrCartDto>.Fail("Cart not found.");
                }

                var uploadedImages = await _uploadImageService.ListByTrCartId(new ListByTrCartIdRequest()
                {
                    TrCartId = request.TrCartId,
                    StoreId = request.StoreId
                });

                if (request.Reason != TrReason.Missing && request.Reason != TrReason.NewListing)
                {
                    if (uploadedImages.Data.Count() < _imageSettings.MinImageUpload)
                    {
                        return ServiceResult<TrCartDto>.Fail($"Minimum {_imageSettings.MinImageUpload} {(_imageSettings.MinImageUpload == 1 ? "image" : "images")} to be uploaded.");
                    }
                }
                else if (request.Reason == TrReason.Missing || request.Reason == TrReason.NewListing)
                {
                    // remove all images if reason is missing or new listing
                    if (uploadedImages.Data.Count() > 0)
                    {
                        var isImagesDeleted = await _uploadImageService.DeleteAllUploadedImagesByTrCartId(new DeleteAllUploadedImagesRequest()
                        {
                            TrCartId = trCart.TrCartId,
                            StoreId = request.StoreId
                        });
                    }
                }

                if(trCart.Brand == (byte)Brand.Own)
                {
                    var storeSalesBand = await _storeSalesBandRepository.GetTypeValue(request.StoreId);
                    if (storeSalesBand == null)
                    {
                        return ServiceResult<TrCartDto>.Fail("Store sales band not found.");
                    }
                    //string salesBand = Enum.GetName(typeof(DTO.Common.SalesBand), DTO.Common.SalesBand.PLU_UNIT_LIMIT_OWN);
                    var pluUnitLimit = storeSalesBand[SalesBandConstants.PLU_UNIT_LIMIT_OWN].Value;
                    if (pluUnitLimit == null)
                    {
                        return ServiceResult<TrCartDto>.Fail("Store sales band not found.");
                    }

                    var monthlyStoreOrder = await _trOrderRepository.GetProductQuantityOfMonthlyStoreOrder(request.StoreId, (byte)trCart.Brand);
                    monthlyStoreOrder.TryGetValue(trCart.Plu, out var monthlyOrderedUnit);
                    if (monthlyOrderedUnit >= pluUnitLimit)
                    {
                        if (string.IsNullOrEmpty(request.Justification))
                        {
                            return ServiceResult<TrCartDto>.Fail($"Justification is required.");
                        }
                        trCart.Justification = request.Justification;
                    }
                }

                trCart.Reason = (byte)request.Reason!;
                trCart.UpdatedBy = request.UpdatedBy;
                await _trCartRepository.Update(trCart);

                var trCartDto = _mapper.Map<TrCartDto>(trCart);
                trCartDto = await MapTrCartDto(trCartDto, null, null, null, null);

                return ServiceResult<TrCartDto>.Success(trCartDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //public async Task<ServiceResult<RequestTrResponse>> RequestTr(RequestTrRequest request)
        //{
        //    if (request.ImageBase64List == null || request.ImageBase64List.Count < 1)
        //    {
        //        return ServiceResult<RequestTrResponse>.Failure("Image is required.");
        //    }

        //    if (request.ImageBase64List != null && request.ImageBase64List.Count > 3)
        //    {
        //        return ServiceResult<RequestTrResponse>.Failure("Maximum 3 images to be uploaded.");
        //    }

        //    var store = await _storeMasterRepository.SelectStore(request.StoreId);
        //    if (store == null)
        //    {
        //        return ServiceResult<RequestTrResponse>.Failure("Store Not Found.");
        //    }

        //    var item = await _itemMasterRepository.SearchByPluOrBarcode(request.Plu);
        //    if (item == null)
        //    {
        //        return ServiceResult<RequestTrResponse>.Failure("Product Not Found.");
        //    }

        //    if (item.Trid != 1)
        //    {
        //        return ServiceResult<RequestTrResponse>.Failure("Product is not tester product.");
        //    }

        //    long.TryParse(item.Item, out var pluId);
        //    var pendingItem = _trRepository.Select(store.StoreId, pluId);
        //    if (pendingItem != null)
        //    {
        //        return ServiceResult<RequestTrResponse>.Failure("Product is already requested.");
        //    }

        //    var trEntity = new TrPlu()
        //    {
        //        Plu = request.Plu,
        //        Barcode = item.Barcode,
        //        Reason = request.Reason,
        //        StoreId = request.StoreId,
        //        Status = (byte)TrStatus.Pending,
        //        SupplierCode = item.SupplierCode,
        //        SupplierName = item.SupplierName,
        //        CreatedBy = request.CreatedBy,
        //    };
        //    await _trRepository.Insert(trEntity);

        //    var imageIndex = 1;
        //    foreach (var base64image in request.ImageBase64List)
        //    {
        //        try
        //        {
        //            var base64imageString = base64image;
        //            var mimeType = "jpeg";
        //            if (base64image.Contains(','))
        //            {
        //                var base64Arr = base64image.Split(',');
        //                base64imageString = base64Arr[1];

        //                //mimeType = base64Arr[0].Split(':')[1].Split(';')[0].Split('/')[1];
        //                //if (mimeType.Equals("jpg", StringComparison.OrdinalIgnoreCase))
        //                //{
        //                //    mimeType = "jpeg";
        //                //}
        //            }

        //            // TODO: check valid image format

        //            var filePath = $"{_imageSettings.FilePath}\\{request.StoreId}";
        //            var imagePath = Path.Combine(filePath, $"{trEntity.TrPluId}_{request.Plu}_{imageIndex}.{mimeType}");
        //            var resizedImagePath = Path.Combine(filePath, $"{trEntity.TrPluId}_{request.Plu}_{imageIndex}_resized.{mimeType}");

        //            if (!Directory.Exists(filePath))
        //            {
        //                Directory.CreateDirectory(filePath);
        //            }

        //            ImageHelper.SaveBase64Image(base64imageString, imagePath, ImageFormat.Jpeg);
        //            ImageHelper.ResizeWithHeightAspectRatio(imagePath, resizedImagePath, _imageSettings.ResizeHeight, ImageFormat.Jpeg);

        //            File.Delete(imagePath);
        //            File.Move(resizedImagePath, imagePath);

        //            var imageUrlPath = imagePath.Replace($"{_imageSettings.FilePath}", $"{_imageSettings.UrlPath}").Replace("\\", "/");
        //            var trImageEntity = new TrImage()
        //            {
        //                TrPluId = trEntity.TrPluId,
        //                ImagePath = imageUrlPath
        //            };
        //            await _trImageRepository.Insert(trImageEntity);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception(ex.ToString());
        //        }
        //        imageIndex++;
        //    }

        //    // TODO : Send Email to ASOM

        //    var response = new RequestTrResponse(Code: ResponseCode.Success, Message: "Successful Requested.");

        //    return ServiceResult<RequestTrResponse>.Success(response);
        //}

        //public async Task<ServiceResult<List<TrPluDto>>> TrList(int storeId, TrStatus? status, String? pluOrBarcode)
        //{
        //    if (storeId == 0)
        //    {
        //        return ServiceResult<List<TrPluDto>>.Failure("StoreId is required.");
        //    }

        //    var store = await _storeMasterRepository.SelectStore(storeId);
        //    if (store == null)
        //    {
        //        return ServiceResult<List<TrPluDto>>.Failure("Store Not Found.");
        //    }

        //    if (status == TrStatus.All)
        //    {
        //        status = null;
        //    }

        //    var trPluListRepository = await _trRepository.GetStoreTrPluList(storeId, (byte?)status, pluOrBarcode);
        //    var trPluList = _mapper.Map<List<TrPluDto>>(trPluListRepository);

        //    // get productName from item master & update immutable record trPluList to updatedTrPluList.
        //    List<TrPluDto> updatedTrPluList = new List<TrPluDto>();
        //    var cachedPluList = new Dictionary<string, string>();
        //    foreach (var trPlu in trPluList)
        //    {
        //        if (cachedPluList.ContainsKey(trPlu.Plu))
        //        {
        //            var updatedTrPlu = trPlu with { ProductName = cachedPluList[trPlu.Plu] };
        //            updatedTrPluList.Add(updatedTrPlu);
        //        }
        //        else
        //        {
        //            var product = await _itemMasterRepository.SearchByPluOrBarcode(trPlu.Plu);
        //            var updatedTrPlu = trPlu with { ProductName = product.RetekItemDesc };
        //            updatedTrPluList.Add(updatedTrPlu);
        //            cachedPluList.Add(trPlu.Plu, product.RetekItemDesc ?? string.Empty);
        //        }
        //    }

        //    return ServiceResult<List<TrPluDto>>.Success(updatedTrPluList);
        //}
    }
}

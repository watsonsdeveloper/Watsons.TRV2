using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.Common.ImageHelpers;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.Repositories;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.DTO.Mobile;

namespace Watsons.TRV2.Services.Mobile
{
    //public interface ITrService
    //{
    //    Task<ServiceResult<RequestTrResponse>> RequestTr(RequestTrRequest request);
    //    Task<ServiceResult<List<TrPluDto>>> TrList(int storeId, TrStatus? status, string? pluOrBarcode);
    //}
    //public class TrService : ITrService
    //{
    //    private readonly IMapper _mapper;
    //    private readonly ITrRepository _trRepository;
    //    private readonly ITrImageRepository _trImageRepository;
    //    private readonly IItemMasterRepository _itemMasterRepository;
    //    private readonly IStoreMasterRepository _storeMasterRepository;
    //    private readonly IImageHelper _imageHelper;

    //    private readonly ImageSettings _imageSettings;
    //    public TrService(IMapper mapper, ITrRepository trRepository, ITrImageRepository trImageRepository, IItemMasterRepository itemMasterRepository,
    //        IStoreMasterRepository storeMasterRepository, IImageHelper imageHelper,
    //        IOptions<ImageSettings> imageSettings)
    //    {
    //        _mapper = mapper;
    //        _trRepository = trRepository;
    //        _trImageRepository = trImageRepository;
    //        _itemMasterRepository = itemMasterRepository;
    //        _storeMasterRepository = storeMasterRepository;
    //        _imageHelper = imageHelper;

    //        _imageSettings = imageSettings.Value;
    //    }

    //    public async Task<ServiceResult<RequestTrResponse>> RequestTr(RequestTrRequest request)
    //    {
    //        if (request.ImageBase64List == null || request.ImageBase64List.Count < 1)
    //        {
    //            return ServiceResult<RequestTrResponse>.Failure("Image is required.");
    //        }

    //        if (request.ImageBase64List != null && request.ImageBase64List.Count > 3)
    //        {
    //            return ServiceResult<RequestTrResponse>.Failure("Maximum 3 images to be uploaded.");
    //        }

    //        var store = await _storeMasterRepository.SelectStore(request.StoreId);
    //        if (store == null)
    //        {
    //            return ServiceResult<RequestTrResponse>.Failure("Store Not Found.");
    //        }

    //        var item = await _itemMasterRepository.SearchByPluOrBarcode(request.Plu);
    //        if (item == null)
    //        {
    //            return ServiceResult<RequestTrResponse>.Failure("Product Not Found.");
    //        }

    //        if (item.Trid != 1)
    //        {
    //            return ServiceResult<RequestTrResponse>.Failure("Product is not tester product.");
    //        }

    //        long.TryParse(item.Item, out var pluId);
    //        var pendingItem = _trRepository.Select(store.StoreId, pluId);
    //        if (pendingItem != null)
    //        {
    //            return ServiceResult<RequestTrResponse>.Failure("Product is already requested.");
    //        }

    //        var trEntity = new TrPlu()
    //        {
    //            Plu = request.Plu,
    //            Barcode = item.Barcode,
    //            Reason = request.Reason,
    //            StoreId = request.StoreId,
    //            Status = (byte)TrStatus.Pending,
    //            SupplierCode = item.SupplierCode,
    //            SupplierName = item.SupplierName,
    //            CreatedBy = request.CreatedBy,
    //        };
    //        await _trRepository.Insert(trEntity);

    //        var imageIndex = 1;
    //        foreach (var base64image in request.ImageBase64List)
    //        {
    //            try
    //            {
    //                var base64imageString = base64image;
    //                var mimeType = "jpeg";
    //                if (base64image.Contains(','))
    //                {
    //                    var base64Arr = base64image.Split(',');
    //                    base64imageString = base64Arr[1];

    //                    //mimeType = base64Arr[0].Split(':')[1].Split(';')[0].Split('/')[1];
    //                    //if (mimeType.Equals("jpg", StringComparison.OrdinalIgnoreCase))
    //                    //{
    //                    //    mimeType = "jpeg";
    //                    //}
    //                }

    //                // TODO: check valid image format

    //                var filePath = $"{_imageSettings.FilePath}\\{request.StoreId}";
    //                var imagePath = Path.Combine(filePath, $"{trEntity.TrPluId}_{request.Plu}_{imageIndex}.{mimeType}");
    //                var resizedImagePath = Path.Combine(filePath, $"{trEntity.TrPluId}_{request.Plu}_{imageIndex}_resized.{mimeType}");

    //                if (!Directory.Exists(filePath))
    //                {
    //                    Directory.CreateDirectory(filePath);
    //                }

    //                ImageHelper.SaveBase64Image(base64imageString, imagePath, ImageFormat.Jpeg);
    //                ImageHelper.ResizeWithHeightAspectRatio(imagePath, resizedImagePath, _imageSettings.ResizeHeight, ImageFormat.Jpeg);

    //                File.Delete(imagePath);
    //                File.Move(resizedImagePath, imagePath);

    //                var imageUrlPath = imagePath.Replace($"{_imageSettings.FilePath}", $"{_imageSettings.UrlPath}").Replace("\\", "/");
    //                var trImageEntity = new TrImage()
    //                {
    //                    //TrPluId = trEntity.TrPluId,
    //                    ImagePath = imageUrlPath
    //                };
    //                await _trImageRepository.Insert(trImageEntity);
    //            }
    //            catch (Exception ex)
    //            {
    //                throw new Exception(ex.ToString());
    //            }
    //            imageIndex++;
    //        }

    //        // TODO : Send Email to ASOM

    //        var response = new RequestTrResponse(Code: ResponseCode.Success, Message: "Successful Requested.");

    //        return ServiceResult<RequestTrResponse>.Success(response);
    //    }

    //    public async Task<ServiceResult<List<TrPluDto>>> TrList(int storeId, TrStatus? status, String? pluOrBarcode)
    //    {
    //        if (storeId == 0)
    //        {
    //            return ServiceResult<List<TrPluDto>>.Failure("StoreId is required.");
    //        }

    //        var store = await _storeMasterRepository.SelectStore(storeId);
    //        if (store == null)
    //        {
    //            return ServiceResult<List<TrPluDto>>.Failure("Store Not Found.");
    //        }

    //        if (status == TrStatus.All)
    //        {
    //            status = null;
    //        }

    //        var trPluListRepository = await _trRepository.GetStoreTrPluList(storeId, (byte?)status, pluOrBarcode);
    //        var trPluList = _mapper.Map<List<TrPluDto>>(trPluListRepository);

    //        // get productName from item master & update immutable record trPluList to updatedTrPluList.
    //        List<TrPluDto> updatedTrPluList = new List<TrPluDto>();
    //        var cachedPluList = new Dictionary<string, string>();
    //        foreach (var trPlu in trPluList)
    //        {
    //            if (cachedPluList.ContainsKey(trPlu.Plu))
    //            {
    //                var updatedTrPlu = trPlu with { ProductName = cachedPluList[trPlu.Plu] };
    //                updatedTrPluList.Add(updatedTrPlu);
    //            }
    //            else
    //            {
    //                var product = await _itemMasterRepository.SearchByPluOrBarcode(trPlu.Plu);
    //                var updatedTrPlu = trPlu with { ProductName = product.RetekItemDesc };
    //                updatedTrPluList.Add(updatedTrPlu);
    //                cachedPluList.Add(trPlu.Plu, product.RetekItemDesc ?? string.Empty);
    //            }
    //        }

    //        return ServiceResult<List<TrPluDto>>.Success(updatedTrPluList);
    //    }
    //}
}

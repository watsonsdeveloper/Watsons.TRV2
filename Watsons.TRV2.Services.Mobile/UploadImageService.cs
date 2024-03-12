using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.Common.ImageHelpers;
using Watsons.TRV2.DA.Repositories;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.DTO.Mobile.UploadedImage;
using Watsons.TRV2.DTO.Mobile.UploadImage;

namespace Watsons.TRV2.Services.Mobile
{
    public interface IUploadImageService
    {
        Task<ServiceResult<List<UploadedImageDto>>> ListByTrCartId(ListByTrCartIdRequest request);
        Task<ServiceResult<List<UploadedImageDto>>> ListByStore(ListByStoreRequest request);
        Task<ServiceResult<UploadedImageDto>> UploadImage(UploadImageRequest request);
        Task<ServiceResult<List<UploadedImageDto>>> GetUploadedImageUrls(GetUploadedImageUrlsRequest request);
        Task<ServiceResult<bool>> DeleteUploadedImagesByImageIds(DeleteUploadedImagesRequest request);
        Task<ServiceResult<bool>> DeleteAllUploadedImagesByTrCartId(DeleteAllUploadedImagesRequest request);
        Task<ServiceResult<bool>> TransferImageFromCartToOrder(TransferImageFromCartToOrderRequest request);
    }
    public class UploadImageService : IUploadImageService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IItemMasterRepository _itemMasterRepository;
        private readonly IStoreMasterRepository _storeMasterRepository;
        private readonly ITrImageRepository _trImageRepository;
        private readonly ITrOrderRepository _trOrderRepository;
        private readonly ITrCartRepository _trCartRepository;
        private readonly ImageSettings _imageSettings;
        private readonly string _baseUrl;

        public UploadImageService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IItemMasterRepository itemMasterRepository,
            IStoreMasterRepository storeMasterRepository,
            ITrImageRepository trImageRepository,
            ITrOrderRepository trOrderRepository,
            ITrCartRepository trCartRepository,
            IOptions<ImageSettings> imageSettings)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _itemMasterRepository = itemMasterRepository;
            _storeMasterRepository = storeMasterRepository;
            _trImageRepository = trImageRepository;
            _trOrderRepository = trOrderRepository;
            _trCartRepository = trCartRepository;
            _imageSettings = imageSettings.Value;

            var httpRequest = _httpContextAccessor.HttpContext.Request;
            _baseUrl = $"http://{httpRequest.Host}{httpRequest.PathBase}";
        }

        public async Task<ServiceResult<List<UploadedImageDto>>> ListByTrCartId(ListByTrCartIdRequest request)
        {
            try
            {
                var cart = await _trCartRepository.Select(request.TrCartId, request.StoreId);
                if (cart == null)
                    return ServiceResult<List<UploadedImageDto>>.Failure("Image not found.");

                List<long> cartIds = new List<long> { request.TrCartId };

                var imagesUploaded = await _trImageRepository.ListByTrCartIds(cartIds) as List<TrImage>;
                var imagesUploadedDto = _mapper.Map<List<UploadedImageDto>>(imagesUploaded);

                return ServiceResult<List<UploadedImageDto>>.Success(imagesUploadedDto ?? []);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<ServiceResult<List<UploadedImageDto>>> ListByStore(ListByStoreRequest request)
        {
            try
            {
                var carts = await _trCartRepository.List(request.StoreId, (byte)request.Brand);
                if(carts == null || carts.Count() <= 0)
                    return ServiceResult<List<UploadedImageDto>>.Success([]);

                var trCartIds = carts.Select(c => c.TrCartId).ToList();

                var imagesUploaded = await _trImageRepository.ListByTrCartIds(trCartIds) as List<TrImage>;

                var imagesUploadedDto = _mapper.Map<List<UploadedImageDto>>(imagesUploaded);
                return ServiceResult<List<UploadedImageDto>>.Success(imagesUploadedDto ?? []);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<ServiceResult<UploadedImageDto>> UploadImage(UploadImageRequest request)
        {
            try
            {
                var imagePath = string.Empty;

                var cart = await _trCartRepository.Select(request.TrCartId, request.StoreId);
                if (cart == null) return ServiceResult<UploadedImageDto>.Failure("Cart not found.");

                var imageUploaded = await _trImageRepository.ListByTrCartId(request.TrCartId);
                if (imageUploaded.Count() + 1 > _imageSettings.MaxImageUpload)
                {
                    return ServiceResult<UploadedImageDto>.Failure("Max image upload is reached.");
                }
                var base64imageString = request.Base64Image;
                var mimeType = "jpeg";
                if (request.Base64Image.Contains(','))
                {
                    var base64Arr = request.Base64Image.Split(',');
                    base64imageString = base64Arr[1];

                    //mimeType = base64Arr[0].Split(':')[1].Split(';')[0].Split('/')[1];
                    //if (mimeType.Equals("jpg", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    mimeType = "jpeg";
                    //}
                }

                // TODO: check valid image format

                var filePath = $"{_imageSettings.FilePath}\\{request.StoreId}";
                var imageName = $"{request.TrCartId}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                var imageFilePath = Path.Combine(filePath, $"{imageName}.{mimeType}");
                var resizedImagePath = Path.Combine(filePath, $"{imageName}_resized.{mimeType}");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                ImageHelper.SaveBase64Image(base64imageString, imageFilePath, ImageFormat.Jpeg);
                ImageHelper.ResizeWithHeightAspectRatio(imageFilePath, resizedImagePath, _imageSettings.ResizeHeight, ImageFormat.Jpeg);

                File.Delete(imageFilePath);
                File.Move(resizedImagePath, imageFilePath);

                //var imageUrlPath = imagePath.Replace($"{_imageSettings.FilePath}", $"{_imageSettings.UrlPath}").Replace("\\", "/");
                imagePath = _imageSettings.UrlPath + imageFilePath.Replace(_imageSettings.FilePath, "").Replace("\\", "/");
                var trImage = new TrImage()
                {
                    ImagePath = imagePath,
                    TrCartId = request.TrCartId,
                };
                await _trImageRepository.Insert(trImage);

                var imageUrl = $"{_baseUrl}{imagePath}";
                var response = new UploadedImageDto
                {
                    TrImageId = trImage.TrImageId,
                    ImageUrl = imageUrl
                };
                return ServiceResult<UploadedImageDto>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<ServiceResult<List<UploadedImageDto>>> GetUploadedImageUrls(GetUploadedImageUrlsRequest request)
        {
            try
            {
                if(request.TrCartId == null && request.TrOrderId == null)
                {
                    return ServiceResult<List<UploadedImageDto>>.Failure("Uploaded image not found.");
                }

                List<TrImage>? imageUploaded = new();
                if (request.TrCartId != null)
                {
                    var trCart = await _trCartRepository.Select(request.TrCartId ?? 0, request.StoreId);
                    if(trCart == null)
                        return ServiceResult<List<UploadedImageDto>>.Failure("Uploaded image not found.");
                    imageUploaded = await _trImageRepository.ListByTrCartId(request.TrCartId ?? 0) as List<TrImage>;
                }
                else if (request.TrCartId != null)
                {
                    var trOrder = await _trOrderRepository.Select(request.TrOrderId ?? 0, request.StoreId);
                    if(trOrder == null)
                        return ServiceResult<List<UploadedImageDto>>.Failure("Uploaded image not found.");
                    imageUploaded = await _trImageRepository.ListByTrOrderId(request.TrOrderId ?? 0) as List<TrImage>;
                }

                if (imageUploaded == null || imageUploaded.Count == 0)
                {
                    return ServiceResult<List<UploadedImageDto>>.Success(new());
                }

                List<UploadedImageDto> response = imageUploaded.Select(image => new UploadedImageDto {
                    TrImageId = image.TrImageId,
                    ImageUrl = $"{_baseUrl}{image.ImagePath}" 
                }).ToList();

                return ServiceResult<List<UploadedImageDto>>.Success(response);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<ServiceResult<bool>> DeleteUploadedImagesByImageIds(DeleteUploadedImagesRequest request)
        {
            if (request.ImageIds == null || request.ImageIds.Count <= 0)
                return ServiceResult<bool>.Failure("No image is deleted.");
            

            var trCart = await _trCartRepository.Select(request.TrCartId, request.StoreId);
            if(trCart == null)  
                return ServiceResult<bool>.Failure("Cart not found.");

            var imagesUploaded = await _trImageRepository.ListByTrCartId(request.TrCartId) as List<TrImage>;

            if(imagesUploaded == null || imagesUploaded.Count <= 0) 
                return ServiceResult<bool>.Failure("Image not found.");
            

            List<long> imageIds = imagesUploaded.Select(image => image.TrImageId).ToList();

            if (request.ImageIds.Any(x => !imageIds.Contains(x)))
                return ServiceResult<bool>.Failure("Image not found.");

            foreach(var imageUploaded in imagesUploaded.Where(i => request.ImageIds.Contains(i.TrImageId)))
            {
                var imageFilePath = $"{_imageSettings.FilePath}{imageUploaded.ImagePath.Replace(_imageSettings.UrlPath, "").Replace("/", "\\")}";
                if (File.Exists(imageFilePath))
                    File.Delete(imageFilePath);
            }

            await _trImageRepository.DeleteRange(request.ImageIds);

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> DeleteAllUploadedImagesByTrCartId(DeleteAllUploadedImagesRequest request)
        {
            var trCart = await _trCartRepository.Select(request.TrCartId, request.StoreId);
            if (trCart == null)
                return ServiceResult<bool>.Failure("Cart not found.");

            var imagesUploaded = await _trImageRepository.ListByTrCartId(request.TrCartId) as List<TrImage>;

            if (imagesUploaded == null || imagesUploaded.Count <= 0)
                return ServiceResult<bool>.Success(true);

            var deleteImageIds = new List<long>();
            foreach (var imageUploaded in imagesUploaded)
            {
                var imageFilePath = $"{_imageSettings.FilePath}{imageUploaded.ImagePath.Replace(_imageSettings.UrlPath, "").Replace("/", "\\")}";
                if (File.Exists(imageFilePath))
                    File.Delete(imageFilePath);
                deleteImageIds.Add(imageUploaded.TrImageId);
            }

            await _trImageRepository.DeleteRange(deleteImageIds);

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> TransferImageFromCartToOrder(TransferImageFromCartToOrderRequest request)
        {
            var trCart = await _trCartRepository.Select(request.TrCartId, request.StoreId);
            if (trCart == null)
                return ServiceResult<bool>.Failure("Cart not found.");

            var trOrder = await _trOrderRepository.Select(request.TrOrderId, request.StoreId);
            if (trOrder == null)
                return ServiceResult<bool>.Failure("Order not found.");

            var imagesUploaded = await _trImageRepository.ListByTrCartId(request.TrCartId) as List<TrImage>;
            if(imagesUploaded == null || imagesUploaded.Count <= 0)
                return ServiceResult<bool>.Failure("Image not found.");

            foreach(var imageUploaded in imagesUploaded)
            {
                imageUploaded.TrOrderId = request.TrOrderId;
            }
            await _trImageRepository.UpdateRange(imagesUploaded);

            return ServiceResult<bool>.Success(true);
        }
    }
}

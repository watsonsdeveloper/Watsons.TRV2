using Microsoft.Extensions.Logging;
using System.Linq;
using Watsons.Common;
using Watsons.TRV2.DA.MyMaster.Repositories;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.DTO.Mobile;

namespace Watsons.TRV2.Services.Mobile
{
    public interface IProductService
    {
        Task<ServiceResult<ProductDetailResponse>> SearchByPluOrBarcode(string pluOrBarcode);
    }
    public class ProductService : IProductService
    {
        ILoggerFactory _loggerFactory;
        private readonly IItemMasterRepository _itemMasterRepository;
        public ProductService(ILoggerFactory loggerFactory, IItemMasterRepository itemMasterRepository)
        {
            _loggerFactory = loggerFactory;
            _itemMasterRepository = itemMasterRepository;
        }
        public async Task<ServiceResult<ProductDetailResponse>> SearchByPluOrBarcode(string pluOrBarcode)
        {
            _loggerFactory.CreateLogger<ProductService>().LogInformation($"SearchByPluOrBarcode: {pluOrBarcode}");

            var result = await _itemMasterRepository.SearchByPluOrBarcode(pluOrBarcode);
            if (result == null)
            {
                return ServiceResult<ProductDetailResponse>.Fail("Product Not Found.");
            }
            var isTesterProducts = new List<int>() { 1, 2, 3 };
            if (!isTesterProducts.Contains(result.Trid ?? 0))
            {
                return ServiceResult<ProductDetailResponse>.Fail("This product is not tester product.");
            }

            var response = new ProductDetailResponse(result.Item, result.RetekItemDesc, string.Empty, result.SupplierCode ?? string.Empty, result.SupplierName ?? string.Empty);
            return ServiceResult<ProductDetailResponse>.Success(response);
        }
    }
}

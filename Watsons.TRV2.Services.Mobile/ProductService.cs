using Watsons.Common;
using Watsons.TRV2.DA.Repositories;
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
        private readonly IItemMasterRepository _itemMasterRepository;
        public ProductService(IItemMasterRepository itemMasterRepository)
        {
            _itemMasterRepository = itemMasterRepository;
        }
        public async Task<ServiceResult<ProductDetailResponse>> SearchByPluOrBarcode(string pluOrBarcode)
        {
            var result = await _itemMasterRepository.SearchByPluOrBarcode(pluOrBarcode);
            if (result == null)
            {
                return ServiceResult<ProductDetailResponse>.Failure("Product Not Found.");
            }
            if (result.Trid != 1)
            {
                return ServiceResult<ProductDetailResponse>.Failure("This product is not tester product.");
            }

            var response = new ProductDetailResponse(result.Item, result.RetekItemDesc, string.Empty, result.SupplierCode ?? string.Empty, result.SupplierName ?? string.Empty);
            return ServiceResult<ProductDetailResponse>.Success(response);
        }
    }
}

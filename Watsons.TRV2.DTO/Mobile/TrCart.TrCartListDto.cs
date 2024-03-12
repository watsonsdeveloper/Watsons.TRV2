using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.DTO.Mobile.UploadedImage;

namespace Watsons.TRV2.DTO.Mobile
{
    public class TrCartListDto
    {
        public record TrCartListRequest([Required] int StoreId, Brand Brand);

        public record TrCartListResponse(
            List<TrCartDto>? CartList = null);
    }

    //public record TrCartDto(long TrCartId, string Plu, string Barcode, int StoreId,
    //int BrandId, string SupplierName, string SupplierCode, string CreatedBy,
    //bool RequireJustify = true, bool IsAvailableStock = false,
    //string? Reason = null, string? Justification = null);

    public class TrCartDto
    {
        public long TrCartId { get; set; }
        public string? ProductName { get; set; }
        public string? BrandName { get; set; }
        public string Plu { get; set; } = null!;
        public string Barcode { get; set; } = null!;
        public int StoreId { get; set; }
        public Brand Brand { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierCode { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool RequireJustify { get; set; } = true;
        public bool IsAvailableStock { get; set; } = false;
        public TrReason? Reason { get; set; }
        public string? Justification { get; set; }
        public string? ErrorMessage { get; set; }
        public List<UploadedImageDto>? UploadedImages { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile
{
    public record ProductDetailRequest(
        [Required] string PluOrBarcode
        );
    public record ProductDetailResponse(
        string? Plu = null,
        string? ProductName = null,
        string? ImageUrl = null,
        string? SupplierCode = null,
        string? SupplierName = null);
}

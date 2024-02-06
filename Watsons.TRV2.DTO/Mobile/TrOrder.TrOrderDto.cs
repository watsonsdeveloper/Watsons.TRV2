using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile.TrOrder
{
    public class TrOrderDto
    {
        public long TrOrderId { get; set; }

        public string Plu { get; set; } = null!;

        public string? Barcode { get; set; }
        public string? ProductName { get; set; }
        public string? BrandName { get; set; }

        public string? Reason { get; set; }

        public string? Justification { get; set; }

        public TrOrderStatus Status { get; set; }

        public string? SupplierName { get; set; }

        public string? SupplierCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set;}
        public string? UpdatedBy { get; set; }
        //public List<TrImageDto> TrImages { get; set; } = new List<TrImageDto>();
    }
}

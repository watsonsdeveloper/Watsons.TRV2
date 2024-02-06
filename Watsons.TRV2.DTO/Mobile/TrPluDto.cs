using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile
{
    //public class TrPluDto
    //{
    //    public long TrPluId { get; set; }

    //    public string Plu { get; set; } = null!;

    //    public string? Barcode { get; set; }

    //    public int StoreId { get; set; }

    //    public string Reason { get; set; } = null!;

    //    public byte Status { get; set; }

    //    public string? SupplierName { get; set; }

    //    public string? SupplierCode { get; set; }

    //    public string CreatedBy { get; set; } = null!;

    //    public string? UpdatedBy { get; set; }

    //    public DateTime? CreatedAt { get; set; }

    //    public DateTime? UpdatedAt { get; set; }
    //}
    public record TrPluDto(
        long TrPluId,
        string Plu,
        int StoreId,
        string Reason,
        TrStatus Status,
        string CreatedBy,
        string? ProductName = null,
        string? Barcode = null,
        string? SupplierName = null,
        string? SupplierCode = null,
        DateTime? CreatedAt = null)
    {
        public TrPluDto With(
            long? TrPluId = null,
            string? Plu = null,
            string? ProductName = null,
            int? StoreId = null,
            string? Reason = null,
            TrStatus? Status = null,
            string? CreatedBy = null,
            string? Barcode = null,
            string? SupplierName = null,
            string? SupplierCode = null,
            DateTime? CreatedAt = null)
        {
            return this with
            {
                TrPluId = TrPluId ?? this.TrPluId,
                Plu = Plu ?? this.Plu,
                ProductName = ProductName ?? this.ProductName,
                StoreId = StoreId ?? this.StoreId,
                Reason = Reason ?? this.Reason,
                Status = Status ?? this.Status,
                CreatedBy = CreatedBy ?? this.CreatedBy,
                Barcode = Barcode ?? this.Barcode,
                SupplierName = SupplierName ?? this.SupplierName,
                SupplierCode = SupplierCode ?? this.SupplierCode,
                CreatedAt = CreatedAt ?? this.CreatedAt
            };
        }
    }
}

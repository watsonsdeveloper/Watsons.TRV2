using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Portal
{
    #region Own Brand Report
    public class ReportOwnDto
    {
        public long TrOrderId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; } = null!;
        public string Plu { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string? Reason { get; set; }
        public decimal? AverageCost { get; set; }
        public string CreatedBy { get; set; } = null!;
        public TrOrderStatus TrOrderStatus { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedAt { get; set; }
    }

    public class ReportOwnRequest
    {
        public List<int>? StoreIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? PluOrBarcode { get; set; }
        public TrOrderStatus? TrOrderStatus { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class ReportOwnResponse
    {
        public List<ReportOwnDto>? Records { get; set; }
        public int TotalRecord { get; set; }
    }
    #endregion Own Brand Report

    #region Supplier Brand Report
    public class ReportSupplierDto
    {
        public long TrOrderId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; } = null!;
        public string Plu { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string? Reason { get; set; }
        public decimal? AverageCost { get; set; }
        public string CreatedBy { get; set; } = null!;
        public TrOrderStatus TrOrderStatus { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedAt { get; set; }
    }
    
    public class ReportSupplierRequest
    {
        public List<int>? StoreIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? PluOrBarcode { get; set; }
        public TrOrderStatus? TrOrderStatus { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class ReportSupplierResponse
    {
        public List<ReportSupplierDto>? Records { get; set; }
        public int TotalRecord { get; set; }
    }
    #endregion Supplier Brand Report

    #region Supplier Fulfilment Report
    public class ReportSupplierFulfillmentDto
    {
        public string SupplierName { get; set; } = null!;
        public int TotalOrder { get; set; }
        public int TotalOrderFulfilled { get; set; }
        public int TotalOrderUnfulfill { get; set; }
        public double FulfilledPercentage
        {
            get
            {
                return Math.Round((double)TotalOrderFulfilled / (double)TotalOrder * 100, 3);
            }
        }
    }

    public class ReportSupplierFulfillmentRequest
    {
        public string? SupplierName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class ReportSupplierFulfillmentResponse
    {
        public List<ReportSupplierFulfillmentDto>? Records { get; set; }
        public int TotalRecord { get; set; }
    }
    #endregion Supplier Fulfilment Report
}

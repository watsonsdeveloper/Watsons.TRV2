using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Portal.OrderDto
{
    public class OrderListDto
    {
        public Brand Brand { get; set; }
        public long OrderId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; } = null!;
        public TrOrderBatchStatus TrOrderBatchStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
    public class FetchOrderListRequest
    {
        [Required]
        public int Page { get; set; } = 1;
        [Required]
        public int PageSize { get; set; } = 10;
        [Required]
        public Brand? Brand { get; set; }
        public long? TrOrderBatchId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int>? StoreId { get; set; }
        public TrOrderBatchStatus? TrOrderBatchStatus { get; set; }
    }
    public class FetchOrderListResponse
    {
        public List<OrderListDto> Orders { get; set; } = new List<OrderListDto>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
    }

    public class OrderDto
    {
        public long TrOrderBatchId { get; set; }
        public long OrderItemId { get; set; }
        public string ProductName { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string Plu { get; set; } = null!;
        public string? Reason { get; set; }
        public string? Justification { get; set; }
        public bool IsRequireJustify { get; set; } = true;
        public TrOrderStatus TrOrderStatus { get; set; } = TrOrderStatus.Pending;
        public string? SupplierName { get; set; }
        public string? SupplierCode { get; set; }
        public decimal? AverageCost { get; set; }
        public string? Remark { get; set; }
        public List<string>? UploadedImages { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class FetchOrderOwnRequest
    {
        [Required]
        public long TrOrderBatchId { get; set; }
        public TrOrderStatus? TrOrderStatus { get; set; }
        public string? PluOrBarcode { get; set; }
    }

    public class FetchOrderOwnResponse
    {
        public List<OrderDto> OrderItems { get; set; } = new List<OrderDto>();
        public long TrOrderBatchId { get; set; }
        public TrOrderBatchStatus TrOrderBatchStatus { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; } = null!;
        public decimal CostThresholdSnapshot { get; set; }
        public decimal AccumulatedCostApproved { get; set; }
        public decimal TotalOrderCost { get; set; }
        public decimal TotalCostApproved { get; set; }
        public decimal TotalCostRejected { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public int TotalRecord { get; set; }
    }

    public class FetchOrderSupplierRequest
    {

    }

    public class FetchOrderSupplierResponse
    {

    }

    public class UpdateOrderItem
    {
        [Required]
        public long TrOrderId { get; set;  }
        [Required]
        public TrOrderStatus? TrOrderStatus { get; set; }
        public string? Remark { get; set; }
        public string? ErrorMessage { get; set; }
    }
    public class UpdateOrderOwnRequest
    {
        [Required]
        public long TrOrderBatchId { get; set; }
        public bool IsConfirmUpdate { get; set; } = false;
        [Required]
        public List<UpdateOrderItem>? OrderItems { get; set; }

    }

    public class UpdateOrderOwnResponse
    {
        public long TrOrderBatchId { get; set; }
        public TrOrderBatchStatus TrOrderBatchStatus { get; set; }
        public decimal CostThresholdSnapshot { get; set; }
        public decimal AccumulatedCostApproved { get; set; }
        public decimal TotalOrderCost { get; set; }
        public decimal TotalCostApproved { get; set; }
        public decimal TotalCostRejected { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public List<UpdateOrderItem>? OrderItems { get; set; }
    }
}

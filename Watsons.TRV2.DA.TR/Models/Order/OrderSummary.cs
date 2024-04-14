using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.TR.Models.Order
{
    public class OrderSummary
    {
        public long TrOrderBatchId { get; set; }
        public byte TrOrderBatchStatus { get; set; }
        public int StoreId { get; set; }
        public string? StoreName { get; set; }
        public decimal? CostThresholdSnapshot { get; set; }
        public decimal? AccumulatedCostApproved { get; set; }
        public decimal? TotalCostApproved { get; set; }
        public decimal? TotalCostRejected { get; set; }
        public decimal? TotalOrderCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

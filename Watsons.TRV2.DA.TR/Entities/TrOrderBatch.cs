using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class TrOrderBatch
{
    public string TrOrderBatchId { get; set; } = null!;

    public decimal? TotalCostUponApproval { get; set; }

    public decimal? Threshold { get; set; }

    public byte Status { get; set; }

    public int StoreId { get; set; }

    public byte BrandId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<TrOrder> TrOrders { get; set; } = new List<TrOrder>();
}

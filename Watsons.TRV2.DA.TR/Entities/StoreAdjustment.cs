using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class StoreAdjustment
{
    public long StoreAdjustmentId { get; set; }

    public long TrOrderBatchId { get; set; }

    public long TrOrderId { get; set; }

    public int StoreId { get; set; }

    public string Plu { get; set; } = null!;

    public int Qty { get; set; }

    public string ReasonCode { get; set; } = null!;

    public string? Remark { get; set; }

    public string? InventoryAdjustmentNumber { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual TrOrder TrOrder { get; set; } = null!;

    public virtual TrOrderBatch TrOrderBatch { get; set; } = null!;
}

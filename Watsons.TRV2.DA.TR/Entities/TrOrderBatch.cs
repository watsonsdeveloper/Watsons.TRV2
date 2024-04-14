using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class TrOrderBatch
{
    public long TrOrderBatchId { get; set; }

    public int StoreId { get; set; }

    public byte Brand { get; set; }

    public byte TrOrderBatchStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual OrderCost? OrderCost { get; set; }

    public virtual ICollection<TrOrder> TrOrders { get; set; } = new List<TrOrder>();
}

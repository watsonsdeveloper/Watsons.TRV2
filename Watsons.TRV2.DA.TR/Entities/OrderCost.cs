using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class OrderCost
{
    public long OderCostId { get; set; }

    public long TrOrderBatchId { get; set; }

    public decimal? CostThresholdSnapshot { get; set; }

    public decimal? AccumulatedCostApproved { get; set; }

    public decimal? TotalOrderCost { get; set; }

    public decimal? TotalCostApproved { get; set; }

    public decimal? TotalCostRejected { get; set; }

    public virtual TrOrderBatch TrOrderBatch { get; set; } = null!;
}

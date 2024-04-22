using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class TrOrder
{
    public long TrOrderId { get; set; }

    public long TrOrderBatchId { get; set; }

    public long TrCartId { get; set; }

    public string? ProductName { get; set; }

    public string? BrandName { get; set; }

    public string Plu { get; set; } = null!;

    public string? Barcode { get; set; }

    public byte? Reason { get; set; }

    public string? Justification { get; set; }

    public bool? IsRequireJustify { get; set; }

    public byte? TrOrderStatus { get; set; }

    public string? SupplierName { get; set; }

    public string? SupplierCode { get; set; }

    public decimal? AverageCost { get; set; }

    public decimal? PluCappedSnapshot { get; set; }

    public string? Remark { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual StoreAdjustment? StoreAdjustment { get; set; }

    public virtual TrCart TrCart { get; set; } = null!;

    public virtual ICollection<TrImage> TrImages { get; set; } = new List<TrImage>();

    public virtual TrOrderBatch TrOrderBatch { get; set; } = null!;
}

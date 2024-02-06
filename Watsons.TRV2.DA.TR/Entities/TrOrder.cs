using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class TrOrder
{
    public long TrOrderId { get; set; }

    public string TrOrderBatchId { get; set; } = null!;

    public string? ProductName { get; set; }

    public string? BrandName { get; set; }

    public string Plu { get; set; } = null!;

    public string? Barcode { get; set; }

    public string? Reason { get; set; }

    public bool RequireJustify { get; set; }

    public string? Justification { get; set; }

    public byte? Status { get; set; }

    public string? SupplierName { get; set; }

    public string? SupplierCode { get; set; }

    public decimal? WeightCost { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<TrImage> TrImages { get; set; } = new List<TrImage>();

    public virtual TrOrderBatch TrOrderBatch { get; set; } = null!;
}

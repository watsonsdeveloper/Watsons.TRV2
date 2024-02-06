using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class TrCart
{
    public long TrCartId { get; set; }

    public string Plu { get; set; } = null!;

    public string? Barcode { get; set; }

    public int? StoreId { get; set; }

    public byte BrandId { get; set; }

    public string? BrandName { get; set; }

    public string? ProductName { get; set; }

    public string? Reason { get; set; }

    public bool RequireJustify { get; set; }

    public string? Justification { get; set; }

    public string? SupplierName { get; set; }

    public string? SupplierCode { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<TrImage> TrImages { get; set; } = new List<TrImage>();
}

using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class TrPlu
{
    public long TrPluId { get; set; }

    public string Plu { get; set; } = null!;

    public string? Barcode { get; set; }

    public int StoreId { get; set; }

    public string Reason { get; set; } = null!;

    public byte Status { get; set; }

    public string? SupplierName { get; set; }

    public string? SupplierCode { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

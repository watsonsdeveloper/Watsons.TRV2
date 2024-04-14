using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class StoreSalesBand
{
    public long StoreSalesBandId { get; set; }

    public int StoreId { get; set; }

    public string Type { get; set; } = null!;

    public int SalesBandId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual SalesBand SalesBand { get; set; } = null!;
}

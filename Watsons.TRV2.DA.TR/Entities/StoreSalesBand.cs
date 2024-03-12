using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class StoreSalesBand
{
    public int StoreSalesBandId { get; set; }

    public int StoreId { get; set; }

    public int SalesBandId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public virtual SalesBand SalesBand { get; set; } = null!;
}

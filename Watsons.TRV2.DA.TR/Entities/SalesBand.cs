using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class SalesBand
{
    public int SalesBandId { get; set; }

    public string Type { get; set; } = null!;

    public string SalesBand1 { get; set; } = null!;

    public decimal Value { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<StoreSalesBand> StoreSalesBands { get; set; } = new List<StoreSalesBand>();
}

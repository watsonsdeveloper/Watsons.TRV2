using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class Brand
{
    public byte BrandId { get; set; }

    public string? Brand1 { get; set; }

    public virtual ICollection<TrCart> TrCarts { get; set; } = new List<TrCart>();

    public virtual ICollection<TrOrderBatch> TrOrderBatches { get; set; } = new List<TrOrderBatch>();
}

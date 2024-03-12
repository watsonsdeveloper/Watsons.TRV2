using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class TrImage
{
    public long TrImageId { get; set; }

    public long? TrOrderId { get; set; }

    public long? TrCartId { get; set; }

    public bool IsDeleted { get; set; }

    public string ImagePath { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual TrCart? TrCart { get; set; }

    public virtual TrOrder? TrOrder { get; set; }
}

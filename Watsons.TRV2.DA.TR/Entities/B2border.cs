using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class B2bOrder
{
    public long TrOrderId { get; set; }

    public string? OrderNumber { get; set; }

    public string? B2bFileName { get; set; }

    public byte? HhtInsertStatus { get; set; }

    public DateTime? HhtInsertAt { get; set; }

    public string? HhtRemark { get; set; }

    public int? ReceivedQty { get; set; }

    public DateTime? StoreReceivedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TrOrder TrOrder { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.MyMaster.Entities;

public partial class SupplierMaster
{
    public string SupplierCode { get; set; } = null!;

    public string? SupplierName { get; set; }

    public string? ContactName { get; set; }

    public string? ContactEmail { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public string? City { get; set; }

    public DateTime? ImportDateTime { get; set; }
}

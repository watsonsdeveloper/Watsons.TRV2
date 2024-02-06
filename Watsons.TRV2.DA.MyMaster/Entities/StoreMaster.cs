using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.MyMaster.Entities;

public partial class StoreMaster
{
    public int StoreId { get; set; }

    public string? StoreName { get; set; }

    public string? StoreAbbreviate { get; set; }

    public string? StoreAddress1 { get; set; }

    public string? StoreAddress2 { get; set; }

    public string? StoreCity { get; set; }

    public string? StorePostCode { get; set; }

    public DateTime? StoreOpenDate { get; set; }

    public DateTime? StoreCloseDate { get; set; }

    public DateTime? ImportDateTime { get; set; }

    public string? AreaName { get; set; }

    public string? StoreFormat { get; set; }

    public string? Pharmacy { get; set; }

    public string? Phone { get; set; }
}

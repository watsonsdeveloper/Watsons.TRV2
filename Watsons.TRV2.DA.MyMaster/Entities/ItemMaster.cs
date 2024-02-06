using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.MyMaster.Entities;

public partial class ItemMaster
{
    public string Item { get; set; } = null!;

    public string? Barcode { get; set; }

    public string? Dept { get; set; }

    public string? ClassName { get; set; }

    public string? Brand { get; set; }

    public string? RetekItemDesc { get; set; }

    public string? EcomItemDesc { get; set; }

    public string? ItemStatus { get; set; }

    public int? Trid { get; set; }

    public string? TesterBarcode { get; set; }

    public string? SupplierCode { get; set; }

    public string? SupplierName { get; set; }

    public string? SupplierProductCode { get; set; }

    public double? SupplierItemCost { get; set; }

    public string? DeptName { get; set; }

    public string? GroupNo { get; set; }

    public string? GroupName { get; set; }

    public double? UnitRetail { get; set; }

    public string? Rtnstatus { get; set; }

    public DateOnly? RtdlatestDate { get; set; }

    public string? Rtdflag { get; set; }

    public string? Wastage { get; set; }

    public DateTime? ImportDateTime { get; set; }

    public int? PackQty { get; set; }

    public string? ReturnPolicy { get; set; }

    public string? ReturnAllowInd { get; set; }

    public string? SupInactiveInd { get; set; }

    public string? ChildPlu { get; set; }

    public string? Indent { get; set; }
}

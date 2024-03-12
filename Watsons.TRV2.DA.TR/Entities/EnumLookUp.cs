using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class EnumLookUp
{
    public string EnumName { get; set; } = null!;

    public string EnumValue { get; set; } = null!;

    public int EnumId { get; set; }

    public string? Description { get; set; }

    public byte? Status { get; set; }
}

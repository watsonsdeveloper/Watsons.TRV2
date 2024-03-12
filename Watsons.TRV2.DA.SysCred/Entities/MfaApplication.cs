using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.SysCred.Entities;

public partial class MfaApplication
{
    public Guid Id { get; set; }

    public string? ApplicationName { get; set; }

    public string? Url { get; set; }

    public string? Description { get; set; }
}

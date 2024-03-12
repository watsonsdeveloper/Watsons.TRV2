using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.SysCred.Entities;

public partial class MfaUserLogin
{
    public Guid UserApplicationId { get; set; }

    public string? Password { get; set; }
}

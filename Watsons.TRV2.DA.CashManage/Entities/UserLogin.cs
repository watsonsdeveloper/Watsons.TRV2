using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.CashManage.Entities;

public partial class UserLogin
{
    public string Username { get; set; } = null!;

    public string? Name { get; set; }

    public string? Password { get; set; }

    public string? RoleCode { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public string? Email { get; set; }
}

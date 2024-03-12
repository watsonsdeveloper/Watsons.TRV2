using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.SysCred.Entities;

public partial class MfaUser
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Guid DepartmentId { get; set; }

    public int? StoreId { get; set; }
}

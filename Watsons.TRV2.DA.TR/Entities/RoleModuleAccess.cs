using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class RoleModuleAccess
{
    public int RoleModuleAccessId { get; set; }

    public Guid RoleId { get; set; }

    public int ModuleId { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Module Module { get; set; } = null!;
}

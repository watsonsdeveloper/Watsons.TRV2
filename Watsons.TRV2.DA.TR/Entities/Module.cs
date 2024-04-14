using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class Module
{
    public int ModuleId { get; set; }

    public string ModuleName { get; set; } = null!;

    public string Action { get; set; } = null!;

    public byte Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<RoleModuleAccess> RoleModuleAccesses { get; set; } = new List<RoleModuleAccess>();
}

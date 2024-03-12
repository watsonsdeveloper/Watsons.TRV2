using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.SysCred.Entities;

public partial class UserApplicationMapping
{
    public Guid? Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ApplicationId { get; set; }
}

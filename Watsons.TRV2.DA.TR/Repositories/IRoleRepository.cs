using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleModuleAccessResult>> GetRoleModuleAccesses(Guid RoleId);
    }
}

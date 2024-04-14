using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public class RoleRepository  : IRoleRepository
    {
        private readonly TrContext _context;
        public RoleRepository(TrContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoleModuleAccessResult>> GetRoleModuleAccesses(Guid RoleId)
        {
            var roleModuleAccess =  await _context.RoleModuleAccesses
                .Include(r => r.Module)
                .Where(r => r.RoleId == RoleId && r.Status == 1)
                .Select(r => new RoleModuleAccessResult()
                {
                    ModuleName = r.Module.ModuleName,
                    Action = r.Module.Action
                })
                .ToListAsync();
            return roleModuleAccess;
        }
    }
}

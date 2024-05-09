using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.CashManage.Entities;
using Watsons.TRV2.DA.CashManage.Models;

namespace Watsons.TRV2.DA.CashManage
{
    public class CashManageRepository : ICashManageRepository 
    {
        private readonly CashManageContext _cashManageContext;
        public CashManageRepository (CashManageContext cashManageContext)
        {
            _cashManageContext = cashManageContext;
        }
        public async Task<string?> GetUserRole(string email)
        {
            return await _cashManageContext.UserLogins.Where(x => x.Email == email).Select(x => x.RoleCode).FirstOrDefaultAsync();
        }

        public async Task<List<int>?> GetUserStoreIds(string email)
        {
            var storeIds = await (from ul in _cashManageContext.UserLogins
                          join us in _cashManageContext.UserStoreIds on ul.Username equals us.Username
                          where ul.Email == email
                          select us.StoreId ?? 0).ToListAsync();
            return storeIds;
        }

        public async Task<List<UserStore>> UserStoreIds(List<int> storeIds, List<string> roles)
        {
            return await (from s in _cashManageContext.UserStoreIds
                            join u in _cashManageContext.UserLogins on s.Username equals u.Username
                            where s.StoreId != null && storeIds.Contains((int)s.StoreId)
                            && !string.IsNullOrEmpty(s.RoleCode) && roles.Contains(s.RoleCode)
                            select new UserStore
                            {
                                Username = u.Username,
                                Email = u.Email,
                                StoreId = s.StoreId,
                                Name = u.Name,
                                RoleCode = u.RoleCode
                            }).ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.CashManage.Entities;

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
    }
}

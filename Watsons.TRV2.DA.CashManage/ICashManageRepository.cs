using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.CashManage.Entities;
using Watsons.TRV2.DA.CashManage.Models;

namespace Watsons.TRV2.DA.CashManage
{
    public interface ICashManageRepository
    {
        Task<string?> GetUserRole(string email);
        Task<List<int>?> GetUserStoreIds(string email);
        Task<List<UserStore>> UserStoreIds(List<int> storeIds, List<string> roles);
    }
}

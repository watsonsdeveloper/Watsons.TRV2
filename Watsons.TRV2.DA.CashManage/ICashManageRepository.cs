using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.CashManage
{
    public interface ICashManageRepository
    {
        Task<string?> GetUserRole(string email);
        Task<List<int>?> GetUserStoreIds(string email);
    }
}

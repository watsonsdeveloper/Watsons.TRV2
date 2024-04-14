using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.MyMaster.Entities;

namespace Watsons.TRV2.DA.MyMaster.Repositories
{
    public interface IStoreMasterRepository : IRepository<StoreMaster>
    {
        Task<StoreMaster?> SelectStore(int storeId);
        Task<Dictionary<int, string>?> SelectStoreName(List<int> storeIds);
    }
}

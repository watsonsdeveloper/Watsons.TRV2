using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.MyMaster.Repositories
{
    public interface IMigrationRepository
    {
        //Task<bool> WriteOffItem(int storeId, string plu, int quantity);
        Task<bool> WriteOffOrder(long orderBatchId);
    }
}

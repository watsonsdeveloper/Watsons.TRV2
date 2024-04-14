using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models;
using Watsons.TRV2.DA.TR.Models.SalesBand;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public interface IStoreSalesBandRepository : IRepository<StoreSalesBand>
    {
        Task<Dictionary<string, StoreSalesBandTypeValue>?> GetTypeValue(int storeId);
        Task<GetStoreSalesBandDetailsResult?> GetStoreSalesBandDetails(int storeId);
    }
}

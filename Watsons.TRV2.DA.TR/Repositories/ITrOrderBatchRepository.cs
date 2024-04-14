using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models.Order;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public interface ITrOrderBatchRepository : IRepository<TrOrderBatch>
    {
        Task<bool> UpdateWithOrderCost(TrOrderBatch entity);        
        Task<TrOrderBatch?> Select(long id);
        Task<TrOrderBatch?> Select(long id, int storeId);
        Task<TrOrderBatch?> SelectWithOrderCost(long id);
        Task<OrderSummary?> SelectSummary(long id);
        Task<IEnumerable<TrOrderBatch>> List(OrderBatchList entity);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models.B2bOrder;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public interface IB2bOrderRepository : IRepository<B2bOrder>
    {
        Task<IEnumerable<TrOrder>> HhtOrderList();
        Task<B2bOrder?> UpdateHhtOrder(UpdateHhtOrderDto dto);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.MyMaster.Entities;
using Watsons.TRV2.DA.MyMaster.Models.SupplierMaster;

namespace Watsons.TRV2.DA.MyMaster.Repositories
{
    public interface ISupplierMasterRepository : IRepository<SupplierMaster>
    {
        Task<IEnumerable<SupplierMaster>>? Search(SearchFilter entity);
    }
}

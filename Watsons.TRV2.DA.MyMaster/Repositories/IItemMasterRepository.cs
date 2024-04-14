using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.MyMaster.Entities;

namespace Watsons.TRV2.DA.MyMaster.Repositories
{
    public interface IItemMasterRepository : IRepository<ItemMaster>
    {
        Task<ItemMaster?> SearchByPluOrBarcode(string pluOrBarcode);
        Task<ItemMaster?> SearchByPlu(string plu);
        Task<ItemMaster?> SearchByBarcode(string barcode);
        Task<Dictionary<string, ItemMaster>> Dictionary(List<string> plus);
    }
}

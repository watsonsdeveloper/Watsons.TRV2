using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.MyMaster.Models;

namespace Watsons.TRV2.DA.MyMaster.Repositories
{
    public interface IMigrationRepository
    {
        //Task<bool> WriteOffItem(int storeId, string plu, int quantity);
        Task<bool> WriteOffOrder(long orderBatchId);
        Task<bool> InsertShipment(InsertShipmentParams parameters);
        Task<bool> InsertShipmentItem(InsertShipmentItemParams parameters);
        Task<bool> InsertShipmentStatusLog(InsertShipmentStatusLogParams parameters);
        Task<List<Entities.ShipmentItem>> SelectShipmentItems(SelectShipmentItemParams parameters);
    }
}

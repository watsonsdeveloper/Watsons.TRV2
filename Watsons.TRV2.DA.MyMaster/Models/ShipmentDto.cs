using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.MyMaster.Models
{
    public record InsertShipmentParams(int StoreId, string ShipmentNumber, string SupplierNumber, DateTime OrderDate, DateTime DeliveryDate, string Remark, string ImportLogId, string CreatedBy = "TRV2");
    public record UpdateShipmentParams(int StoreId, string ShipmentNumber, string Status, string ModifiedBy);

    public record InsertShipmentItemParams(int StoreId, string ShipmentNumber, string ItemCode, string SupplierItemCode, string Bardcode, int Qty, string CreatedBy = "TRV2");
    public record UpdateShipmentItemParams(int StoreId, string ShipmentNumber, string ItemCode, int ReceivedQty, string ModifiedBy);
    
    public record InsertShipmentStatusLogParams(int StoreId, string ShipmentNumber, string Status, string CreatedBy = "TRV2", string Type = "Shipment");

    public record SelectShipmentItemParams(int StoreId, List<string>ShipmentNumbers);
    public record ShipmentItem(string ShipmentNumber, string ItemCode, string SupplierItemCode, int Qty, int OriginalQty, int ReceivedQty, string ModifiedBy, DateTime ModificationTime);
}

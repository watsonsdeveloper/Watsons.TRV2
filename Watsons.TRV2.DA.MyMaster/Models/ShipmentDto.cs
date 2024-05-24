using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.MyMaster.Models
{
    public record InsertShipmentDto(int StoreId, string ShipmentNumber, string SupplierNumber, DateTime OrderDate, DateTime DeliveryDate, string Remark, string ImportLogId, string CreatedBy = "TRV2");
    public record UpdateShipmentDto(int StoreId, string ShipmentNumber, string Status, string ModifiedBy);

    public record InsertShipmentItemDto(int StoreId, string ShipmentNumber, string ItemCode, string SupplierItemCode, string Bardcode, int Qty, string CreatedBy = "TRV2");
    public record UpdateShipmentItemDto(int StoreId, string ShipmentNumber, string ItemCode, int ReceivedQty, string ModifiedBy);
    
    public record InsertShipmentStatusLog(int StoreId, string ShipmentNumber, string Status, string Remark, string CreatedBy, string Type = "Shipment");
}

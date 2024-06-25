using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.MyMaster.Entities
{
    public class ShipmentItem
    {
        public string ShipmentNumber { get; set; } = null!;
        public string ItemCode { get; set; } = null!;
        public string SupplierItemCode { get; set; } = null!;
        public int? Qty { get; set; }
        public int? OriginalQty { get; set; }
        public int? ReceivedQty { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModificationTime { get; set; }
    }
}

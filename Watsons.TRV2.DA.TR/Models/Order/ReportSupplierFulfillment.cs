using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.TR.Models.Order
{
    public class ReportSupplierFulFillmentResult
    {
        public string? SupplierName { get; set; }
        public int TotalOrder { get; set; }
        public int TotalOrderFulfilled { get; set; }
        public int TotalOrderUnfulfill { get; set; }
    }
}

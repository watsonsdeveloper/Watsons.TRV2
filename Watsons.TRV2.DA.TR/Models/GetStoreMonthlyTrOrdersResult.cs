using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.TR.Entities;

namespace Watsons.TRV2.DA.TR.Models
{

    public class GetStoreMonthlyTrOrdersResult
    {
        public List<TrOrderDetail> TrOrderList { get; set; }
        public decimal TotalCostUponApproval { get; set; }
        public decimal Threshold { get; set; }
    }
}

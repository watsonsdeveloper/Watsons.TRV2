using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.TR.Models.Order
{
    public class ListSearchParams
    {
        public long? TrOrderBatchId { get; set; }
        public byte? TrOrderStatus { get; set; }
        public string? PluOrBarcode { get; set; }
        public List<int>? StoreIds { get; set; }
        public byte? Brand { get; set; }
        public string? BrandName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

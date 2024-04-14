using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.TR.Models.Order
{
    public class OrderBatchList
    {
        public byte? BrandId { get; set; }
        public byte? TrOrderBatchStatus { get; set; }
        public List<int>? StoreIds { get; set; }
        public string? PluOrBarcode { get; set; }
        public long? TrOrderBatchId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

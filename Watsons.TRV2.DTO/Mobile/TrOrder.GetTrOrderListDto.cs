using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile.TrOrder
{
    public record GetTrOrderListRequest(int StoreId, string? PluOrBarcode, string? TrOrderBatchId, TrOrderStatus Status = TrOrderStatus.All);
    //public class GetTrOrderListRequest
    //{
    //    public string TrOrderBatchId { get; set; } = null!;
    //    public int StoreId { get; set; }
    //    public TrOrderStatus Status { get; set; } = TrOrderStatus.All;
    //    public string? PluOrBarcode { get; set; }
    //}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile.TrOrder
{
    public record GetTrOrderListRequest(int StoreId, Brand Brand, string? PluOrBarcode, long TrOrderBatchId, TrOrderStatus Status = TrOrderStatus.All);
}

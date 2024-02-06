using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DTO.Mobile
{
    public record GetTrOrderRequest(long TrOrderId, int StoreId);
}

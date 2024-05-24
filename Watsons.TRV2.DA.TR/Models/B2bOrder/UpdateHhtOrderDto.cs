using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DA.TR.Models.B2bOrder
{
    public record UpdateHhtOrderDto(long TrOrderId, HhtOrderStatus HhtOrderStatus, string? HhtRemark);
}

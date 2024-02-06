using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile
{
    public record TrListRequest([Required] int StoreId, TrStatus? Status);

    public record TrListResponse(
        List<TrPluDto>? TrPluList = null);
}

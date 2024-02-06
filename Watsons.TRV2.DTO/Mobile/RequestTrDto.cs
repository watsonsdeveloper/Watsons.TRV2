using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile
{
    public record RequestTrRequest(
        [Required] string Plu,
        [Required] int StoreId,
        [Required] string Reason,
        [Required] string CreatedBy,
        [Required] List<string> ImageBase64List
    );

    public record RequestTrResponse(ResponseCode Code = 0, string? Message = null)
        : Response(Code, Message);
}

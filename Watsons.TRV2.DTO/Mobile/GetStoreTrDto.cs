using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DTO.Mobile
{
    public record GetStoreTrRequest(
        [Required] string Store
        //TestingProductStatus? Status
        );

    public record GetStoreTrResponse(
        ) : Response;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO
{
    public record Response(ResponseCode Code = 0, string? Message = null);
    //public class Response
    //{
    //    public ResponseCode Code { get; set; } = ResponseCode.Error;
    //    public string? Message { get; set; }
    //}
}

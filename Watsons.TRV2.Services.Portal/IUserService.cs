using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DTO.Portal.User;
using Watsons.TRV2.DTO.Portal.User.MfaLoginDto;

namespace Watsons.TRV2.Services.Portal
{
    public interface IUserService
    {
        Task<ServiceResult<DTO.Portal.User.MfaLoginDto.Response>> MfaLogin(DTO.Portal.User.MfaLoginDto.Request request);
        Task<ServiceResult<string>> VerifyMfaLoginOtp(VerifyLoginOtpRequest reqeust);
    }
}

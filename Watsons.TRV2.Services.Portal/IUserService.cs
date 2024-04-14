using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DTO.Portal;
using Watsons.TRV2.DTO.Portal.User;
using Watsons.TRV2.DTO.Portal.User.MfaLoginDto;

namespace Watsons.TRV2.Services.Portal
{
    public interface IUserService
    {
        Task<string> DecodeJwtToken();
        Task<ServiceResult<Response>> MfaLogin(Request request);
        Task<ServiceResult<bool>> MfaLogout();
        Task<ServiceResult<string>> VerifyMfaLoginOtp(VerifyLoginOtpRequest reqeust);
        Task<ServiceResult<FetchUserProfileResponse>> FetchUserProfile(FetchUserProfileRequest request);
        ServiceResult<bool> AuthorizeStoreAccess(int storeId);
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DTO.Mobile.UserDto;

namespace Watsons.TRV2.Services.Mobile
{
    public interface IUserService
    {
        Task<ServiceResult<LoginResponse>> Login(LoginRequest request);
    }
}

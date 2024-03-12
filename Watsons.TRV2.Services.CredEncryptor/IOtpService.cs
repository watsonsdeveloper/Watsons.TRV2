using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.Services.CredEncryptor.DTO.OtpDto;

namespace Watsons.TRV2.Services.CredEncryptor
{
    public interface IOtpService
    {
        Task<bool> SendOtpByEmail(SendOtpByEmailRequest request);
        Task<bool> VerifyOtp(VerifyOtpRequest request);
    }
}

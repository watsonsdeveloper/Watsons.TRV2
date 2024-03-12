using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.Services.CredEncryptor
{
    public static class CredEncryptorApi
    {
        public static readonly string MfaLogin = "/Mfa/MfaLogin";
        public static readonly string SendOtpByEmail = "/Otp/SendOtpByEmail";
        public static readonly string VerifyOtp = "/Otp/VerifyOtp";
    }
}

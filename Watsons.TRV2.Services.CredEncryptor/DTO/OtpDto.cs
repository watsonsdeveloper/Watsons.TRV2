using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.Services.CredEncryptor.DTO.OtpDto
{
    public class SendOtpByEmailRequest
    {
        public string Email { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
    }
    public class VerifyOtpRequest 
    {
        public string Otp { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

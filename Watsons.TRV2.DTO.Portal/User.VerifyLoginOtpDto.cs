using System.ComponentModel.DataAnnotations;

namespace Watsons.TRV2.DTO.Portal.User
{
    public class VerifyLoginOtpRequest
    {
        [Required]
        public Guid? UserId { get; set; } = null!;
        [Required]
        public Guid? ApplicationId { get; set; } = null!;
        [Required]
        public string? Otp { get; set; } = null!;
    }
}

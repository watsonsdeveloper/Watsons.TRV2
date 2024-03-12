using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DTO.Portal.User.MfaLoginDto
{
    public class Request
    {
        [Required]
        public string? Username { get; set; } = null!;
        [Required]
        public string? Password { get; set; } = null!;
    }

    public class Response
    {
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

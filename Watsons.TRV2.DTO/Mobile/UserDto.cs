using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DTO.Mobile.UserDto
{
    public class LoginRequest
    {
        [Required]
        public string Url { get; set; }
        [Required]
        public string Request { get; set; }
        [Required]
        public string Response { get; set; }
    }
    public class LoginResponse
    {
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Watsons.TRV2.Services.CredEncryptor.DTO.MfaLoginDto
{
    public class Request
    {
        public Guid ApplicationId { get; set; }
        public string? Username { get; set; } = null!;
        public string? Password { get; set; } = null!;
    }

    public class Response
    {
        //[JsonPropertyName("userId")]
        public Guid? UserId { get; set; }
        //[JsonPropertyName("applicationId")]
        public Guid? ApplicationId { get; set; }
    }
}

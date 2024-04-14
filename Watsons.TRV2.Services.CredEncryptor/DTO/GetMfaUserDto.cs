using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.Services.CredEncryptor.DTO.GetMfaUserDto
{
    public class Request
    {
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
    }

    public class Response
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public Guid ApplicationId { get; set; }
        public string ApplicationName { get; set; } = null!;
    }
}

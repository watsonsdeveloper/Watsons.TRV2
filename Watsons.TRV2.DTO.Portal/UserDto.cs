using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DTO.Portal
{
    public class UserDto
    {
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public List<ModuleAccess>? ModuleAccesses { get; set; }
        public List<int>? StoreAccess { get; set; }
    }

    public class ModuleAccess
    {
        public string ModuleName { get; set; } = null!;
        public string Action { get; set; } = null!;
    }

    #region VerifyLoginOtpDto
    public class VerifyLoginOtpRequest
    {
        [Required]
        public Guid? UserId { get; set; }
        [Required]
        public Guid? ApplicationId { get; set; }
        [Required]
        public string? Otp { get; set; }
    }
    #endregion

    #region FetchUserPermissionDto
    public class FetchUserProfileRequest
    {
        [Required]
        public Guid? UserId { get; set; }
        [Required]
        public Guid? ApplicationId { get; set; }
    }

    public class FetchUserProfileResponse : UserDto
    {
        public Dictionary<string, List<string>> MassageModuleAccesses {
            get
            {
                if(this.ModuleAccesses == null)
                {
                    return new();
                }

                var moduleAccesses = new Dictionary<string, List<string>>();
                foreach (var module in this.ModuleAccesses)
                {
                    if (!moduleAccesses.TryGetValue(module.ModuleName, out var actions))
                    {
                        actions = new List<string>();
                        moduleAccesses[module.ModuleName] = actions;
                    }
                    actions.Add(module.Action);
                }
                return moduleAccesses;
            }
        }
    }
    #endregion
}

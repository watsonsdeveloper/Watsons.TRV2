using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.TR.Models
{
    public class RoleModuleAccessResult
    {
        public string ModuleAccess
        {
            get
            {
                return ModuleName + "_" + Action;
            }
        }
        public string ModuleName { get; set; } = null!;
        public string Action { get; set; } = null!;

    }
}

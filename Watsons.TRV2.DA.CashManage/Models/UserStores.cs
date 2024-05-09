using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.CashManage.Models
{
    public class UserStore
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? RoleCode { get; set; }
        public int? StoreId { get; set; }
        public string? Name { get; set; }
    }
}

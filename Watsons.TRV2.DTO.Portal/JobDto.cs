using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DTO.Portal
{
    public class UserStoreOrderPending
    {
        public string? Email { get; set; }
        public List<int>? StoreList { get; set; }
    }
}

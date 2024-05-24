using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.Services.Portal.Settings
{
    public class SupplierOrderSettings
    {
        public string? POFilePath { get; set; }
        public double B2BNumOfExpiryDay { get; set; } = 14;
    }
}

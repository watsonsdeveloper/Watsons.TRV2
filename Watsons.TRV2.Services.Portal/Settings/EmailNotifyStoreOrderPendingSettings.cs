using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.Services.Portal.Settings
{
    public class EmailNotifyStoreOrderPendingSettings
    {
        public List<string>? CopyRecipients { get; set; }
        public int RsomPendingDays { get; set; }
    }
}

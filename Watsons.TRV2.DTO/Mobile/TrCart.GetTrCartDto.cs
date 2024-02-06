using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile
{
    public class GetTrCartRequest
    {
        public int StoreId { get; set; }
        public long TrCartId { get; set; }
    }
}

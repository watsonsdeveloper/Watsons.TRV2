using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.TR.Entities;

namespace Watsons.TRV2.DA.TR.Models
{
    public class TrOrderDetail : TrOrder
    {
        public int StoreId { get; set; }

        public int BrandId { get; set; }
    }
}
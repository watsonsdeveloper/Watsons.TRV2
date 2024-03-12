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

        public byte Brand { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdateBy { get; set; }
    }
}
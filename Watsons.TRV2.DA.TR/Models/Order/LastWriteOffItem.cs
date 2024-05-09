using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DA.TR.Models.Order
{
    public class LastWriteOffItem
    {
        public string Plu { get; set; }
        public DateTime? LastWriteOffAt { get; set; }
        public int? WriteOffQuantity { get; set; }
    }

}

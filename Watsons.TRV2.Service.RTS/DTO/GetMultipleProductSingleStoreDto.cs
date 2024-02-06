using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.Service.RTS.DTO.GetMultipleProductSingleStore
{
    public class Request
    {
        public int storeID { get; set; }
        public List<string> pluList { get; set; }
    }
    public class Response
    {

    }
}

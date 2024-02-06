using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.Service.RTS.DTO.GetSingleProductMultiStore
{
    public class Request
    {
        public List<int> storeList { get; set; }
        public string plu { get; set; }
    }

    public class Response
    {

    }
}

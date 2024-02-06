using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.DTO.Mobile;

namespace Watsons.TRV2.Services.Mobile
{
    public class AddToTrOrderRequest
    {
        public List<TrCartDto> TrCartDtoList { get; set; } = null!;
        public int StoreId { get; set; }
        public Brand Brand { get; set; }
        public string CreatedBy { get; set; } = null!;
    }
}

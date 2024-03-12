using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile.TrCart
{
    public class AddToTrCartRequest
    {
        public string PluOrBarcode { get; set; } = null!;
        public int StoreId { get; set; }
        public Brand Brand { get; set; }
        public string CreatedBy { get; set; } = null!;
    }

    public record AddToTrCartResponse(TrCartDto? TrCart);
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile.TrCart
{
    public class UpdateTrCartRequirementRequest
    {
        public int StoreId { get; set; }
        public long TrCartId { get; set; }
        [Required]
        public TrReason? Reason { get; set; }
        public string? Justification { get; set; }
        public string UpdatedBy { get; set; } = null!;
    }
}

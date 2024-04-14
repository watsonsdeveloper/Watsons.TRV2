using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile
{
    public class GetTrOrderBatchListRequest
    {
        [Required]
        public int StoreId { get; set; }
        public Brand? Brand { get; set; }
        public long? TrOrderBatchId { get; set; }
        public TrOrderBatchStatus? TrOrderBatchStatus { get; set; }
        public string? PluOrBarcode { get; set; }
        [Required]
        public int Page { get; set; } = 0;
        [Required]
        public int PageSize { get; set; }

    }
}

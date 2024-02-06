using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DTO.Mobile.TrOrder
{
    public record TrOrderBatchDto(string TrOrderBatchId, TrOrderBatchStatus Status, int StoreId, Brand Brand, DateTime CreatedAt, string CreatedBy);

    //public class TrOrderBatchDto
    //{
    //    public string TrOrderBatchId { get; set; } = null!;

    //    public TrOrderBatchStatus Status { get; set; }

    //    public int StoreId { get; set; }

    //    public Brand Brand { get; set; }

    //    public DateTime? CreatedAt { get; set; }

    //    public string? CreatedBy { get; set; }
    //}
}

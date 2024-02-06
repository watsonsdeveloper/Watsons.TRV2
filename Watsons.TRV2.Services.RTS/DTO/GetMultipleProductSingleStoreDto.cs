using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Watsons.TRV2.Services.RTS.DTO.GetMultipleProductSingleStore
{
    public class Request
    {
        public int StoreID { get; set; }
        public List<string> PluList { get; set; } = null!;
    }
    public class Response
    {
        [JsonPropertyName("storeID")]
        public int StoreID { get; set; }
        [JsonPropertyName("plu")]
        public string? Plu { get; set; }
        [JsonPropertyName("availableStock")]
        public double AvailableStock { get; set; }
    }
}

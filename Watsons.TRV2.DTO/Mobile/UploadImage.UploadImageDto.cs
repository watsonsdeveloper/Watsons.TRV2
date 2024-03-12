using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DTO.Mobile.UploadImage
{
    public class UploadImageRequest
    {
        public int StoreId { get; set; }
        public string Base64Image { get; set; } = null!;
        public long TrCartId { get; set; }
    }
}

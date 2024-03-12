using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DTO.Mobile.UploadImage
{
    public class DeleteUploadedImagesRequest
    {
        public int StoreId { get; set; }
        public long TrCartId { get; set; }
        public List<long> ImageIds { get; set; }
    }
}

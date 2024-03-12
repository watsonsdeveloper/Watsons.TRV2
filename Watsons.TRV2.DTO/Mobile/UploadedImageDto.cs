using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DTO.Mobile.UploadedImage
{
    public class UploadedImageDto
    {
        public long TrImageId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public long TrCartId { get; set; }
    }
}

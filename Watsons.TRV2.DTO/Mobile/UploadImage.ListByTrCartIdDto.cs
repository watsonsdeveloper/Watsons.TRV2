﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DTO.Mobile.UploadImage
{
    public class ListByTrCartIdRequest
    {
        public int StoreId { get; set; }
        public long TrCartId { get; set; }
    }
}

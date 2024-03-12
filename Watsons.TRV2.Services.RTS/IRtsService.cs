using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.Services.RTS
{
    public interface IRtsService
    {
        Task<Dictionary<string, int>?> GetMultipleProductSingleStore(DTO.GetMultipleProductSingleStore.Request request);
    }
}

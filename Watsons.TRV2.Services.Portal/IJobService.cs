using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;

namespace Watsons.TRV2.Services.Portal
{
    public interface IJobService
    {
        Task<ServiceResult<bool>> EmailNotifyStoreOrderPending();
        Task<ServiceResult<bool>> SubmitToB2B();
        Task<ServiceResult<bool>> CreateStoreHhtOrder();
    }
}

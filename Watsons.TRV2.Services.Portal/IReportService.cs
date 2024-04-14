using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DTO.Portal;

namespace Watsons.TRV2.Services.Portal
{
    public interface IReportService
    {
        Task<ServiceResult<ReportOwnResponse>> FetchReportOwn(ReportOwnRequest request);
        Task<ServiceResult<ReportSupplierResponse>> FetchReportSupplier(ReportSupplierRequest request);
        //Task<ServiceResult<ReportSupplierFulfillmentResponse>> FetchReportSupplierFulfillment(ReportSupplierFulfillmentRequest request);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models.Order;

namespace Watsons.TRV2.DA.TR.Repositories
{

    public interface ITrOrderRepository : IRepository<TrOrder>
    {
        Task<bool> UpdateRange(List<TrOrder> entities);
        Task<TrOrder?> Select(long id);
        Task<TrOrder?> Select(long id, int storeId);
        Task<decimal?> TotalAccumulatedApprovedCost(long trBatchOrderId);
        Task<TrOrder> InsertTrOrder(TrOrder entity);
        Task<List<TrOrder>> InsertRangeTrOrder(List<TrOrder> entities);
        Task<IEnumerable<TrOrder>> ListSearch(ListSearchParams parameters);
        Task<IEnumerable<TrOrder>> List(int storeId, byte? status);
        Task<IEnumerable<ReportSupplierFulFillmentResult>> ReportSupplierFulFillment(List<int>? storeIds, DateTime? startDT, DateTime? endDT, string? supplierName);
        //Task<OrderItemsWithCostSummary?> GetOrderItemsWithCostSummary(long trOrderBatchId, byte? trOrderStatus, string? pluOrBarcode);
        Task<GetStoreMonthlyTrOrdersResult?> GetStoreMonthlyTrOrders(int storeId, byte brand);
        Task<Dictionary<string, int>> GetProductQuantityOfMonthlyStoreOrder(int storeId, byte brand);
        Task<bool> HasOrderPending(int storeId, string plu);
        Task<bool> HasOrderProcessed(int storeId, string plu);

    }
}

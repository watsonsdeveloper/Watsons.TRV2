using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public interface ITrOrderRepository : IRepository<TrOrder>
    {
        Task<TrOrder?> Select(long id);
        Task<TrOrder?> Select(long id, int storeId);
        Task<TrOrder> InsertTrOrder(TrOrder entity);
        Task<List<TrOrder>> InsertRangeTrOrder(List<TrOrder> entities);
        Task<IEnumerable<TrOrder>> List(long? trOrderBatchId, byte? status, string? pluOrBarcode);
        Task<IEnumerable<TrOrder>> List(int storeId, byte? status);
        Task<GetStoreMonthlyTrOrdersResult?> GetStoreMonthlyTrOrders(int storeId, byte brand);
        Task<Dictionary<string, int>> GetProductQuantityOfMonthlyStoreOrder(int storeId, byte brand);
        Task<bool> HasOrderPending(int storeId, string plu);
    }
    public class TrOrderRepository : ITrOrderRepository
    {
        private readonly TrContext _context;
        public TrOrderRepository(TrContext context)
        {
            _context = context;
        }

        public Task<bool> Delete(TrOrder entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TrOrder> Insert(TrOrder entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TrOrder>> List(TrOrder entity)
        {
            throw new NotImplementedException();
        }

        public Task<TrOrder> Select(TrOrder entity)
        {
            throw new NotImplementedException();
        }

        public Task<TrOrder> Update(TrOrder entity)
        {
            throw new NotImplementedException();
        }
        public async Task<TrOrder?> Select(long id)
        {
            return await _context.TrOrders
                .FirstOrDefaultAsync(o => o.TrOrderId == id);
        }
        public async Task<TrOrder?> Select(long id, int storeId)
        {
            return await _context.TrOrders
                .Include(o => o.TrOrderBatch)
                .Where(o => o.TrOrderId == id && o.TrOrderBatch.StoreId == storeId)
                .FirstOrDefaultAsync();
        }

        public async Task<TrOrder> InsertTrOrder(TrOrder entity)
        {
            await _context.TrOrders.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<List<TrOrder>> InsertRangeTrOrder(List<TrOrder> entities)
        {
            await _context.TrOrders.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return entities;
        }

        /// <summary>
        /// return a batch of order list.
        /// </summary>
        /// <param name="trOrderBatchId"></param>
        /// <param name="storeId"></param>
        /// <param name="status"></param>
        /// <param name="pluOrBarcode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<TrOrder>> List(long? trOrderBatchId, byte? status, string? pluOrBarcode)
        {
            var list = new List<TrOrder>();
            var query = _context.TrOrders
                .Include(o => o.TrOrderBatch)
                .AsNoTracking();

            if (trOrderBatchId != null)
                query = query.Where(o => o.TrOrderBatchId == trOrderBatchId);

            if (status != null)
                query = query.Where(o => o.TrOrderStatus == status);

            if (!string.IsNullOrWhiteSpace(pluOrBarcode))
                query = query.Where(o => o.Plu.Contains(pluOrBarcode) || (o.Barcode != null && o.Barcode.Contains(pluOrBarcode)));

            return await query.ToListAsync();
        }
        /// <summary>
        /// return store order list.
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TrOrder>> List(int storeId, byte? status)
        {
            var list = new List<TrOrder>();
            var query = _context.TrOrders
                .Include(o => o.TrOrderBatch)
                .Where(o => o.TrOrderBatch.StoreId == storeId);

            if (status != null)
                query = query.Where(o => o.TrOrderStatus == status);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<GetStoreMonthlyTrOrdersResult?> GetStoreMonthlyTrOrders(int storeId, byte brand)
        {
            GetStoreMonthlyTrOrdersResult? result;
            var now = DateTime.Now;
            var startDT = new DateTime(now.Year, now.Month, 1);
            var endDT = startDT.AddMonths(1).AddSeconds(-1);


            var list = await _context.TrOrderBatches
                .Where(o => o.StoreId == storeId && o.Brand == brand
                && o.TrOrderBatchStatus == (byte)TrOrderBatchStatus.Completed
                //&& o.TrOrders.Any(t => t.Status == (byte)TrOrderStatus.Approved)
                && o.CreatedAt >= startDT && o.CreatedAt <= endDT)
                .SelectMany(o => o.TrOrders
                .Where(t => t.TrOrderStatus == (byte)TrOrderStatus.Approved).Select(t => new TrOrderDetail()
                {
                    // Properties from TrOrderBatches
                    StoreId = o.StoreId,
                    Brand = o.Brand,
                    //BatchCreatedAt = o.CreatedAt,
                    // Properties from TrOrders
                    TrOrderId = t.TrOrderId,
                    Plu = t.Plu,
                    Barcode = t.Barcode,
                    Reason = t.Reason,
                    Justification = t.Justification,
                    TrOrderStatus = t.TrOrderStatus,
                    SupplierName = t.SupplierName,
                    SupplierCode = t.SupplierCode,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                    CreatedBy = t.CreatedBy
                }))
                .ToListAsync();
            result = new GetStoreMonthlyTrOrdersResult()
            {
                TrOrderList = list,
                //TotalCostUponApproval = list.Sum(o => o.CostUponApproval),
                //TotalCostUponApprovalWithTax = list.Sum(o => o.CostUponApprovalWithTax)
            };
            return result;
        }

        public async Task<Dictionary<string, int>> GetProductQuantityOfMonthlyStoreOrder(int storeId, byte brand)
        {
            var now = DateTime.Now;
            var startDT = new DateTime(now.Year, now.Month, 1);
            var endDT = startDT.AddMonths(1).AddSeconds(-1);

            Dictionary<string, int> result = await _context.TrOrderBatches
                .Where(o => o.StoreId == storeId && o.Brand == brand
                && o.TrOrderBatchStatus == (byte)TrOrderBatchStatus.Completed
                && o.CreatedAt >= startDT && o.CreatedAt <= endDT)
                .SelectMany(o => o.TrOrders.Where(t => t.TrOrderStatus == (byte)TrOrderStatus.Approved))
                .GroupBy(x => x.Plu)
                .ToDictionaryAsync(x => x.Key, x => x.Count());
            
            return result;
        }

        /// <summary>
        /// for store checking any order pending before add to cart.
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="plu"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> HasOrderPending(int storeId, string plu)
        {
            return await _context.TrOrders
                .Include(o => o.TrOrderBatch)
                .Where(o => o.TrOrderBatch.StoreId == storeId && o.Plu == plu && o.TrOrderStatus == (byte)TrOrderStatus.Pending)
                .AnyAsync();
        }
    }
}

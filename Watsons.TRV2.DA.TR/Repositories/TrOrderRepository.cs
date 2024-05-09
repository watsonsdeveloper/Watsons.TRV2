using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Watsons.Common.ConnectionHelpers;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models.Order;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public class TrOrderRepository : ITrOrderRepository
    {
        private readonly TrContext _context;
        private readonly IConfiguration _configuration;

        public TrOrderRepository(TrContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        public async Task<bool> UpdateRange(List<TrOrder> entities)
        {
            //var trOrderBatchId = entities.FirstOrDefault()?.TrOrderBatchId;
            //var list = await _context.TrOrders.Where(o => o.TrOrderBatchId == trOrderBatchId).ToListAsync();

            //foreach (var entity in entities)
            //{
            //    var item = list.FirstOrDefault(o => o.TrOrderId == entity.TrOrderId);
            //    if (item != null)
            //    {
            //        item.Plu = entity.Plu;
            //        item.Barcode = entity.Barcode;
            //        item.Reason = entity.Reason;
            //        item.Justification = entity.Justification;
            //        item.TrOrderStatus = entity.TrOrderStatus;
            //        item.SupplierName = entity.SupplierName;
            //        item.SupplierCode = entity.SupplierCode;
            //        item.CreatedAt = entity.CreatedAt;
            //        item.CreatedBy = entity.CreatedBy;
            //    }
            //}

            //_mapper.Map(entities, list);

            _context.TrOrders.UpdateRange(entities);
            await _context.SaveChangesAsync();

            return true;

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
        public async Task<decimal?> TotalAccumulatedApprovedCost(long trBatchOrderId)
        {
            var trBatchOrder = _context.TrOrderBatches
                .Where(o => o.TrOrderBatchId == trBatchOrderId)
                .AsNoTracking()
                .FirstOrDefault();

            if (trBatchOrder == null)
            {
                return null;
            }

            return await _context.TrOrders
                .Include(o => o.TrOrderBatch)
                .Where(o => o.TrOrderBatch.StoreId == trBatchOrder.StoreId
                    && o.TrOrderBatch.Brand == trBatchOrder.Brand
                    && o.TrOrderBatchId != trBatchOrderId
                    && o.TrOrderStatus == (byte)TrOrderStatus.Approved)
                .SumAsync(o => o.AverageCost);
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
        public async Task<IEnumerable<TrOrder>> ListSearch(ListSearchParams parameters)
        {
            IQueryable<TrOrder> query = _context.TrOrders
                .Include(o => o.TrOrderBatch);

            if (parameters.TrOrderBatchId != null)
                query = query.Where(o => o.TrOrderBatchId == parameters.TrOrderBatchId);

            if (parameters.StoreIds != null && parameters.StoreIds.Count > 0)
                query = query.Where(o => parameters.StoreIds.Contains(o.TrOrderBatch.StoreId));

            if (parameters.Brand != null)
                query = query.Where(o => o.TrOrderBatch.Brand == parameters.Brand);

            if (parameters.Status != null)
                query = query.Where(o => o.TrOrderStatus == parameters.Status);

            if (parameters.StartDate != null && parameters.EndDate != null)
                query = query.Where(o => o.TrOrderBatch.CreatedAt >= parameters.StartDate && o.TrOrderBatch.CreatedAt <= parameters.EndDate);

            if (!string.IsNullOrWhiteSpace(parameters.PluOrBarcode))
                query = query.Where(o => o.Plu.Contains(parameters.PluOrBarcode) || (o.Barcode != null && o.Barcode.Contains(parameters.PluOrBarcode)));

            return await query.OrderByDescending(x => x.TrOrderId).ToListAsync();
        }

        public async Task<IEnumerable<ReportSupplierFulFillmentResult>> ReportSupplierFulFillment(List<int>? storeIds, DateTime? startDate, DateTime? endDate, string? supplierName)
        {
            IQueryable<TrOrder> query = _context.TrOrders
               .Include(o => o.TrOrderBatch);

            if (storeIds != null && storeIds.Count > 0)
                query = query.Where(o => storeIds.Contains(o.TrOrderBatch.StoreId));

            //query = query.Where(o => o.TrOrderBatch.Brand == (byte)Brand.Supplier);

            //query = query.Where(o => o.TrOrderStatus == (byte)TrOrderStatus.Fulfilled || o.TrOrderStatus == (byte)TrOrderStatus.Unfulfilled);
            query = query.Where(o => o.TrOrderStatus == (byte)TrOrderStatus.Approved || o.TrOrderStatus == (byte)TrOrderStatus.Rejected);

            if (startDate != null && endDate != null)
                query = query.Where(o => o.TrOrderBatch.CreatedAt >= startDate && o.TrOrderBatch.CreatedAt <= endDate);

            if (!string.IsNullOrWhiteSpace(supplierName))
                query = query.Where(o => o.SupplierName != null && o.SupplierName.Contains(supplierName));

            var result = await query.GroupBy(o => o.SupplierCode)
                .Select(o => new ReportSupplierFulFillmentResult
                {
                    SupplierName = o.FirstOrDefault().SupplierName,
                    TotalOrder = o.Count(),
                    TotalOrderFulfilled = o.Where(t => t.TrOrderStatus == (byte)TrOrderStatus.Approved).Count(),
                    TotalOrderUnfulfill = o.Where(t => t.TrOrderStatus == (byte)TrOrderStatus.Rejected).Count(),
                })
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// for add to cart checking purpose.
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

        //public async Task<OrderItemsWithCostSummary?> GetOrderItemsWithCostSummary(long trOrderBatchId, byte? trOrderStatus, string? pluOrBarcode)
        //{
        //    var list = new List<TrOrder>();

        //    var query = _context.TrOrders
        //        .AsNoTracking()
        //        .Where(o => o.TrOrderBatchId == trOrderBatchId);

        //    if (trOrderStatus != null)
        //        query = query.Where(o => o.TrOrderStatus == trOrderStatus);

        //    var trOrde = await query.ToListAsync();

        //    if (!string.IsNullOrWhiteSpace(pluOrBarcode))
        //        query = query.Where(o => o.Plu.Contains(pluOrBarcode) || (o.Barcode != null && o.Barcode.Contains(pluOrBarcode)));

        //    var orderItems = await query.ToListAsync();

        //    var orderSummary = _context.TrOrderBatches
        //        .Include(o => o.OrderCost)
        //        .Where(o => o.TrOrderBatchId == trOrderBatchId)
        //        .Select(o => new OrderItemsWithCostSummary()
        //        {
        //            TrOrderItems = orderItems,
        //            StoreId = o.StoreId,
        //            CostThresholdSnapshot = o.OrderCost != null ? o.OrderCost.CostThresholdSnapshot : null,
        //            AccumulatedCostApproved = o.OrderCost != null ? o.OrderCost.AccumulatedCostApproved : null,
        //            TotalOrderCost = o.OrderCost != null ? o.OrderCost.TotalOrderCost : null,
        //            TotalCostApproved = o.OrderCost != null ? o.OrderCost.TotalCostApproved : null,
        //            TotalCostRejected = o.OrderCost != null ? o.OrderCost.TotalCostRejected : null,
        //            CreatedAt = o.CreatedAt,
        //            CreatedBy = o.CreatedBy ?? string.Empty,
        //            UpdatedAt = o.UpdatedAt,
        //            UpdatedBy = o.UpdatedBy
        //        })
        //        .FirstOrDefault();

        //    return orderSummary;
        //}

        public async Task<GetStoreMonthlyTrOrdersResult?> GetStoreMonthlyTrOrders(int storeId, byte brand)
        {
            GetStoreMonthlyTrOrdersResult? result;
            var now = DateTime.Now;
            var startDT = new DateTime(now.Year, now.Month, 1);
            var endDT = startDT.AddMonths(1).AddSeconds(-1);


            var list = await _context.TrOrderBatches
                .Where(o => o.StoreId == storeId && o.Brand == brand
                && o.TrOrderBatchStatus == (byte)TrOrderBatchStatus.Processed
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
                && o.TrOrderBatchStatus == (byte)TrOrderBatchStatus.Processed
                && o.CreatedAt >= startDT && o.CreatedAt <= endDT)
                .SelectMany(o => o.TrOrders.Where(t => t.TrOrderStatus == (byte)TrOrderStatus.Approved || t.TrOrderStatus == (byte)TrOrderStatus.Fulfilled))
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

        public async Task<bool> HasOrderProcessed(int storeId, string plu)
        {
            return await _context.TrOrders
                .Include(o => o.TrOrderBatch)
                .Where(o => o.TrOrderBatch.StoreId == storeId && o.Plu == plu && o.TrOrderStatus == (byte)TrOrderStatus.Processed)
                .AnyAsync();
        }

        public async Task<bool> InsertStoreAdjustment(long TrOrderBatchId)
        {
            var orders = await _context.TrOrders
                .Include(o => o.TrOrderBatch)
                .Where(o => o.TrOrderBatchId == TrOrderBatchId
                    && o.TrOrderStatus == (byte)TrOrderStatus.Approved
                    && o.TrOrderBatch.TrOrderBatchStatus == (byte)TrOrderBatchStatus.Processed
                    && o.TrOrderBatch.Brand == (byte)Brand.Own)
                .ToListAsync();

            if (orders == null || orders.Count == 0)
            {
                return false;
            }

            foreach (var order in orders)
            {
                var storeAdjustmentItem = new StoreAdjustment()
                {
                    TrOrderBatchId = TrOrderBatchId,
                    TrOrderId = order.TrOrderId,
                    StoreId = order.TrOrderBatch.StoreId,
                    Plu = order.Plu,
                    Qty = 1,
                    ReasonCode = "40",
                    CreatedBy = $"{_configuration["AppName"]} - {order.TrOrderId}",
                };
                await _context.StoreAdjustments.AddAsync(storeAdjustmentItem);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Dictionary<string, LastWriteOffItem>?> LastWriteOffDict(int storeId, List<string> pluList)
        {
            Dictionary<string, LastWriteOffItem>? result = await _context.TrOrders.Include(o => o.TrOrderBatch)
                .Where(o => o.TrOrderBatch.StoreId == storeId
                    && o.TrOrderStatus == (byte)TrOrderStatus.Approved
                    && pluList.Contains(o.Plu))
                .GroupBy(o => o.Plu)
                .Select(group => group.OrderByDescending(o => o.TrOrderBatch.UpdatedAt).First())
                .ToDictionaryAsync(o => o.Plu, o => new LastWriteOffItem
                {
                    Plu = o.Plu,
                    LastWriteOffAt = o.TrOrderBatch.UpdatedAt,
                    WriteOffQuantity = 1
                });
            return result;
        }
    }

}

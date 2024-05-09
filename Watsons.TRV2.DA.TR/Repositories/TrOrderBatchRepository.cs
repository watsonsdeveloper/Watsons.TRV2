using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models.Order;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public class TrOrderBatchRepository : ITrOrderBatchRepository
    {
        private readonly TrContext _context;
        private readonly IMapper _mapper;
        public TrOrderBatchRepository(TrContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<bool> Delete(TrOrderBatch entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TrOrderBatch> Insert(TrOrderBatch entity)
        {
            try
            {
                await _context.TrOrderBatches.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception();
            }
            return entity;
        }

        public Task<IEnumerable<TrOrderBatch>> List(TrOrderBatch entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// For Admin and Store share use.
        /// storeId list is required for Admin and Store.
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="bandId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<TrOrderBatch>> List(OrderBatchList entity)
        {
            var list = new List<TrOrderBatch>();
            var query = _context.TrOrderBatches
                .Include(o => o.TrOrders)
                .Where(o => o.Brand == entity.BrandId);

            if(entity.TrOrderBatchId != null)
            {
                query = query.Where(o => o.TrOrderBatchId == entity.TrOrderBatchId);
            }

            if (entity.StoreIds != null && entity.StoreIds.Count > 0)
            {
                query = query.Where(o => entity.StoreIds.Contains(o.StoreId));
            }

            if (entity.TrOrderBatchStatus != null && entity.TrOrderBatchStatus != 0)
            {
                query = query.Where(o => o.TrOrderBatchStatus == entity.TrOrderBatchStatus);
            }

            if (entity.StartDate != null && entity.EndDate != null)
            {
                query = query.Where(o => o.CreatedAt >= entity.StartDate && o.CreatedAt <= entity.EndDate);
            }

            if (!string.IsNullOrEmpty(entity.PluOrBarcode))
            {
                query.Where(o => o.TrOrders.Any(o => o.Plu == entity.PluOrBarcode || o.Barcode == entity.PluOrBarcode));
            }

            list = await query.OrderByDescending(o => o.TrOrderBatchId).AsNoTracking().ToListAsync();

            return list;
        }

        public Task<TrOrderBatch> Select(TrOrderBatch entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TrOrderBatch?> Select(long id)
        {
            return await _context.TrOrderBatches
                .FirstOrDefaultAsync(o => o.TrOrderBatchId == id);
        }

        public async Task<TrOrderBatch?> Select(long id, int storeId)
        {
            return await _context.TrOrderBatches
                .FirstOrDefaultAsync(o => o.TrOrderBatchId == id && o.StoreId == storeId);
        }

        public async Task<TrOrderBatch?> SelectWithOrderCost(long id)
        {
            return await _context.TrOrderBatches
                .Include(o => o.OrderCost)
                .Where(o => o.TrOrderBatchId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<OrderSummary?> SelectSummary(long id)
        {
            var result = await _context.TrOrderBatches
                .Include(o => o.OrderCost)
                .Where(o => o.TrOrderBatchId == id)
                .Select(o => new OrderSummary
                {
                    TrOrderBatchId = o.TrOrderBatchId,
                    StoreId = o.StoreId,
                    TrOrderBatchStatus = o.TrOrderBatchStatus,
                    CreatedAt = o.CreatedAt,
                    CreatedBy = o.CreatedBy ?? string.Empty,
                    UpdatedAt = o.UpdatedAt,
                    UpdatedBy = o.UpdatedBy,
                    CostThresholdSnapshot = o.OrderCost != null ? o.OrderCost.CostThresholdSnapshot : null,
                    AccumulatedCostApproved = o.OrderCost != null ? o.OrderCost.AccumulatedCostApproved : null,
                    TotalCostApproved = o.OrderCost != null ? o.OrderCost.TotalCostApproved : null,
                    TotalCostRejected = o.OrderCost != null ? o.OrderCost.TotalCostRejected : null,
                    TotalOrderCost = o.OrderCost != null ? o.OrderCost.TotalOrderCost : null
                }).FirstOrDefaultAsync();

            return result;
        }

        public Task<TrOrderBatch> Update(TrOrderBatch entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWithOrderCost(TrOrderBatch entity)
        {
            var updatedEntity = await _context.TrOrderBatches
                .Include(o => o.OrderCost)
                .Where(o => o.TrOrderBatchId == entity.TrOrderBatchId)
                .FirstOrDefaultAsync();

            if (updatedEntity == null)
            {
                return false;
            }

            updatedEntity = _mapper.Map<TrOrderBatch>(entity);
            _context.TrOrderBatches.Update(updatedEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<TrOrderBatch>> PendingList()
        {
            return await _context.TrOrderBatches
                .Where(o => o.TrOrderBatchStatus == (byte)TrOrderBatchStatus.Pending)
                .ToListAsync();
        }

    }
}

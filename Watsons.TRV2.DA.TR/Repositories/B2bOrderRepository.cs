using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models.B2bOrder;
using Watsons.TRV2.DTO.Common;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public class B2bOrderRepository : IB2bOrderRepository
    {
        private readonly TrContext _context;
        public B2bOrderRepository(TrContext context)
        {
            _context = context;
        }   
        public Task<bool> Delete(B2bOrder entity)
        {
            throw new NotImplementedException();
        }

        public async Task<B2bOrder> Insert(B2bOrder entity)
        {
            var exists = await _context.B2bOrders.FindAsync(entity.TrOrderId) != null;
            if(exists)
            {
                return entity;
            }
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<B2bOrder>> List(B2bOrder entity)
        {
            IQueryable<B2bOrder> query = _context.B2bOrders;

            if (entity.HhtInsertStatus != 0)
            {
                query = query.Where(o => o.HhtInsertStatus == entity.HhtInsertStatus);
            }

            return await query.ToListAsync();
        }

        public Task<B2bOrder> Select(B2bOrder entity)
        {
            throw new NotImplementedException();
        }

        public async Task<B2bOrder> Update(B2bOrder entity)
        {
            // prevent entity some property donot update
            throw new NotImplementedException();
        }

        public async Task<B2bOrder?> UpdateHhtOrder(UpdateHhtOrderDto dto)
        {
            var hhtOrder = await _context.B2bOrders.FindAsync(dto.TrOrderId);
            if (hhtOrder != null)
            {
                hhtOrder.HhtInsertStatus = (byte)dto.HhtOrderStatus;
                hhtOrder.HhtInsertAt = DateTime.Now;
                hhtOrder.HhtRemark = dto.HhtRemark;
                await _context.SaveChangesAsync();
            }
            return hhtOrder;
        }

        public async Task<IEnumerable<TrOrder>> HhtOrderList()
        {
            IQueryable<TrOrder> query = _context.TrOrders
                .Include(o => o.TrOrderBatch)
                .Include(o => o.B2bOrder)
                .Where(o => o.TrOrderBatch.Brand == (byte)Brand.Supplier 
                        && o.TrOrderBatch.TrOrderBatchStatus == (byte)TrOrderBatchStatus.Processed
                        && o.TrOrderStatus == (byte)TrOrderStatus.Processing);

            return await query.ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public interface ITrOrderBatchRepository : IRepository<TrOrderBatch>
    {
        Task<TrOrderBatch?> Select(long id);
        Task<TrOrderBatch?> Select(long id, int storeId);
        Task<IEnumerable<TrOrderBatch>> List(List<int> storeIds, byte? brandId, byte? status, string? pluOrBarcode);
    }
    public class TrOrderBatchRepository : ITrOrderBatchRepository
    {
        private readonly TrContext _context;
        public TrOrderBatchRepository(TrContext context)
        {
            _context = context;
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
        public async Task<IEnumerable<TrOrderBatch>> List(List<int> storeIds, byte? brandId, byte? status, string? pluOrBarcode)
        {
            var list = new List<TrOrderBatch>();
            try
            {
                var query = _context.TrOrderBatches
                    .Include(o => o.TrOrders)
                    .Where(o => storeIds.Contains(o.StoreId));

                if (status != null)
                    query = query.Where(o => o.TrOrders.Any(o => o.TrOrderStatus == status));

                if (brandId != null)
                    query = query.Where(o => o.Brand == brandId);

                if(!string.IsNullOrEmpty(pluOrBarcode))
                    query.Where(o => o.TrOrders.Any(o => o.Plu == pluOrBarcode || o.Barcode == pluOrBarcode));

                list = await query.OrderByDescending(o => o.TrOrderBatchId).AsNoTracking().ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception();
            }
            return list;
        }

        public Task<TrOrderBatch> Select(TrOrderBatch entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TrOrderBatch?> Select(long id)
        {
            try
            {
                return await _context.TrOrderBatches
                    .FirstOrDefaultAsync(o => o.TrOrderBatchId == id);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        public async Task<TrOrderBatch?> Select(long id, int storeId)
        {
            try
            {
                return await _context.TrOrderBatches
                    .FirstOrDefaultAsync(o => o.TrOrderBatchId == id && o.StoreId == storeId);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        public Task<TrOrderBatch> Update(TrOrderBatch entity)
        {
            throw new NotImplementedException();
        }

        
    }
}

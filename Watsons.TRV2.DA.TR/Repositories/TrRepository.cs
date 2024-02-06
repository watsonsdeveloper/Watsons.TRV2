using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;

namespace Watsons.TRV2.DA.Repositories
{
    public interface ITrRepository : IRepository<TrPlu>
    {
        Task<TrPlu?> Select(long storeId, long pluId);
        Task<IEnumerable<TrPlu>> GetStoreTrPluList(int store, byte? status, string? pluOrBarcode);
    }
    public class TrRepository : ITrRepository
    {
        private readonly TrContext _context;
        public TrRepository(TrContext context)
        {
            _context = context;
        }
        public Task<bool> Delete(TrPlu entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TrPlu> Insert(TrPlu entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<IEnumerable<TrPlu>> List(TrPlu entity)
        {
            throw new NotImplementedException();
        }

        public Task<TrPlu> Select(TrPlu entity)
        {
            throw new NotImplementedException();
        }

        public Task<TrPlu> Update(TrPlu entity)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<TrPlu>> GetStoreTrPluList(int storeId, byte? status, string? pluOrBarcode)
        {
            var query = _context.TrPlus.AsNoTracking().Where(tp => tp.StoreId == storeId);
            if (status != null)
            {
                query = query.Where(tp => tp.Status == status);
            }
            if (!string.IsNullOrWhiteSpace(pluOrBarcode))
            {
                query = query.Where(tp => tp.Plu.Contains(pluOrBarcode) || tp.Barcode.Contains(pluOrBarcode));
            }
            return await query.Take(100).ToListAsync();
        }
        public async Task<TrPlu?> Select(long storeId, long pluId)
        {
            return await _context.TrPlus.AsNoTracking().FirstOrDefaultAsync(tp => tp.StoreId == storeId && tp.TrPluId == pluId);
        }
        

    }
}

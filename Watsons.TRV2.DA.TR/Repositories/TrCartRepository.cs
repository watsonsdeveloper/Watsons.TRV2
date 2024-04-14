using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public interface ITrCartRepository : IRepository<TrCart>
    {
        Task<bool> Delete(TrCart entity);
        Task<bool> DeleteRange(List<long> trCartIds);
        Task<TrCart> Insert(TrCart entity);
        Task<IEnumerable<TrCart>> List(int storeId, byte brandId);
        Task<TrCart?> Select(long trCartId, int storeId);

        Task<bool> HasInCart(int storeId, string plu);
    }
    public class TrCartRepository : ITrCartRepository
    {
        private readonly TrContext _context;
        public TrCartRepository(TrContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(TrCart entity)
        {
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRange(List<long> trCartIds)
        {
            var trCart = _context.TrCarts
                .Where(c => trCartIds.Contains(c.TrCartId))
                .ToList();

            trCart.ForEach(c => c.IsDeleted = true);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TrCart> Insert(TrCart entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TrCart>> List(TrCart entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TrCart>> List(int storeId, byte brand)
        {
            return await _context.TrCarts.AsNoTracking()
                .Where(c => c.StoreId == storeId && c.Brand == brand && !c.IsDeleted)
                .OrderByDescending(c => c.TrCartId)
                .ToListAsync();
        }

        public Task<TrCart> Select(TrCart entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TrCart> Update(TrCart entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TrCart?> Select(long trCartId, int storeId)
        {
            return await _context.TrCarts.FirstOrDefaultAsync(c => c.TrCartId == trCartId && c.StoreId == storeId && !c.IsDeleted);
        }

        public async Task<bool> HasInCart(int storeId, string plu)
        {
            return await _context.TrCarts.AsNoTracking()
                .Where(c => c.StoreId == storeId && c.Plu == plu && !c.IsDeleted)
                .AnyAsync();
        }


    }
}

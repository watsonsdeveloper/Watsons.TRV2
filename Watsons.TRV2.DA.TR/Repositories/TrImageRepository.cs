using Microsoft.EntityFrameworkCore;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public interface ITrImageRepository : IRepository<TrImage>
    {
        
        Task<bool> DeleteRange(List<long> imageIds);
        Task<List<TrImage>> UpdateRange(List<TrImage> entities);
        Task<IEnumerable<TrImage>> ListByTrCartIds(List<long> trCartIds);
        Task<IEnumerable<TrImage>> ListByTrCartId(long trCartId);
        Task<IEnumerable<TrImage>> ListByTrOrderId(long trOrderId);
    }

    public class TrImageRepository : ITrImageRepository
    {
        private readonly TrContext _trContext;
        public TrImageRepository(TrContext trContext)
        {
            _trContext = trContext;
        }

        public Task<bool> Delete(TrImage entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteRange(List<long> imageIds)
        {
            try
            {
                var uploadedImages = _trContext.TrImages.Where(i => imageIds.Contains(i.TrImageId));
                foreach (var image in uploadedImages)
                {
                    image.IsDeleted = true;
                }
                await _trContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TrImage>> UpdateRange(List<TrImage> entities)
        {
            try
            {
                _trContext.TrImages.UpdateRange(entities);
                await _trContext.SaveChangesAsync();
                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TrImage> Insert(TrImage entity)
        {
            try
            {
                _trContext.TrImages.Add(entity);
                await _trContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<IEnumerable<TrImage>> List(TrImage entity)
        {
            throw new NotImplementedException();
        }

        public Task<TrImage> Select(TrImage entity)
        {
            throw new NotImplementedException();
        }

        public Task<TrImage> Update(TrImage entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TrImage>> ListByTrCartIds(List<long> trCartIds)
        {
            return await _trContext.TrImages.Where(i => i.TrCartId != null && trCartIds.Contains(i.TrCartId ?? 0) && !i.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<TrImage>> ListByTrCartId(long trCartId)
        {
            return await _trContext.TrImages.Where(i => i.TrCartId == trCartId && !i.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<TrImage>> ListByTrOrderId(long trOrderId)
        {
            return await _trContext.TrImages.Where(i => i.TrOrderId == trOrderId && !i.IsDeleted).ToListAsync();
        }
    }
}

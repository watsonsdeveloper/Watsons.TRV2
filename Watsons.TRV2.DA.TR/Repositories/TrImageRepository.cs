using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public interface ITrImageRepository : IRepository<TrImage>
    {
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

        public async Task<TrImage> Insert(TrImage entity)
        {
            //_trContext.TrImages.Add(entity);
            //await _trContext.SaveChangesAsync();
            //return entity;
            throw new NotImplementedException();
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Task<Dictionary<long, TrImage>> DictionaryByTrOrderIds(List<long> trOrderIds);
        Task<List<TrImage>> ListByTrOrderIds(List<long> trOrderIds);
        Task<IEnumerable<TrImage>> ListByTrOrderId(long trOrderId);
    }

}

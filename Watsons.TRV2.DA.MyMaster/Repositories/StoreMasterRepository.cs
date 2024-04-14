using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.MyMaster.Entities;

namespace Watsons.TRV2.DA.MyMaster.Repositories
{
   
    public class StoreMasterRepository : IStoreMasterRepository
    {
        public readonly MyMasterContext _context;
        public StoreMasterRepository(MyMasterContext context)
        {
            _context = context;
        }
        public Task<bool> Delete(StoreMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<StoreMaster> Insert(StoreMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StoreMaster>> List(StoreMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<StoreMaster> Select(StoreMaster entity)
        {
            throw new NotImplementedException();
        }

        public async Task<StoreMaster?> SelectStore(int storeId)
        {
            return await _context.StoreMasters.AsNoTracking().FirstOrDefaultAsync(x => x.StoreId == storeId);
        }

        public async Task<Dictionary<int, string>?> SelectStoreName(List<int> storeIds)
        {
            if (storeIds == null || !storeIds.Any())
            {
                return new Dictionary<int, string>();
            }

            var result = await _context.StoreMasters
                .Where(x => storeIds.Contains(x.StoreId))
                .ToDictionaryAsync(x => x.StoreId, x => x.StoreName ?? "");
           
            return result;
        }

        public Task<StoreMaster> Update(StoreMaster entity)
        {
            throw new NotImplementedException();
        }
    }
}

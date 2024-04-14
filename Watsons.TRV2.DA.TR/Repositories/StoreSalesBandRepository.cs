using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models;
using Watsons.TRV2.DA.TR.Models.SalesBand;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public class StoreSalesBandRepository : IStoreSalesBandRepository
    {
        private readonly TrContext _context;

        public StoreSalesBandRepository(TrContext context)
        {
            _context = context;
        }

        public Task<bool> Delete(StoreSalesBand entity)
        {
            throw new NotImplementedException();
        }

        public async Task<StoreSalesBand> Insert(StoreSalesBand entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<StoreSalesBand>> List(StoreSalesBand entity)
        {
            throw new NotImplementedException();
        }

        public Task<StoreSalesBand> Select(StoreSalesBand entity)
        {
            throw new NotImplementedException();
        }

        public Task<StoreSalesBand> Update(StoreSalesBand entity)
        {
            throw new NotImplementedException();
        }

        public async Task<GetStoreSalesBandDetailsResult?> GetStoreSalesBandDetails(int storeId)
        {
            return await _context.StoreSalesBands
                .Include(s => s.SalesBand)
                .Where(s => s.StoreId == storeId)
                .Select(x => new GetStoreSalesBandDetailsResult
                {
                    StoreId = x.StoreId,
                    //PluCapped = x.SalesBand.PluCapped
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Dictionary<string, StoreSalesBandTypeValue>?> GetTypeValue(int storeId)
        {
            return await _context.StoreSalesBands
                .Include(s => s.SalesBand)
                .Where(s => s.StoreId == storeId)
                .ToDictionaryAsync(s => s.Type, s => new StoreSalesBandTypeValue()
                {
                    Type = s.Type,
                    Value = s.SalesBand.Value
                });
        }
    }   
}

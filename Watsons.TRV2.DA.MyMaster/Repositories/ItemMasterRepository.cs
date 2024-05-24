using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.MyMaster.Entities;
using Watsons.TRV2.DA.MyMaster.Models.ItemMaster;

namespace Watsons.TRV2.DA.MyMaster.Repositories 
{ 
    public class ItemMasterRepository : IItemMasterRepository
    {
        private readonly MyMasterContext _context;
        public ItemMasterRepository(MyMasterContext context)
        {
            _context = context;
        }

        public Task<bool> Delete(ItemMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<ItemMaster> Insert(ItemMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ItemMaster>> List(ItemMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<ItemMaster> Select(ItemMaster entity)
        {
            throw new NotImplementedException();
        }
        public Task<ItemMaster> Update(ItemMaster entity)
        {
            throw new NotImplementedException();
        }
        public async Task<ItemMaster?> SearchByPluOrBarcode(string pluOrBarcode)
        {   
            return await _context.ItemMasters.AsNoTracking().FirstOrDefaultAsync(x => x.Item == pluOrBarcode || x.Barcode == pluOrBarcode);
        }
        public async Task<ItemMaster?> SearchByPlu(string plu)
        {
            return await _context.ItemMasters.AsNoTracking().FirstOrDefaultAsync(x => x.Item == plu);
        }
        public async Task<ItemMaster?> SearchByBarcode(string barcode)
        {
            return await _context.ItemMasters.AsNoTracking().FirstOrDefaultAsync(x => x.Item == barcode);
        }
        public async Task<Dictionary<string, ItemMaster>> Dictionary(List<string> plus)
        {
            var result = await _context.ItemMasters.AsNoTracking().Where(x => x.Item != null && plus.Contains(x.Item)).ToListAsync();
            return result.ToDictionary(x => x.Item, x => x);
        }
        public async Task<IEnumerable<ItemMaster>>? Search(SearchFilter entity)
        {
            IQueryable<ItemMaster> query = _context.ItemMasters;

            if (entity.Plus != null)
            {
                query = query.Where(x => entity.Plus.Contains(x.Item));
            }

            return await query.ToListAsync();
        }
    }
}

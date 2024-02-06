using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.MyMaster.Entities;

namespace Watsons.TRV2.DA.Repositories
{
    public interface IItemMasterRepository : IRepository<ItemMaster>
    {
        Task<ItemMaster?> SearchByPluOrBarcode(string pluOrBarcode);
        Task<ItemMaster?> SearchByPlu(string plu);
        Task<ItemMaster?> SearchByBarcode(string barcode);
    }
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
    }
}

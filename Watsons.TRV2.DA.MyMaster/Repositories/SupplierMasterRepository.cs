using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.MyMaster.Entities;
using Watsons.TRV2.DA.MyMaster.Models.SupplierMaster;

namespace Watsons.TRV2.DA.MyMaster.Repositories
{
    public class SupplierMasterRepository : ISupplierMasterRepository
    {
        private readonly MyMasterContext _context;
        public SupplierMasterRepository(MyMasterContext context)
        {
            _context = context;
        }
        public Task<bool> Delete(SupplierMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<SupplierMaster> Insert(SupplierMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SupplierMaster>> List(SupplierMaster entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SupplierMaster>>? Search(SearchFilter entity)
        {
            IQueryable<SupplierMaster> query = _context.SupplierMasters;
            if (entity.SupplierCodes != null)
            {
                query = query.Where(s => entity.SupplierCodes.Contains(s.SupplierCode));
            }
            return await query.ToListAsync();
        }

        public Task<SupplierMaster> Select(SupplierMaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<SupplierMaster> Update(SupplierMaster entity)
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.TR.Entities;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public class TrCommonRepository : ITrCommonRepository
    {
        private readonly TrContext _context;
        public TrCommonRepository(TrContext context) { 
            _context = context;
        }
        public async Task<string?> SelectSysParam(string param)
        {
            var result = await _context.SysParams.Where(s => s.Param == param).FirstOrDefaultAsync();
            return result?.Value;
        }
        public async Task<SysParam?> SelectSysParamByEntity(string param)
        {
            return await _context.SysParams.Where(s => s.Param == param).FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateSysParamByParam(string param, string value)
        {
            var entity = await _context.SysParams.Where(s => s.Param == param).FirstOrDefaultAsync();
            if (entity == null)
            {
                return false;
            }
            entity.Value = value;
            await _context.SaveChangesAsync();
       
            return true;
        }

        public async Task<bool> UpdateSysParam()
        {
            try
            {
                //var entity = await _context.SysParams.Where(p => p.Param == "OrderNumber").FirstOrDefaultAsync();
                //if (entity == null)
                //{
                //    return false;
                //}
                //var isTracked = _context.Entry(entity).State == EntityState.Modified ||
                //    _context.Entry(entity).State == EntityState.Added ||
                //    _context.Entry(entity).State == EntityState.Unchanged;

                //if (!isTracked)
                //{
                //    // If entity is not being tracked, log the issue or handle it accordingly
                //    return false;
                //}
                //entity.Value = (int.Parse(entity.Value) + 1).ToString();
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}

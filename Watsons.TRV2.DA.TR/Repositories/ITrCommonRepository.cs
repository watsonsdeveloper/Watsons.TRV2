using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DA.TR.Entities;

namespace Watsons.TRV2.DA.TR.Repositories
{
    public interface ITrCommonRepository 
    {
        Task<string?> SelectSysParam(string param);
        Task<SysParam?> SelectSysParamByEntity(string param);
        Task<bool> UpdateSysParamByParam(string param, string value);
        Task<bool> UpdateSysParam();
    }
}

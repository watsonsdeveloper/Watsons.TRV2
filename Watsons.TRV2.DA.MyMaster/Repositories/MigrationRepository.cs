using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.MyMaster.Entities;

namespace Watsons.TRV2.DA.MyMaster.Repositories
{
    public class MigrationRepository: IMigrationRepository
    {
        private readonly MigrationContext _context;
        private readonly IConfiguration _configuration;
        public MigrationRepository(MigrationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        //public async Task<bool> WriteOffItem(int storeId, string plu, int quantity)
        //{
        //    // query store procedure
        //    var sqlQuery = "EXEC [dbo].[SP_RSIM_Adjustment_TR_APP] @storeId, @plu, @quantity";
        //    var storeIdParam = new SqlParameter("@storeId", storeId);
        //    var pluParam = new SqlParameter("@plu", plu);
        //    var quantityParam = new SqlParameter("@quantity", quantity);
        //    // execute store procedure
        //    await _context.Database.ExecuteSqlRawAsync(sqlQuery, storeIdParam, pluParam, quantityParam);

        //    //var sqlQuery = "SELECT * FROM YourTable WHERE Condition1 = @param1 AND Condition2 = @param2";
        //    //var param1 = new SqlParameter("@param1", param1Value);
        //    //var param2 = new SqlParameter("@param2", param2Value);

        //    //var results = _context.Database.SqlQuery<YourEntityType>(sqlQuery, param1, param2);
        //    return false;
        //}

        public async Task<bool> WriteOffOrder(long trOrderBatchId)
        {
            var sqlQuery = "EXEC [dbo].[SP_RSIM_Adjustment_TR_APP] @TrOrderBatchId, @ENV";
            var trOrderBatchIdParam = new SqlParameter("@TrOrderBatchId", trOrderBatchId);
            var environment = _configuration.GetSection("Environment").Value.ToString();
            var envParam = new SqlParameter("@ENV", environment);

            var results = await _context.Database.ExecuteSqlRawAsync(sqlQuery, trOrderBatchIdParam, envParam);
            if (results > 0)
            {
                return true;
            }

            return false;
        }
    }

    
       
}

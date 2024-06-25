using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.TRV2.DA.MyMaster.Entities;
using Watsons.TRV2.DA.MyMaster.Models;

namespace Watsons.TRV2.DA.MyMaster.Repositories
{
    public class MigrationRepository : IMigrationRepository
    {
        private readonly string? _environment;
        private readonly Serilog.ILogger _logger;
        private readonly MigrationContext _context;
        private readonly IConfiguration _configuration;
        
        public MigrationRepository(Serilog.ILogger logger,  MigrationContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;

            _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
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
            try
            {
                var sqlQuery = "EXEC [dbo].[SP_RSIM_Adjustment_TR_APP] @TrOrderBatchId, @ENV";
                var trOrderBatchIdParam = new SqlParameter("@TrOrderBatchId", trOrderBatchId);
                //var environment = _configuration.GetSection("Environment").Value.ToString();
                var envParam = new SqlParameter("@ENV", _environment);

                var results = await _context.Database.ExecuteSqlRawAsync(sqlQuery, trOrderBatchIdParam, envParam);
                if (results > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in WriteOffOrder");
                return false;
            }
        }

        public async Task<bool> InsertShipment(InsertShipmentParams parameters)
        {
            var storeId = parameters.StoreId < 1000 ? parameters.StoreId.ToString("000") : parameters.StoreId.ToString();

            if (_environment != "PROD")
            {
                storeId = "000";
            }

            string sql = @"IF NOT EXISTS(SELECT TOP 1 OrderNumber FROM [" + storeId + "].[RSIM2].[dbo].[Shipment] WITH (NOLOCK) WHERE [Number] = @ShipmentNumber) " +
                        "BEGIN INSERT INTO [" + storeId + "].[RSIM2].[dbo].[Shipment] " +
                        "([Number], [ConsolidatingNumber], [OrderNumber], [Type], [ReceivingStoreNumber], [SupplierNumber], [OrderDate], [DeliveryDate], [PpIndicator], [Status], [Remark], [ImportLogId], [CreationDate], [CreationTime], [CreateBy], [ModificationTime], [ModifiedBy], [LocationCode], [SendingStoreNumber], [SendReason], [Locked], [OneClickReceive]) " +
                        "VALUES (@ShipmentNumber, @ShipmentNumber, @ShipmentNumber, 'RL', @StoreId, @SupplierNumber, @OrderDate, @DeliveryDate, 'Push', 10, @Remark, 0, GETDATE(), GETDATE(), @CreatedBy, GETDATE(), @ModifiedBy, 'BackendStoreroom', '', '', 0, 0); END";
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@ShipmentNumber", parameters.ShipmentNumber),
                new SqlParameter("@StoreId", storeId),
                new SqlParameter("@SupplierNumber", parameters.SupplierNumber),
                new SqlParameter("@OrderDate", parameters.OrderDate.Date),
                new SqlParameter("@DeliveryDate", parameters.DeliveryDate.Date),
                new SqlParameter("@Remark", parameters.Remark),
                new SqlParameter("@ModifiedBy", parameters.CreatedBy),
                new SqlParameter("@CreatedBy", parameters.CreatedBy),
            };

            var results = await _context.Database.ExecuteSqlRawAsync(sql, sqlParams);
            if (results > 0) // SHIPMENT IS EXISTS IF `results = -1`
            {
                return true;
            }

            return false; 
        }

        public async Task<bool> InsertShipmentItem(InsertShipmentItemParams parameters)
        {
            var storeId = parameters.StoreId < 1000 ? parameters.StoreId.ToString("000") : parameters.StoreId.ToString();

            if (_environment != "PROD")
            {
                storeId = "000";
            }

            string sql = @"IF NOT EXISTS (SELECT TOP 1 ShipmentNumber FROM [" + storeId + "].[RSIM2].[dbo].[ShipmentItem] WITH (NOLOCK) WHERE [ShipmentNumber] = @ShipmentNumber AND [ItemCode] = @ItemCode)" +
                        "BEGIN  INSERT INTO [" + storeId + "].[RSIM2].[dbo].[ShipmentItem] " +
                        "([ShipmentNumber], [ItemCode], [SupplierItemCode], [Barcode], [Qty], [CreateBy], [ModifiedBy], [CreationTime], [ModificationTime], [ExpiryDate], [PackingQty], [OriginalQty], [ImportLogId], [LocationCode], [Damaged], [BatchId], [ImportedBatchInfo])" +
                        " VALUES (@ShipmentNumber, @ItemCode, @SupplierItemCode, @Barcode, @Qty, @CreateBy, @ModifiedBy, GETDATE(), GETDATE(), @ExpiryDate, 0, 0, 0, 'BackendStoreroom', 0, 0, 0); END";
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@ShipmentNumber", parameters.ShipmentNumber),
                new SqlParameter("@StoreId", storeId),
                new SqlParameter("@ItemCode", parameters.ItemCode),
                new SqlParameter("@SupplierItemCode", parameters.SupplierItemCode),
                new SqlParameter("@Barcode", parameters.Bardcode),
                new SqlParameter("@Qty", parameters.Qty),
                new SqlParameter("@ModifiedBy", parameters.CreatedBy),
                new SqlParameter("@CreateBy", parameters.CreatedBy),
                new SqlParameter("@ExpiryDate", DateTime.Now.AddDays(14)),
            };

            var results = await _context.Database.ExecuteSqlRawAsync(sql, sqlParams);
            if (results > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> InsertShipmentStatusLog(InsertShipmentStatusLogParams parameters)
        {
            var storeId = parameters.StoreId < 1000 ? parameters.StoreId.ToString("000") : parameters.StoreId.ToString();

            if (_environment != "PROD")
            {
                storeId = "000";
            }

            string sql = @"IF NOT EXISTS (SELECT TOP 1 [Number] FROM [" + storeId + "].[RSIM2].[dbo].[StatusLog] WITH (NOLOCK) WHERE [Type] = 'Shipment' AND [Number] = @ShipmentNumber AND Status = '10')" +
                                  "BEGIN  INSERT INTO [" + storeId + "].[RSIM2].[dbo].[StatusLog] " +
                                  "([Type], [Number], [Status], [CreateBy], [CreationTime])" +
                                  " VALUES (@Type, @ShipmentNumber, @Status, @CreatedBy, GETDATE()); " +
                                  "END";
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@StoreId", storeId),
                new SqlParameter("@Type", parameters.Type),
                new SqlParameter("@ShipmentNumber", parameters.ShipmentNumber),
                new SqlParameter("@Status", parameters.Status),
                new SqlParameter("@CreatedBy", parameters.CreatedBy),
            };

            var results = await _context.Database.ExecuteSqlRawAsync(sql, sqlParams);
            if (results > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<List<Entities.ShipmentItem>> SelectShipmentItems(SelectShipmentItemParams parameters)
        {
            try
            {
                var storeId = parameters.StoreId < 1000 ? parameters.StoreId.ToString("000") : parameters.StoreId.ToString();

                if (_environment != "PROD")
                {
                    storeId = "000";
                }

                string shipmentNumbers = string.Join(",", parameters.ShipmentNumbers.Select(shipmentNumber => $"'{shipmentNumber}'"));
                string query = $@"SELECT SI.ShipmentNumber, SI.ItemCode, SI.SupplierItemCode, SI.Qty, SI.OriginalQty, SI.ReceivedQty, SI.ModifiedBy, SI.ModificationTime FROM [{storeId}].[RSIM2].[dbo].[Shipment] S 
                                LEFT JOIN [{storeId}].[RSIM2].[dbo].[ShipmentItem] SI ON S.[Number] = SI.[ShipmentNumber]
                                WHERE S.Status = '20' AND SI.[ShipmentNumber] IN ({shipmentNumbers})";

                return await _context.ShipmentItems.FromSqlRaw(query).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in SelectShipmentItems");
            }
            return new List<Entities.ShipmentItem>();
        }

    }



}

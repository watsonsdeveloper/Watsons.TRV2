using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.Common.EmailHelpers;
using Watsons.Common.EmailHelpers.DTO;
using Watsons.TRV2.DA.CashManage;
using Watsons.TRV2.DA.MyMaster.Entities;
using Watsons.TRV2.DA.MyMaster.Models;
using Watsons.TRV2.DA.MyMaster.Repositories;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models.B2bOrder;
using Watsons.TRV2.DA.TR.Models.Order;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.Services.Portal.Settings;

namespace Watsons.TRV2.Services.Portal
{
    public class JobService : IJobService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ITrCommonRepository _trCommonRepository;
        private readonly ITrOrderRepository _trOrderRepository;
        private readonly ITrOrderBatchRepository _trOrderBatchRepository;
        private readonly IB2bOrderRepository _b2bOrderRepository;
        private readonly ICashManageRepository _cashManageRepository;
        private readonly IItemMasterRepository _itemMasterRepository;
        private readonly IStoreMasterRepository _storeMasterRepository;
        private readonly ISupplierMasterRepository _supplierMasterRepository;
        private readonly IMigrationRepository _migrationRepository;
        private readonly SupplierOrderSettings _supplierOrderSettings;
        private readonly EmailHelper _emailHelper;
        private readonly string _environment;
        public JobService(
            Serilog.ILogger logger,
            IConfiguration configuration,
            ITrCommonRepository trCommonRepository,
            ITrOrderRepository trOrderRepository,
            ITrOrderBatchRepository trOrderBatchRepository,
            IB2bOrderRepository b2BOrderRepository,
            ICashManageRepository cashManageRepository,
            IItemMasterRepository itemMasterRepository,
            IStoreMasterRepository storeMasterRepository,
            ISupplierMasterRepository supplierMasterRepository,
            IMigrationRepository migrationRepository,
            IOptionsSnapshot<SupplierOrderSettings> supplierOrderSettings,
            EmailHelper emailHelper)
        {
            _logger = logger;
            _configuration = configuration;
            _trCommonRepository = trCommonRepository;
            _trOrderRepository = trOrderRepository;
            _trOrderBatchRepository = trOrderBatchRepository;
            _b2bOrderRepository = b2BOrderRepository;
            _cashManageRepository = cashManageRepository;
            _itemMasterRepository = itemMasterRepository;
            _storeMasterRepository = storeMasterRepository;
            _supplierMasterRepository = supplierMasterRepository;
            _migrationRepository = migrationRepository;
            _supplierOrderSettings = supplierOrderSettings.Value;
            _emailHelper = emailHelper;
            _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        }

        public async Task<ServiceResult<bool>> EmailNotifyStoreOrderPending()
        {
            var orderPendingList = await _trOrderBatchRepository.PendingList();
            var storeIds = orderPendingList.Select(o => o.StoreId).ToList();
            if (storeIds == null || storeIds.Count() <= 0)
            {
                return ServiceResult<bool>.Success(true);
            }

            var roles = new List<string>() { "ASOM", "RSOM" };
            var userStoreList = await _cashManageRepository.UserStoreIds(storeIds, roles);
            if (userStoreList == null || userStoreList.Count() <= 0)
            {
                return ServiceResult<bool>.Fail("No user found.");
            }

            var userStoreDict = userStoreList.GroupBy(u => u.Email).ToDictionary(u => u.Key, u => u.ToList());
            if (userStoreDict == null || userStoreDict.Count() <= 0)
            {
                return ServiceResult<bool>.Success(true);
            }

            var orderItemsPending = await _trOrderRepository.OrderPending();
            orderItemsPending = orderItemsPending.Where(o => o.TrOrderBatch.Brand == (byte)Brand.Own);
            var orderItemPendingDict = orderItemsPending.GroupBy(o => o.TrOrderBatch.StoreId)
                .ToDictionary(o => o.Key, o => o.GroupBy(o => o.TrOrderBatchId).ToDictionary(o => o.Key, o => o.Count()));
            if(orderItemPendingDict == null || orderItemPendingDict.Count() <= 0)
            {
                return ServiceResult<bool>.Success(true);
            }

            foreach (var userStore in userStoreDict) // loop through each user
            {
                try
                {
                    var email = userStore.Key;
                    var userStores = userStore.Value;
                    var user = userStores.FirstOrDefault();
                    var env = _environment != "PROD" ? $"({_environment})": "";
                    var subject = $"Tester Own Label Request - Pending Approval {env}";
                    var body = System.IO.File.ReadAllText("EmailTemplates/EmailNotifyStoreOrderPending.html");
                    body = body.Replace("{{fullname}}", user?.Name);

                    var bodyContent = string.Empty;
                    foreach(var store in userStores) // loop throuh each store
                    {
                        var orderPending = orderItemPendingDict.Where(o => o.Key == store.StoreId).FirstOrDefault().Value;
                        if (orderPending == null || orderPending.Count() <= 0)
                        {
                            continue;
                        }
                        foreach (var order in orderPending) // loop through each order
                        {
                            bodyContent += $"<tr>" +
                                $"<td style=\"border: 1px solid\">{store.StoreId}</td>" +
                                $"<td style=\"border: 1px solid\">{order.Key}</td>" +
                                $"<td style=\"border: 1px solid\">{order.Value}</td>" +
                                $"</tr>";
                        }
                    }
                    body = body.Replace("{{tableContent}}", bodyContent);
                    body = body.Replace("{{link}}", _configuration.GetValue<string>("PortalUrl"));

                    var emailParams = new SendEmailBySpParams()
                    {
                        Recipients = new List<string>() { email },
                        Subject = subject,
                        Body = body
                    };

                    await _emailHelper.SendEmailBySp(emailParams);

                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error in EmailNotifyStoreOrderPending");
                }
            }

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> SubmitToB2B()
        {
            var filesToZip = new List<string>();

            var orders = await _trOrderRepository.ListSearch(new ListSearchParams()
            {
                Status = (byte)TrOrderStatus.Pending,
                Brand = (byte)Brand.Supplier,
            });
            var stores = orders.GroupBy(o => o.TrOrderBatch.StoreId);

            var orderNumberEntity = await _trCommonRepository.SelectSysParamByEntity("OrderNumber");
            Int64.TryParse(orderNumberEntity?.Value, out var orderNumber);

            try
            {
                foreach (var storeOrders in stores)
                {
                    var storeOrder = storeOrders.FirstOrDefault();
                    if (storeOrder == null)
                    {
                        // TODO: log
                        continue;
                    }

                    var storeId = storeOrder?.TrOrderBatch.StoreId;
                    if (storeId == null)
                    {
                        // TODO: log 
                        continue;
                    }
                    var storeRepo = await _storeMasterRepository.SelectStore((int)storeId);
                    if (storeRepo == null)
                    {
                        // TODO: log 
                        continue;
                    }

                    var itemMasterList = new List<ItemMaster>();
                    var pluList = storeOrders.Where(o => !string.IsNullOrWhiteSpace(o.Plu)).Select(o => o.Plu).Distinct().ToList();
                    var itemMasterSearchFilter = new DA.MyMaster.Models.ItemMaster.SearchFilter()
                    {
                        Plus = pluList
                    };
                    itemMasterList = (await _itemMasterRepository.Search(itemMasterSearchFilter)!).ToList();

                    var suppliers = new List<SupplierMaster>();
                    var supplierCodeList = storeOrders.Where(o => !string.IsNullOrWhiteSpace(o.SupplierCode)).Select(o => o.SupplierCode).Distinct().ToList();
                    if (supplierCodeList != null && supplierCodeList.Count() > 0)
                    {
                        var supplierSearchFilter = new DA.MyMaster.Models.SupplierMaster.SearchFilter()
                        {
                            SupplierCodes = supplierCodeList!
                        };
                        suppliers = (await _supplierMasterRepository.Search(supplierSearchFilter)!).ToList();
                    }

                    foreach (var supplier in suppliers)
                    {
                        orderNumber++;
                        string formattedOrderNumber = "99" + orderNumber.ToString("000000");
                        var filePath = string.Empty;
                        var fileName = string.Empty;
                        var supplierOrders = storeOrders.Where(o => o.SupplierCode == supplier.SupplierCode).ToList();

                        var isGenerated = GeneratePOFile(supplierOrders, storeRepo, supplier, itemMasterList, formattedOrderNumber, ref fileName, ref filePath);
                        if (isGenerated)
                        {
                            filesToZip.Add(filePath);
                        }

                        foreach (var orderItem in supplierOrders)
                        {
                            // TODO : orderItem AverageCost, PluCappedSnapshot
                            await _trOrderRepository.UpdateTrOrderStatus(orderItem.TrOrderId, TrOrderStatus.Processed);
                            await _b2bOrderRepository.Insert(new B2bOrder()
                            {
                                TrOrderId = orderItem.TrOrderId,
                                OrderNumber = formattedOrderNumber,
                                B2bFileName = filePath,
                                HhtInsertStatus = (byte)HhtOrderStatus.Pending,
                                CreatedAt = DateTime.Now
                            });
                        }
                    }

                    var trOrderBatchIds = storeOrders.Select(o => o.TrOrderBatchId).Distinct().ToList();
                    foreach(var trOrderBatchId in trOrderBatchIds)
                    {
                        await _trOrderBatchRepository.UpdateTrOrderBatchStatus(trOrderBatchId, TrOrderBatchStatus.Processed);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO : log
                return ServiceResult<bool>.Fail(ex.Message);
            }

            await _trCommonRepository.UpdateSysParamByParam("OrderNumber", orderNumber.ToString());

            string zipFileName = "TRPOcsv." + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
            string zipPath = Path.Combine(_supplierOrderSettings.POFilePath, zipFileName);
            CreateZipFile(zipPath, filesToZip);
            DeleteFiles(_supplierOrderSettings.POFilePath);

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> CreateStoreHhtOrder()
        {

            var orders = await _b2bOrderRepository.HhtOrderList();
            if (orders == null || orders.Count() <= 0)
            {
                return ServiceResult<bool>.Success(true);
            }

            var hhtOrderStatus = new List<byte>()
            {
                (byte)HhtOrderStatus.Error,
                (byte)HhtOrderStatus.Pending,
            };
            orders = orders.Where(o => o.B2bOrder != null
                        && (o.B2bOrder.HhtInsertStatus == (byte)HhtOrderStatus.Error || o.B2bOrder.HhtInsertStatus == (byte)HhtOrderStatus.Pending));
            var orderNumberBatch = orders.GroupBy(o => o.B2bOrder?.OrderNumber);

            foreach (var orderItems in orderNumberBatch)
            {
                if (orderItems == null)
                {
                    continue;
                }
                var firstOrderItem = orderItems?.FirstOrDefault();
                if (firstOrderItem == null)
                {
                    continue;
                }
                if (firstOrderItem?.TrOrderBatch.StoreId == null)
                {
                    continue;
                }
                if (firstOrderItem.SupplierCode == null)
                {
                    continue;
                }
                if (firstOrderItem.B2bOrder == null || firstOrderItem.B2bOrder.OrderNumber == null)
                {
                    continue;
                }

                try
                {
                    var storeId = firstOrderItem.TrOrderBatch.StoreId;
                    var supplierCode = firstOrderItem.SupplierCode;

                    var today = DateTime.Now;
                    var orderDate = today.Date;
                    var deliveryDate = orderDate.AddDays(14).Date;
                    var shipmentDto = new InsertShipmentDto(storeId, firstOrderItem.B2bOrder.OrderNumber, firstOrderItem.SupplierCode,
                        orderDate, deliveryDate, firstOrderItem.TrOrderBatchId.ToString(), firstOrderItem.TrOrderBatchId.ToString());
                    var insertShipment = await _migrationRepository.InsertShipment(shipmentDto);

                    foreach (var item in orderItems!)
                    {
                        try
                        {
                            if (item == null || item.B2bOrder == null)
                            {
                                continue;
                            }
                            if (item.B2bOrder?.OrderNumber == null)
                            {
                                continue;
                            }
                            if (item.SupplierCode == null)
                            {
                                continue;
                            }
                            if (item.Barcode == null)
                            {
                                continue;
                            }
                            var shipmentItemDto = new InsertShipmentItemDto(storeId, item.B2bOrder.OrderNumber, item.Plu, item.SupplierCode, item.Barcode, 1);
                            await _migrationRepository.InsertShipmentItem(shipmentItemDto);
                            var updateHhtOrderDto = new UpdateHhtOrderDto(item.TrOrderId, HhtOrderStatus.Shipping, null);
                            await _b2bOrderRepository.UpdateHhtOrder(updateHhtOrderDto);
                        }
                        catch (Exception ex)
                        {
                            // TODO : log
                            var updateHhtOrderDto = new UpdateHhtOrderDto(item.TrOrderId, HhtOrderStatus.Error, ex.Message);
                            await _b2bOrderRepository.UpdateHhtOrder(updateHhtOrderDto);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO : log
                }
            }

            return ServiceResult<bool>.Success(true);
        }

        protected bool GeneratePOFile(List<DA.TR.Entities.TrOrder> orderDetail, StoreMaster storeMaster, SupplierMaster supplierMaster, List<ItemMaster> itemMasterList, string orderNumber, ref string FileName, ref string FilePath)
        {
            bool Result = true;
            string CSVContent = "";
            try
            {
                StringBuilder Header = new StringBuilder();
                string Separator = "|";

                string Row1 = "FMMHDR" + Separator + "SCMWATSON" + Separator + "ORDERS" + Separator + "1" + Separator + orderDetail.Count + Separator + "1";
                string Row2 = "HREC" + Separator;
                string Row3 = "SCMWATSON" + Separator;
                string Row4 = orderNumber + Separator + (storeMaster.StoreCity.Contains("SABAH") || storeMaster.StoreCity.Contains("SARAWAK") ? "EAST" : "WEST") + Separator + "9";
                string Row5 = System.DateTime.Now.ToString("yyyyMMdd") + Separator + System.DateTime.Now.AddDays(_supplierOrderSettings.B2BNumOfExpiryDay).ToString("yyyyMMdd") + Separator + System.DateTime.Now.ToString("yyyyMMdd") + Separator + "ARB" + Separator + "APPROVED" + Separator + "N" + Separator + Separator + Separator + "Tester Request " + System.DateTime.Now.ToString("yyyyMMdd") + Separator + orderNumber + Separator + orderNumber;
                string Row6 = supplierMaster.SupplierCode + Separator + supplierMaster.SupplierName + Separator + (String.IsNullOrWhiteSpace(supplierMaster.Address1) ? "" : supplierMaster.Address1) + Separator + (String.IsNullOrWhiteSpace(supplierMaster.Address2) ? "" : supplierMaster.Address2) + Separator + (String.IsNullOrWhiteSpace(supplierMaster.Address3) ? "" : supplierMaster.Address3);
                string Row7 = "WATSON'S" + Separator + "PERSONAL CARE STORES" + Separator + storeMaster.StoreId.ToString() + Separator + (String.IsNullOrEmpty(storeMaster.StoreName) ? "" : storeMaster.StoreName) + Separator + (String.IsNullOrWhiteSpace(storeMaster.StoreAddress1) ? "" : storeMaster.StoreAddress1) + Separator + (String.IsNullOrWhiteSpace(storeMaster.StoreAddress2) ? "" : storeMaster.StoreAddress2) + Separator + (String.IsNullOrWhiteSpace(storeMaster.StoreCity) ? "" : storeMaster.StoreCity) + Separator + "899" + Separator + "WATSON'S PERSONAL CA" + Separator + "19th Floor Wisma Chuang" + Separator + "No.34, Jalan Sultan Ismail" + Separator + "50250 Kuala Lumpur" + Separator + "MYR";
                string Row8 = "START";

                Header.AppendLine(Row1);
                Header.AppendLine(Row2);
                Header.AppendLine(Row3);
                Header.AppendLine(Row4);
                //Header.AppendLine(Row5);
                Header.AppendLine(Row6);
                Header.AppendLine(Row7);
                Header.AppendLine(Row8);

                double TotalAmount = 0;
                foreach (var orderItem in orderDetail)
                {
                    ItemMaster itemMaster = itemMasterList.Where(i => i.Item == orderItem.Plu).FirstOrDefault();
                    string Row9 = "";

                    if (itemMaster != null)
                    {
                        Row9 = orderItem.Barcode + Separator + (String.IsNullOrWhiteSpace(orderItem.ProductName) ? "" : orderItem.ProductName) + Separator + (String.IsNullOrWhiteSpace(itemMaster.SupplierProductCode) ? "" : itemMaster.SupplierProductCode) + Separator + "1" + Separator + "EAC" + Separator + "1" + Separator + "EAC" + Separator + "0" + Separator + "EAC" + Separator + (itemMaster.SupplierItemCost * 10000).ToString() + Separator + orderItem.Plu + Separator + Separator + Separator + (itemMaster.SupplierItemCost * 10000).ToString() + Separator + "EAC" + Separator + Separator + Separator + Separator + Separator + "S" + Separator + "0%" + Separator + "0" + Separator + (itemMaster.SupplierItemCost * 10000).ToString();
                        Header.AppendLine(Row9);
                        TotalAmount = TotalAmount + itemMaster.SupplierItemCost ?? 0;
                    }
                    else
                    {
                        Row9 = orderItem.Barcode + Separator + (String.IsNullOrWhiteSpace(orderItem.ProductName) ? "" : orderItem.ProductName) + Separator + ("") + Separator + "1" + Separator + "EAC" + Separator + "1" + Separator + "EAC" + Separator + "0" + Separator + "EAC" + Separator + ("0") + Separator + orderItem.Plu + Separator + Separator + Separator + ("0") + Separator + "EAC" + Separator + Separator + Separator + Separator + Separator + "S" + Separator + "0%" + Separator + "0" + Separator + ("0");
                        Header.AppendLine(Row9);
                        TotalAmount = TotalAmount + 0;
                    }
                }

                string Row11 = "END";
                string Row12 = (TotalAmount * 10000).ToString();
                string Row13 = "TREC";

                Header.AppendLine(Row11);
                Header.AppendLine(Row12);
                Header.AppendLine(Row13);

                Header = Header.Replace(",", "?,");
                Header = Header.Replace("|", ",");
                CSVContent = Header.ToString();

                FileName = "TRPO_" + storeMaster.StoreId.ToString("000") + "_" + orderNumber + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                FilePath = _supplierOrderSettings.POFilePath + "\\" + FileName;
                File.WriteAllText(FilePath, CSVContent);

            }
            catch (Exception ex)
            {
                Result = false;
                // TODO : log
                //sp.InsertFileCreationLog(System.DateTime.Now, storeMaster.StoreId, supplierMaster.SupplierCode, orderNumber, ex.ToString());
            }

            return Result;
        }

        protected static void CreateZipFile(string zipPath, List<string> filesToZip)
        {
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                foreach (string file in filesToZip)
                {
                    if (File.Exists(file))
                    {
                        archive.CreateEntryFromFile(file, Path.GetFileName(file));
                    }
                }
            }
        }

        protected static void DeleteFiles(string filePath)
        {
            foreach (string file in System.IO.Directory.GetFiles(filePath, "*.csv"))
            {
                System.IO.File.Delete(file);
            }
        }
    }
}

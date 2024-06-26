﻿using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.Common.ConnectionHelpers;
using Watsons.Common.EmailHelpers;
using Watsons.Common.EmailHelpers.DTO;
using Watsons.Common.JwtHelpers;
using Watsons.TRV2.DA.CashManage;
using Watsons.TRV2.DA.MyMaster.Entities;
using Watsons.TRV2.DA.MyMaster.Repositories;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models.Order;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.DTO.Portal.OrderDto;
using Watsons.TRV2.Services.Portal.Settings;
using Watsons.TRV2.Services.RTS;

namespace Watsons.TRV2.Services.Portal
{
    public class OrderService : IOrderService
    {
        private readonly Logger<OrderService> logger;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IItemMasterRepository _itemMasterRepository;
        private readonly ITrOrderBatchRepository _trOrderBatchRepository;
        private readonly ITrOrderRepository _trOrderRepository;
        private readonly ITrImageRepository _trImageRepository;
        private readonly IStoreMasterRepository _storeMasterRepository;
        private readonly IStoreSalesBandRepository _storeSalesBandRepository;
        private readonly IMigrationRepository _migrationRepository;
        private readonly ICashManageRepository _cashManageRepository;
        private readonly IUserService _userService;
        private readonly IRtsService _rtsService;
        private readonly JwtHelper _jwtHelper;
        private readonly EmailHelper _emailHelper;
        private readonly RtsSettings _rtsSettings;
        private readonly EmailNotifyStoreOrderPendingSettings _emailNotifyStoreOrderPendingSettings;
        private readonly string? _environment;

        public OrderService(Serilog.ILogger logger,
            IMapper mapper, IConfiguration configuration,
            IOptionsSnapshot<RtsSettings> rtsSettings,
            IOptionsSnapshot<EmailNotifyStoreOrderPendingSettings> emailNotifyStoreOrderPendingSettings,
            IItemMasterRepository itemMasterRepository,
            ITrOrderBatchRepository trOrderBatchRepository, ITrOrderRepository trOrderRepository,
            ITrImageRepository trImageRepository, IStoreMasterRepository storeMasterRepository,
            IStoreSalesBandRepository storeSalesBandRepository,
            IMigrationRepository migrationRepository,
            ICashManageRepository cashManageRepository,
            IUserService userService, IRtsService rtsService,
            EmailHelper emailHelper, JwtHelper jwtHelper)
        {
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _itemMasterRepository = itemMasterRepository;
            _trOrderBatchRepository = trOrderBatchRepository;
            _trOrderRepository = trOrderRepository;
            _trImageRepository = trImageRepository;
            _storeMasterRepository = storeMasterRepository;
            _storeSalesBandRepository = storeSalesBandRepository;
            _migrationRepository = migrationRepository;
            _cashManageRepository = cashManageRepository;
            _userService = userService;
            _rtsService = rtsService;
            _emailHelper = emailHelper;
            _jwtHelper = jwtHelper;
            _rtsSettings = rtsSettings.Value;
            _emailNotifyStoreOrderPendingSettings = emailNotifyStoreOrderPendingSettings.Value;
            _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
        public async Task<ServiceResult<FetchOrderListResponse>> FetchOrderList(FetchOrderListRequest request)
        {
            List<int>? storeIds = null;
            var roleLimitedStoreAccess = _configuration.GetSection(AppSettings.ROLE_LIMITED_STORE_ACCESS).Get<List<string>>();

            var role = _jwtHelper.GetRole();
            if (roleLimitedStoreAccess != null && roleLimitedStoreAccess.Contains(role))
            {
                var jwtPayload = _jwtHelper.Payload();
                storeIds = jwtPayload.Claims.Where(c => c.Type == "StoreId").Select(c => int.Parse(c.Value)).ToList();
            }

            byte brandId = request.Brand != null ? (byte)request.Brand : (byte)Brand.Own;
            byte trOrderBatchStatus = request.TrOrderBatchStatus != null ? (byte)request.TrOrderBatchStatus : (byte)TrOrderBatchStatus.All;
            var parameters = new OrderBatchList()
            {
                BrandId = brandId,
                TrOrderBatchId = request.TrOrderBatchId,
                StoreIds = storeIds,
                TrOrderBatchStatus = trOrderBatchStatus,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            };
            var result = await _trOrderBatchRepository.List(parameters);
            var resultPagination = result.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            var orderDtos = _mapper.Map<List<OrderListDto>>(resultPagination);

            var distinctStoreIds = orderDtos.Select(x => x.StoreId).Distinct().ToList();
            var storeName = await _storeMasterRepository.SelectStoreName(distinctStoreIds);
            if (storeName != null && storeName.Count > 0)
            {
                foreach (var orderDto in orderDtos)
                {
                    orderDto.StoreName = storeName[orderDto.StoreId];
                }
            }

            var response = new FetchOrderListResponse
            {
                Orders = orderDtos,
                TotalRecord = result.Count(),
            };
            return ServiceResult<FetchOrderListResponse>.Success(response);
        }

        public async Task<ServiceResult<FetchOrderOwnResponse>> FetchOrderOwn(FetchOrderOwnRequest request)
        {
            try
            {
                List<int>? storeIds = null;
                var roleLimitedStoreAccess = _configuration.GetSection(AppSettings.ROLE_LIMITED_STORE_ACCESS).Get<List<string>>();

                var role = _jwtHelper.GetRole();
                if (roleLimitedStoreAccess != null && roleLimitedStoreAccess.Contains(role))
                {
                    var jwtPayload = _jwtHelper.Payload();
                    storeIds = jwtPayload.Claims.Where(c => c.Type == "StoreId").Select(c => int.Parse(c.Value)).ToList();
                }

                var orderSummary = await _trOrderBatchRepository.SelectSummary(request.TrOrderBatchId);
                if (orderSummary == null)
                {
                    return ServiceResult<FetchOrderOwnResponse>.Fail("Order not found");
                }

                byte? trOrderStatus = request.TrOrderStatus != null ? (byte)request.TrOrderStatus : null;
                ListSearchParams parameters = new ListSearchParams()
                {
                    StoreIds = storeIds,
                    TrOrderBatchId = request.TrOrderBatchId,
                    TrOrderStatus = trOrderStatus,
                    PluOrBarcode = request.PluOrBarcode,
                    BrandName = request.BrandName,
                };
                var trOrderItems = await _trOrderRepository.ListSearch(parameters);
                var orderDtos = _mapper.Map<List<OrderDto>>(trOrderItems);

                var orderItemIds = orderDtos.Select(x => x.OrderItemId).ToList();
                var orderItemImages = await _trImageRepository.ListByTrOrderIds(orderItemIds);

                Dictionary<string, ItemMaster>? items = null;
                Dictionary<string, DA.TR.Models.Order.LastWriteOffItem>? lastWriteOffItems = null;
                if (orderSummary.TrOrderBatchStatus == (byte)TrOrderBatchStatus.Pending)
                {
                    var plus = orderDtos.Select(x => x.Plu).ToList();
                    items = await _itemMasterRepository.Dictionary(plus);

                    // get store cost threshold
                    var storeSalesBand = await _storeSalesBandRepository.GetTypeValue(orderSummary.StoreId);
                    if (storeSalesBand == null || !storeSalesBand.ContainsKey(SalesBandConstants.COST_LIMIT_OWN))
                    {
                        return ServiceResult<FetchOrderOwnResponse>.Fail("Store Sales Band not found.");
                    }
                    orderSummary.CostThresholdSnapshot = storeSalesBand[SalesBandConstants.COST_LIMIT_OWN].Value;

                    orderSummary.AccumulatedCostApproved = await _trOrderRepository.TotalAccumulatedApprovedCost(orderSummary.TrOrderBatchId) ?? 0;

                    lastWriteOffItems = await _trOrderRepository.LastWriteOffDict(orderSummary.StoreId, plus);
                }

                decimal totalOrderItemCost = 0;
                foreach (var orderDto in orderDtos)
                {
                    // mapping uploaded images
                    orderDto.UploadedImages = orderItemImages.Where(x => x.TrOrderId == orderDto.OrderItemId).Select(x => x.ImagePath).ToList();

                    if (items != null && items[orderDto.Plu] != null)
                    {
                        // mapping avcost from live item master
                        if (items.ContainsKey(orderDto.Plu) && items[orderDto.Plu].Avcost != null)
                        {
                            decimal.TryParse(items[orderDto.Plu].Avcost.ToString(), out var averageCost);
                            orderDto.AverageCost = averageCost;
                            totalOrderItemCost += averageCost;
                        }
                    }
                    else if (orderDto.AverageCost != null)
                    {
                        // mapping avcost from snapshot
                        totalOrderItemCost += (decimal)orderDto.AverageCost;
                    }

                    if (lastWriteOffItems != null && lastWriteOffItems.Count > 0 && lastWriteOffItems.ContainsKey(orderDto.Plu))
                    {
                        orderDto.LastWriteOffAt = lastWriteOffItems[orderDto.Plu].LastWriteOffAt;
                    }
                }
                orderSummary.TotalOrderCost = totalOrderItemCost;

                var response = _mapper.Map<FetchOrderOwnResponse>(orderSummary);
                response.OrderItems = orderDtos;

                var storeName = await _storeMasterRepository.SelectStoreName(new List<int>() { response.StoreId });
                if (storeName != null && storeName.Count > 0)
                {
                    response.StoreName = storeName[response.StoreId];
                }

                return ServiceResult<FetchOrderOwnResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ServiceResult<FetchOrderOwnResponse>.Fail(ex.Message);
            }

        }

        public async Task<ServiceResult<UpdateOrderOwnResponse>> UpdateOrdersOwn(UpdateOrderOwnRequest request)
        {
            var trOrderBatch = await _trOrderBatchRepository.SelectWithOrderCost(request.TrOrderBatchId);
            if (trOrderBatch == null)
            {
                return ServiceResult<UpdateOrderOwnResponse>.Fail("Order not found");
            }

            //if(trOrderBatch.TrOrderBatchStatus != (byte)TrOrderBatchStatus.Pending)
            //{
            //    return ServiceResult<UpdateOrderOwnResponse>.Fail($"Order is {((TrOrderBatchStatus)trOrderBatch.TrOrderBatchStatus)}");
            //}

            // check user able access to this store.
            var authorizeStoreAccess = _userService.AuthorizeStoreAccess(trOrderBatch.StoreId);
            if (!authorizeStoreAccess.IsSuccess)
            {
                return ServiceResult<UpdateOrderOwnResponse>.Fail(authorizeStoreAccess.ErrorMessage);
            }

            ListSearchParams parameters = new ListSearchParams()
            {
                TrOrderBatchId = request.TrOrderBatchId,
            };
            var trOrderItems = await _trOrderRepository.ListSearch(parameters);

            //if (request.OrderItems == null || request.OrderItems.Count() != trOrderItems.Count())
            //{
            //    return ServiceResult<UpdateOrderOwnResponse>.Fail("Order items is not completed list");
            //}

            var requestOrderItemIds = request.OrderItems.Select(x => x.TrOrderId).ToList();
            var trOrderItemIds = trOrderItems.Select(x => x.TrOrderId).ToList();

            if (trOrderItemIds.Except(requestOrderItemIds).Any())
            {
                return ServiceResult<UpdateOrderOwnResponse>.Fail("Order items is not completed list");
            }

            var pluList = trOrderItems.Select(c => c.Plu).ToList();
            Dictionary<string, int>? rtsDictionary = null;
            if (trOrderBatch.Brand == (byte)Brand.Own)
            {
                try
                {
                    rtsDictionary = await _rtsService.GetMultipleProductSingleStore(new RTS.DTO.GetMultipleProductSingleStore.Request
                    {
                        StoreID = trOrderBatch.StoreId,
                        PluList = pluList
                    });
                    if (rtsDictionary == null || !rtsDictionary.Any())
                    {
                        return ServiceResult<UpdateOrderOwnResponse>.Fail("Store Sales Band Not Found.");
                    }
                }
                catch (Exception ex)
                {
                    return ServiceResult<UpdateOrderOwnResponse>.Fail($"RTS Error - {ex.Message}");
                }
            }


            var lastWriteOffItemsDict = await _trOrderRepository.LastWriteOffDict(trOrderBatch.StoreId, pluList);
            var trOrderItemsDict = trOrderItems.ToDictionary(x => x.TrOrderId, x => x);
            foreach (var orderItem in request.OrderItems)
            {
                if (!trOrderItemsDict.ContainsKey(orderItem.TrOrderId))
                {
                    orderItem.ErrorMessage = "Item not found in this order";
                    continue;
                }

                if (orderItem.TrOrderStatus == null || !Enum.IsDefined(typeof(TrOrderStatus), orderItem.TrOrderStatus) || orderItem.TrOrderStatus == TrOrderStatus.All)
                {
                    orderItem.ErrorMessage = "Status cannot be empty";
                    continue;
                }

                if (orderItem.TrOrderStatus == TrOrderStatus.Pending)
                {
                    orderItem.ErrorMessage = "Status cannot be pending";
                    continue;
                }

                if (orderItem.TrOrderStatus == TrOrderStatus.Rejected && string.IsNullOrEmpty(orderItem.Remark))
                {
                    orderItem.ErrorMessage = "Remark is required for rejected item";
                    continue;
                }

                trOrderItemsDict.TryGetValue(orderItem.TrOrderId, out var item);
                if ((item.IsRequireJustify == true || item.IsRequireJustify == null) && orderItem.TrOrderStatus == TrOrderStatus.Approved && string.IsNullOrEmpty(orderItem.Remark))
                {
                    orderItem.ErrorMessage = "Remark is required for approved item";
                    continue;
                }

                if (rtsDictionary != null)
                {
                    var availableStock = rtsDictionary[item.Plu];
                    if (availableStock <= _rtsSettings.MinStockRequired && orderItem.TrOrderStatus != TrOrderStatus.Rejected)
                    {
                        orderItem.ErrorMessage = $"SOH is less than {availableStock} units. Please reject the item.";
                        continue;
                    }
                }

                if (orderItem.TrOrderStatus != TrOrderStatus.Rejected && item.IsRequireJustify == false)
                {
                    orderItem.Remark = null;
                }

                var trOrderItem = trOrderItemsDict[orderItem.TrOrderId];
                trOrderItem.TrOrderStatus = (byte)orderItem.TrOrderStatus;
                trOrderItem.Remark = orderItem.Remark;
                if (lastWriteOffItemsDict != null && lastWriteOffItemsDict.Count > 0 && lastWriteOffItemsDict.ContainsKey(trOrderItem.Plu))
                {
                    trOrderItem.LastWriteOffAt = lastWriteOffItemsDict[trOrderItem.Plu].LastWriteOffAt;
                }

            }

            var response = new UpdateOrderOwnResponse()
            {
                OrderItems = request.OrderItems
            };

            if (request.OrderItems.Any(x => !string.IsNullOrEmpty(x.ErrorMessage)))
            {
                return ServiceResult<UpdateOrderOwnResponse>.FailureData(response, "Some order items got error");
            }

            var plus = trOrderItems.Select(x => x.Plu).ToList();
            var items = await _itemMasterRepository.Dictionary(plus);

            // prepare cost 
            decimal totalApprovedCost = 0;
            decimal totalRejectedCost = 0;
            decimal totalOrderItemCost = 0;
            decimal totalOrderItemCostTallyChecking = 0;
            foreach (var orderItem in trOrderItems)
            {
                if (decimal.TryParse(items[orderItem.Plu].Avcost.ToString(), out var averageCost))
                {
                    orderItem.AverageCost = averageCost;
                }
                else
                {
                    orderItem.AverageCost = 0;
                }

                if (orderItem.TrOrderStatus == (byte)TrOrderStatus.Approved)
                {
                    totalApprovedCost += averageCost;
                    totalOrderItemCost += averageCost;
                }
                else if (orderItem.TrOrderStatus == (byte)TrOrderStatus.Rejected)
                {
                    totalRejectedCost += averageCost;
                    totalOrderItemCost += averageCost;
                }

                totalOrderItemCostTallyChecking += averageCost;
            }

            if (totalOrderItemCostTallyChecking != totalOrderItemCost)
            {
                return ServiceResult<UpdateOrderOwnResponse>.Fail("Total order item cost is not tally");
            }

            var storeSalesBand = await _storeSalesBandRepository.GetTypeValue(trOrderBatch.StoreId);
            if (storeSalesBand == null || !storeSalesBand.ContainsKey(SalesBandConstants.COST_LIMIT_OWN))
            {
                return ServiceResult<UpdateOrderOwnResponse>.Fail("Store Sales Band not found.");
            }

            // prepare summary 
            var accumulatedCostApproved = await _trOrderRepository.TotalAccumulatedApprovedCost(trOrderBatch.TrOrderBatchId) ?? 0;
            var updatedBy = _jwtHelper.GetEmail();
            var updatedAt = DateTime.Now;

            response.TrOrderBatchId = trOrderBatch.TrOrderBatchId;
            response.TrOrderBatchStatus = TrOrderBatchStatus.Processed;
            response.CostThresholdSnapshot = storeSalesBand[SalesBandConstants.COST_LIMIT_OWN].Value;
            response.AccumulatedCostApproved = accumulatedCostApproved;
            response.TotalOrderCost = totalOrderItemCost;
            response.TotalCostApproved = totalApprovedCost;
            response.TotalCostRejected = totalRejectedCost;
            response.UpdatedAt = updatedAt;
            response.UpdatedBy = updatedBy;

            if (request.IsConfirmUpdate)
            {
                if (trOrderBatch.OrderCost == null) // EF auto upsert if not found.
                {
                    trOrderBatch.OrderCost = new OrderCost()
                    {
                        AccumulatedCostApproved = response.AccumulatedCostApproved,
                        CostThresholdSnapshot = response.CostThresholdSnapshot,
                        TotalCostApproved = response.TotalCostApproved,
                        TotalCostRejected = response.TotalCostRejected,
                        TotalOrderCost = response.TotalOrderCost
                    };
                }
                else
                {
                    trOrderBatch.OrderCost.AccumulatedCostApproved = response.AccumulatedCostApproved;
                    trOrderBatch.OrderCost.CostThresholdSnapshot = response.CostThresholdSnapshot;
                    trOrderBatch.OrderCost.TotalCostApproved = response.TotalCostApproved;
                    trOrderBatch.OrderCost.TotalCostRejected = response.TotalCostRejected;
                    trOrderBatch.OrderCost.TotalOrderCost = response.TotalOrderCost;
                }

                trOrderBatch.UpdatedAt = updatedAt;
                trOrderBatch.UpdatedBy = updatedBy;
                trOrderBatch.TrOrderBatchStatus = (byte)TrOrderBatchStatus.Processed;

                // update summary
                var isUpdatedSuccess = await _trOrderBatchRepository.UpdateWithOrderCost(trOrderBatch);
                if (isUpdatedSuccess)
                {
                    await _trOrderRepository.UpdateRange(trOrderItems.ToList());
                }

                if (trOrderBatch.OrderCost.TotalCostApproved > 0)
                {
                    await _trOrderRepository.InsertStoreAdjustment(trOrderBatch.TrOrderBatchId);
                    try
                    {
                        var writeOffOrderResult = await _migrationRepository.WriteOffOrder(trOrderBatch.TrOrderBatchId);
                        if (!writeOffOrderResult)
                        {
                            return ServiceResult<UpdateOrderOwnResponse>.Fail("Stock adjustment failed.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"UpdateOrdersOwn \n{ex} \n{JsonSerializer.Serialize(request)} \n\n\n");
                        return ServiceResult<UpdateOrderOwnResponse>.Fail("Stock adjustment failed.");
                    }
                }

                if (_environment == "PROD")
                {
                    if (trOrderBatch.Brand == (byte)Brand.Own)
                    {
                        var storeIds = new List<int>() { trOrderBatch.StoreId };
                        var roles = new List<string>() { "STORE" };
                        var userStores = await _cashManageRepository.UserStoreIds(storeIds, roles);
                        var userStore = userStores.FirstOrDefault(x => x.StoreId == trOrderBatch.StoreId);
                        await _emailHelper.SendEmailBySp(new SendEmailBySpParams()
                        {
                            Recipients = new List<string>() { userStore?.Email },
                            Subject = $"Permohonan Own Brand Tester {userStore?.Name} Diproses",
                            Body = @$"{userStore?.Name} <br/> Dimaklumkan bahawa permohonan Tester Own Brand, Order No: {trOrderBatch.TrOrderBatchId} telah diproses. <br/><br/><br/> 
                                    <p><u>Item Diluluskan</u></p>
                                    <b>Tindakan Stor:</b> <br/>
                                    1. Semak laporan adjustment dalam RSIM 2.0. <br/>
                                    2. Pastikan stor melekatkan 'Try Me Sticker' pada setiap PLU yang telah diluluskan. <br/>
                                    3. Paparkan stok sebagai tester di shelving dalam masa 48 jam setelah diluluskan. <br/>
                                    <br/><br/>
                                    <p><u>Item <b>TIDAK</b> Diluluskan</u></p>
                                    <br/>
                                    <b>Faktor penolakan permohonan : </b><br/>
                                    <p>Tidak memenuhi kriteria untuk tester – SOH stok < 3 unit atau jumlah permintaan telah melebihi had kuota yang ditetapkan.</p>
                                    <br/>
                                    <b>Tindakan Stor:</b><br/>
                                    <p>1. Pamerkan semula item yang telah diasingkan sebelum ini di Planogram.</p>
                                    <br/>
                                    <p>Sila rujuk kepada ASOM untuk sebarang pertanyaan.</p><br/><br/>
                                    <p>Thank you.</p><br/>
                                    <p>* This is an automated email, please DO NOT reply.</p>",
                            CopyRecipients = _emailNotifyStoreOrderPendingSettings.CopyRecipients,
                        });
                    }
                }
            }

            return ServiceResult<UpdateOrderOwnResponse>.Success(response);
        }
    }
}

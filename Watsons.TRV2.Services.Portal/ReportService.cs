using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.Common.JwtHelpers;
using Watsons.TRV2.DA.TR.Models.Order;
using Watsons.TRV2.DTO.Common;
using Watsons.TRV2.DTO.Portal.OrderDto;
using Watsons.TRV2.DA.MyMaster.Repositories;
using AutoMapper;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.DTO.Portal;

namespace Watsons.TRV2.Services.Portal
{
    public class ReportService : IReportService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IStoreMasterRepository _storeMasterRepository;
        private readonly ITrOrderRepository _trOrderRepository;
        private readonly JwtHelper _jwtHelper;

        public ReportService(
            IConfiguration configuration, IMapper mapper,
            IStoreMasterRepository storeMasterRepository,
            ITrOrderRepository trOrderRepository,
            JwtHelper jwtHelper)
        {
            _configuration = configuration;
            _mapper = mapper;
            _storeMasterRepository = storeMasterRepository;
            _trOrderRepository = trOrderRepository;
            _jwtHelper = jwtHelper;
        }
        public async Task<ServiceResult<ReportOwnResponse>> FetchReportOwn(ReportOwnRequest request)
        {
            List<int>? storeIds = null;
            var roleLimitedStoreAccess = _configuration.GetSection(AppSettings.ROLE_LIMITED_STORE_ACCESS).Get<List<string>>();

            var role = _jwtHelper.GetRole();
            if (roleLimitedStoreAccess != null && roleLimitedStoreAccess.Contains(role))
            {
                var jwtPayload = _jwtHelper.Payload();
                storeIds = jwtPayload.Claims.Where(c => c.Type == "StoreId").Select(c => int.Parse(c.Value)).ToList();
            }

            if(request.StoreIds != null && request.StoreIds.Count > 0)
            {
                // storeIds == null allow all stores access.
                storeIds = storeIds != null ? storeIds.Intersect(request.StoreIds).ToList() : request.StoreIds;
            }
            
            byte? trOrderStatus = request.TrOrderStatus != null ? (byte)request.TrOrderStatus : null;
            ListSearchParams parameters = new ListSearchParams()
            {
                PluOrBarcode = request.PluOrBarcode,
                StoreIds = storeIds,
                Brand = (byte)Brand.Own,
                Status = trOrderStatus,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };
            var result = await _trOrderRepository.ListSearch(parameters);
            var resultPagination = result.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            var reportOwnDtos = _mapper.Map<List<ReportOwnDto>>(resultPagination);

            var distinctStoreIds = reportOwnDtos.Select(x => x.StoreId).Distinct().ToList();
            var storeName = await _storeMasterRepository.SelectStoreName(distinctStoreIds);
            if (storeName != null && storeName.Count > 0)
            {
                foreach (var reportOwnDto in reportOwnDtos)
                {
                    reportOwnDto.StoreName = storeName[reportOwnDto.StoreId];
                }
            }

            var response = new ReportOwnResponse()
            {
                Records = reportOwnDtos,
                TotalRecord = result.Count(),
            };
            return ServiceResult<ReportOwnResponse>.Success(response);
        }

        public async Task<ServiceResult<ReportSupplierResponse>> FetchReportSupplier(ReportSupplierRequest request)
        {
            List<int>? storeIds = null;
            var roleLimitedStoreAccess = _configuration.GetSection(AppSettings.ROLE_LIMITED_STORE_ACCESS).Get<List<string>>();

            var role = _jwtHelper.GetRole();
            if (roleLimitedStoreAccess != null && roleLimitedStoreAccess.Contains(role))
            {
                var jwtPayload = _jwtHelper.Payload();
                storeIds = jwtPayload.Claims.Where(c => c.Type == "StoreId").Select(c => int.Parse(c.Value)).ToList();
            }

            if (request.StoreIds != null && request.StoreIds.Count > 0)
            {
                // storeIds == null allow all stores access.
                storeIds = storeIds != null ? storeIds.Intersect(request.StoreIds).ToList() : request.StoreIds;
            }

            byte? trOrderStatus = request.TrOrderStatus != null ? (byte)request.TrOrderStatus : null;
            ListSearchParams parameters = new ListSearchParams()
            {
                PluOrBarcode = request.PluOrBarcode,
                StoreIds = storeIds,
                Brand = (byte)Brand.Supplier,
                Status = trOrderStatus,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };
            var result = await _trOrderRepository.ListSearch(parameters);
            var resultPagination = result.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            var reportOwnDtos = _mapper.Map<List<ReportSupplierDto>>(resultPagination);

            var distinctStoreIds = reportOwnDtos.Select(x => x.StoreId).Distinct().ToList();
            var storeName = await _storeMasterRepository.SelectStoreName(distinctStoreIds);
            if (storeName != null && storeName.Count > 0)
            {
                foreach (var reportOwnDto in reportOwnDtos)
                {
                    reportOwnDto.StoreName = storeName[reportOwnDto.StoreId];
                }
            }

            var response = new ReportSupplierResponse()
            {
                Records = reportOwnDtos,
                TotalRecord = result.Count(),
            };
            return ServiceResult<ReportSupplierResponse>.Success(response);
        }

        public async Task<ServiceResult<ReportSupplierFulfillmentResponse>> FetchReportSupplierFulfillment(ReportSupplierFulfillmentRequest request)
        {
            List<int>? storeIds = null;
            var roleLimitedStoreAccess = _configuration.GetSection(AppSettings.ROLE_LIMITED_STORE_ACCESS).Get<List<string>>();

            var role = _jwtHelper.GetRole();
            if (roleLimitedStoreAccess != null && roleLimitedStoreAccess.Contains(role))
            {
                var jwtPayload = _jwtHelper.Payload();
                storeIds = jwtPayload.Claims.Where(c => c.Type == "StoreId").Select(c => int.Parse(c.Value)).ToList();
            }

            var result = await _trOrderRepository.ReportSupplierFulFillment(storeIds, request.StartDate, request.EndDate, request.SupplierName);
            var resultPagination = result.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            var dtos = _mapper.Map<List<ReportSupplierFulfillmentDto>>(resultPagination);

            var response = new ReportSupplierFulfillmentResponse()
            {
                Records = dtos,
                TotalRecord = result.Count(),
            };
            return ServiceResult<ReportSupplierFulfillmentResponse>.Success(response);
        }
    }
}

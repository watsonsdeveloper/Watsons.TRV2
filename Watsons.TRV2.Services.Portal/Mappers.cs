using AutoMapper;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models;
using Watsons.TRV2.DA.TR.Models.Order;
using Watsons.TRV2.DTO.Portal;
using Watsons.TRV2.DTO.Portal.OrderDto;

namespace Watsons.TRV2.Services.Portal
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<RoleModuleAccessResult, ModuleAccess>().ReverseMap();
            CreateMap<UserDto, FetchUserProfileResponse>().ReverseMap();
            CreateMap<CredEncryptor.DTO.GetMfaUserDto.Response, UserDto>().ReverseMap(); 
            CreateMap<CredEncryptor.DTO.MfaLoginDto.Response, DTO.Portal.User.MfaLoginDto.Response>().ReverseMap();
            CreateMap<TrOrderBatch, OrderListDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.TrOrderBatchId))
                .ReverseMap();
            CreateMap<OrderSummary, FetchOrderOwnResponse>().ReverseMap();
            CreateMap<TrOrder, OrderDto>()
                .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.TrOrderId))
                .ReverseMap();
            CreateMap<TrOrder, ReportOwnDto>()
                .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.TrOrderBatch.StoreId))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.TrOrderBatch.UpdatedAt))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.TrOrderBatch.UpdatedBy));
            CreateMap<TrOrder, ReportSupplierDto>()
                .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.TrOrderBatch.StoreId))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.TrOrderBatch.UpdatedAt))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.TrOrderBatch.UpdatedBy));
            CreateMap<ReportSupplierFulFillmentResult, ReportSupplierFulfillmentDto>();
        }
    }
}

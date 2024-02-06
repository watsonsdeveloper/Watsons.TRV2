using AutoMapper;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DTO.Mobile;
using Watsons.TRV2.DTO.Mobile.TrOrder;

namespace Watsons.TRV2.Mobile
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<TrPlu, TrPluDto>().ReverseMap();
            CreateMap<TrCart, TrCartDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => Enum.IsDefined(typeof(DTO.Common.Brand), src.BrandId) ? (DTO.Common.Brand)src.BrandId : default(DTO.Common.Brand)))
                .ReverseMap()
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => (byte)src.Brand));
            
            CreateMap<TrOrder, TrOrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.IsDefined(typeof(DTO.Common.TrOrderStatus), src.Status) ? (DTO.Common.TrOrderStatus)src.Status : default(DTO.Common.TrOrderStatus)))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (byte)src.Status));

            CreateMap<TrOrderBatch, TrOrderBatchDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => Enum.IsDefined(typeof(DTO.Common.Brand), src.BrandId) ? (DTO.Common.Brand)src.BrandId : default(DTO.Common.Brand)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.IsDefined(typeof(DTO.Common.TrOrderBatchStatus), src.Status) ? (DTO.Common.TrOrderBatchStatus)src.Status : default(DTO.Common.TrOrderBatchStatus)))
                .ReverseMap()
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => (byte)src.Brand))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (byte)src.Status));
        }
    }
}

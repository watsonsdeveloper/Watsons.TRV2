using AutoMapper;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Models;
using Watsons.TRV2.DTO.Mobile;
using Watsons.TRV2.DTO.Mobile.TrOrder;
using Watsons.TRV2.DTO.Mobile.UploadedImage;
using Enum = System.Enum;

namespace Watsons.TRV2.Mobile
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<TrCart, TrCartDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => Enum.IsDefined(typeof(DTO.Common.Brand), src.Brand) ? (DTO.Common.Brand)src.Brand : default(DTO.Common.Brand)))
                .ReverseMap()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => (byte)src.Brand));

            CreateMap<TrOrder, TrOrderDto>()
                .ForMember(dest => dest.TrOrderStatus, opt => opt.MapFrom(src => Enum.IsDefined(typeof(DTO.Common.TrOrderStatus), src.TrOrderStatus) ? (DTO.Common.TrOrderStatus)src.TrOrderStatus : default(DTO.Common.TrOrderStatus)))
                .ReverseMap()
                .ForMember(dest => dest.TrOrderStatus, opt => opt.MapFrom(src => (byte)src.TrOrderStatus));

            CreateMap<TrOrderBatch, TrOrderBatchDto>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => Enum.IsDefined(typeof(DTO.Common.Brand), src.Brand) ? (DTO.Common.Brand)src.Brand : default(DTO.Common.Brand)))
                .ForMember(dest => dest.TrOrderBatchStatus, opt => opt.MapFrom(src => Enum.IsDefined(typeof(DTO.Common.TrOrderBatchStatus), src.TrOrderBatchStatus) ? (DTO.Common.TrOrderBatchStatus)src.TrOrderBatchStatus : default(DTO.Common.TrOrderBatchStatus)))
                .ReverseMap()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => (byte)src.Brand))
                .ForMember(dest => dest.TrOrderBatchStatus, opt => opt.MapFrom(src => (byte)src.TrOrderBatchStatus));

            CreateMap<GetStoreSalesBandDetailsResult, StoreSalesBandDto>().ReverseMap();

            CreateMap<TrImage, UploadedImageDto>();
        }
    }
}

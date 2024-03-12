using AutoMapper;
using Enum = System.Enum;

namespace Watsons.TRV2.Services.Portal
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<CredEncryptor.DTO.MfaLoginDto.Response, DTO.Portal.User.MfaLoginDto.Response>().ReverseMap();
        }
    }
}

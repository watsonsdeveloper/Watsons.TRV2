using Watsons.TRV2.Services.CredEncryptor.DTO;

namespace Watsons.TRV2.Services.CredEncryptor
{
    public interface IMfaService
    {
        Task<DTO.MfaLoginDto.Response> MfaLogin(DTO.MfaLoginDto.Request request);
    }
}

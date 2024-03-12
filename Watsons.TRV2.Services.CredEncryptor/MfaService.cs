using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.Common.HttpServices;

namespace Watsons.TRV2.Services.CredEncryptor
{
    public class MfaService : IMfaService
    {
        private readonly CredEncryptorSettings _credEncryptorSettings;
        private readonly IHttpService _httpService;
        public MfaService(IOptions<CredEncryptorSettings> credEncryptorSettings, IHttpService httpService)
        {
            _credEncryptorSettings = credEncryptorSettings.Value;
            _httpService = httpService;
        }
        public async Task<DTO.MfaLoginDto.Response> MfaLogin(DTO.MfaLoginDto.Request request)
        {
            var errorMessage = "Failed to login.";
            try
            {
                var url = _credEncryptorSettings.Url + CredEncryptorApi.MfaLogin;
                var response = await _httpService.PostAsnyc<DTO.MfaLoginDto.Request, ServiceResult<DTO.MfaLoginDto.Response>>(url, request);

                if (response == null || !response.IsSuccess)
                {
                    errorMessage = response?.ErrorMessage ?? errorMessage;
                    throw new Exception(errorMessage);
                }

                return response.Data;
            }
            catch (Exception ex)
            {
                throw new Exception(errorMessage, ex);
            }
        }
    }
}

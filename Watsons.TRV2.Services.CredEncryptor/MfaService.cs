using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.Common.ConnectionHelpers;
using Watsons.Common.HttpServices;

namespace Watsons.TRV2.Services.CredEncryptor
{
    public class MfaService : IMfaService
    {
        private readonly ConnectionSettings _sysCredSettings;
        private readonly IHttpService _httpService;
        public MfaService(IOptionsSnapshot<ConnectionSettings> sysCredSettings, IHttpService httpService)
        {
            _sysCredSettings = sysCredSettings.Get("SysCredConnectionSettings");
            _httpService = httpService;
        }
        public async Task<DTO.MfaLoginDto.Response> MfaLogin(DTO.MfaLoginDto.Request request)
        {
            var errorMessage = "Failed to login.";
            try
            {
                var url = _sysCredSettings.Url + CredEncryptorApi.MfaLogin;
                var response = await _httpService.PostAsnyc<DTO.MfaLoginDto.Request, ServiceResult<DTO.MfaLoginDto.Response>>(url, request);

                if (response == null || !response.IsSuccess)
                {
                    errorMessage = response?.ErrorMessage ?? errorMessage;
                    if(errorMessage.Contains("Invalid Password"))
                    {
                        errorMessage = "Incorrect user ID or password.";
                    }
                    throw new Exception(errorMessage);
                }

                return response.Data;
            }
            catch (Exception ex)
            {
                throw new Exception(errorMessage, ex);
            }
        }

        public async Task<DTO.GetMfaUserDto.Response> GetMfaUser(DTO.GetMfaUserDto.Request request)
        {
            var errorMessage = "User not found.";
            try
            {
                var url = _sysCredSettings.Url + CredEncryptorApi.GetMfaUser;
                var response = await _httpService.PostAsnyc<DTO.GetMfaUserDto.Request, ServiceResult<DTO.GetMfaUserDto.Response>>(url, request);

                if (response == null || !response.IsSuccess)
                {
                    errorMessage = response?.ErrorMessage ?? errorMessage;
                    throw new Exception(errorMessage);
                }

                return response.Data;
            }
            catch(Exception ex)
            {
                throw new Exception(errorMessage, ex);
            }
        }
    }
}

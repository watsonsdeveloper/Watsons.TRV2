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
using Watsons.TRV2.Services.CredEncryptor.DTO.OtpDto;

namespace Watsons.TRV2.Services.CredEncryptor
{
    public class OtpService : IOtpService
    {
        private readonly CredEncryptorSettings _credEncryptorSettings;
        private readonly IHttpService _httpService;
        public OtpService(IOptions<CredEncryptorSettings> credEncryptorSettings, IHttpService httpService)
        {
            _credEncryptorSettings = credEncryptorSettings.Value;
            _httpService = httpService;
        }

        public async Task<bool> SendOtpByEmail(SendOtpByEmailRequest request)
        {
            string errorMessage = "Failed to send OTP.";

            try
            {
                var url = _credEncryptorSettings.Url + CredEncryptorApi.SendOtpByEmail;
                var response = await _httpService.PostAsnyc<SendOtpByEmailRequest, ServiceResult<bool>>(url, request);

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

        public async Task<bool> VerifyOtp(VerifyOtpRequest request)
        {
            string errorMessage = "Failed to verify OTP.";

            try
            {
                var url = _credEncryptorSettings.Url + CredEncryptorApi.VerifyOtp;
                var response = await _httpService.PostAsnyc<VerifyOtpRequest, ServiceResult<bool>>(url, request);

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

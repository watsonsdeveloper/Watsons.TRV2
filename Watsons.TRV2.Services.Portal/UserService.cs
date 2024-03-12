using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DTO.Portal.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Watsons.Common.JwtHelpers;
using Watsons.TRV2.Services.CredEncryptor;
using Microsoft.Extensions.Options;
using AutoMapper;
using Watsons.TRV2.Services.CredEncryptor.DTO.OtpDto;

namespace Watsons.TRV2.Services.Portal
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMfaService _mfaService;
        private readonly IOtpService _otpService;
        private readonly JwtHelper _jwtHelper;
        private readonly JwtSettings _jwtSettings;
        public UserService(
            IMapper mapper,
            //IHttpContextAccessor httpContextAccessor,
            IMfaService mfaService, IOtpService otpService,
            IOptions<JwtSettings> jwtSettings, JwtHelper jwtHelper
            )
        {
            _mapper = mapper;
            //_httpContextAccessor = httpContextAccessor;
            _mfaService = mfaService;
            _otpService = otpService;
            _jwtSettings = jwtSettings.Value;
            _jwtHelper = jwtHelper;
        }
        public async Task<ServiceResult<DTO.Portal.User.MfaLoginDto.Response>> MfaLogin(DTO.Portal.User.MfaLoginDto.Request request)
        {
            CredEncryptor.DTO.MfaLoginDto.Response? mfaLoginResponse;
            try
            {
                mfaLoginResponse = await _mfaService.MfaLogin(new CredEncryptor.DTO.MfaLoginDto.Request()
                {
                    ApplicationId = Guid.Parse(_jwtSettings.MfaApplicationId),
                    Username = request.Username,
                    Password = request.Password
                });

                var isOtpSend = await _otpService.SendOtpByEmail(new SendOtpByEmailRequest()
                {
                    Email = request.Username ?? string.Empty,
                    UserId = mfaLoginResponse.UserId ?? Guid.Empty,
                    ApplicationId = mfaLoginResponse.ApplicationId ?? Guid.Empty
                });

                if (!isOtpSend)
                {
                    return ServiceResult<DTO.Portal.User.MfaLoginDto.Response>.Failure("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<DTO.Portal.User.MfaLoginDto.Response>.Failure(ex.Message);
            }

            var response = _mapper.Map<DTO.Portal.User.MfaLoginDto.Response>(mfaLoginResponse);
            return ServiceResult<DTO.Portal.User.MfaLoginDto.Response>.Success(response);
        }

        public async Task<ServiceResult<string>> VerifyMfaLoginOtp(VerifyLoginOtpRequest request)
        {
            try
            {
                var isOtpValid = await _otpService.VerifyOtp(new VerifyOtpRequest()
                {
                    ApplicationId = request.ApplicationId ?? Guid.Empty,
                    UserId = request.UserId ?? Guid.Empty,
                    Otp = request.Otp ?? string.Empty
                });
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure(ex.Message);
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, request.UserId.ToString()),
                new Claim(ClaimTypes.Role, request.UserId.ToString())
            };
            //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
            var principal = new ClaimsPrincipal(identity);

            var token = _jwtHelper.GenerateJwtToken(claims);

            //await _httpContextAccessor.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            //{
            //    IsPersistent = false
            //});

            return ServiceResult<string>.Success(token);
        }
    }
}

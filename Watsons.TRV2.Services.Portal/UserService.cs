using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.TRV2.DTO.Portal.User;
using System.Security.Claims;
using Watsons.Common.JwtHelpers;
using Watsons.TRV2.Services.CredEncryptor;
using Microsoft.Extensions.Options;
using AutoMapper;
using Watsons.TRV2.Services.CredEncryptor.DTO.OtpDto;
using Microsoft.AspNetCore.Http;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.DA.CashManage;
using Watsons.TRV2.DTO.Portal;
using Watsons.TRV2.DA.CashManage.Entities;
using Microsoft.Extensions.Configuration;
using Watsons.Common.EmailHelpers;
using Watsons.Common.EmailHelpers.DTO;
using Watsons.TRV2.Services.Portal.Settings;

namespace Watsons.TRV2.Services.Portal
{
    public class UserService : IUserService
    {
        private readonly EmailHelper _emailHelper;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IMfaService _mfaService;
        private readonly IOtpService _otpService;
        private readonly JwtHelper _jwtHelper;
        private readonly JwtSettings _jwtSettings;
        private readonly IRoleRepository _roleRepository;
        private readonly ICashManageRepository _cashManageRepository;
        public UserService(
            EmailHelper emailHelper,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IMfaService mfaService, IOtpService otpService,
            IOptions<JwtSettings> jwtSettings, JwtHelper jwtHelper,
            IRoleRepository roleRepository, ICashManageRepository cashManageRepository
            )
        {
            _emailHelper = emailHelper;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _mfaService = mfaService;
            _otpService = otpService;
            _jwtSettings = jwtSettings.Value;
            _jwtHelper = jwtHelper;
            _roleRepository = roleRepository;
            _cashManageRepository = cashManageRepository;
        }

        public async Task<bool> SendEmail()
        {
            try
            {
                var sendEmailBySpParams = new SendEmailBySpParams()
                {
                    Recipients = new List<string>() { "jhiwei.wong@watsons.com.my" },
                    Subject = "Test email subject",
                    Body = "Test email body",
                };
                var isSent = await _emailHelper.SendEmailBySp(sendEmailBySpParams);
                return isSent;
            }
            catch (Exception ex)
            {
                return false;
            }
          
        }

        internal async Task<UserDto> GetUserProfile(Guid applicationId, Guid userId)
        {
            var response = new UserDto();
            var user = await _mfaService.GetMfaUser(new CredEncryptor.DTO.GetMfaUserDto.Request()
            {
                UserId = userId,
                ApplicationId = applicationId,
            });

            var modules = await _roleRepository.GetRoleModuleAccesses(user.DepartmentId);

            // get store access
            var storeIds = new List<int>();
            var rolesLimitedAccess = _configuration.GetSection(AppSettings.ROLE_LIMITED_STORE_ACCESS).Get<List<string>>();
            if (rolesLimitedAccess != null && rolesLimitedAccess.Contains(user.DepartmentName))
            {
                storeIds = await _cashManageRepository.GetUserStoreIds(user.Email);
            }

            response = _mapper.Map<UserDto>(user);
            response.StoreAccess = storeIds;
            response.ModuleAccesses = _mapper.Map<List<ModuleAccess>>(modules);

            return response;
        }

        public async Task<string?> DecodeJwtToken()
        {
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
            var jwtToken = _jwtHelper.DecodeJwtToken(token);
            return jwtToken;
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
                    return ServiceResult<DTO.Portal.User.MfaLoginDto.Response>.Fail("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<DTO.Portal.User.MfaLoginDto.Response>.Fail(ex.Message);
            }

            var response = _mapper.Map<DTO.Portal.User.MfaLoginDto.Response>(mfaLoginResponse);
            return ServiceResult<DTO.Portal.User.MfaLoginDto.Response>.Success(response);
        }

        public async Task<ServiceResult<bool>> MfaLogout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1), // Set to a past date to expire the cookie
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(_jwtSettings.CookieName, "", cookieOptions);
            return ServiceResult<bool>.Success(true);
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
                return ServiceResult<string>.Fail(ex.Message);
            }

            var user = await GetUserProfile(request.ApplicationId ?? Guid.Empty, request.UserId ?? Guid.Empty);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, request.UserId.ToString()!),
                new Claim(ClaimTypes.Role, user.DepartmentName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var module in user.ModuleAccesses)
            {
                claims.Add(new Claim(module.ModuleName, module.Action));
            }

            foreach (var storeId in user.StoreAccess)
            {
                claims.Add(new Claim("StoreId", storeId.ToString()));
            }

            ////Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
            //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ////Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
            //var principal = new ClaimsPrincipal(identity);

            var token = _jwtHelper.GenerateJwtToken(claims);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                //Expires = DateTime.UtcNow.AddHours(1),
                Expires = DateTime.Now.AddMinutes(60)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(_jwtSettings.CookieName, token, cookieOptions);

            //await _httpContextAccessor.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            //{
            //    IsPersistent = false
            //});

            return ServiceResult<string>.Success(token);
        }

        public async Task<ServiceResult<FetchUserProfileResponse>> FetchUserProfile(FetchUserProfileRequest request)
        {
            try
            {
                var user = await GetUserProfile(request?.ApplicationId ?? Guid.Empty, request?.UserId ?? Guid.Empty);

                var response = _mapper.Map<FetchUserProfileResponse>(user);
                return ServiceResult<FetchUserProfileResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ServiceResult<FetchUserProfileResponse>.Fail(ex.Message);
            }
        }

        public ServiceResult<bool> AuthorizeStoreAccess(int storeId)
        {
            var roleLimitedStoreAccess = _configuration.GetSection(AppSettings.ROLE_LIMITED_STORE_ACCESS).Get<List<string>>();
            var role = _jwtHelper.GetRole();
            if (roleLimitedStoreAccess != null && roleLimitedStoreAccess.Contains(role))
            {
                var jwtPayload = _jwtHelper.Payload();
                var storeIds = jwtPayload.Claims.Where(c => c.Type == "StoreId").Select(c => int.Parse(c.Value)).ToList();
                if (!storeIds.Contains(storeId))
                {
                    return ServiceResult<bool>.Fail("User not allowed to access this store");
                }
            }

            return ServiceResult<bool>.Success(true);
        }


        //[HttpPost]
        //[Route("logout")]
        //public IActionResult Logout()
        //{
        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        Expires = DateTime.UtcNow.AddDays(-1), // Set to a past date to expire the cookie
        //    };

        //    Response.Cookies.Append("AuthCookie", "", cookieOptions);

        //    return Ok(new { message = "Logout successful" });
        //}


    }
}

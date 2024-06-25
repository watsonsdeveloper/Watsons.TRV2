using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.Common.EmailHelpers;
using Watsons.TRV2.DTO.Mobile.UserDto;

namespace Watsons.TRV2.Services.Mobile
{
    public class UserService : IUserService
    {
        private readonly Serilog.ILogger _logger;

        public UserService(Serilog.ILogger logger) {
            _logger = logger;
        }
        public async Task<ServiceResult<LoginResponse>> Login(LoginRequest request)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.File("Logs/login.log", rollingInterval: RollingInterval.Day,
                         retainedFileCountLimit: 10,
                         rollOnFileSizeLimit: false)
                    .CreateLogger();

                Log.Information(JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                }));
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"{ex.Message}");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            return new ServiceResult<LoginResponse>()
            {
                IsSuccess = true,
            };
        }
    }
}

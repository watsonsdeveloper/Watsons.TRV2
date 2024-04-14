using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Watsons.Common;
using Watsons.Common.ConnectionHelpers;
using Watsons.Common.CorsHelpers;
using Watsons.Common.EmailHelpers;
using Watsons.Common.HttpServices;
using Watsons.Common.ImageHelpers;
using Watsons.Common.JwtHelpers;
using Watsons.TRV2.API.Portal;
using Watsons.TRV2.DA.CashManage;
using Watsons.TRV2.DA.CashManage.Entities;
using Watsons.TRV2.DA.MyMaster.Entities;
using Watsons.TRV2.DA.MyMaster.Repositories;
using Watsons.TRV2.DA.SysCred.Entities;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.Services.CredEncryptor;
using Watsons.TRV2.Services.Portal;
using Watsons.TRV2.Services.RTS;

var builder = WebApplication.CreateBuilder(args);

var trv2ConnectionSettings = new ConnectionSettings();
builder.Configuration.GetSection("Trv2ConnectionSettings").Bind(trv2ConnectionSettings);
var cashManageConnectionSettings = new ConnectionSettings();
builder.Configuration.GetSection("CashManageConnectionSettings").Bind(cashManageConnectionSettings);
var myMasterConnectionSettings = new ConnectionSettings();
builder.Configuration.GetSection("MyMasterConnectionSettings").Bind(myMasterConnectionSettings);
var sysCredConnectionSettings = new ConnectionSettings();
builder.Configuration.GetSection("SysCredConnectionSettings").Bind(sysCredConnectionSettings);

var tRV2Connection = SysCredential.GetConnectionString(trv2ConnectionSettings.Server, trv2ConnectionSettings.Database);
var cashManageConnection = SysCredential.GetConnectionString(cashManageConnectionSettings.Server, cashManageConnectionSettings.Database);
var myMasterConnection = SysCredential.GetConnectionString(myMasterConnectionSettings.Server, myMasterConnectionSettings.Database);
var sysCredConnection = SysCredential.GetConnectionString(sysCredConnectionSettings.Server, sysCredConnectionSettings.Database);

builder.Services.AddDbContextFactory<TrContext>(options =>
{
    options.UseSqlServer(tRV2Connection,
         sqlOptions => sqlOptions.EnableRetryOnFailure())
    .EnableServiceProviderCaching(true);
});

builder.Services.AddDbContextFactory<CashManageContext>(options =>
{
    options.UseSqlServer(cashManageConnection,
        sqlOptions => sqlOptions.EnableRetryOnFailure())
    .EnableServiceProviderCaching(true);
});

builder.Services.AddDbContextFactory<SysCredContext>(options =>
{
    options.UseSqlServer(cashManageConnection,
         sqlOptions => sqlOptions.EnableRetryOnFailure())
    .EnableServiceProviderCaching(true);
});

builder.Services.AddDbContextFactory<MyMasterContext>(options =>
{
    options.UseSqlServer(myMasterConnection,
         sqlOptions => {
             sqlOptions.UseCompatibilityLevel(120); // https://github.com/dotnet/efcore/issues/31362 # to fix EF contains error OPENJSON $ With 
             sqlOptions.EnableRetryOnFailure();
         })
    .EnableServiceProviderCaching(true);
});

builder.Services.AddOptions();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<ImageSettings>(builder.Configuration.GetSection("ImageSettings"));
builder.Services.Configure<RtsSettings>(builder.Configuration.GetSection("RtsSettings"));
builder.Services.Configure<ConnectionSettings>("SysCredConnectionSettings", builder.Configuration.GetSection("SysCredConnectionSettings"));


var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey(jwtSettings.CookieName))
            {
                context.Token = context.Request.Cookies[jwtSettings.CookieName];
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization(options =>
{
    Type type = typeof(PolicyRoles);
    FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
    foreach (var field in fields)
    {
        if (field.IsLiteral && !field.IsInitOnly)
        {
            options.AddPolicy(field.Name, policy => policy.RequireRole(field.Name));
        }
    }

    type = typeof(PolicyClaims);
    fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
    foreach (var field in fields)
    {
        if (field.IsLiteral && !field.IsInitOnly)
        {
            int underscoreIndex = field.Name.LastIndexOf('_');
            string key = field.Name.Substring(0, underscoreIndex);
            string value = field.Name.Substring(underscoreIndex + 1);
            options.AddPolicy(field.Name, policy => policy.RequireClaim(key, value));
        }
    }
});

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IHttpService, HttpService>();

builder.Services.AddScoped<EmailHelper>();
builder.Services.AddScoped<IImageHelper, ImageHelper>();
builder.Services.AddScoped<JwtHelper>();

//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(Mappers).Assembly);

//builder.Services.AddScoped<ProductService>();
//builder.Services.AddScoped<TrCartService>();
//builder.Services.AddScoped<TrOrderService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IMfaService, MfaService>();
builder.Services.AddScoped<IRtsService, RtsService>();

var corsSettings = new CorsSettings();
builder.Configuration.GetSection("CorsSettings").Bind(corsSettings);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
               builder =>
               {
                   builder.WithOrigins(corsSettings.AllowedOrigins)
                           //.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
               });
});

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ITrImageRepository, TrImageRepository>();
//builder.Services.AddScoped<ITrCartRepository, TrCartRepository>();
builder.Services.AddScoped<ITrOrderRepository, TrOrderRepository>();
builder.Services.AddScoped<IStoreSalesBandRepository, StoreSalesBandRepository>();
builder.Services.AddScoped<ITrOrderBatchRepository, TrOrderBatchRepository>();
builder.Services.AddScoped<IItemMasterRepository, ItemMasterRepository>();
builder.Services.AddScoped<IStoreMasterRepository, StoreMasterRepository>();
builder.Services.AddScoped<ICashManageRepository, CashManageRepository>();
//builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
//{
//    options.SuppressModelStateInvalidFilter = false;
//});

//builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opts =>
//    opts.SerializerOptions.IncludeFields = true);

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

var staticFilePath = app.Configuration.GetValue<string>("StaticFilePath");
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(staticFilePath)),
    //RequestPath = new PathString("/secureFile"),
});

app.UseAuthentication(); // Ensure this is called before UseAuthorization
app.UseAuthorization();
//app.UseMiddleware<ValidationMiddleware>();
//app.MapControllers();
Routers.ConfigureEndpoints(app);


//app.UseMiddleware<ExceptionHandlerMiddleware>();
//app.UseMiddleware<RequestResponseLoggingMiddleware>();
//app.UseMiddleware<ResponseMiddleware>();



app.Run();

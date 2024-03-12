using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Watsons.Common;
using Watsons.Common.EmailHelpers;
using Watsons.Common.HttpServices;
using Watsons.Common.ImageHelpers;
using Watsons.Common.JwtHelpers;
using Watsons.TRV2.API.Portal;
using Watsons.TRV2.DA.CashManage.Entities;
using Watsons.TRV2.DA.MyMaster.Entities;
using Watsons.TRV2.DA.Repositories;
using Watsons.TRV2.DA.SysCred.Entities;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.Services.CredEncryptor;
using Watsons.TRV2.Services.Portal;
using Watsons.TRV2.Services.RTS;

var builder = WebApplication.CreateBuilder(args);

var tRV2Connection = SysCredential.GetConnectionString("Server248", "TRV2");
var cashManageConnection = SysCredential.GetConnectionString("Server248", "CashManage");
var myMasterConnection = SysCredential.GetConnectionString("Server121", "MyMaster");
var sysCredConnection = SysCredential.GetConnectionString("Server185", "SysCred");

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
        sqlOptions => sqlOptions.EnableRetryOnFailure())
    .EnableServiceProviderCaching(true);
});

builder.Services.AddOptions();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<ImageSettings>(builder.Configuration.GetSection("ImageSettings"));
builder.Services.Configure<RtsSettings>(builder.Configuration.GetSection("RtsSettings"));
builder.Services.Configure<CredEncryptorSettings>(builder.Configuration.GetSection("CredEncryptorSettings"));

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
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
    };
});
builder.Services.AddAuthorization();

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
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IMfaService, MfaService>();
builder.Services.AddScoped<IRtsService, RtsService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
               builder =>
               {
                   builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
               });
});

//builder.Services.AddScoped<ITrImageRepository, TrImageRepository>();
//builder.Services.AddScoped<ITrCartRepository, TrCartRepository>();
//builder.Services.AddScoped<ITrOrderRepository, TrOrderRepository>();
//builder.Services.AddScoped<IStoreSalesBandRepository, StoreSalesBandRepository>();
//builder.Services.AddScoped<ITrOrderBatchRepository, TrOrderBatchRepository>();
//builder.Services.AddScoped<IItemMasterRepository, ItemMasterRepository>();
//builder.Services.AddScoped<IStoreMasterRepository, StoreMasterRepository>();
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

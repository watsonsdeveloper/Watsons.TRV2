using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Watsons.Common;
using Watsons.Common.ConnectionHelpers;
using Watsons.Common.EmailHelpers;
using Watsons.Common.EmailHelpers.Entities;
using Watsons.Common.ImageHelpers;
using Watsons.Common.TrafficLogHelpers;
using Watsons.TRV2.API.Mobile;
using Watsons.TRV2.API.Mobile.Middlewares;
using Watsons.TRV2.DA.MyMaster.Entities;
using Watsons.TRV2.DA.MyMaster.Repositories;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.Mobile;
using Watsons.TRV2.Services.Mobile;
using Watsons.TRV2.Services.RTS;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.Logger(log => log
        .Filter.ByIncludingOnly(e => e.Level == Serilog.Events.LogEventLevel.Information)
        .WriteTo.File("Logs/information.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30,
        rollOnFileSizeLimit: false))
    .WriteTo.Logger(log => log
        .Filter.ByIncludingOnly(e => e.Level == Serilog.Events.LogEventLevel.Debug)
        .WriteTo.File("Logs/debug.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30,
        rollOnFileSizeLimit: true, fileSizeLimitBytes: 419430400))
    .WriteTo.Logger(log => log
        .Filter.ByIncludingOnly(e => e.Level == Serilog.Events.LogEventLevel.Error)
        .WriteTo.File("Logs/errors.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30,
        rollOnFileSizeLimit: true, fileSizeLimitBytes: 419430400))
    .WriteTo.Logger(log => log
        .Filter.ByIncludingOnly(e => e.Level == Serilog.Events.LogEventLevel.Fatal)
        .WriteTo.File("Logs/fatal.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30,
        rollOnFileSizeLimit: true, fileSizeLimitBytes: 419430400))
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddSingleton<Serilog.ILogger>(provider => Log.Logger);

//var tRV2Connection = SysCredential.GetConnectionString("Server248", "TRV2");
//var myMasterConnection = SysCredential.GetConnectionString("Server121", "MyMaster");

var trv2ConnectionSettings = new ConnectionSettings();
var myMasterConnectionSettings = new ConnectionSettings();
var emailConnectionSettings = new ConnectionSettings();

builder.Configuration.GetSection("Trv2ConnectionSettings").Bind(trv2ConnectionSettings);
builder.Configuration.GetSection("MyMasterConnectionSettings").Bind(myMasterConnectionSettings);
builder.Configuration.GetSection("EmailConnectionSettings").Bind(emailConnectionSettings);

var trv2ConnectionString = SysCredential.GetConnectionString(trv2ConnectionSettings.Server, trv2ConnectionSettings.Database);
var myMasterConnectionString = SysCredential.GetConnectionString(myMasterConnectionSettings.Server, myMasterConnectionSettings.Database);
var emailConnectionString = SysCredential.GetConnectionString(emailConnectionSettings.Server, emailConnectionSettings.Database);

builder.Services.AddDbContextFactory<TrafficContext>(options =>
{
    options.UseSqlServer(trv2ConnectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure())
    .EnableServiceProviderCaching(true);
});

builder.Services.AddSingleton<TrafficContext>(provider =>
{
    var dbContextFactory = provider.GetRequiredService<IDbContextFactory<TrafficContext>>();
    return dbContextFactory.CreateDbContext();
});

builder.Services.AddDbContextFactory<EmailContext>(options =>
{
    options.UseSqlServer(emailConnectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure())
    .EnableServiceProviderCaching(true);
});

builder.Services.AddDbContextFactory<TrContext>(options =>
{
    options.UseSqlServer(trv2ConnectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure())
    .EnableServiceProviderCaching(true);
});

builder.Services.AddDbContextFactory<MyMasterContext>(options =>
{
    options.UseSqlServer(myMasterConnectionString,
         sqlOptions => {
             sqlOptions.UseCompatibilityLevel(120); // https://github.com/dotnet/efcore/issues/31362 # to fix EF contains error OPENJSON $ With 
             sqlOptions.EnableRetryOnFailure();
         })
    .EnableServiceProviderCaching(true);
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpLogging(logging => {
    logging.LoggingFields = HttpLoggingFields.All;
    //logging.RequestHeaders.Add("sec-ch-ua");
    //logging.RequestHeaders.Add("Authorization");
    //logging.ResponseHeaders.Add("Authorization");
    //logging.RequestHeaders.Add("AuthCookie");
    //logging.CombineLogs = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();


builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<ImageSettings>(builder.Configuration.GetSection("ImageSettings"));
builder.Services.Configure<RtsSettings>(builder.Configuration.GetSection("RtsSettings"));

builder.Services.AddScoped<EmailHelper>();
builder.Services.AddScoped<IImageHelper, ImageHelper>();

//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(Mappers).Assembly);

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<TrCartService>();
builder.Services.AddScoped<TrOrderService>();
builder.Services.AddScoped<UploadImageService>();
builder.Services.AddScoped<IUploadImageService, UploadImageService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRtsService, RtsService>();

builder.Services.AddScoped<ITrImageRepository, TrImageRepository>();
builder.Services.AddScoped<ITrCartRepository, TrCartRepository>();
builder.Services.AddScoped<ITrOrderRepository, TrOrderRepository>();
builder.Services.AddScoped<IStoreSalesBandRepository, StoreSalesBandRepository>();
builder.Services.AddScoped<ITrOrderBatchRepository, TrOrderBatchRepository>();
builder.Services.AddScoped<IItemMasterRepository, ItemMasterRepository>();
builder.Services.AddScoped<IStoreMasterRepository, StoreMasterRepository>();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();
}

app.UseMiddleware<TrafficLogMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<ResponseMiddleware>();

app.UseHttpLogging();
app.UseHttpsRedirection();
var staticFilePath = app.Configuration.GetValue<string>("StaticFilePath");
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(staticFilePath)),
    //RequestPath = new PathString("/secureFile"),
});

Routers.ConfigureEndpoints(app);

app.Run();

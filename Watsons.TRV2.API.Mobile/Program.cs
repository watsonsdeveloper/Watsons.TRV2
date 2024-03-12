using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Watsons.Common;
using Watsons.Common.EmailHelpers;
using Watsons.Common.ImageHelpers;
using Watsons.TRV2.API.Mobile;
using Watsons.TRV2.API.Mobile.Middlewares;
using Watsons.TRV2.DA.MyMaster.Entities;
using Watsons.TRV2.DA.Repositories;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.Mobile;
using Watsons.TRV2.Services.Mobile;
using Watsons.TRV2.Services.RTS;

var builder = WebApplication.CreateBuilder(args);

var tRV2Connection = SysCredential.GetConnectionString("Server248", "TRV2");
var myMasterConnection = SysCredential.GetConnectionString("Server121", "MyMaster");

builder.Services.AddDbContextFactory<TrContext>(options =>
{
    options.UseSqlServer(tRV2Connection,
        sqlOptions => sqlOptions.EnableRetryOnFailure())
    .EnableServiceProviderCaching(true);
});

builder.Services.AddDbContextFactory<MyMasterContext>(options =>
{
    options.UseSqlServer(myMasterConnection,
        sqlOptions => sqlOptions.EnableRetryOnFailure())
    .EnableServiceProviderCaching(true);
});

builder.Services.AddHttpContextAccessor();
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

//app.UseHttpsRedirection();
var staticFilePath = app.Configuration.GetValue<string>("StaticFilePath");
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(staticFilePath)),
    //RequestPath = new PathString("/secureFile"),
});

Routers.ConfigureEndpoints(app);

app.UseMiddleware<ExceptionHandlerMiddleware>();
//app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ResponseMiddleware>();


app.Run();

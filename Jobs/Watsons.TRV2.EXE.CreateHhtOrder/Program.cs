// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

Console.WriteLine("Create Store HHT Order");

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

var basePath = AppContext.BaseDirectory; // Directory.GetCurrentDirectory();
Console.WriteLine($"basePath : {basePath}");

var configuration = new ConfigurationBuilder()
    .SetBasePath(basePath)
     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
     .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
     .Build();

string baseAddress = configuration["BaseAddress"];

Console.WriteLine($"baseAddress : {baseAddress}");
Thread.Sleep(2000);

CreateHHTOrder(baseAddress).GetAwaiter().GetResult();

static async Task CreateHHTOrder(string baseAddress)
{
    HttpClientHandler clientHandler = new HttpClientHandler();
    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
    HttpClient client = new HttpClient(clientHandler);
    client.Timeout = Timeout.InfiniteTimeSpan;
    client.BaseAddress = new Uri(baseAddress);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

    var response = await client.GetAsync("/job/createStoreHhtOrder");

    Console.WriteLine(response);
    Thread.Sleep(5000);
}

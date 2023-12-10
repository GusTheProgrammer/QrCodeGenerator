using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using QrCodeGenerator;
using QrCodeGenerator.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient<ApiService>();
builder.Services.AddMudServices();

var apiServiceConfig = new ApiServiceConfig
{
    ApiKey = Environment.GetEnvironmentVariable("X_RAPID_API_KEY"),
};

// Register ApiServiceConfig
builder.Services.AddSingleton(apiServiceConfig);

await builder.Build().RunAsync();

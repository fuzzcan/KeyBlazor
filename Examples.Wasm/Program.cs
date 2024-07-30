using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Examples.Wasm;
using KeyBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var config = builder.Configuration;
var services = builder.Services;
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register service provider
var baseAddress = builder.HostEnvironment.BaseAddress;
var httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
builder.Services.AddScoped(sp => httpClient);

// Register logging 
var logging = builder.Logging;
logging.AddConfiguration(config.GetSection("Logging"));
logging.SetMinimumLevel(LogLevel.Debug);

// Register KeyboardEventService as a singleton and configure options
services.Configure<KeyboardEventServiceOptions>(
   config.GetSection("KeyboardEventServiceOptions"));
services.AddSingleton<KeyboardEventService>();

await builder.Build().RunAsync();
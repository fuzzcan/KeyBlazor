using KeyBlazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SampleApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
    { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

// Register logging 
// builder.Logging.AddConfiguration(
//     builder.Configuration.GetSection("Logging"));
// builder.Logging.SetMinimumLevel(LogLevel.Information);

// Register KeyboardEventService as a singleton and configure options
builder.Services.Configure<KeyboardEventServiceOptions>(
    builder.Configuration.GetSection("KeyboardEventServiceOptions"));
builder.Services.AddScoped<KeyboardEventService>();

await builder.Build().RunAsync();
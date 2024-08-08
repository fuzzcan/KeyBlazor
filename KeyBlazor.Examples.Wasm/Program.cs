using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KeyBlazor.Examples.Wasm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var config = builder.Configuration;
var services = builder.Services;
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register service provider
var baseAddress = builder.HostEnvironment.BaseAddress;
var httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
builder.Services.AddScoped(sp => httpClient);

services.AddScoped<KeyBlazor.Service>();

await builder.Build().RunAsync();
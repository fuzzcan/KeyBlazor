using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KeyBlazor;

public static class ServiceRegistrar
{
    public static void AddKeyBlazor(this IServiceCollection services)
    {
        services.AddScoped<Service>();
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KeyBlazor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKeyboardEventService(this IServiceCollection services)
    {
        services.Configure<KeyboardEventServiceOptions>(options =>
        {
           
        });
        services.AddScoped<KeyboardEventService>();

        return services;
    }
}
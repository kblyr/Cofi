using Microsoft.Extensions.DependencyInjection;

namespace Cofi.Contracts;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApiTypeMapRegistry(this IServiceCollection services, Action<ApiTypeMapRegistry> registerTypeMaps)
    {
        var registry = new ApiTypeMapRegistry();
        registerTypeMaps(registry);
        services.AddSingleton(registry);
        return services;
    }
}
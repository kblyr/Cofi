using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace Cofi;

public static class IEndpointRouteBuilderExtensions
{
    static readonly Type _endpointMapperType = typeof(EndpointMapper);

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder, Assembly assemblyMarker)
    {
        var mapperTypes = assemblyMarker.GetTypes()
            .Where(mapperType =>
                mapperType.IsClass 
                && !mapperType.IsAbstract
                && !mapperType.IsGenericType
                && mapperType.GetConstructor(Type.EmptyTypes) is not null
                && _endpointMapperType.IsAssignableFrom(mapperType)
            );

        if (mapperTypes is not null)
        {
            foreach (var mapperType in mapperTypes)
            {
                var instance =  (EndpointMapper?)Activator.CreateInstance(mapperType);
                instance?.Map(builder);
            }
        }

        return builder;
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder) => builder.MapEndpoints(Assembly.GetCallingAssembly());
}
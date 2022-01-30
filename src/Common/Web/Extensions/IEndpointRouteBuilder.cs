using Microsoft.AspNetCore.Routing;

namespace Cofi;

public static class IEndpointRouteBuilder_Extensions
{
    public static EndpointMapper MapEndpoints(this IEndpointRouteBuilder builder) => new(builder);
}
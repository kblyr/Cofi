using Microsoft.AspNetCore.Routing;

namespace Cofi;

public interface EndpointMapper
{
    void Map(IEndpointRouteBuilder builder);
}
using System.ComponentModel;
using Microsoft.AspNetCore.Routing;

namespace Cofi;

public class EndpointMapper
{
    public IEndpointRouteBuilder Builder { get; }

    public EndpointMapper(IEndpointRouteBuilder builder) { Builder = builder; }
}
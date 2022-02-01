using System.Net.Mime;
using Cofi.Messaging;

namespace Cofi.Identity.Endpoints;

sealed class User_EndpointMapper : EndpointMapper
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/user/sign-up", Signup)
            .Accepts<SignupUserRequest>(MediaTypeNames.Application.Json)
            .Produces<SignupUserResponse>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
            .Produces<ApiFailedResponse>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json);
    }

    static async Task<IResult> Signup(MappedMediatorAdapter mediator, SignupUserRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send<SignupUserRequest, SignupUser>(request, cancellationToken);
        return Results.Extensions.MappedCreated<SignupUser.Response>(response);
    }
}
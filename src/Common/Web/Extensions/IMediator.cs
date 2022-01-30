using Microsoft.AspNetCore.Http;

namespace Cofi;

public static class IMediator_Extensions
{
    public static async Task<IResult> SendThenOk<TRequest, TSuccessResponse>(this IMediator mediator, TRequest request, CancellationToken cancellationToken = default) where TRequest : CofiRequest
    {
        return await mediator.Send(request, cancellationToken).ConfigureAwait(false) switch
        {
            TSuccessResponse response => Results.Ok(response),
            FailedResponse response => Results.BadRequest(response),
            _ => throw new UnsupportedResponseException()
        };
    }

    public static async Task<IResult> SendThenCreated<TRequest, TSuccessResponse>(this IMediator mediator, TRequest request, BuildCreateUri<TSuccessResponse> buildUri, CancellationToken cancellationToken = default) 
        where TRequest : CofiRequest
        where TSuccessResponse : CofiResponse
    {
        return await mediator.Send(request, cancellationToken).ConfigureAwait(false) switch
        {
            TSuccessResponse response => Results.Created(buildUri(response), response),
            _ => throw new UnsupportedResponseException()
        };
    }
}

public delegate string BuildCreateUri<TSuccessResponse>(TSuccessResponse response) where TSuccessResponse : CofiResponse;
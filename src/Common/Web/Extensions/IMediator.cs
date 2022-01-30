using Microsoft.AspNetCore.Http;

namespace Cofi;

public static class IMediatorExtensions
{
    public static async Task<IResult> SendAndTransformToApiResult<TRequest, TSuccessResponse>(this IMediator mediator, TRequest request, CancellationToken cancellationToken = default) where TRequest : CofiRequest
    {
        return await mediator.Send(request, cancellationToken).ConfigureAwait(false) switch
        {
            TSuccessResponse response => Results.Ok(response),
            FailedResponse response => Results.BadRequest(response),
            _ => throw new UnsupportedResponseException()
        };
    }
}
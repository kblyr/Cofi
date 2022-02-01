using Microsoft.AspNetCore.Http;

namespace Cofi;

public static class CofiWebApiResults
{
    public static IResult Mapped<TResponse>(this IResultExtensions extensions, CofiResponse response, int statusCode) where TResponse : CofiResponse
    {
        return new MappedResponseResult<TResponse>(response, statusCode);
    }

    public static IResult MappedOK<TResponse>(this IResultExtensions extensions, CofiResponse response) where TResponse : CofiResponse => extensions.Mapped<TResponse>(response, StatusCodes.Status200OK);

    public static IResult MappedCreated<TResponse>(this IResultExtensions extensions, CofiResponse response) where TResponse : CofiResponse => extensions.Mapped<TResponse>(response, StatusCodes.Status201Created);
}
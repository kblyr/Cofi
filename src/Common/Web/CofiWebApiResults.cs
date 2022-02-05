using Microsoft.AspNetCore.Http;

namespace Cofi;

public static class CofiWebApiResults
{
    public static IResult Mapped<TResponse>(this IResultExtensions extensions, Response response, int statusCode) where TResponse : Response
    {
        return new MappedResponseResult<TResponse>(response, statusCode);
    }

    public static IResult MappedOK<TResponse>(this IResultExtensions extensions, Response response) where TResponse : Response => extensions.Mapped<TResponse>(response, StatusCodes.Status200OK);

    public static IResult MappedCreated<TResponse>(this IResultExtensions extensions, Response response) where TResponse : Response => extensions.Mapped<TResponse>(response, StatusCodes.Status201Created);
}
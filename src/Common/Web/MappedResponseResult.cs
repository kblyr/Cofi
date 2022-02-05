using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cofi;

public class MappedResponseResult<TResponse> : IResult where TResponse : Response
{
    static readonly object _lockObj = new();
    static readonly Dictionary<Type, string> _errorTypes = new();
    readonly Response _response;
    readonly int _statusCode;

    public MappedResponseResult(Response response, int statusCode)
    {
        _response = response;
        _statusCode = statusCode;
    }

    public async Task ExecuteAsync(HttpContext context)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        
        switch (_response)
        {
            case TResponse response:
                await RespondSuccess(context, response, _statusCode).ConfigureAwait(false);
                return;
            case FailedResponse response:
                await RespondFailed(context, response).ConfigureAwait(false);
                return;
            default: throw new UnsupportedResponseException();
        }
    }

    static async Task RespondSuccess(HttpContext context, TResponse response, int statusCode)
    {
        var mapper = context.RequestServices.GetRequiredService<ApiTypeMapper>();
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(mapper.Map(response)).ConfigureAwait(false);
    }

    static async Task RespondFailed(HttpContext context, FailedResponse response)
    {
        var mapper = context.RequestServices.GetRequiredService<ApiTypeMapper>();
        var responseType = response.GetType();
        var apiFailedResponse = new ApiFailedResponse(GetErrorType(responseType), mapper.Map(response, responseType));
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(apiFailedResponse).ConfigureAwait(false);
    }

    static string GetErrorType(Type responseType)
    {
        lock(_lockObj)
        {
            if (_errorTypes.ContainsKey(responseType))
                return _errorTypes[responseType];

            var attribute = Attribute.GetCustomAttribute(responseType, typeof(FailedResponseMetadataAttribute));

            if (attribute is FailedResponseMetadataAttribute metadata)
            {
                _errorTypes[responseType] = metadata.ErrorType;
                return metadata.ErrorType;
            }

            _errorTypes[responseType] = responseType.Name;
            return responseType.Name;
        }
    }
}

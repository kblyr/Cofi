namespace Cofi.Contracts;

public record ApiFailedResponse
{
    public string ErrorType { get; init; } = default!;
    public object Error { get; init; } = default!;

    public ApiFailedResponse() { }

    public ApiFailedResponse(string errorType, object error)
    {
        ErrorType = errorType;
        Error = error;
    }
}
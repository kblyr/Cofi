using AutoMapper;

namespace Cofi.Schema;

public record ValidationFailedResponse
{
    public const string ErrorType = "ValidationFailed";

    public IEnumerable<FailureObj> Failures { get; init; } = default!;

    public record FailureObj
    {
        public string PropertyName { get; init; } = default!;
        public string ErrorMessage { get; init; } = default!;
    }
}

sealed class ValidationFailedResponse_Mapping : Profile
{
    public ValidationFailedResponse_Mapping()
    {
        CreateMap<ValidationFailed, ValidationFailedResponse>();
        CreateMap<ValidationFailed.FailureObj, ValidationFailedResponse.FailureObj>();
    }
}
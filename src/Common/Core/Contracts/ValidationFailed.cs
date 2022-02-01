namespace Cofi.Contracts;

[FailedResponseMetadata("Validation Failed")]
public record ValidationFailed : FailedResponse
{
    public IEnumerable<FailureObj> Failures { get; init; } = default!;

    public record FailureObj
    {
        public string PropertyName { get; init; } = default!;
        public string ErrorMessage { get; init; } = default!;
    }
}

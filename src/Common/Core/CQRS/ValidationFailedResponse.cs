namespace Cofi;

public record ValidationFailedResponse : FailedResponse
{
    public IEnumerable<ValidationFailureObj> Failures { get; init; } = default!;

    public record ValidationFailureObj
    {
        public string PropertyName { get; init; } = default!;
        public string ErrorMessage { get; init; } = default!;
    }
}
namespace Cofi.Schema;

public record ValidationFailedResponse
{
    public IEnumerable<FailureObj> Failures { get; init; } = default!;

    public record FailureObj
    {
        public string PropertyName { get; init; } = default!;
        public string ErrorMessage { get; init; } = default!;
    }
}

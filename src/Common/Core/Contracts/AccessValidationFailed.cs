namespace Cofi.Contracts;

[FailedResponseMetadata("Access Validation Failed")]
public record AccessValidationFailed : FailedResponse
{
    public IEnumerable<FailedRuleObj> FailedRules { get; init; } = default!;

    public record FailedRuleObj
    {
        public string Name { get; init; } = default!;
        public IDictionary<string, object?> Data { get; init; } = default!;
    }
}
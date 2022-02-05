namespace Cofi.Schema;

public record AccessValidationFailedResponse
{
    public IEnumerable<FailedRuleObj> FailedRules { get; init; } = default!;
    
    public record FailedRuleObj
    {
        public string RuleName { get; init; } = default!;
        public IDictionary<string, object?> Payload { get; init; } = default!;
    }
}
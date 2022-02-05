namespace Cofi.Validation;

sealed class AccessValidationContext : IAccessValidationContext
{
    readonly AccessValidationRules _rules = new();
    public IAccessValidationRules Rules => _rules;
    public AccessValidationMode Mode { get; set; }

    public IAccessValidationContext Require(IAccessValidationRule rule)
    {
        _rules.Add(rule);
        return this;
    }
}

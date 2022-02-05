namespace Cofi.Validation;

public abstract class AccessValidatorBase
{
    protected abstract Task<bool> Validate(IAccessValidationRule rule, CancellationToken cancellationToken);

    public async Task<AccessValidationResult> Validate(AccessValidationMode mode, IAccessValidationRules rules, CancellationToken cancellationToken = default)
    {
        if (rules.Count == 0)
            return new(true, AccessValidationRules.Empty);

        return mode switch
        {
            AccessValidationMode.Any => await ValidateAny(rules, cancellationToken).ConfigureAwait(false),
            AccessValidationMode.All => await ValidateAll(rules, cancellationToken).ConfigureAwait(false),
            _ => throw new CofiException($"Value for enum '{typeof(AccessValidationMode).Name}' is not supported")
        };
    }

    async Task<AccessValidationResult> ValidateAny(IAccessValidationRules rules, CancellationToken cancellationToken)
    {
        var failedRules = new AccessValidationRules();

        foreach (var rule in rules)
        {
            if (await Validate(rule, cancellationToken).ConfigureAwait(false))
                return new(true, failedRules);
        }

        return new(false, failedRules);
    }

    async Task<AccessValidationResult> ValidateAll(IAccessValidationRules rules, CancellationToken cancellationToken)
    {
        foreach (var rule in rules)
        {
            if (await Validate(rule, cancellationToken).ConfigureAwait(false) == false)
                return new(false, new AccessValidationRules().Add(rule));
        }

        return new(true, AccessValidationRules.Empty);
    }
}

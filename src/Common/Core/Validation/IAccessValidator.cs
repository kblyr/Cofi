namespace Cofi.Validation;

public interface IAccessValidator
{
    Task<AccessValidationResult> Validate(AccessValidationMode mode, IAccessValidationRules rules, CancellationToken cancellationToken = default);
}

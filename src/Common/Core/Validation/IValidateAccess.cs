namespace Cofi.Validation;

public interface IValidateAccess<TRule> where TRule : IAccessValidationRule
{
    ValueTask<bool> Validate(TRule rule, CancellationToken cancellationToken = default);
}
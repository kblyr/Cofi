namespace Cofi.Validation;

public struct AccessValidationResult
{
    public bool IsSucceeded { get; }
    public IAccessValidationRules FailedRules { get; }

    public AccessValidationResult(bool isSucceeded, IAccessValidationRules failedRules)
    {
        IsSucceeded = isSucceeded;
        FailedRules = failedRules;
    }
}
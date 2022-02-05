namespace Cofi.Validation;

public interface IAccessValidationContext
{
    IAccessValidationContext Require(IAccessValidationRule rule);
}

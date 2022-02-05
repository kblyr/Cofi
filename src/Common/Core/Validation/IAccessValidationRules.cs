namespace Cofi.Validation;

public interface IAccessValidationRules : IEnumerable<IAccessValidationRule>
{
    public int Count { get; }
    IAccessValidationRules Add(IAccessValidationRule rule);
}

namespace Cofi.Validation;

public interface IAccessValidationRule
{
    string Name { get; }
    IDictionary<string, object?> Data { get; }
}

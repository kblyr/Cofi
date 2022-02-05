using System.Collections;

namespace Cofi.Validation;

public class AccessValidationRules : IAccessValidationRules
{
    readonly List<IAccessValidationRule> _rules = new();

    public int Count => _rules.Count;

    public IAccessValidationRules Add(IAccessValidationRule rule)
    {
        _rules.Add(rule);
        return this;
    }

    public IEnumerator<IAccessValidationRule> GetEnumerator() => _rules.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static readonly IAccessValidationRules Empty = new AccessValidationRules();
}
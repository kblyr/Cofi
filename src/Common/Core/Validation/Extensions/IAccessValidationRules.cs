using Cofi.Validation.Rules;

namespace Cofi.Validation;

public static class IAccessValidationRules_Extensions
{
    public static IAccessValidationRules AddPermission(this IAccessValidationRules rules, string permission)
    {
        return rules.Add(new Permission_AVR(permission));
    }
}
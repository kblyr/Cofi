using Cofi.Validation.Rules;

namespace Cofi.Validation;

public static class IAccessValidationContext_Extensions
{
    public static IAccessValidationContext RequireIf<TRule>(this IAccessValidationContext context, TRule rule, bool condition) where TRule : IAccessValidationRule
    {
        if (condition)
            context.Require(rule);

        return context;
    }

    public static IAccessValidationContext RequirePermission(this IAccessValidationContext context, string permission)
    {
        return context.Require(new Permission_AVR(permission));
    }

    public static IAccessValidationContext RequirePermissionIf(this IAccessValidationContext context, string permission, bool condition)
    {
        return context.RequireIf(new Permission_AVR(permission), condition);
    }
}
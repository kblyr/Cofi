namespace Cofi.Validation.Rules;

sealed class Permission_AVR : AccessValidationRuleBase, IAccessValidationRule
{
    public string Name { get; } = "Cofi.Validation.ByPermission";
    public string Permission { get; }

    public Permission_AVR(string permission)
    {
        Permission = permission;
    }

    protected override void SetData(IDictionary<string, object?> payload)
    {
        payload.Add(nameof(Permission), Permission);
    }
}
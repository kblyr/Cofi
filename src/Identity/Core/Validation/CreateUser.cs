using Cofi.Validation;

namespace Cofi.Identity.Validation;

sealed class CreateUser_AVC : IAccessValidationConfiguration<CreateUser>
{
    public void Configure(IAccessValidationContext context, CreateUser resource)
    {
        context.RequirePermission(Permissions.User.Add)
            .RequirePermissionIf(Permissions.UserDomain.Add, resource.DomainIds.Any())
            .RequirePermissionIf(Permissions.UserRole.Add, resource.RoleIds.Any())
            .RequirePermissionIf(Permissions.UserPermission.Add, resource.PermissionIds.Any());
    }
}

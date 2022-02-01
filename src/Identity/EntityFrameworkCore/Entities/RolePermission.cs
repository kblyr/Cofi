namespace Cofi.Identity.Entities;

public class RolePermission
{
    public long Id { get; set; }
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
    public bool IsDeleted { get; set; }
    public int? InsertedById { get; set; }
    public DateTimeOffset? InsertedOn { get; set; }
    public int? DeletedById { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }

    public Role Role { get; set; } = default!;
    public Permission Permission { get; set; } = default!;
}
namespace Cofi.Identity.Entities;

public class UserPermission
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public int PermissionId { get; set; }
    public bool IsDirectLink { get; set; }
    public bool IsDeleted { get; set; }
    public int? InsertedById { get; set; }
    public DateTimeOffset? InsertedOn { get; set; }
    public int? DeletedById { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }

    public User User { get; set; } = default!;
    public Permission Permission { get; set; } = default!;
}
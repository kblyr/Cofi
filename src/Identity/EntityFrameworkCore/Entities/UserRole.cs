namespace Cofi.Identity.Entities;

public record UserRole
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public bool IsDeleted { get; set; }
    public int? InsertedById { get; set; }
    public DateTimeOffset? InsertedOn { get; set; }
    public int? DeletedById { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }

    public User User { get; set; } = default!;
    public Role Role { get; set; } = default!;
}
namespace Cofi.Identity.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string HashedPassword { get; set; } = default!;
    public UserStatus Status { get; set; }
    public bool IsPasswordChangeRequired { get; set; }
    public bool IsDeleted { get; set; }
    public int? InsertedById { get; set; }
    public DateTimeOffset? InsertedOn { get; set; }
    public int? UpdatedById { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public int? DeletedById { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
}

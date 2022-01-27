namespace Cofi.Identity.Entities;

public record User
{
    public int Id { get; init; }
    public string Username { get; set; } = default!;
    public string HashedPassword { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public bool IsActive { get; set; }
    public bool IsPasswordChangeRequired { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsDeleted { get; set; }
    public int? InsertedById { get; set; }
    public DateTimeOffset? InsertedOn { get; set; }
    public int? DeletedById { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
}

namespace Cofi.Identity.Contracts;

public record UserCreated
{
    public int Id { get; init; }
    public string Username { get; init; } = default!;
    public string EmailAddress { get; init; } = default!;
    public bool IsActive { get; init; }
    public bool IsPasswordChangeRequired { get; init; }
    public bool IsEmailVerified { get; init; }
}
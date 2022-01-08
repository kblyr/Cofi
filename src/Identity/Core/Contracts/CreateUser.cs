namespace Cofi.Identity.Contracts;

public record CreateUser
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string EmailAddress { get; init; } = default!;
    public bool IsActive { get; init; }
    public bool IsPasswordChangeRequired { get; init; }
    public bool IsEmailVerified { get; init; }
}
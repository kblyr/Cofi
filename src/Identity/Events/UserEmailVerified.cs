namespace Cofi.Identity.Events;

public record UserEmailVerified
{
    public int UserId { get; init; }
    public string EmailAddress { get; init; } = "";
}

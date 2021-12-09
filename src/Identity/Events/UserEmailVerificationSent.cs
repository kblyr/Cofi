namespace Cofi.Identity.Events;

public record UserEmailVerificationSent
{
    public int UserId { get; init; }
    public string EmailAddress { get; init; } = "";
}

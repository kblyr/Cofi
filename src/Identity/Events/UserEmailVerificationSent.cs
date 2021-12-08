namespace Cofi.Identity.Events;

public record UserEmailVerificationSent
{
    public int UserId { get; init; }
}

namespace Cofi.Identity.Contracts;

public record UserEmailVerificationSent
{
    public int Id { get; init; }
    public string EmailAddress { get; init; } = default!;
}

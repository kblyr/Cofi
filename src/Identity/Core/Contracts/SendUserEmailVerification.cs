namespace Cofi.Identity.Contracts;

public record SendUserEmailVerification
{
    public int Id { get; init; }
    public string EmailAddress { get; init; } = default!;
}
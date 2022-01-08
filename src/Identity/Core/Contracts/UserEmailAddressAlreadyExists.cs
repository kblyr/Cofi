namespace Cofi.Identity.Contracts;

public record UserEmailAddressAlreadyExists
{
    public string EmailAddress { get; init; } = default!;
}
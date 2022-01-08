namespace Cofi.Identity.Contracts;

public record CreateUserFailed
{
    public UsernameAlreadyExists? UsernameAlreadyExists { get; init; }
    public UserEmailAddressAlreadyExists? EmailAddressAlreadyExists { get; init; }
}
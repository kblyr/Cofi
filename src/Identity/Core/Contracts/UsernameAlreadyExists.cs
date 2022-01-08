namespace Cofi.Identity.Contracts;

public record UsernameAlreadyExists
{
    public string Username { get; init; } = default!;
}

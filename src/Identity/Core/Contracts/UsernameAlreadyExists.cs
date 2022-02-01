namespace Cofi.Identity.Contracts;

public record UsernameAlreadyExists : FailedResponse
{
    public string Username { get; init; } = default!;
}
namespace Cofi.Identity.Contracts;

public record UserSignedUp
{
    public int Id { get; init; }
    public string Username { get; init; } = default!;
    public string Status { get; init; } = default!;
}

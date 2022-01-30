namespace Cofi.Identity.Events;

public record UserCreated
{
    public int UserId { get; init; }
    public string Username { get; init; } = default!;
    public bool IsActive { get; init; }
}
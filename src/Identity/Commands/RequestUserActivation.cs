namespace Cofi.Identity.Commands;

public record RequestUserActivation
{
    public int UserId { get; init; }
}
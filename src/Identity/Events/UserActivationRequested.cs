namespace Cofi.Identity.Events;

public record UserActivationRequested
{
    public int UserId { get; init; }
}

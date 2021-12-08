namespace Cofi.Identity.Events;

public record UserDeactivated
{
    public int UserId { get; init; }
    public int? DeactivatedById { get; init; }
}
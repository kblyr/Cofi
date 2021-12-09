namespace Cofi.Identity.Events;

public record UserActivated
{
    public int UserId { get; init; }
    public int? ActivatedById { get; init; }
}
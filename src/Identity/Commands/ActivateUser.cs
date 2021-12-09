namespace Cofi.Identity.Commands;

public record ActivateUser
{
    public int UserId { get; init; }
    public int ActivatedById { get; init; }
}

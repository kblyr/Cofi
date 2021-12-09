namespace Cofi.Identity.Commands;

public record DeactivateUser
{
    public int UserId { get; init; }
    public int DeactivatedById { get; init; }
}

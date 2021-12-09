namespace Cofi.Identity.Events;

public record UserActivationRequestApproved
{
    public int UserId { get; init; }
    public int ApprovedById { get; init; }
}

namespace Cofi.Identity.Commands;

public record ApproveUserActivationRequest
{
    public int UserId { get; init; }
    public int ApprovedById { get; init; }
}

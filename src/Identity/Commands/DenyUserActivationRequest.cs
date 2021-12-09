namespace Cofi.Identity.Commands;

public record DenyUserActivationRequest
{
    public int UserId { get; init; }
    public int DeniedById { get; init; }
}
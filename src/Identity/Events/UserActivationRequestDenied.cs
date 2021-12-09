namespace Cofi.Identity.Events;

public record UserActivationRequestDenied
{
    public int UserId { get; init; }
    public int DeniedById { get; init; }
}
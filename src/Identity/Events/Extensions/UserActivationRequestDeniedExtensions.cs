namespace Cofi.Identity.Events;

public static class UserActivationRequestDeniedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserActivationRequestDenied @event) => new(string, object?)[]
    {
        (nameof(@event.UserId), @event.UserId),
        (nameof(@event.DeniedById), @event.DeniedById)
    };
}
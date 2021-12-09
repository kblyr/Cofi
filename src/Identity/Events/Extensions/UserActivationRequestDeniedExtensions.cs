namespace Cofi.Identity.Events;

public static class UserActivationRequestDeniedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserActivationRequestDenied @event) => new(string, object?)[]
    {
        ("UserId", @event.UserId),
        ("DeniedById", @event.DeniedById)
    };
}
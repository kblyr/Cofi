namespace Cofi.Identity.Events;

public static class UserActivationRequestedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserActivationRequested @event) => new(string, object?)[]
    {
        ("UserId", @event.UserId)
    };
}
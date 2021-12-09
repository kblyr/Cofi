namespace Cofi.Identity.Events;

public static class UserDeactivatedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserDeactivated @event) => new(string, object?)[]
    {
        ("UserId", @event.UserId),
        ("DeactivatedById", @event.DeactivatedById)
    };
}

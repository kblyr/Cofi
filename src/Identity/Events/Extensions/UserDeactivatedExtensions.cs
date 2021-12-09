namespace Cofi.Identity.Events;

public static class UserDeactivatedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserDeactivated @event) => new(string, object?)[]
    {
        (nameof(@event.UserId), @event.UserId),
        (nameof(@event.DeactivatedById), @event.DeactivatedById)
    };
}

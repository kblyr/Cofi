namespace Cofi.Identity.Events;

public static class UserActivatedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserActivated @event) => new(string, object?)[]
    {
        (nameof(@event.UserId), @event.UserId),
        (nameof(@event.ActivatedById), @event.ActivatedById)
    };
}

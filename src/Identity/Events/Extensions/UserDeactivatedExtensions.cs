namespace Cofi.Identity.Events;

public static class UserDeactivatedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserDeactivated @event) => new(string, object?)[]
    {
        ("user_id", @event.UserId),
        ("deactivated_by_id", @event.DeactivatedById)
    };
}
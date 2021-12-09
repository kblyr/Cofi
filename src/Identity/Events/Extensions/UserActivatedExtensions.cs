namespace Cofi.Identity.Events;

public static class UserActivatedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserActivated @event) => new(string, object?)[]
    {
        ("user_id", @event.UserId),
        ("activated_by_id", @event.ActivatedById)
    };
}
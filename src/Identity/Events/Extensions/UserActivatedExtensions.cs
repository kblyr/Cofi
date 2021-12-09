namespace Cofi.Identity.Events;

public static class UserActivatedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserActivated @event) => new(string, object?)[]
    {
        ("UserId", @event.UserId),
        ("ActivatedById", @event.ActivatedById)
    };
}

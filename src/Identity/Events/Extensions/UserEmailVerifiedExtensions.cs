namespace Cofi.Identity.Events;

public static class UserEmailVerifiedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserEmailVerified @event) => new(string, object?)[]
    {
        (nameof(@event.UserId), @event.UserId)
    };
}
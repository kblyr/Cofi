namespace Cofi.Identity.Events;

public static class UserEmailVerificationSentExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserEmailVerificationSent @event) => new (string, object?)[]
    {
        ("UserId", @event.UserId)
    };
}

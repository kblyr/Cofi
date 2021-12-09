namespace Cofi.Identity.Events;

public static class UserCreatedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserCreated @event) => new (string, object?)[] 
    {
        (nameof(@event.Id), @event.Id),
        (nameof(@event.IsActive), @event.IsActive),
        (nameof(@event.IsEmailAddressVerified), @event.IsEmailAddressVerified),
        (nameof(@event.IsPasswordChangeRequired), @event.IsPasswordChangeRequired)
    };
}

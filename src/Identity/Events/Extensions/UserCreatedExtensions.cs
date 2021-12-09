namespace Cofi.Identity.Events;

public static class UserCreatedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserCreated @event) => new (string, object?)[] 
    {
        ("UserId", @event.Id)
    };
}

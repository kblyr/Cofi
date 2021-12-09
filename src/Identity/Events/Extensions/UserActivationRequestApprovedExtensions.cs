namespace Cofi.Identity.Events;

public static class UserActivationRequestApprovedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserActivationRequestApproved @event) => new(string, object?)[]
    {
        (nameof(@event.UserId), @event.UserId),
        (nameof(@event.ApprovedById), @event.ApprovedById)
    };
}

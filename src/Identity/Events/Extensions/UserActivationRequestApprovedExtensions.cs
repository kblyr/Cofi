namespace Cofi.Identity.Events;

public static class UserActivationRequestApprovedExtensions
{
    public static (string, object?)[] GetLoggingProps(this UserActivationRequestApproved @event) => new(string, object?)[]
    {
        ("UserId", @event.UserId),
        ("ApprovedById", @event.ApprovedById)
    };
}

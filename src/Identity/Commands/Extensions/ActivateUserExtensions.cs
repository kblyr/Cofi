namespace Cofi.Identity.Commands;

public static class ActivateUserExtensions
{
    public static (string, object?)[] GetLoggingProps(this ActivateUser command) => new(string, object?)[]
    {
        (nameof(command.UserId), command.UserId),
        (nameof(command.ActivatedById), command.ActivatedById)
    };
}
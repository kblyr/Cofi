namespace Cofi.Identity.Commands;

public record VerifyUserEmail
{
    public int UserId { get; init; }
    public string EmailAddress { get; init; } = "";
}
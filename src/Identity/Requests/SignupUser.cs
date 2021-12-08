namespace Cofi.Identity.Requests;

public record SignupUser
{
    public string Username { get; init; } = "";
    public string EmailAddress { get; init; } = "";
    public string Password { get; init; } = "";
}

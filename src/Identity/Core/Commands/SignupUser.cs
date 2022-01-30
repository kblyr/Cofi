namespace Cofi.Identity.Commands;

public record SignupUser : CofiRequest
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;

    public record Response(int Id) : CofiResponse;
}
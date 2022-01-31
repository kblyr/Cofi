namespace Cofi.Identity.Schema;

public record SignupUserRequest
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}

public record SignupUserResponse
{
    public int Id { get; init; }
}
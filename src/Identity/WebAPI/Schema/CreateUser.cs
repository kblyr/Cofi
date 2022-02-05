namespace Cofi.Identity.Schema;

public record CreateUserRequest
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
    public byte Status { get; init; }
    public bool IsPasswordChangeRequired { get; init; }
    public IEnumerable<short> DomainIds { get; init; } = default!;
    public IEnumerable<int> RoleIds { get; init; } = default!;
    public IEnumerable<int> PermissionIds { get; init; } = default!;
}

public record CreateUserResponse
{
    public int Id { get; init; }
}
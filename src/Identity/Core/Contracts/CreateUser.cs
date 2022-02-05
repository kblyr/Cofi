namespace Cofi.Identity.Contracts;

public record CreateUser : Request
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
    public byte Status { get; init; }
    public bool IsPasswordChangeRequired { get; init; }

    public IEnumerable<short> DomainIds { get; init; } = default!;
    public IEnumerable<int> RoleIds { get; init; } = default!;
    public IEnumerable<int> PermissionIds { get; init; } = default!;

    public record Response : Cofi.Contracts.Response
    {
        public int Id { get; init; }

        public Response() { }

        public Response(int id)
        {
            Id = id;
        }
    }
}
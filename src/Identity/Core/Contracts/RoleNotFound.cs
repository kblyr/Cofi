namespace Cofi.Identity.Contracts;

public record RoleNotFound : FailedResponse
{
    public int Id { get; init; }

    public RoleNotFound() { }

    public RoleNotFound(int id)
    {
        Id  = id;
    }
}

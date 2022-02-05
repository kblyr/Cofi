namespace Cofi.Identity.Contracts;

public record PermissionNotFound : FailedResponse
{
    public int Id { get; init; }

    public PermissionNotFound() { }

    public PermissionNotFound(int id)
    {
        Id = id;
    }
}
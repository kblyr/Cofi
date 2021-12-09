namespace Cofi.Identity.Responses;

public record UserAlreadyActive
{
    public int Id { get; init; }

    public UserAlreadyActive() { }

    public UserAlreadyActive(int id) => Id = id;
}
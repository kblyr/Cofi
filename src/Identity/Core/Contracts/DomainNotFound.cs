namespace Cofi.Identity.Contracts;

public record DomainNotFound : FailedResponse
{
    public short Id { get; init; }

    public DomainNotFound() { }

    public DomainNotFound(short id)
    {
        Id = id;
    }
}
namespace Cofi.Identity.Entities;

public record Domain
{
    public short Id { get; set; }
    public string Name { get; set; } = default!;
    public string? LookupKey { get; set; }
    public bool IsDeleted { get; set; }
    public int? InsertedById { get; set; }
    public DateTimeOffset? InsertedOn { get; set; }
    public int? DeletedById { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
}
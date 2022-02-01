namespace Cofi.Identity.Entities;

public class Domain
{
    public short Id { get; set; }
    public string? LookupKey { get; set; }
    public string Name { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public int? InsertedById { get; set; }
    public DateTimeOffset? InsertedOn { get; set; }
    public int? UpdatedById { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public int? DeletedById { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
}

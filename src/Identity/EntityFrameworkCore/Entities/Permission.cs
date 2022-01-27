namespace Cofi.Identity.Entities;

public record Permission
{
    public int Id { get; set; }
    public short? DomainId { get; set; }
    public int? RoleId { get; set; }
    public string Name { get; set; } = default!;
    public string? LookupKey { get; set; }
    public bool IsDeleted { get; set; }
    public int? InsertedById { get; set; }
    public DateTimeOffset? InsertedOn { get; set; }
    public int? DeletedById { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }

    public Domain? Domain { get; set; }
    public Role? Role { get; set; }
}

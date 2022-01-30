namespace Cofi.Auditing;

public record AuditInfo
{
    public int? UserId { get; init; }
    public DateTimeOffset? Timestamp { get; init; }
}
namespace Cofi.Auditing;

public interface ICurrentAuditInfoProvider
{
    public AuditInfo Current { get; }
}
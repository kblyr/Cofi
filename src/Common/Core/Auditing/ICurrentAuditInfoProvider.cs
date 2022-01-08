namespace Cofi.Auditing;

public interface ICurrentAuditInfoProvider
{
    Task<AuditInfo> GetCurrent();
}

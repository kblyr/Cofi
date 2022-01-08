namespace Cofi.Auditing;

public interface ICurrentUserIdProvider
{
    Task<int> GetCurrent();
}

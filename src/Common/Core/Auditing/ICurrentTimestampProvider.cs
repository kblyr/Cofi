namespace Cofi.Auditing;

public interface ICurrentTimestampProvider
{
    Task<DateTimeOffset> GetCurrent();
}

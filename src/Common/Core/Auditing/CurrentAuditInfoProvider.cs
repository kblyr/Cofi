namespace Cofi.Auditing;

sealed class CurrentAuditInfoProvider : ICurrentAuditInfoProvider
{
    readonly ICurrentUserIdProvider _userIdProvider;
    readonly ICurrentTimestampProvider _timestampProvider;

    public CurrentAuditInfoProvider(ICurrentUserIdProvider userIdProvider, ICurrentTimestampProvider timestampProvider)
    {
        _userIdProvider = userIdProvider;
        _timestampProvider = timestampProvider;
    }

    public async Task<AuditInfo> GetCurrent()
    {
        return new AuditInfo
        {
            UserId = await _userIdProvider.GetCurrent().ConfigureAwait(false),
            Timestamp = await _timestampProvider.GetCurrent().ConfigureAwait(false)
        };
    }
}
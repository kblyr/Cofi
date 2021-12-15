namespace Cofi.Logging;

public sealed class DbContextLogging
{
    readonly ILogger _logger;

    internal DbContextLogging(ILogger logger)
    {
        _logger = logger;
    }

    public void Creating(string dbContextName) => _logger.LogTrace("Creating database context: {DbContext}", dbContextName);

    public void Creating<TDbContext>() => Creating(typeof(TDbContext).Name);

    public void SavingChanges() => _logger.LogTrace("Saving changes to database");

    public void CommittingTransaction() => _logger.LogTrace("Committing database transaction");
}
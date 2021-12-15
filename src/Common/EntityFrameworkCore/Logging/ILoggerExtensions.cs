namespace Cofi.Logging;

public static class ILoggerExtensions
{
    static DbContextLogging? _dbContext;
    public static DbContextLogging DbContext(this ILogger logger) => _dbContext ??= new(logger);
}

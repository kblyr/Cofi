namespace Cofi.Logging;

public sealed class DbContextLogging
{
    readonly ILogger _logger;

    internal DbContextLogging(ILogger logger)
    {
        _logger = logger;
    }
}
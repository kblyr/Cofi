namespace Cofi
{
    public static class ILoggerExtensions
    {
        public static IDisposable BeginScopeWithProps(this ILogger logger, params (string Name, object? Value)[] properties)
        {
            var dictionary = properties.ToDictionary(property => property.Name, pair => pair.Value);
            return logger.BeginScope(dictionary);
        }
    }
}
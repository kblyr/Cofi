using Microsoft.Extensions.Logging;

namespace Cofi.Logging;

public static class ILoggerExtensions
{

    public static void TryLogTrace(this ILogger logger, string message)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(message);
    }

    public static void TryLogTrace<T0>(this ILogger logger, string message, T0 arg0)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(message, arg0);
    }

    public static void TryLogTrace<T0, T1>(this ILogger logger, string message, T0 arg0, T1 arg1)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(message, arg0, arg1);
    }

    public static void TryLogTrace<T0, T1, T2>(this ILogger logger, string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace(message, arg0, arg1, arg2);
    }

    public static void TryLogDebug(this ILogger logger, string message)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(message);
    }

    public static void TryLogDebug<T0>(this ILogger logger, string message, T0 arg0)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(message, arg0);
    }

    public static void TryLogDebug<T0, T1>(this ILogger logger, string message, T0 arg0, T1 arg1)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(message, arg0, arg1);
    }

    public static void TryLogDebug<T0, T1, T2>(this ILogger logger, string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(message, arg0, arg1, arg2);
    }

    public static void TryLogInformation(this ILogger logger, string message)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(message);
    }

    public static void TryLogInformation<T0>(this ILogger logger, string message, T0 arg0)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(message, arg0);
    }

    public static void TryLogInformation<T0, T1>(this ILogger logger, string message, T0 arg0, T1 arg1)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(message, arg0, arg1);
    }

    public static void TryLogInformation<T0, T1, T2>(this ILogger logger, string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(message, arg0, arg1, arg2);
    }

    public static void TryLogWarning(this ILogger logger, string message)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(message);
    }

    public static void TryLogWarning<T0>(this ILogger logger, string message, T0 arg0)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(message, arg0);
    }

    public static void TryLogWarning<T0, T1>(this ILogger logger, string message, T0 arg0, T1 arg1)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(message, arg0, arg1);
    }

    public static void TryLogWarning<T0, T1, T2>(this ILogger logger, string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(message, arg0, arg1, arg2);
    }

    public static void TryLogError(this ILogger logger, string message)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(message);
    }

    public static void TryLogError<T0>(this ILogger logger, string message, T0 arg0)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(message, arg0);
    }

    public static void TryLogError<T0, T1>(this ILogger logger, string message, T0 arg0, T1 arg1)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(message, arg0, arg1);
    }

    public static void TryLogError<T0, T1, T2>(this ILogger logger, string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(message, arg0, arg1, arg2);
    }

    public static void TryLogCritical(this ILogger logger, string message)
    {
        if (logger.IsEnabled(LogLevel.Critical))
            logger.LogCritical(message);
    }

    public static void TryLogCritical<T0>(this ILogger logger, string message, T0 arg0)
    {
        if (logger.IsEnabled(LogLevel.Critical))
            logger.LogCritical(message, arg0);
    }

    public static void TryLogCritical<T0, T1>(this ILogger logger, string message, T0 arg0, T1 arg1)
    {
        if (logger.IsEnabled(LogLevel.Critical))
            logger.LogCritical(message, arg0, arg1);
    }

    public static void TryLogCritical<T0, T1, T2>(this ILogger logger, string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (logger.IsEnabled(LogLevel.Critical))
            logger.LogCritical(message, arg0, arg1, arg2);
    }
}
namespace Cofi.Messaging;

public record MessageBusConfig
{
    public const string ConfigKey = "MessageBus";

    public bool IsEnabled { get; init; }
}
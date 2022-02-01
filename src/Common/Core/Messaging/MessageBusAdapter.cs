using MassTransit;
using Microsoft.Extensions.Options;

namespace Cofi.Messaging;

public class MessageBusAdapter
{
    readonly MessageBusConfig _config;

    public IBus Bus { get; }

    public MessageBusAdapter(IOptions<MessageBusConfig> config, IBus bus)
    {
        _config = config.Value;
        Bus = bus;
    }

    public async Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        if (_config.IsEnabled)
            await Bus.Publish(message, cancellationToken).ConfigureAwait(false);
    }

    public async Task Publish<T>(Func<T> buildMessage, CancellationToken cancellationToken = default) where T : class
    {
        if (_config.IsEnabled)
            await Bus.Publish(buildMessage(), cancellationToken).ConfigureAwait(false);
    }
}
using MassTransit;

namespace Cofi.Extensions.MassTransit;

public static class IConsumeContextExtensions
{
    public static async Task TryRespondAsync<T>(this ConsumeContext context, T message) where T : class 
    {
        if (context.RequestId.HasValue)
            await context.RespondAsync(message).ConfigureAwait(false);
    }

    public static bool IsRequest(this ConsumeContext context) => context.RequestId.HasValue;
}
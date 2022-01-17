namespace Cofi.Identity.Consumers;

sealed class SendUserEmailVerificationConsumer : IConsumer<SendUserEmailVerification>
{
    readonly IBus _bus;
    readonly IMapper _mapper;

    public SendUserEmailVerificationConsumer(IBus bus, IMapper mapper)
    {
        _bus = bus;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<SendUserEmailVerification> context)
    {
        

        var userEmailVerificationSent = _mapper.Map<SendUserEmailVerification, UserEmailVerificationSent>(context.Message);
        await _bus.Publish(userEmailVerificationSent).ConfigureAwait(false);
    }
}
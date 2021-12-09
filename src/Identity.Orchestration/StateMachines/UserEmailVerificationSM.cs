namespace Cofi.Identity.StateMachines;

sealed class UserEmailVerificationSM : MassTransitStateMachine<UserEmailVerificationSMI>
{
    readonly ILogger<UserEmailVerificationSM> _logger;

    public UserEmailVerificationSM(ILogger<UserEmailVerificationSM> logger)
    {
        _logger = logger;

        InstanceState(instance => instance.CurrentState);
        ConfigureEvents();
        ConfigureEventActivities();
    }

    public State Pending { get; private set; } = default!;
    public State Sent { get; private set; } = default!;

    public Event<UserCreated> UserCreated { get; private set; } = default!;
    public Event<UserEmailVerificationSent> UserEmailVerificationSent { get; private set; } = default!;
    public Event<UserEmailVerified> UserEmailVerified { get; private set; } = default!;

    void ConfigureEvents()
    {
        Event(() => UserCreated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.Id && instance.EmailAddress == context.Message.EmailAddress)
                    .SelectId(context => NewId.NextGuid());
            }
        );

        Event(() => UserEmailVerificationSent,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.UserId && instance.EmailAddress == context.Message.EmailAddress)
                    .SelectId(context => NewId.NextGuid());
            }
        );

        Event(() => UserEmailVerified,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.UserId && instance.EmailAddress == context.Message.EmailAddress);
                correlation.OnMissingInstance(instance => instance.ExecuteAsync(OnUserEmailVerifiedMissingInstanceAsync));
            }
        );
    }

    void ConfigureEventActivities()
    {
        Initially(
            Ignore(UserEmailVerified),
            When(UserCreated, context => context.Data.IsEmailAddressVerified == false)
                .Then(OnUserCreated)
                .Send(context => new SendUserEmailVerification(context.Data.Id, context.Data.EmailAddress))
                .TransitionTo(Pending),
            When(UserEmailVerificationSent)
                .Then(OnUserEmailVerificationSent)
                .TransitionTo(Sent),
            When(UserEmailVerified)
                .Then(OnUserEmailVerified)
                .Finalize()
        );
    }

    void OnUserCreated(BehaviorContext<UserEmailVerificationSMI, UserCreated> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.UserId = context.Data.Id;
        context.Instance.EmailAddress = context.Data.EmailAddress;
        context.Instance.IsVerified = false;
        _logger.LogInformation("Pending user email verification");
    }

    void OnUserEmailVerificationSent(BehaviorContext<UserEmailVerificationSMI, UserEmailVerificationSent> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.UserId = context.Data.UserId;
        context.Instance.EmailAddress = context.Data.EmailAddress;
        context.Instance.IsVerified = false;
        _logger.LogInformation("User email verification has been sent");
    }

    void OnUserEmailVerified(BehaviorContext<UserEmailVerificationSMI, UserEmailVerified> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.IsVerified = true;
        _logger.LogInformation("User email has been verified");
    }

    async Task OnUserEmailVerifiedMissingInstanceAsync(ConsumeContext<UserEmailVerified> context)
    {
        using (_logger.BeginScopeWithProps(context.Message.GetLoggingProps()))
        _logger.LogWarning("Trying to set the current state of a non-existing user email verification to '{state}'", nameof(Final));

        if (context.RequestId.HasValue)
            await context.RespondAsync(new UserNotFound(context.Message.UserId)).ConfigureAwait(false);
    }
}

sealed class UserEmailVerificationSMI : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;

    public int UserId { get; set; }
    public string EmailAddress { get; set; } = "";
    public bool IsVerified { get; set; }
}
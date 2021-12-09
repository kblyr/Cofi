namespace Cofi.Identity.StateMachines;

sealed class UserSM : MassTransitStateMachine<UserSMI>
{
    readonly ILogger<UserSM> _logger;

    public UserSM(ILogger<UserSM> logger)
    {
        _logger = logger;

        InstanceState(instance => instance.CurrentState);

        ConfigureEvents();
        ConfigureEventActivities();
    }

    public State Active { get; private set; } = default!;
    public State Inactive { get; private set; } = default!;

    public Event<UserCreated> UserCreated { get; private set; } = default!;
    public Event<UserActivated> UserActivated { get; private set; } = default!;
    public Event<UserDeactivated> UserDeactivated { get; private set; } = default!;

    void ConfigureEvents()
    {
        Event(() => UserCreated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.Data.Id == context.Message.Id)
                    .SelectId(context => NewId.NextGuid());
            }
        );

        Event(() => UserActivated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.Data.Id == context.Message.UserId);
                correlation.OnMissingInstance(instance => instance.ExecuteAsync(OnUserActivatedMissingInstanceAsync));
            }
        );

        Event(() => UserDeactivated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.Data.Id == context.Message.UserId);
                correlation.OnMissingInstance(instance => instance.ExecuteAsync(OnUserDeactivatedMissingInstanceAsync));
            }
        );
    }

    void ConfigureEventActivities()
    {
        Initially(
            Ignore(UserActivated),
            Ignore(UserDeactivated),
            When(UserCreated, context => context.Data is { IsActive: true, IsEmailAddressVerified: true })
                .Then(OnUserCreated)
                .TransitionTo(Active),
            When(UserCreated, context => context.Data.IsActive == false)
                .Then(OnUserCreated)
                .TransitionTo(Inactive)
        );

        During(Active,
            Ignore(UserCreated),
            Ignore(UserActivated),
            When(UserDeactivated)
                .Then(OnUserDeactivated)
                .TransitionTo(Inactive)
        );

        During(Inactive,
            Ignore(UserCreated),
            Ignore(UserDeactivated),
            When(UserActivated)
                .Then(OnUserActivated)
                .TransitionTo(Active)
        );
    }

    void OnUserCreated(BehaviorContext<UserSMI, UserCreated> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        _logger.LogInformation("User was created");
        context.Instance.Data.Id = context.Data.Id;
        context.Instance.Data.IsActive = context.Data.IsActive;
    }

    void OnUserActivated(BehaviorContext<UserSMI, UserActivated> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.Data.IsActive = true;
        _logger.LogInformation("User was activated");
    }

    async Task OnUserActivatedMissingInstanceAsync(ConsumeContext<UserActivated> context)
    {
        using (_logger.BeginScopeWithProps(context.Message.GetLoggingProps()))
        _logger.LogWarning("Trying to set status 'Activated' to non-existing user");

        if (context.RequestId.HasValue)
            await context.RespondAsync(new UserNotFound(context.Message.UserId)).ConfigureAwait(false);
    }

    void OnUserDeactivated(BehaviorContext<UserSMI, UserDeactivated> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.Data.IsActive = false;
        _logger.LogInformation("User was deactivated");
    }

    async Task OnUserDeactivatedMissingInstanceAsync(ConsumeContext<UserDeactivated> context)
    {
        using (_logger.BeginScopeWithProps(context.Message.GetLoggingProps()))
        _logger.LogWarning("Trying to set status 'Deactivated' to non-existing user");

        if (context.RequestId.HasValue)
            await context.RespondAsync(new UserNotFound(context.Message.UserId)).ConfigureAwait(false);
    }
}

sealed class UserSMI : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;

    public DataObj Data { get; } = new();

    public class DataObj
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}
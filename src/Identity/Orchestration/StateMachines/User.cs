namespace Cofi.Identity.StateMachines;

sealed class User_SM : MassTransitStateMachine<User_SMI>
{
    readonly ILogger<User_SM> _logger;

    public User_SM(ILogger<User_SM> logger)
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
        _logger.TryLogTrace("Configuring events");

        Event(() => UserCreated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.Id)
                    .SelectId(context => NewId.NextGuid());
            }
        );

        Event(() => UserActivated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.Id)
                    .SelectId(context => NewId.NextGuid());
            }
        );

        Event(() => UserDeactivated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.Id)
                    .SelectId(context => NewId.NextGuid());
            }
        );
    }

    void ConfigureEventActivities()
    {
        _logger.TryLogTrace("Configuring event activities");

        Initially(
            When(UserCreated, context => context.Data.IsActive == true)
                .Then(OnUserCreated)
                .TransitionTo(Active),
            When(UserCreated, context => context.Data.IsActive == false)
                .Then(OnUserCreated)
                .TransitionTo(Inactive),
            When(UserActivated)
                .Then(OnUserActivated)
                .TransitionTo(Active),
            When(UserDeactivated)
                .Then(OnUserDeactivated)
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

    void OnUserCreated(BehaviorContext<User_SMI, UserCreated> context)
    {
        _logger.TryLogTrace("User was created");
        context.Instance.UserId = context.Data.Id;
        context.Instance.IsActive = context.Data.IsActive;
    }

    void OnUserActivated(BehaviorContext<User_SMI, UserActivated> context)
    {
        _logger.TryLogTrace("User was activated");
        context.Instance.UserId = context.Data.Id;
        context.Instance.IsActive = true;
    }

    void OnUserDeactivated(BehaviorContext<User_SMI, UserDeactivated> context)
    {
        _logger.TryLogTrace("User was deactivated");
        context.Instance.UserId = context.Data.Id;
        context.Instance.IsActive = false;
    }
}

record User_SMI : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; } = default!;

    public int UserId { get; set; }
    public bool IsActive { get; set; }
}
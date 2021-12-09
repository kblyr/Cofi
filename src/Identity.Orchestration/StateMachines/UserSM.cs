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
    public State ActivationRequested { get; private set; } = default!;

    public Event<UserCreated> UserCreated { get; private set; } = default!;
    public Event<UserActivated> UserActivated { get; private set; } = default!;
    public Event<UserDeactivated> UserDeactivated { get; private set; } = default!;
    public Event<UserActivationRequested> UserActivationRequested { get; private set; } = default!;
    public Event<UserActivationRequestApproved> UserActivationRequestApproved { get; private set; } = default!;
    public Event<UserActivationRequestDenied> UserActivationRequestDenied { get; private set; } = default!;

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
                correlation.CorrelateBy((instance, context) => instance.Data.Id == context.Message.UserId)
                    .SelectId(context => NewId.NextGuid());
            }
        );

        Event(() => UserDeactivated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.Data.Id == context.Message.UserId)
                    .SelectId(context => NewId.NextGuid());
            }
        );

        Event(() => UserActivationRequested,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.Data.Id == context.Message.UserId)
                    .SelectId(context => NewId.NextGuid());
            }
        );

        Event(() => UserActivationRequestApproved,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.Data.Id == context.Message.UserId);
                correlation.OnMissingInstance(instance => instance.ExecuteAsync(OnUserActivationRequestApprovedMissingInstanceAsync));
            }
        );

        Event(() => UserActivationRequestDenied,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.Data.Id == context.Message.UserId);
                correlation.OnMissingInstance(instance => instance.ExecuteAsync(OnUserActivationRequestDeniedMissingInstanceAsync));
            }
        );
    }

    void ConfigureEventActivities()
    {
        Initially(
            Ignore(UserActivationRequestApproved),
            Ignore(UserActivationRequestDenied),
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
            Ignore(UserActivationRequested),
            Ignore(UserActivationRequestApproved),
            Ignore(UserActivationRequestDenied),
            When(UserDeactivated)
                .Then(OnUserDeactivated)
                .TransitionTo(Inactive)
        );

        During(Inactive,
            Ignore(UserCreated),
            Ignore(UserDeactivated),
            Ignore(UserActivationRequestApproved),
            Ignore(UserActivationRequestDenied),
            When(UserActivated)
                .Then(OnUserActivated)
                .TransitionTo(Active),
            When(UserActivationRequested)
                .Then(OnUserActivationRequested)
                .TransitionTo(ActivationRequested)
        );

        During(ActivationRequested,
            Ignore(UserCreated),
            Ignore(UserActivationRequested),
            When(UserActivated)
                .Then(OnUserActivated)
                .TransitionTo(Active),
            When(UserDeactivated)
                .Then(OnUserDeactivated)
                .TransitionTo(Inactive),
            When(UserActivationRequestApproved)
                .Then(OnUserActivationRequestApproved)
                .TransitionTo(Active),
            When(UserActivationRequestDenied)
                .Then(OnUserActivationRequestDenied)
                .TransitionTo(Inactive)
        );
    }

    void OnUserCreated(BehaviorContext<UserSMI, UserCreated> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        _logger.LogInformation("User has been created");
        context.Instance.Data.Id = context.Data.Id;
        context.Instance.Data.IsActive = context.Data.IsActive;
    }

    void OnUserActivated(BehaviorContext<UserSMI, UserActivated> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.Data.Id = context.Data.UserId;
        context.Instance.Data.IsActive = true;
        _logger.LogInformation("User has been activated");
    }

    void OnUserDeactivated(BehaviorContext<UserSMI, UserDeactivated> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.Data.Id = context.Data.UserId;
        context.Instance.Data.IsActive = false;
        _logger.LogInformation("User has been deactivated");
    }

    void OnUserActivationRequested(BehaviorContext<UserSMI, UserActivationRequested> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.Data.Id = context.Data.UserId;
        context.Instance.Data.IsActive = false;
        _logger.LogInformation("User requested activation");
    }

    void OnUserActivationRequestApproved(BehaviorContext<UserSMI, UserActivationRequestApproved> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.Data.IsActive = true;
        _logger.LogInformation("User request for activation has been approved");
    }

    async Task OnUserActivationRequestApprovedMissingInstanceAsync(ConsumeContext<UserActivationRequestApproved> context)
    {
        using (_logger.BeginScopeWithProps(context.Message.GetLoggingProps()))
        _logger.LogWarning("Trying to set the current state of a non-existing user to '{state}'", nameof(Active));

        if (context.RequestId.HasValue)
            await context.RespondAsync(new UserNotFound(context.Message.UserId)).ConfigureAwait(false);
    }

    void OnUserActivationRequestDenied(BehaviorContext<UserSMI, UserActivationRequestDenied> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.Data.IsActive = false;
        _logger.LogInformation("User request for activation has been denied");
    }

    async Task OnUserActivationRequestDeniedMissingInstanceAsync(ConsumeContext<UserActivationRequestDenied> context)
    {
        using (_logger.BeginScopeWithProps(context.Message.GetLoggingProps()));
        _logger.LogWarning("Trying to set the current state of a non-existing user to '{state}'", nameof(Inactive));

        if (context.RequestId.HasValue)
            await context.RespondAsync(new UserNotFound(context.Message.UserId)).ConfigureAwait(false);
    }
}

sealed class UserSMI : SagaStateMachineInstance<UserSMIData> { }

sealed class UserSMIData
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}
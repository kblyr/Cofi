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
    public State PendingEmailVerification { get; private set; } = default!;
    public State EmailVerificationSent { get; private set; } = default!;
    public State Deactivated { get; private set; } = default!;

    public Event<UserCreated> UserCreated { get; private set; } = default!;
    public Event<UserEmailVerificationSent> UserEmailVerificationSent { get; private set; } = default!;
    public Event<UserEmailVerified> UserEmailVerified { get; private set; } = default!;
    public Event<UserDeactivated> UserDeactivated { get; private set; } = default!;

    void ConfigureEvents()
    {
        Event(() => UserCreated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.User.Id == context.Message.Id)
                    .SelectId(context => NewId.NextGuid());
            }
        );

        Event(() => UserEmailVerificationSent,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.User.Id == context.Message.UserId);
                correlation.OnMissingInstance(instance => instance.ExecuteAsync(OnUserEmailVerificationSentMissingInstanceAsync));
            }
        );

        Event(() => UserEmailVerified,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.User.Id == context.Message.UserId);
                correlation.OnMissingInstance(instance => instance.ExecuteAsync(OnUserEmailVerifiedMissingInstanceAsync));
            }
        );

        Event(() => UserDeactivated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.User.Id == context.Message.UserId);
                correlation.OnMissingInstance(instance => instance.ExecuteAsync(OnUserDeactivatedMissingInstanceAsync));
            }
        );
    }

    void ConfigureEventActivities()
    {
        Initially(
            When(UserCreated, context => context.Data is { IsActive: true, IsEmailAddressVerified: true })
                .Then(OnUserCreated)
                .TransitionTo(Active),
            When(UserCreated, context => context.Data is { IsActive: true, IsEmailAddressVerified: false })
                .Then(OnUserCreated)
                .TransitionTo(PendingEmailVerification)
                .Send(context => new SendUserEmailVerification(context.Data.Id, context.Data.EmailAddress)),
            When(UserCreated, context => context.Data.IsActive == false)
                .Then(OnUserCreated)
                .TransitionTo(Inactive)
        );

        During(PendingEmailVerification,
            Ignore(UserCreated),
            When(UserEmailVerificationSent)
                .Then(OnUserEmailVerificationSent)
                .TransitionTo(EmailVerificationSent)
        );

        During(EmailVerificationSent,
            Ignore(UserCreated),
            When(UserEmailVerified, context => context.Instance.User is { IsActive: true })
                .Then(OnUserEmailVerified)
                .TransitionTo(Active),
            When(UserEmailVerified, context => context.Instance.User is { IsActive: false })
                .Then(OnUserEmailVerified)
                .TransitionTo(Inactive)
        );

        During(Active,
            Ignore(UserCreated),
            Ignore(UserEmailVerificationSent),
            Ignore(UserEmailVerified),
            When(UserDeactivated)
                .Then(OnUserDeactivated)
                .TransitionTo(Deactivated)
        );

        During(Inactive,
            Ignore(UserCreated),
            Ignore(UserEmailVerificationSent),
            Ignore(UserEmailVerified),
            Ignore(UserDeactivated)
        );
    }

    void OnUserCreated(BehaviorContext<UserSMI, UserCreated> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        _logger.LogInformation("User was created");
        context.Instance.User.From(context.Data);

        if (context.Data is { IsActive: true, IsEmailAddressVerified: false })
        {
            context.Instance.User.EmailVerification.MarkAsPending();
            _logger.LogInformation("User needs email verification");
        }
    }

    void OnUserEmailVerificationSent(BehaviorContext<UserSMI, UserEmailVerificationSent> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.User.EmailVerification.MarkAsSent();
        _logger.LogInformation("User email verification was sent");
    }

    async Task OnUserEmailVerificationSentMissingInstanceAsync(ConsumeContext<UserEmailVerificationSent> context)
    {
        using (_logger.BeginScopeWithProps(context.Message.GetLoggingProps()))
        _logger.LogWarning("Trying to set status 'Email Verification Sent' to non-existing user");

        if (context.RequestId.HasValue)
            await context.RespondAsync(new UserNotFound(context.Message.UserId));
    }

    void OnUserEmailVerified(BehaviorContext<UserSMI, UserEmailVerified> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.User.IsEmailAddressVerified = true;
        context.Instance.User.EmailVerification.MarkAsVerified();
        _logger.LogInformation("User email was verified");
    }

    async Task OnUserEmailVerifiedMissingInstanceAsync(ConsumeContext<UserEmailVerified> context)
    {
        using (_logger.BeginScopeWithProps(context.Message.GetLoggingProps()))
        _logger.LogWarning("Trying to set status 'Email Verified' to non-existing user");

        if (context.RequestId.HasValue)
            await context.RespondAsync(new UserNotFound(context.Message.UserId));
    }

    void OnUserDeactivated(BehaviorContext<UserSMI, UserDeactivated> context)
    {
        using (_logger.BeginScopeWithProps(context.Data.GetLoggingProps()))
        context.Instance.User.IsActive = false;
        context.Instance.User.Deactivation.MarkAsDeactivated(context.Data.DeactivatedById ?? context.Data.UserId);
        _logger.LogInformation("User was deactivated");
    }

    async Task OnUserDeactivatedMissingInstanceAsync(ConsumeContext<UserDeactivated> context)
    {
        using (_logger.BeginScopeWithProps(context.Message.GetLoggingProps()))
        _logger.LogWarning("Trying to set status 'Deactivated' to non-existing user");

        if (context.RequestId.HasValue)
            await context.RespondAsync(new UserNotFound(context.Message.UserId));
    }
}

sealed class UserSMI : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;

    public UserObj User { get; } = new();

    public class UserObj
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public bool IsActive { get; set; }
        public bool IsEmailAddressVerified { get; set; }
        public bool IsPasswordChangeRequired { get; set; }

        public UserEmailVerificationObj EmailVerification { get; } = new();
        public UserDeactivationObj Deactivation { get; } = new();

        public void From(UserCreated userCreated)
        {
            Id = userCreated.Id;
            Username = userCreated.Username;
            EmailAddress = userCreated.EmailAddress;
            IsActive = userCreated.IsActive;
            IsEmailAddressVerified = userCreated.IsEmailAddressVerified;
            IsPasswordChangeRequired = userCreated.IsPasswordChangeRequired;
        }
    }

    public sealed class UserEmailVerificationObj
    {
        public bool IsSent { get; set; }
        public DateTimeOffset? SentOn { get; set; }
        public DateTimeOffset? VerifiedOn { get; set; }

        public void MarkAsPending()
        {
            IsSent = false;
            SentOn = null;
            VerifiedOn = null;
        }

        public void MarkAsSent(DateTimeOffset? sentOn = null)
        {
            IsSent = true;
            SentOn = sentOn ?? DateTimeOffset.Now;
            VerifiedOn = null;
        }

        public void MarkAsVerified(DateTimeOffset? verifiedOn = null)
        {
            IsSent = false;
            SentOn = null;
            VerifiedOn = verifiedOn ?? DateTimeOffset.Now;
        }
    }

    public sealed class UserDeactivationObj
    {
        public int? DeactivatedById { get; set; }
        public DateTimeOffset? DeactivatedOn { get; set; }

        public void Reset()
        {
            DeactivatedById = null;
            DeactivatedOn = null;
        }

        public void MarkAsDeactivated(int deactivatedById, DateTimeOffset? deactivatedOn = null)
        {
            DeactivatedById = deactivatedById;
            DeactivatedOn = deactivatedOn ?? DateTimeOffset.Now;
        }
    }
}
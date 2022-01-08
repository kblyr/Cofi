using System.Net.Mime;
namespace Cofi.Identity.StateMachines;

sealed class UserEmailVerification_SM : MassTransitStateMachine<UserEmailVerification_SMI>
{
    readonly ILogger<UserEmailVerification_SM> _logger;
    readonly IMapper _mapper;

    public UserEmailVerification_SM(ILogger<UserEmailVerification_SM> logger, IMapper mapper)
    {
        _logger = logger;

        InstanceState(instance => instance.CurrentState);
        ConfigureEvents();
        ConfigureEventActivities();
        _mapper = mapper;
    }

    public State Pending { get; private set; } = default!;
    public State Sent { get; private set; } = default!;
    public State Verified { get; private set; } = default!;

    public Event<UserCreated> UserCreated { get; private set; } = default!;
    public Event<UserEmailVerificationSent> UserEmailVerificationSent { get; private set; } = default!;
    public Event<UserEmailVerified> UserEmailVerified { get; private set; } = default!;

    void ConfigureEvents()
    {
        _logger.TryLogTrace("Configuring events");

        Event(() => UserCreated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.Id && instance.EmailAddress == context.Message.EmailAddress)
                    .SelectId(context => NewId.NextGuid());
            }
        );
        
        Event(() => UserEmailVerificationSent,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.Id && instance.EmailAddress == context.Message.EmailAddress)
                    .SelectId(contex => NewId.NextGuid());
            }
        );
    }

    void ConfigureEventActivities()
    {
        _logger.TryLogTrace("Configure event activities");

        Initially(
            When(UserCreated, context => context.Data.IsEmailVerified == true)
                .Then(OnUserCreated)
                .TransitionTo(Verified),
            When(UserCreated, context => context.Data.IsEmailVerified == false)
                .Then(OnUserCreated)
                .Publish(context => _mapper.Map<UserCreated, SendUserEmailVerification>(context.Data))
                .TransitionTo(Pending),
            When(UserEmailVerificationSent)
                .Then(OnUserEmailVerificationSent)
                .TransitionTo(Sent),
            When(UserEmailVerified)
                .Then(OnUserEmailVerified)
                .TransitionTo(Verified)
        );

        DuringAny(Ignore(UserCreated));

        During(Pending,
            When(UserEmailVerificationSent)
                .Then(OnUserEmailVerificationSent)
                .TransitionTo(Sent),
            When(UserEmailVerified)
                .Then(OnUserEmailVerified)
                .TransitionTo(Verified)
        );

        During(Sent,
            Ignore(UserEmailVerificationSent),
            When(UserEmailVerified)
                .Then(OnUserEmailVerified)
                .TransitionTo(Verified)
        );

        During(Verified,
            Ignore(UserEmailVerificationSent),
            Ignore(UserEmailVerified) 
        );
    }

    void OnUserCreated(BehaviorContext<UserEmailVerification_SMI, UserCreated> context)
    {
        _logger.TryLogTrace("User was created");
        context.Instance.UserId = context.Data.Id;
        context.Instance.EmailAddress = context.Data.EmailAddress;
        context.Instance.IsVerified = context.Data.IsEmailVerified;
    }

    void OnUserEmailVerificationSent(BehaviorContext<UserEmailVerification_SMI, UserEmailVerificationSent> context)
    {
        _logger.TryLogTrace("User email verification was sent");
        context.Instance.UserId = context.Data.Id;
        context.Instance.EmailAddress = context.Data.EmailAddress;
        context.Instance.IsVerified = false;
    }

    void OnUserEmailVerified(BehaviorContext<UserEmailVerification_SMI, UserEmailVerified> context)
    {
        _logger.TryLogTrace("User email was verified");
        context.Instance.UserId = context.Data.Id;
        context.Instance.EmailAddress = context.Data.EmailAddress;
        context.Instance.IsVerified = true;
    }
}

record UserEmailVerification_SMI : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; } = default!;

    public int UserId { get; set; }
    public string EmailAddress { get; set; } = default!;
    public bool IsVerified { get; set; }
}
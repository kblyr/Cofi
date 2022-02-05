using Automatonymous;
using MassTransit.Saga;

namespace Cofi.Identity.StateMachines;

sealed class User_SM : MassTransitStateMachine<User_SMI>
{
    public State Pending { get; private set; } = default!;

    public Event<UserCreated> UserCreated { get; private set; } = default!;
}

sealed class User_SMI : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; } = default!;
}

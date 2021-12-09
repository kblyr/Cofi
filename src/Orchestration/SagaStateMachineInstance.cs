namespace Cofi;

public abstract class SagaStateMachineInstance<TData> : SagaStateMachineInstance
    where TData : class, new()
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;
    public TData Data { get; } = new();
}
using EsTaskManager.Domain.Aggregates;

namespace EsTaskManager.Domain.Events;

public abstract record DomainEvent
{
    public DomainEvent(AggregateId id)
    {
        Id = id;
    }

    public AggregateId Id { get; }
}
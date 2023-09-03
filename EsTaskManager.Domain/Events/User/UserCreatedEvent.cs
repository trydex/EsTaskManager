using EsTaskManager.Domain.Aggregates;

namespace EsTaskManager.Domain.Events.User;

public record UserCreatedEvent : DomainEvent
{
    public UserCreatedEvent(AggregateId id, string name, string email) : base(id)
    {
        Name = name;
        Email = email;
    }

    public string Name { get; }
    public string Email { get; }
}
using EsTaskManager.Domain.Aggregates;

namespace EsTaskManager.Domain.Events.User;

public record UserUpdatedEvent : DomainEvent
{
    public UserUpdatedEvent(AggregateId id, string Name, string Email) : base(id)
    {
        this.Name = Name;
        this.Email = Email;
    }
    
    public string Name { get; }
    public string Email { get; }
}
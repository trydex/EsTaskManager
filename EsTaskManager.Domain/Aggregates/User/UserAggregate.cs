using EsTaskManager.Domain.Events.User;

namespace EsTaskManager.Domain.Aggregates.User;

public class UserAggregate : Aggregate
{
    public UserAggregate(AggregateId id, string name, string email) : base(id)
    {
        var @event = new UserCreatedEvent(id, name, email);
        RaiseEvent(@event);
    }
    
    private UserAggregate(AggregateId id) : base(id)
    {
    }
    
    public string Name { get; private set; }
    public string Email { get; private set; }

    internal void Apply(UserCreatedEvent @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Email = @event.Email;
    }
    
    internal void Apply(UserUpdatedEvent @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Email = @event.Email;
    }

    public void UpdateUserData(string name, string email)
    {
        RaiseEvent(new UserUpdatedEvent(Id, name, email));
    }
}